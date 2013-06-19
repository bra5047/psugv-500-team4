using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			foreach (WatchListItem item in Items)
			{
				if (item.SymbolName == symbol.name && item.ListName == listName)
					return false;
			}

			Items.Add(new WatchListItem(symbol, listName));
			return true;
		}

        public bool removeFromList(ISymbol symbol, string listName)
        {
			if (Items.FindAll(x => (x.SymbolName == symbol.name && x.ListName == listName)).Count >= 1)
			{
				Items.RemoveAll(x => (x.SymbolName == symbol.name && x.ListName == listName));
				return true;
			}
			else
			{
				return false;
			}
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

		public WatchList(string listName)
		{
			ListName = "Default";
			Items = new List<WatchListItem>();
		}

		public WatchList()
		{

		}
    }
}