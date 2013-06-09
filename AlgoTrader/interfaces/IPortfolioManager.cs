using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    interface IPortfolioManager
    {
        List<IPosition> positions { get; }

        void sell(ISymbol symbol, int quantity);
        void buy(ISymbol symbol, int quantity);
        void getAvailableCash();
    }
}
