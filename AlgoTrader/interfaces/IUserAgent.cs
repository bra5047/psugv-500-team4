using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoTrader.Interfaces
{
    interface IUserAgent
    {
        void generateAlert(ISymbol symbol, tradeTypes type, int quantity);
        void processAlertResponse(string alertID, string alertResponse);
    }
}
