using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;

namespace AlgoTrader.Interfaces
{
    interface IEmailLog
    {
        string UserEmail { get; set; }
        int quantity { get; set; }
        string Symbol { get; set; }
        DateTime TimeStamp { get; set; }
        tradeTypes TradeType { get; set; }
        string Approved { get; set; }
    }
}
