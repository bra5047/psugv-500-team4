using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;

namespace AlgoTrader.portfolio
{
    public interface PortfolioRule
    {
        bool Apply(IPortfolio p, ITrade t);
    }
}
