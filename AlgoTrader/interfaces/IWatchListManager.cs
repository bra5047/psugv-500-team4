using System;
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
		List<Quote> GetQuotes();

		IWatchList GetWatchList(string listName);
	}
}
