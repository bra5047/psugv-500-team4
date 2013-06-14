﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace AlgoTrader.Interfaces
{
    [ServiceContract]
    public interface IStrategy
    {
        [OperationContract(IsOneWay = true)]
        void NewQuote(QuoteMessage quote);

        [OperationContract]
        bool startWatching(string symbolName);
        [OperationContract]
        bool stopWatching(string symbolName);

        [OperationContract]
        StrategySummary getSummary(string symbolName);

        IStrategyDetail getDetailedAnalysis(ISymbol symbol);
    }

    [DataContract]
    public class QuoteMessage
    {
        [DataMember]
        public int QuoteId { get; set; }
        [DataMember]
        public double price { get; set; }
        [DataMember]
        public DateTime timestamp { get; set; }
        [DataMember]
        public string SymbolName { get; set; }
    }

    [DataContract]
    public class StrategySummary
    {
        [DataMember]
        public string SymbolName;
        [DataMember]
        public DateTime AsOf;
        [DataMember]
        public StrategySignal CurrentSignal;
    }

    [DataContract]
    public enum StrategySignal
    {
        [EnumMember]
        Buy,
        [EnumMember]
        Sell,
        [EnumMember]
        None
    }
}
