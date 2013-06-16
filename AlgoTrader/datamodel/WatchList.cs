using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    public class WatchList : IWatchList
    {
        [Key]
        public string ListName { get; set; }
        public virtual List<WatchListItem> Items { get; set; }

        public bool addToList(ISymbol symbol, string listName)
        {
			WatchListItem item = new WatchListItem(symbol, listName);
			if (!Items.Contains(item)) {
				Items.Add(item);
				return true;
			}
            return false;
        }

        public bool removeFromList(ISymbol symbol, string listName)
        {
			if (Items.Contains(new WatchListItem(symbol, listName)))
			{
				Items.RemoveAll(item => (item.SymbolName == symbol.ToString() && item.ListName == listName));
				return true;
			}
            return false;
        }

        public List<ISymbol> symbols
        {
            get
            {
                List<ISymbol> result = new List<ISymbol>();
                foreach (WatchListItem i in Items)
                {
                    result.Add(i.Symbol);
                }
                return result;
            }
        }
    }
}