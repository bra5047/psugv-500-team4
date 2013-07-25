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
        int TradeId { get; set; }
        ISymbol symbol { get; set; }
        int quantity { get; set; }
        int? InitialQuantity { get; set; }
        double price { get; set; }
        DateTime timestamp { get; set; }
        tradeTypes type { get; set; }
        tradeStatus Status { get; set; }
        double PaidCommission { get; set; }
        string TransactionId { get; set; }
        int? RelatedTradeId { get; set; }
        double value { get; }
    }
}
