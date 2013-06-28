using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using NUnit.Framework;

namespace AlgoTrader.NUnit
{
    [TestFixture]
    public class TradeTests
    {
        [Test]
        public void TestSell()
        {
            Symbol s = new Symbol("GOOG");
            Position pos = new Position();
            Trade t = new Trade();
            t.Status = tradeStatus.Active;
            t.type = tradeTypes.Buy;
            t.Position = pos;
            t.Symbol = s;
            t.timestamp = new DateTime(2013, 6, 1);
            t.price = 10;
            t.quantity = 5;

            Trade sell = t.sell(2);
            Assert.AreEqual(s, sell.Symbol);
            Assert.AreEqual(pos, sell.Position);
            Assert.AreEqual(tradeStatus.Closed, sell.Status);
            Assert.AreEqual(tradeTypes.Sell, sell.type);
            Assert.AreEqual(2, sell.quantity);
            Assert.AreNotEqual(t.timestamp, sell.timestamp);
            Assert.AreEqual(t, sell.RelatedTrade);
        }

        [Test]
        public void TestNegativeSellQuantity()
        {
            Trade t = new Trade();
            t.Status = tradeStatus.Active;
            t.type = tradeTypes.Buy;
            t.quantity = 5;
            Assert.Throws<ArgumentException>(() => t.sell(-2));
        }

        [Test]
        public void TestSellQuantityTooLarge()
        {
            Trade t = new Trade();
            t.Status = tradeStatus.Active;
            t.type = tradeTypes.Buy;
            t.quantity = 5;
            Assert.Throws<ArgumentException>(() => t.sell(10));
        }
    }
}
