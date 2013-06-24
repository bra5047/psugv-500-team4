using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Interfaces
{
    public interface IPortfolio
    {
        int PortfolioId { get; set; }
        double Cash { get; set; }
        List<IPosition> Positions { get; }
        double Value { get; }
    }
}
