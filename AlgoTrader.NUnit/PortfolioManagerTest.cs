using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using AlgoTrader.portfolio;
using NUnit.Framework;

namespace AlgoTrader.NUnit
{
    [TestFixture]
    public class PortfolioManagerTest
    {
        [Test]
        public void CreateWithSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings.Add("MAX_POSITION_RATIO", "0.9");

            PortfolioManager pm = new PortfolioManager(settings);
            PortfolioRule r = pm.Rules.FirstOrDefault<PortfolioRule>(x => typeof(AllocationRule).IsInstanceOfType(x));
            Assert.IsNotNull(r);
            AllocationRule a = (AllocationRule)r;
            Assert.AreEqual(0.9, a.MaxAllocation);
        }

        [Test]
        public void LoadSettingsFromDb()
        {
            PortfolioManager pm = new PortfolioManager();
            pm.LoadSettings();
            PortfolioRule r = pm.Rules.FirstOrDefault<PortfolioRule>(x => typeof(AllocationRule).IsInstanceOfType(x));
            Assert.IsNotNull(r);
            AllocationRule a = (AllocationRule)r;
            Assert.AreEqual(0.9, a.MaxAllocation);
        }

        [Test]
        public void PlaceABuyTrade()
        {
            Portfolio port = new Portfolio();
            port.Cash = 1000;
            Position pos = new Position();
            pos.PositionId = 3;
            pos.quantity = 0;
            pos.price = 0;
            Trade t = new Trade();
            PortfolioManager pm = new PortfolioManager();  // this test might need to initialize the rules
            pm.ProcessBuyTrade(t, "GOOG", 10, 2.5, pos, port);
            Assert.AreEqual("GOOG", t.SymbolName);
            Assert.AreEqual(10, t.quantity);
            Assert.AreEqual(2.5, t.price);
            Assert.AreEqual(tradeTypes.Buy, t.type);
            Assert.AreEqual(3, t.PositionId);
            Assert.AreEqual(10, pos.quantity);
            Assert.AreEqual(25, pos.price);
            Assert.AreEqual(975, port.Cash);
        }
    }
}
