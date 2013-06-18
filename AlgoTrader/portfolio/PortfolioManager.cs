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
            Portfolio p = db.Portfolios.FirstOrDefault(); // assumes only one portfolio for now
            Position pos = db.Positions.Where(x => x.PortfolioId == p.PortfolioId && x.SymbolName == symbolName).FirstOrDefault();
            Trade t = db.Trades.Create();
            t.type = tradeTypes.Buy;
            t.SymbolName = symbolName;
            t.quantity = quantity;
            t.timestamp = DateTime.Now;
            t.price = 5;
            t.PositionId = pos.PositionId;
            db.Trades.Add(t);
            p.Cash -= t.price * t.quantity;
            db.SaveChanges();
            db.Dispose();
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
