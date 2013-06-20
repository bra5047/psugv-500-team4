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
	[ServiceContract]
	public interface IWatchListManager
	{
		[OperationContract]
		List<Quote> GetQuotes();

		[OperationContract]
		IWatchList GetWatchList(string listName);
	}
}
