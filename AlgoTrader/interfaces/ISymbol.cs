using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    public interface ISymbol
    {
        string name { get; set; }
        string CompanyName { get; set; }
    }
}
