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
        [NotMapped]
        private int _quantity;

        [Key]
        public int TradeId { get; set; }
        public int? InitialQuantity { get; set; }

        public int quantity
        {
            get { return _quantity; }
            set
            {
                if (InitialQuantity == null) InitialQuantity = value;
                _quantity = value;
            }
        }

        public double price { get; set; }
        public DateTime timestamp { get; set; }
        public tradeTypes type { get; set; }
        public string TransactionId { get; set; }
        public tradeStatus Status { get; set; }
        public double PaidCommission { get; set; }

        public string SymbolName { get; set; }
        [ForeignKey("SymbolName")]
        public virtual Symbol Symbol { get; set; }

        public int? PositionId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }

        public int? RelatedTradeId { get; set; }
        [ForeignKey("RelatedTradeId")]
        public virtual Trade RelatedTrade { get; set; }

        public Trade sell(int sellQuantity)
        {
            if (sellQuantity < 1 || sellQuantity > this.quantity) throw new ArgumentException("sellQuantity");
            //if (this.type == tradeTypes.Sell) throw new NotImplementedException();
            //if (this.Status == tradeStatus.Closed) throw new NotImplementedException();

            Trade t = new Trade();
            t.Symbol = Symbol;
            t.RelatedTrade = this;
            t.Position = Position;
            t.type = tradeTypes.Sell;
            t.Status = tradeStatus.Closed;
            t.timestamp = DateTime.Now;
            t.quantity = sellQuantity;

            this.quantity -= sellQuantity;
            if (this.quantity == 0) this.Status = tradeStatus.Closed;
            return t;
        }

        public Trade()
        {
            PaidCommission = 4.75;
        }

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
