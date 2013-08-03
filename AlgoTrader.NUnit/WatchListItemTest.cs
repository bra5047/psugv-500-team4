using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AlgoTrader.datamodel;

namespace AlgoTrader
{
	[TestFixture]
	class WatchListItemTest
	{
		[Test]
		public void WatchListItemNoParams()
		{
			WatchListItem wli = new WatchListItem();
			Assert.IsTrue(wli.Symbol == null);
			Assert.IsTrue(wli.ListName == null);
			Assert.IsTrue(wli.SymbolName == null);
			Assert.IsTrue(wli.WatchList == null);
		}

		[Test]
		public void WatchListItemParams()
		{
			Symbol s = new Symbol("GOOG");
			WatchListItem wli = new WatchListItem(s, "Default");
			Assert.AreEqual(wli.ListName, "Default");
			Assert.AreEqual(wli.SymbolName, "GOOG");
		}
	}
}
