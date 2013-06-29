using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.watchlist
{
	public class WatchListManager : IWatchListManager
	{
		private TraderContext _dbContext;

		protected TraderContext DbContext
		{
			get
			{
				if (_dbContext != null)
				{
					return _dbContext;
				}
				else
				{
					return new TraderContext();
				}
			}
		}

		IWatchList IWatchListManager.GetWatchList(string listName)
		{
			TraderContext db = new TraderContext();
            var query = db.WatchListItems.Where(a => a.ListName.Equals(listName)).OrderBy(a => a.SymbolName).ToList();
            IWatchList result = new WatchList();
            
			foreach (WatchListItem q in query)
			{
				result.AddToList(q.Symbol, q.ListName);
			}

			return result;
		}

		List<Quote> IWatchListManager.GetQuotes(string symbolName)
		{
			TraderContext db = new TraderContext();
            var query = db.Quotes.Where(a => a.SymbolName.Equals(symbolName));
            List<Quote> result = new List<Quote>();

            foreach (Quote q in query)
            {
                result.Add(q);
            }

			return result;
		}

		public WatchListManager()
		{
			//_dbContext = null;
		}

		public WatchListManager(TraderContext db)
		{
			_dbContext = db;
		}
	}
}
