using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlgoTrader.Interfaces
{
    [DataContract]
    public class ArgumentExceptionFault
    {
        [DataMember]
        public string FaultMessage;
        [DataMember]
        public string ParameterName;

        public ArgumentExceptionFault(ArgumentException ex)
        {
            FaultMessage = ex.Message;
            ParameterName = ex.ParamName;
        }
    }
}
