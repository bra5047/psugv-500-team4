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

        List<WatchList> IWatchListManager.GetAllWatchLists()
        {
            TraderContext db = new TraderContext();
            var query = db.WatchLists;
            List<WatchList> result = new List<WatchList>();

            foreach (WatchList w in query)
            {
                result.Add(w);
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
