using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    enum tradeTypes { Buy, Sell };

    interface ITrade
    {
        ISymbol symbol { get; set; }
        int quantity { get; set; }
        double price { get; set; }
        DateTime timestamp { get; set; }
        tradeTypes type { get; set; }
    }
}
