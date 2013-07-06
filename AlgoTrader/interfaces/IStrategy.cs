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
        void NewQuote(QuoteMessage quote);

        void NewQuotes(List<QuoteMessage> quotes);

        [OperationContract]
        bool startWatching(string symbolName);
        [OperationContract]
        bool stopWatching(string symbolName);

        [OperationContract]
        StrategySummary getSummary(string symbolName);

        [OperationContract]
        StrategyDetail getDetailedAnalysis(string symbolName);
    }

    [DataContract]
    public class QuoteMessage
    {
        [DataMember]
        public int QuoteId { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public DateTime timestamp { get; set; }
        [DataMember]
        public string SymbolName { get; set; }

        public QuoteMessage()
        {
            //whatever
        }

        public QuoteMessage(double price, DateTime tstamp, string symbolName)
        {
            Price = price;
            timestamp = tstamp;
            SymbolName = symbolName;
        }

        public QuoteMessage(IQuote quote)
        {
            Price = quote.price;
            SymbolName = quote.symbol.name;
            timestamp = quote.timestamp;
        }
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

    [DataContract]
    public class StrategyDetail
    {
        [DataMember]
        public string SymbolName;
        [DataMember]
        public SortedList<DateTime, double> History_Series_1;
        [DataMember]
        public SortedList<DateTime, double> History_Series_2;
        [DataMember]
        public SortedList<DateTime, double> History_Series_3;
        [DataMember]
        public string Metric_1_Label;
        [DataMember]
        public double Metric_1;
        [DataMember]
        public string Metric_2_Label;
        [DataMember]
        public double Metric_2;
        [DataMember]
        public string Metric_3_Label;
        [DataMember]
        public double Metric_3;
    }
}
