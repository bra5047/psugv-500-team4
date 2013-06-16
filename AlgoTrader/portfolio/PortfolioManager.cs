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
            TraderContext db = new TraderContext();
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
            TraderContext db = new TraderContext();
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
            throw new NotImplementedException();
        }

        public double getAvailableCash()
        {
            TraderContext db = new TraderContext();
            Portfolio pf = db.Portfolios.FirstOrDefault();
            return pf.Cash;
        }
    }
}
