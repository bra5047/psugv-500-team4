using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
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

        [OperationContract]
        void buy(string symbolName, int quantity);

        [OperationContract]
        double getAvailableCash();
    }
}
