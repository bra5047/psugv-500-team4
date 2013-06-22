using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using AlgoTrader.portfolio;

namespace AlgoTrader.Interfaces
{
    [ServiceContract]
    public interface IPortfolioManager
    {
        [OperationContract]
        List<PositionMessage> GetOpenPositions();

        [OperationContract]
        PositionMessage GetPosition(string SymbolName);

        [OperationContract]
        void sell(string symbolName, int quantity);

        [FaultContract(typeof(InsufficientFundsFault))]
        [OperationContract]
        void buy(string symbolName, int quantity);

        [OperationContract]
        double getAvailableCash();
    }

    [DataContract]
    public class InsufficientFundsFault
    {
        [DataMember]
        public string FaultMessage;
        [DataMember]
        public double TransactionAmount;
        [DataMember]
        public double AvailableAmount;

        public InsufficientFundsFault()
        {
            FaultMessage = "Insufficient funds to perform the requested transaction.";
        }

        public InsufficientFundsFault(double transaction, double available)
            : this()
        {
            TransactionAmount = transaction;
            AvailableAmount = available;
        }
    }
}
