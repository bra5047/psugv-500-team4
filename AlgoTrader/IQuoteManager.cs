using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    interface IQuoteManager
    {
        bool startWatching(ISymbol symbol);
        bool stopWatching(ISymbol symbol);
        event EventHandler newQuote;

        //symbol validation?
    }
}
