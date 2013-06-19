using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace AlgoTrader.Interfaces
{
    [ServiceContract]
    public interface IUserAgent
    {
        [OperationContract(IsOneWay = true)]
        void generateAlert(string symbolName, tradeTypes type, int quantity, double price);
        [OperationContract(IsOneWay = true)]
        void processAlertResponse(string alertID, string alertResponse);
    }
}
