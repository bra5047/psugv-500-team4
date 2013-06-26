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
    public class Trade : ITrade
    {
        [Key]
        public int TradeId { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public DateTime timestamp { get; set; }
        public tradeTypes type { get; set; }
        public string TransactionId { get; set; }
        public tradeStatus Status { get; set; }

        public string SymbolName { get; set; }
        [ForeignKey("SymbolName")]
        public virtual Symbol Symbol { get; set; }

        public int? PositionId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }

        public int? RelatedTradeId { get; set; }
        [ForeignKey("RelatedTradeId")]
        public virtual Trade RelatedTrade { get; set; }

        // ITrade stuff
        ISymbol ITrade.symbol
        {
            get
            {
                if (Symbol == null)
                {
                    return new Symbol(SymbolName);
                }
                else
                {
                    return Symbol;
                }
            }
            set { throw new NotImplementedException(); }
        }

        double ITrade.value
        {
            get
            {
                return price * quantity;
            }
        }
    }
}
