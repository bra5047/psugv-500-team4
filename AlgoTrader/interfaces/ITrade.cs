using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AlgoTrader.Interfaces
{
    [DataContract]
    public enum tradeTypes
    {
        [EnumMember]
        Buy,
        [EnumMember]
        Sell
    };

    [DataContract]
    public enum tradeStatus
    {
        [EnumMember]
        Active,
        [EnumMember]
        Closed
    };

    public interface ITrade
    {
        ISymbol symbol { get; set; }
        int quantity { get; set; }
        double price { get; set; }
        DateTime timestamp { get; set; }
        tradeTypes type { get; set; }
        double value { get; }
    }
}
