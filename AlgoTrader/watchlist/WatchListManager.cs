﻿using System;
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
			var query = db.Quotes.Where(x => x.SymbolName.Equals(symbolName));
			List<Quote> result = new List<Quote>();

			foreach (Quote q in query)
			{
				result.Add(q);
			}

			return result;
		}

		string IWatchListManager.GetLongName(string symbolName)
		{
			TraderContext db = new TraderContext();
			string longname = db.Symbols.Where(x => x.name.Equals(symbolName)).Select(x => x.CompanyName).FirstOrDefault();
			return longname;
		}

		IWatchList IWatchListManager.GetWatchList(string listName)
		{
			TraderContext db = new TraderContext();
			var query = db.WatchListItems.Where(x => x.ListName.Equals(listName));
			IWatchList result = new WatchList();

			foreach (WatchListItem q in query)
			{
				result.items.Add(q);
			}

			return result;
		}

		List<WatchList> IWatchListManager.GetAllWatchLists()
		{
			TraderContext db = new TraderContext();
			var query = db.WatchLists.OrderBy(x => x.ListName);
			List<WatchList> result = new List<WatchList>();

			foreach (WatchList w in query)
			{
				result.Add(w);
			}

			return result;
		}

		bool IWatchListManager.AddWatchList(string listName)
		{
			TraderContext db = new TraderContext();
			string l = listName;
			if (listName.Length == 0)
			{
				l = "Default";
			}
			var query = db.WatchLists.Where(x => x.ListName.Equals(l));
			if (query.Count() == 0)
			{
				WatchList w = new WatchList(l);
				db.WatchLists.Add(w);
				db.SaveChanges();
				return true;
			}

			return false;
		}

		bool IWatchListManager.DeleteWatchList(string listName)
		{
			TraderContext db = new TraderContext();
			string l = listName;
			var query = db.WatchLists.Where(x => x.ListName.Equals(l)).ToList();

			if (query.Count > 0)
			{
				foreach (WatchList w in query)
				{
					db.WatchLists.Remove(w);
				}
				db.SaveChanges();
				return true;
			}
			return false;
		}

		public WatchListManager()
		{
			_dbContext = null;
		}

		public WatchListManager(TraderContext db)
		{
			_dbContext = db;
		}
	}
}
