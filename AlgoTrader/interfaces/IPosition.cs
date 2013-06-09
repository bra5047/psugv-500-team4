using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    public enum positionStatus { Open, Closed };

    public interface IPosition
    {
        ISymbol symbol { get; set; }
        double price { get; set; }
        int quantity { get; set; }
        positionStatus status { get; set; }
        List<ITrade> trades { get; }
    
        void updatePosition(ITrade trade);
        void closePosition();
    }
}
