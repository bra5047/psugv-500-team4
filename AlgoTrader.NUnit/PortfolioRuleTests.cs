using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using AlgoTrader.portfolio;
using NUnit.Framework;

namespace AlgoTrader.NUnit
{
    [TestFixture]
    public class PortfolioRuleTests
    {
        [Test]
        public void EnoughFundsRuleYes()
        {
            Portfolio p = new Portfolio();
            p.Cash = 5000;

            Trade t = new Trade();
            t.type = tradeTypes.Buy;
            t.quantity = 2;
            t.price = 10.25;

            EnoughFundsRule r = new EnoughFundsRule();
            Assert.IsTrue(r.Apply(p, t));
        }

        [Test]
        public void EnoughFundsRuleNo()
        {
            Portfolio p = new Portfolio();
            p.Cash = 1;

            Trade t = new Trade();
            t.type = tradeTypes.Buy;
            t.quantity = 2;
            t.price = 10.25;

            EnoughFundsRule r = new EnoughFundsRule();
            Assert.Throws<InsufficientFunds>(() => r.Apply(p, t));
        }

        [Test]
        public void AllocationRuleYes()
        {
            Portfolio p = new Portfolio();

            Position pos1 = new Position();
            pos1.status = positionStatus.Open;
            pos1.Symbol = new Symbol("SYM1");
            pos1.price = 10;
            pos1.quantity = 1;

            Position pos2 = new Position();
            pos2.status = positionStatus.Open;
            pos1.Symbol = new Symbol("SYM2");
            pos2.price = 10;
            pos2.quantity = 1;

            p.Positions = new List<Position>();
            p.Positions.Add(pos1);
            p.Positions.Add(pos2);

            Trade t = new Trade();
            t.type = tradeTypes.Buy;
            t.Symbol = new Symbol("SYM2");
            t.price = 1;
            t.quantity = 2;

            AllocationRule r = new AllocationRule(0.6);
            Assert.IsTrue(r.Apply(p, t));
        }

        [Test]
        public void AllocationRuleNo()
        {
            Portfolio p = new Portfolio();

            Position pos1 = new Position();
            pos1.status = positionStatus.Open;
            pos1.Symbol = new Symbol("SYM1");
            pos1.price = 10;
            pos1.quantity = 1;

            Position pos2 = new Position();
            pos2.status = positionStatus.Open;
            pos1.Symbol = new Symbol("SYM2");
            pos2.price = 10;
            pos2.quantity = 1;

            p.Positions = new List<Position>();
            p.Positions.Add(pos1);
            p.Positions.Add(pos2);

            Trade t = new Trade();
            t.type = tradeTypes.Buy;
            t.Symbol = new Symbol("SYM2");
            t.price = 1;
            t.quantity = 2;

            AllocationRule r = new AllocationRule(0.5);
            Assert.Throws<AllocationViolation>(() => r.Apply(p, t));
        }
    }
}
