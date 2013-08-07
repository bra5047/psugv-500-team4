using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTrader.qoute;

namespace AlgoTrader.NUnit
{
	[TestFixture]
	class QuoteTest
	{
		[Test]
		public void IQuoteProperties()
		{
			IQuote q = new Quote();
			q.price = 1.25;
			Assert.AreEqual(q.price, 1.25);
			q.timestamp = new DateTime(2013, 1, 1);
			Assert.AreEqual(q.timestamp, new DateTime(2013, 1, 1));
		}

        [Test]
        public void GuoteManagerCheckFail()
        {
            IQuoteManager q = new QuoteManager();
            Assert.IsFalse(q.startWatching("asdf"));
            Assert.IsTrue(q.startWatching("GOOG"));
        }

		[Test]
		public void QuoteProperties()
		{
			Quote q = new Quote();
			q.QuoteId = 1;
			Assert.AreEqual(q.QuoteId, 1);
			q.price = 1.25;
			Assert.AreEqual(q.price, 1.25);
			q.SymbolName = "GOOG";
			Assert.AreEqual(q.SymbolName, "GOOG");
			q.timestamp = new DateTime(2013, 1, 1);
			Assert.AreEqual(q.timestamp, new DateTime(2013, 1, 1));
		}
	}
}
