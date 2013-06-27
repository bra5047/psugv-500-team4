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

        [FaultContract(typeof(InsufficientQuantityFault))]
        [FaultContract(typeof(ArgumentExceptionFault))]
        [OperationContract]
        void sell(string symbolName, int quantity);

        [FaultContract(typeof(InsufficientFundsFault))]
        [FaultContract(typeof(AllocationViolationFault))]
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

    [DataContract]
    public class AllocationViolationFault
    {
        [DataMember]
        public string FaultMessage;

        public AllocationViolationFault()
        {
            FaultMessage = "Requested transaction would violate a portfolio allocation rule.";
        }
    }

    [DataContract]
    public class InsufficientQuantityFault
    {
        [DataMember]
        public string FaultMessage;
        [DataMember]
        public double RequestedQuantity;
        [DataMember]
        public double AvailableQuantity;

        public InsufficientQuantityFault()
        {
            FaultMessage = "Not enough shares available to complete the requested transaction.";
        }

        public InsufficientQuantityFault(double requested, double available)
            : this()
        {
            RequestedQuantity = requested;
            AvailableQuantity = available;
        }

    }
}
