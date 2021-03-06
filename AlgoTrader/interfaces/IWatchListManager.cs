﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using AlgoTrader.datamodel;
using AlgoTrader.watchlist;

namespace AlgoTrader.Interfaces
{
	public interface IWatchListManager
	{
		List<Quote> GetQuotes(string symbolName);
		string GetLongName(string SymbolName);
		IWatchList GetWatchList(string listName);
		bool AddWatchList(string listName);
		bool DeleteWatchList(string listName);

        List<WatchList> GetAllWatchLists();
	}
}
