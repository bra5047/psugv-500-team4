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
		public void WatchListName()
		{
			WatchList w = new WatchList();
			Assert.AreEqual(w.ListName, "Default");
			w.ListName = "Test";
			Assert.AreEqual(w.ListName, "Test");
		}

		[Test]
		public void WatchListItems()
		{
			WatchList w = new WatchList();
			WatchListItem item = new WatchListItem();
			item.ListName = "Default";
			item.Symbol = new Symbol("GOOG");
			w.Items.Add(item);
			Assert.IsTrue(w.Items.Count == 1);
			Assert.AreEqual(w.Items[0], item);
		}

		[Test]
		public void RemoveFromList()
		{
			WatchList w = new WatchList();
			WatchListItem item = new WatchListItem(new Symbol("GOOG"), "Default");
			w.Items.Add(item);
			w.Items.Remove(item);
			Assert.IsTrue(w.Items.Count == 0);
		}
	}
}