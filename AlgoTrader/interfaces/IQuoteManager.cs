using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace AlgoTrader.Interfaces
{
    [ServiceContract]
    public interface IQuoteManager
    {
        [OperationContract]
        bool startWatching(string SymbolName);
        //[OperationContract]
        //bool stopWatching(string SymbolName);
    }
}
