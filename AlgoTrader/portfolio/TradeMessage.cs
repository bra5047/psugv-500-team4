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
        public int Quantity;
        [DataMember]
        public DateTime Timestamp;
        [DataMember]
        public tradeTypes Type;

        public TradeMessage()
        {
            // whatever
        }

        public TradeMessage(ITrade trade)
            : this()
        {
            SymbolName = trade.symbol.name;
            Price = trade.price;
            Quantity = trade.quantity;
            Timestamp = trade.timestamp;
            Type = trade.type;
        }
    }
}
