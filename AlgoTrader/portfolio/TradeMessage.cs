using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using AlgoTrader.Interfaces;

namespace AlgoTrader.portfolio
{
    [DataContract]
    public class TradeMessage
    {
        [DataMember]
        public int TradeId;
        [DataMember]
        public string SymbolName;
        [DataMember]
        public double Price;
        [DataMember]
        public int? InitialQuantity;
        [DataMember]
        public int Quantity;
        [DataMember]
        public DateTime Timestamp;
        [DataMember]
        public tradeTypes Type;
        [DataMember]
        public tradeStatus Status;
        [DataMember]
        public double PaidCommission;
        [DataMember]
        public string TransactionId;
        [DataMember]
        public int? RelatedTradeId;

        public TradeMessage()
        {
            // whatever
        }

        public TradeMessage(ITrade trade)
            : this()
        {
            TradeId = trade.TradeId;
            SymbolName = trade.symbol.name;
            Price = trade.price;
            Quantity = trade.quantity;
            InitialQuantity = trade.InitialQuantity;
            Timestamp = trade.timestamp;
            Type = trade.type;
            Status = trade.Status;
            PaidCommission = trade.PaidCommission;
            TransactionId = trade.TransactionId;
            RelatedTradeId = trade.RelatedTradeId;
        }
    }
}
