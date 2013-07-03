using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AlgoTrader.Interfaces
{
    [DataContract]
    public enum positionStatus 
    {
        [EnumMember]
        Open,
        [EnumMember]
        Closed
    };

    public interface IPosition
    {
        ISymbol symbol { get; set; }
        double price { get; set; }
        int quantity { get; set; }
        positionStatus status { get; set; }
        List<ITrade> trades { get; }
        double basis { get; }
    
        void updatePosition(ITrade trade);
    }
}
