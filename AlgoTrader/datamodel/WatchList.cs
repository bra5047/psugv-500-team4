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
			string sname = symbol.name;
			string lname = listName;

			if (sname.Length > 0 && lname.Length > 0)
			{
				TraderContext db = new TraderContext();
				// checks if watchlist item exists
				var WLI_QUERY = db.WatchListItems.Where(x => (x.SymbolName.Equals(sname) && x.ListName.Equals(lname))).ToList();
				if (WLI_QUERY.Count >= 1)
				{
					return false;
				}
				else // if not, checks if symbol exists
				{
					var SYMBOL_QUERY = db.Symbols.Where(x => x.name.Equals(sname)).ToList();
					if (SYMBOL_QUERY.Count == 0)
					{
						Symbol s = new Symbol(sname);
						db.Symbols.Add(s);
						db.SaveChanges();
					}
					db.WatchListItems.Add(new WatchListItem(symbol, listName));
					db.SaveChanges();
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		bool IWatchList.RemoveFromList(ISymbol symbol, string listName)
		{
			TraderContext db = new TraderContext();
			var query = db.WatchListItems.Where(x => (x.SymbolName.Equals(symbol.name) && x.ListName.Equals(listName)));
			if (query.Count() > 0)
			{
				foreach (WatchListItem item in query)
				{
					db.WatchListItems.Remove(item);
				}
				db.SaveChanges();
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

			if (listName.Length > 0)
			{
				ListName = listName;
			}
			else
			{
				ListName = "Default";
			}
		}

		public WatchList()
		{
			Items = new List<WatchListItem>();
			ListName = "Default";
		}
	}
}