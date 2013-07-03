using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using AlgoTrader.watchlist;

namespace AlgoTrader.NUnit
{
	[TestFixture]
	class WatchListTests
	{
		[Test]
		public void AddToList()
		{
			IWatchList w = new WatchList();
			w.AddToList(new Symbol("GOOG"), "Default");
			w.AddToList(new Symbol("GOOG"), "Other");
			w.AddToList(new Symbol("MSFT"), "Default");
			w.AddToList(new Symbol("MSFT"), "Default");
			Assert.AreEqual(w.items[0].SymbolName, "GOOG");
			Assert.AreEqual(w.items[1].SymbolName, "GOOG");
			Assert.AreEqual(w.items[2].SymbolName, "MSFT");
			Assert.IsTrue(w.items.Count == 3);
		}

		[Test]
		public void RemoveFromList()
		{

		}
	}
}