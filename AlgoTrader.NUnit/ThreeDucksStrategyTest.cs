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
    public class ThreeDucksStrategyTest
    {
        [Test]
        public void AddOneSymbol()
        {
            ThreeDucksStrategy tds = new ThreeDucksStrategy();
            Assert.IsTrue(tds.startWatching("GOOG"));
        }

        [Test]
        public void AddDuplicateSymbol()
        {
            ThreeDucksStrategy tds = new ThreeDucksStrategy();
            tds.startWatching("GOOG");
            Assert.IsFalse(tds.startWatching("GOOG"));
        }

        [Test]
        public void AddQuoteAndCheckSignal()
        {
            ThreeDucksStrategy tds = new ThreeDucksStrategy();
            tds.startWatching("GOOG");
            QuoteMessage q = new QuoteMessage();
            q.SymbolName = "GOOG";
            q.timestamp = DateTime.Now;
            q.Price = 10;
            tds.NewQuote(q);

            StrategySummary s = tds.getSummary("GOOG");
            Assert.AreEqual("GOOG", s.SymbolName);
            Assert.IsNotNull(s.AsOf);
            Assert.AreEqual(StrategySignal.None, s.CurrentSignal);            
        }

        [Test]
        public void CreateWithSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings.Add("FIRST_DUCK_SECONDS", "1");
            settings.Add("SECOND_DUCK_SECONDS", "1");
            settings.Add("THIRD_DUCK_SECONDS", "1");
            settings.Add("MOVING_AVERAGE_WINDOW", "1");

            ThreeDucksStrategy tds = new ThreeDucksStrategy(settings);
            Assert.AreEqual(1, tds.First_Duck_Seconds);
            Assert.AreEqual(1, tds.Second_Duck_Seconds);
            Assert.AreEqual(1, tds.Third_Duck_Seconds);
            Assert.AreEqual(1, tds.Moving_Average_Window);
        }

        [Test]
        public void TestForBuySignal()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings.Add("FIRST_DUCK_SECONDS", "1");
            settings.Add("SECOND_DUCK_SECONDS", "1");
            settings.Add("THIRD_DUCK_SECONDS", "1");
            settings.Add("MOVING_AVERAGE_WINDOW", "2");

            ThreeDucksStrategy tds = new ThreeDucksStrategy(settings);
            tds.startWatching("GOOG");

            QuoteMessage q1 = new QuoteMessage(10, new DateTime(2013, 06, 13, 0, 0, 1), "GOOG");
            QuoteMessage q2 = new QuoteMessage(20, new DateTime(2013, 06, 13, 0, 0, 2), "GOOG");

            tds.NewQuote(q1);
            tds.NewQuote(q2);

            StrategySummary result = tds.getSummary("GOOG");
            Assert.AreEqual(StrategySignal.Buy, result.CurrentSignal);
        }

        [Test]
        public void TestForSellSignal()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings.Add("FIRST_DUCK_SECONDS", "1");
            settings.Add("SECOND_DUCK_SECONDS", "1");
            settings.Add("THIRD_DUCK_SECONDS", "1");
            settings.Add("MOVING_AVERAGE_WINDOW", "2");

            ThreeDucksStrategy tds = new ThreeDucksStrategy(settings);
            tds.startWatching("GOOG");

            QuoteMessage q1 = new QuoteMessage(10, new DateTime(2013, 06, 13, 0, 0, 1), "GOOG");
            QuoteMessage q2 = new QuoteMessage(5, new DateTime(2013, 06, 13, 0, 0, 2), "GOOG");

            tds.NewQuote(q1);
            tds.NewQuote(q2);

            StrategySummary result = tds.getSummary("GOOG");
            Assert.AreEqual(StrategySignal.Sell, result.CurrentSignal);
        }
    }
}
