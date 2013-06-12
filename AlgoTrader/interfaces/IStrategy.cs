using System;
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
        public void NewQuote(QuoteMessage quote);

        bool startWatching(ISymbol symbol);
        bool stopWatching(ISymbol symbol);
        IStrategyDetail getDetailedAnalysis(ISymbol symbol);
        IStrategySummary getSummary(ISymbol symbol);
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
}
