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
    public class PositionMessage
    {
        [DataMember]
        public string SymbolName;
        [DataMember]
        public double Price;
        [DataMember]
        public int Quantity;
        [DataMember]
        public positionStatus Status;
        [DataMember]
        public List<TradeMessage> Trades;
        [DataMember]
        public double Basis;

        public PositionMessage()
        {
            Trades = new List<TradeMessage>();
        }

        public PositionMessage(IPosition position) : this()
        {
            this.SymbolName = position.symbol.name;
            this.Price = position.price;
            this.Quantity = position.quantity;
            this.Status = position.status;
            this.Basis = position.basis;

            foreach (ITrade t in position.trades)
            {
                Trades.Add(new TradeMessage(t));
            }
        }
    }
}
