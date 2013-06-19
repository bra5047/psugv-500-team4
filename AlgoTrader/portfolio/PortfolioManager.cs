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
            throw new NotImplementedException();
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

            if (!HasEnoughCash(p, quantity, lastQuote.price))
            {
                // again, just bail if we have a problem. might need to throw an exception
                return;
            }
            Position pos = db.Positions.Where(x => x.PortfolioId == p.PortfolioId && x.SymbolName == symbolName).FirstOrDefault();
            Trade t = db.Trades.Create();
            ProcessBuyTrade(t, symbolName, quantity, lastQuote.price, pos, p);
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
            // this record keeping stuff could be implemented in the data model, but it's easier to implement here right now
            t.PositionId = pos.PositionId;
            pos.quantity += t.quantity;
            pos.price += t.price * t.quantity;
            port.Cash -= t.price * t.quantity;
        }

        public bool HasEnoughCash(Portfolio port, int quantity, double price)
        {
            return (port.Cash > quantity * price);
        }

        public double getAvailableCash()
        {
            TraderContext db = DbContext;
            Portfolio pf = db.Portfolios.FirstOrDefault();
            return pf.Cash;
        }


        public PortfolioManager()
        {
            _dbContext = null;
        }

        public PortfolioManager(TraderContext db)
        {
            _dbContext = db;
        }
    }
}
