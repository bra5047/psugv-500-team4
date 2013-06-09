using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    interface IStrategy
    {
        bool startWatching(ISymbol symbol);
        bool stopWatching(ISymbol symbol);
        IStrategyDetail getDetailedAnalysis(ISymbol symbol);
        IStrategySummary getSummary(ISymbol symbol);
        event EventHandler newAlert;
    }
}
