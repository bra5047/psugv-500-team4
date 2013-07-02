using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.strategy;
using NUnit.Framework;

namespace AlgoTrader.NUnit
{
    [TestFixture]
    public class ParabolicSarMetricTests
    {
        [Test]
        public void CreateMetric()
        {
            ParabolicSarMetric m = new ParabolicSarMetric(1, 10.25, 12.15);
            Assert.AreEqual(0.02, m.AccelerationFactor);
            Assert.AreEqual(0.02, m.StepIncrement);
            Assert.AreEqual(0.2, m.MaximumStep);
            Assert.AreEqual(1, m.Period);
            Assert.AreEqual(10.25, m.SAR);
        }

        [Test]
        public void SarAbovePreviousLow()
        {
            ParabolicSarMetric m = new ParabolicSarMetric(1, 10.25, 12.15);
            m.Add(DateTime.Now.AddSeconds(5), 10.50);
            Assert.AreEqual(10.25, m.SAR);
        }

        [Test]
        public void NewExtremePoint()
        {
            ParabolicSarMetric m = new ParabolicSarMetric(1, 10.25, 12.15);
            m.Add(DateTime.Now.AddSeconds(5), 15.25);
            Assert.AreEqual(0.04, m.AccelerationFactor);
        }

        [Test]
        public void ChangeInSar()
        {
            ParabolicSarMetric m = new ParabolicSarMetric(5, 10.25, 12.15);
            DateTime time = DateTime.Now;
            m.Add(time.AddSeconds(1), 11.15);
            m.Add(time.AddSeconds(2), 11.15);
            m.Add(time.AddSeconds(6), 12.25);
            m.Add(time.AddSeconds(7), 12.25);
            m.Add(time.AddSeconds(11), 12.50);
            Assert.AreEqual(10.385, m.SAR);
            Assert.AreEqual(0.06, m.AccelerationFactor);
        }
    }
}
