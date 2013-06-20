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

		List<ISymbol> IWatchList.symbols
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

		List<WatchListItem> IWatchList.items
		{
			get { return Items; }
		}

		bool IWatchList.AddToList(ISymbol symbol, string listName)
		{
			foreach (WatchListItem item in Items)
			{
				if (item.SymbolName == symbol.name && item.ListName == listName)
					return false;
			}

			Items.Add(new WatchListItem(symbol, listName));
			return true;
		}

		bool IWatchList.RemoveFromList(ISymbol symbol, string listName)
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

		public WatchList(string listName)
		{
			Items = new List<WatchListItem>();
			ListName = listName;
		}

		public WatchList()
		{
			Items = new List<WatchListItem>();
			ListName = "Default";
		}
    }
}