using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    public class Position : IPosition
    {
        [Key]
        public int PositionId { get; set; }
        public string SymbolName { get; set; }
        [ForeignKey("SymbolName")]
        public virtual Symbol Symbol { get; set; }
        public int? PortfolioId { get; set; }
        [ForeignKey("PortfolioId")]
        public virtual Portfolio Portfolio { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public positionStatus status { get; set; }
        public virtual List<Trade> Trades { get; set; }

        // IPosition stuff
        public ISymbol symbol
        {
            get { return Symbol; }
            set { throw new NotImplementedException(); }
        }

        public List<ITrade> trades
        {
            get { return Trades.ToList<ITrade>(); }
        }

        public void updatePosition(ITrade trade)
        {
            throw new NotImplementedException();
        }

        public void closePosition()
        {
            throw new NotImplementedException();
        }
    }
}
