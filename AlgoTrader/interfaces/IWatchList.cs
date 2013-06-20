using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgoTrader.datamodel;

namespace AlgoTrader.Interfaces
{
    public interface IWatchList
    {
        List<ISymbol> symbols { get; }
		List<WatchListItem> items { get; set; }
		
        bool AddToList(ISymbol symbol, string listName);
        bool RemoveFromList(ISymbol symbol, string listName);
    }
}
