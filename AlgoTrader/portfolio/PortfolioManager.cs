using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using log4net;

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
            ILog log = LogManager.GetLogger(typeof(PortfolioManager));
            log.DebugFormat("GetPosition() called for {0}", SymbolName);
            try
            {
                TraderContext db = DbContext;
                Portfolio pf = db.Portfolios.FirstOrDefault();
                var q = pf.Positions.Where(p => p.status == positionStatus.Open && p.SymbolName == SymbolName).Select(p => p).FirstOrDefault();
                return new PositionMessage(q);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Exception: {0}", ex.Message);
                log.ErrorFormat("Stack Trace: {0}", ex.StackTrace);
            }
            return new PositionMessage();
        }

        public void sell(string symbolName, int quantity)
        {
            if (quantity < 1) throw new System.ServiceModel.FaultException<ArgumentException>(new ArgumentException("Quantity must be greater than zero.", "quantity"));
            
            TraderContext db = DbContext;
            Symbol s = db.Symbols.Where(x => x.name == symbolName).FirstOrDefault();
            if (s == null) throw new System.ServiceModel.FaultException<ArgumentException>(new ArgumentException("Symbol not found.", "symbol"));
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
                throw new System.ServiceModel.FaultException<InsufficientQuantityFault>(new InsufficientQuantityFault(quantity, pos.quantity));
            }
            // figure out which shares to sell to get the best price
            List<Trade> byProfit = pos.Trades.Where(t => t.type == tradeTypes.Buy).OrderByDescending(o => (lastPrice.price - o.price)).ToList<Trade>();
            List<Trade> toSell = new List<Trade>();
            string transaction_id = Guid.NewGuid().ToString();
            foreach (Trade t in byProfit)
            {
                int remaining = quantity - toSell.Sum(x => x.quantity);
                Trade next = t.sell(Math.Min(t.quantity, remaining));
                next.price = lastPrice.price;
                next.TransactionId = transaction_id;
                toSell.Add(next);
                if (toSell.Sum(x => x.quantity) == quantity) break;
            }
            pos.Trades.AddRange(toSell);
            pos.Recalculate();
            portfolio.Cash += toSell.Sum(x => (x.price * x.quantity) - x.PaidCommission);
            db.SaveChanges();
            db.Dispose();
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
            port.Cash -= (t.price * t.quantity) - t.PaidCommission;
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
