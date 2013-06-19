using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.watchlist
{
	public class WatchListManager
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

		public WatchList GetWatchList(string listName){
			// pull from db and add to w.Items eventually
			//TraderContext db = new TraderContext();
			//WatchList w = db.WatchLists.FirstOrDefault();
			//var query = w.Items.Where(a => a.ListName.Equals(listName)).OrderBy(a => a.SymbolName).ToList();
			//List<WatchListItem> result = new List<WatchListItem>();
			//foreach (WatchListItem q in query)
			//{
			//	result.Add(q);
			//}

			WatchList result = new WatchList();
			result.addToList(new Symbol("GOOG"), listName);
			result.addToList(new Symbol("AAPL"), listName);
			result.addToList(new Symbol("VZ"), listName);
			result.addToList(new Symbol("INTC"), listName);
			result.addToList(new Symbol("MSFT"), listName);
			result.addToList(new Symbol("HP"), listName);
			result.addToList(new Symbol("AMD"), listName);
			result.addToList(new Symbol("NVDA"), listName);
			result.addToList(new Symbol("QCOM"), listName);
			result.addToList(new Symbol("PANL"), listName);

			return result;
		}

		public List<Quote> GetQuotes()
		{
			//TraderContext db = new TraderContext();
			//List<Quote> result = db.Quotes.Where(a => a.SymbolName.Equals(symbol.name)).ToList();

			List<Quote> result = new List<Quote>();
			
			Quote q1 = new Quote();
			q1.price = 890.22;
			q1.timestamp = new DateTime(2013, 06, 18);
			q1.SymbolName = "GOOG";
			result.Add(q1);

			Quote q2 = new Quote();
			q2.price = 760.47;
			q2.timestamp = new DateTime(2013, 06, 17);
			q2.SymbolName = "GOOG";
			result.Add(q2);

			Quote q3 = new Quote();
			q3.price = 438.89;
			q3.timestamp = new DateTime(2013, 06, 18);
			q3.SymbolName = "AAPL";
			result.Add(q3);

			Quote q4 = new Quote();
			q4.price = 441.03;
			q4.timestamp = new DateTime(2013, 06, 17);
			q4.SymbolName = "AAPL";

			result.Add(q4);

			Quote q5 = new Quote();
			q5.price = 50.53;
			q5.timestamp = new DateTime(2013, 06, 18);
			q5.SymbolName = "VZ";
			result.Add(q5);

			Quote q6 = new Quote();
			q6.price = 49.14;
			q6.timestamp = new DateTime(2013, 06, 17);
			q6.SymbolName = "VZ";
			result.Add(q6);

			Quote q7 = new Quote();
			q7.price = 25.01;
			q7.timestamp = new DateTime(2013, 06, 18);
			q7.SymbolName = "INTC";
			result.Add(q7);

			Quote q8 = new Quote();
			q8.price = 24.53;
			q8.timestamp = new DateTime(2013, 06, 17);
			q8.SymbolName = "INTC";
			result.Add(q8);

			Quote q9 = new Quote();
			q9.price = 35.47;
			q9.timestamp = new DateTime(2013, 06, 18);
			q9.SymbolName = "MSFT";
			result.Add(q9);

			Quote q10 = new Quote();
			q10.price = 35.67;
			q10.timestamp = new DateTime(2013, 06, 17);
			q10.SymbolName = "MSFT";
			result.Add(q10);

			Quote q11 = new Quote();
			q11.price = 20.12;
			q11.timestamp = new DateTime(2013, 06, 18);
			q11.SymbolName = "HP";
			result.Add(q11);

			Quote q12 = new Quote();
			q12.price = 22.43;
			q12.timestamp = new DateTime(2013, 06, 17);
			q12.SymbolName = "HP";
			result.Add(q12);

			Quote q13 = new Quote();
			q13.price = 25.10;
			q13.timestamp = new DateTime(2013, 06, 18);
			q13.SymbolName = "NVDA";
			result.Add(q13);

			Quote q14 = new Quote();
			q14.price = 25.10;
			q14.timestamp = new DateTime(2013, 06, 14);
			q14.SymbolName = "NVDA";
			result.Add(q14);

			Quote q15 = new Quote();
			q15.price = 130.40;
			q15.timestamp = new DateTime(2013, 06, 18);
			q15.SymbolName = "QCOM";
			result.Add(q15);

			Quote q16 = new Quote();
			q16.price = 118.27;
			q16.timestamp = new DateTime(2013, 06, 16);
			q16.SymbolName = "QCOM";
			result.Add(q16);

			Quote q17 = new Quote();
			q17.price = 83.61;
			q17.timestamp = new DateTime(2013, 06, 18);
			q17.SymbolName = "PANL";
			result.Add(q17);

			Quote q18 = new Quote();
			q18.price = 90.43;
			q18.timestamp = new DateTime(2013, 06, 16);
			q18.SymbolName = "PANL";
			result.Add(q18);

			Quote q19 = new Quote();
			q19.price = 4.14;
			q19.timestamp = new DateTime(2013, 06, 18);
			q19.SymbolName = "AMD";
			result.Add(q19);

			Quote q20 = new Quote();
			q20.price = 4.08;
			q20.timestamp = new DateTime(2013, 06, 17);
			q20.SymbolName = "AMD";
			result.Add(q20);

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
