using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    interface IQuote
    {
        ISymbol symbol { get; set; }
        double price { get; set; }
        DateTime timestamp { get; set; }
    }
}
