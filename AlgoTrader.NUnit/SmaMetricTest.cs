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
    [TestFixture]
    public class SmaMetricTest
    {
        [Test]
        public void ComputeAverage()
        {
            SmaMetric sma = new SmaMetric("GOOG", 5, 5);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 0), 10);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 5), 20);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 10), 10);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 15), 20);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 20), 10);

            Assert.AreEqual(14, sma.Avg);
        }

        [Test]
        public void AverageWithNoQuotes()
        {
            SmaMetric sma = new SmaMetric("GOOG", 5, 5);
            Assert.AreEqual(0, sma.Avg);
        }

        [Test]
        public void TestIntervals()
        {
            SmaMetric sma = new SmaMetric("GOOG", 5, 5);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 0), 10);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 2), 1000);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 5), 10);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 7), 1000);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 10), 10);

            Assert.AreEqual(10, sma.Avg);
        }

        [Test]
        public void TestWindowSize()
        {
            SmaMetric sma = new SmaMetric("GOOG", 4, 5);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 0), 1000);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 5), 10);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 10), 10);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 15), 10);
            sma.Add(new DateTime(2013, 6, 13, 5, 0, 20), 10);

            Assert.AreEqual(10, sma.Avg);
        }
    }

}
