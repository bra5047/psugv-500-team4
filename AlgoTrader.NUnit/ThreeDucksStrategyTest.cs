﻿using System;
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
            q.price = 10;
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
    }
}
