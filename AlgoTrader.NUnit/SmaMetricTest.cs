using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AlgoTrader.Interfaces;
using AlgoTrader.strategy;
using AlgoTrader.datamodel;

namespace AlgoTrader.NUnit
{
    class SymbolMock : ISymbol
    {
        public string name { get; set; }
        public SymbolMock(string _name) { name = _name; }
    }

    class QuoteMock : IQuote
    {
        public ISymbol symbol { get; set; }
        public double price { get; set; }
        public DateTime timestamp { get; set; }
        public QuoteMock(ISymbol _symbol, double _price, DateTime _timestamp)
        {
            symbol = _symbol;
            price = _price;
            timestamp = _timestamp;
        }
    }

    [TestFixture]
    public class SmaMetricTest
    {
        [Test]
        public void ComputeAverage()
        {
            ISymbol goog = new SymbolMock("GOOG");
            Queue<IQuote> data = new Queue<IQuote>(5);

            data.Enqueue(new QuoteMock(goog, 10, new DateTime(2013, 6, 13, 5, 0, 0)));
            data.Enqueue(new QuoteMock(goog, 10, new DateTime(2013, 6, 13, 5, 0, 5)));
            data.Enqueue(new QuoteMock(goog, 10, new DateTime(2013, 6, 13, 5, 0, 10)));
            data.Enqueue(new QuoteMock(goog, 10, new DateTime(2013, 6, 13, 5, 0, 15)));
            data.Enqueue(new QuoteMock(goog, 10, new DateTime(2013, 6, 13, 5, 0, 20)));

            SmaMetric sma = new SmaMetric(goog, 5, 5);
            foreach (IQuote q in data)
            {
                sma.Add(q);
            }
            Assert.AreEqual(10, sma.Avg);
        }
    }

}
