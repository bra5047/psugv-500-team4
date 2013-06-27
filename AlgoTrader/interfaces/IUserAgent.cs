using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace AlgoTrader.Interfaces
{
    [DataContract]
    public enum responseCodes
    {
        [EnumMember]
        Accept,
        [EnumMember]
        Reject,
        [EnumMember]
        Pending
    };

    [ServiceContract]
    public interface IUserAgent
    {
        [OperationContract(IsOneWay = true)]
        void generateAlert(string symbolName, tradeTypes type, int quantity, double price);
        [OperationContract(IsOneWay = true)]
        void processAlertResponse(string alertID, string alertResponse);
        [OperationContract]
        List<AlertMessage> getPendingAlerts();
    }

    [DataContract]
    public class AlertMessage
    {
        [DataMember]
        public string AlertId { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public string SymbolName { get; set; }
        [DataMember]
        public tradeTypes Type { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public string SentTo { get; set; }
        [DataMember]
        public responseCodes? ResponseCode { get; set; }
        [DataMember]
        public string Response { get; set; }

        public AlertMessage()
        {
            // no-arg constructor
        }

        public AlertMessage(IAlert alert)
        {
            AlertId = alert.AlertId.ToString();
            Timestamp = alert.Timestamp;
            SymbolName = alert.Symbol.name;
            Type = alert.Type;
            Quantity = alert.Quantity;
            Price = alert.Price;
            SentTo = alert.SentTo;
            ResponseCode = alert.ResponseCode;
            Response = alert.Response;
        }
    }
}
