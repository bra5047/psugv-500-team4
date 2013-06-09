using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    interface IStrategySummary
    {
        ISymbol symbol { get; set; }
    }
}
