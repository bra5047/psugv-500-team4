﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;

namespace AlgoTrader.portfolio
{
    public class AllocationRule : PortfolioRule
    {
        private double _maxAllocation;

        public double MaxAllocation
        {
            get { return _maxAllocation; }
            set
            {
                if (value <= 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _maxAllocation = value;
            }
        }

        public bool Apply(IPortfolio p, ITrade t)
        {
            IPosition pos = p.Positions.Where(x => x.symbol.name == t.symbol.name).FirstOrDefault();
            if (pos == null) return false;
            if ((t.value + pos.price) / p.Value > MaxAllocation)
            {
                throw new AllocationViolation();
            }
            return true;
        }

        public AllocationRule()
        {
            MaxAllocation = 0.25;
        }

        public AllocationRule(double max)
        {
            MaxAllocation = max;
        }
    }

    public class AllocationViolation : Exception
    {
        public AllocationViolation() : base("The transaction would violate a portfolio allocation rule.") { }
    }
}
