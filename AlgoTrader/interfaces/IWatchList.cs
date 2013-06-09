using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    interface IWatchList
    {
        List<ISymbol> symbols { get; }

        bool addToList(ISymbol symbol);
        bool removeFromList(ISymbol symbol);
    }
}
