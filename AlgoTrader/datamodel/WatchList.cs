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
			set { Items = value; }
		}

		bool IWatchList.AddToList(ISymbol symbol, string listName)
		{
			TraderContext db = new TraderContext();
			var SYMBOL_QUERY = db.Symbols.Where(x => x.name.Equals(symbol.name)).ToList();
			var WLI_QUERY = db.WatchListItems.Where(x => (x.SymbolName.Equals(symbol.name) && x.ListName.Equals(listName))).ToList();

			if (WLI_QUERY.Count>=1)
			{
				return false;
			}
			else
			{
				if (SYMBOL_QUERY.Count == 0)
				{
					Symbol s = new Symbol(symbol.name);
					db.Symbols.Add(s);
					db.SaveChanges();
				}

				db.WatchListItems.Add(new WatchListItem(symbol, listName));
				db.SaveChanges();
				return true;
			}
		}

		bool IWatchList.RemoveFromList(ISymbol symbol, string listName)
		{
			TraderContext db = new TraderContext();
			var query = db.WatchListItems.Where(x => (x.SymbolName.Equals(symbol.name) && x.ListName.Equals(ListName))).ToList();

			if (query.Count >= 1)
			{
				foreach (WatchListItem item in query)
				{
					db.WatchListItems.Remove(item);
					db.SaveChanges();
				}

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