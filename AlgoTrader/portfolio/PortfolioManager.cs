using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.portfolio
{
    public class PortfolioManager : IPortfolioManager
    {
        public List<PositionMessage> GetOpenPositions()
        {
            TraderContext db = DbContext;
            Portfolio pf = db.Portfolios.FirstOrDefault();
            var q = pf.Positions.Where(p => p.status == positionStatus.Open).Select(p => p);
            List<PositionMessage> result = new List<PositionMessage>();
            foreach (IPosition p in q)
            {
                result.Add(new PositionMessage(p));
            }
            return result;
        }

        public PositionMessage GetPosition(string SymbolName)
        {
            TraderContext db = DbContext;
            Portfolio pf = db.Portfolios.FirstOrDefault();
            var q = pf.Positions.Where(p => p.status == positionStatus.Open && p.SymbolName == SymbolName).Select(p => p).FirstOrDefault();            
            return new PositionMessage(q);
        }

        public void sell(string symbolName, int quantity)
        {
            if (quantity < 1) throw new ArgumentException();
            
            TraderContext db = DbContext;
            Symbol s = db.Symbols.Where(x => x.name == symbolName).FirstOrDefault();
            if (s == null) throw new ArgumentException();
            Quote lastPrice = db.FindLastQuoteFor(s);
            if (lastPrice == null)
            {
                // not sure what to do here
                return;
            }
            Portfolio portfolio = db.Portfolios.FirstOrDefault();
            Position pos = portfolio.Positions.Where(x => x.Symbol == s && x.status == positionStatus.Open).FirstOrDefault();
            if (pos == null || pos.quantity < quantity)
            {
                throw new Exception(); // insufficient quantity exception
            }
            List<Trade> byProfit = pos.Trades.Where(t => t.type == tradeTypes.Buy).OrderByDescending(o => (lastPrice.price - o.price)).ToList<Trade>();
            // figure out which shares to sell to get the best price
            List<Trade> toSell = new List<Trade>();
            string transaction_id = Guid.NewGuid().ToString();
            foreach (Trade t in byProfit)
            {
                // TODO: Trade sell = buyTrade.reduceBy(2);
                Trade next = db.Trades.Create();
                next.type = tradeTypes.Sell;
                next.Status = tradeStatus.Closed;
                next.timestamp = DateTime.Now;
                next.TransactionId = transaction_id;
                next.RelatedTrade = t;
                next.Symbol = s;
                next.Position = pos;
                next.price = lastPrice.price;
                int remaining = quantity - toSell.Sum(x => x.quantity);
                next.quantity = Math.Min(t.quantity, remaining);
                t.quantity -= next.quantity;
                if (t.quantity == 0) t.Status = tradeStatus.Closed;
                toSell.Add(next);
                if (toSell.Sum(x => x.quantity) == quantity) break;
            }
            pos.Trades.AddRange(toSell);
            pos.Recalculate();
            portfolio.Cash += toSell.Sum(x => x.price * x.quantity);
            db.SaveChanges();
        }

        public void buy(string symbolName, int quantity)
        {
            TraderContext db = DbContext;
            Quote lastQuote = db.Quotes.Where(x => x.SymbolName == symbolName).OrderByDescending(y => y.timestamp).FirstOrDefault();
            if (lastQuote == null)
            {
                // just bail for now; maybe it should wait until a quote comes in? Synchronous call into QuoteManager?
                return;
            }
            Portfolio p = db.Portfolios.FirstOrDefault(); // assumes only one portfolio for now

            Position pos = db.Positions.Where(x => x.PortfolioId == p.PortfolioId && x.SymbolName == symbolName).FirstOrDefault();
            Trade t = db.Trades.Create();

            try
            {
                ProcessBuyTrade(t, symbolName, quantity, lastQuote.price, pos, p);
            }
            catch (InsufficientFunds insuf)
            {
                throw new System.ServiceModel.FaultException<InsufficientFundsFault>(new InsufficientFundsFault(double.Parse(insuf.Data["TransactionAmount"].ToString()), double.Parse(insuf.Data["AvailableFunds"].ToString())));
            }
            catch (AllocationViolation alloc)
            {
                throw new System.ServiceModel.FaultException<AllocationViolationFault>(new AllocationViolationFault());
            }

            db.Trades.Add(t);
            db.SaveChanges();
            db.Dispose();
        }

        public void ProcessBuyTrade(Trade t, string symbolName, int quantity, double price, Position pos, Portfolio port)
        {
            t.type = tradeTypes.Buy;
            t.SymbolName = symbolName;
            t.quantity = quantity;
            t.timestamp = DateTime.Now;
            t.price = price;
            ApplyRules(port, t);
            t.PositionId = pos.PositionId;
            pos.quantity += t.quantity;
            pos.price += t.price * t.quantity;
            port.Cash -= t.price * t.quantity;
        }

        public double getAvailableCash()
        {
            TraderContext db = DbContext;
            Portfolio pf = db.Portfolios.FirstOrDefault();
            return pf.Cash;
        }

        public void ApplyRules(Portfolio p, Trade t)
        {
            foreach (PortfolioRule r in Rules)
            {
                r.Apply(p, t);
            }
        }

        public List<PortfolioRule> Rules { get; set; }

        public void LoadSettings(Dictionary<string, string> settings)
        {
            Rules.Clear();
            Rules.Add(new EnoughFundsRule());
            foreach (string s in settings.Keys)
            {
                switch (s)
                {
                    case "MAX_POSITION_RATIO":
                        double max = double.Parse(settings[s]);
                        Rules.Add(new AllocationRule(max));
                        break;
                }
            }
        }

        public void LoadSettings()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            TraderContext db = DbContext;
            var settings = from s in db.SystemSettings where s.Module == "Portfolio" select s;
            foreach (var i in settings)
            {
                config.Add(i.Name, i.Value);
            }
            LoadSettings(config);
            db.Dispose();
        }

        private TraderContext _dbContext;

        protected TraderContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }
                else
                {
                    return new TraderContext();
                }
            }
        }

        public PortfolioManager()
        {
            _dbContext = null;
            Rules = new List<PortfolioRule>();
            LoadSettings();
        }

        public PortfolioManager(TraderContext db) : this()
        {
            _dbContext = db;
        }

        public PortfolioManager(Dictionary<string, string> settings)
        {
            Rules = new List<PortfolioRule>();
            LoadSettings(settings);
        }
    }
}
