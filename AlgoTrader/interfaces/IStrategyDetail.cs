﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    public interface IStrategyDetail
    {
        ISymbol symbol { get; set; }
    }
}
