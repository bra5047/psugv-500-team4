using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;

namespace AlgoTrader.NUnit
{
	[TestFixture]
	class PositionTest
	{
		[Test]
		public void IPositionOpen()
		{
			IPosition pos = new Position();
			pos.status = positionStatus.Open;
			Assert.AreEqual(pos.status, positionStatus.Open);
		}

		[Test]
		public void IPositionClosed()
		{
			IPosition pos = new Position();
			pos.status = positionStatus.Closed;
			Assert.AreEqual(pos.status, positionStatus.Closed);
		}

		[Test]
		public void IPositionProperties()
		{
			IPosition pos = new Position();
			pos.price = 1.25;
			pos.quantity = 5;
			Assert.That(pos.price == 1.25);
			Assert.That(pos.quantity == 5);
		}
	}
}
