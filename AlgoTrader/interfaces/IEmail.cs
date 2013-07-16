using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.datamodel;

namespace AlgoTrader.Interfaces
{
    public interface IEmail
    {
        void sendEmail(string Recipient, string SymbolName, string CurrentPrice, tradeTypes TradeType, int Quantity);

    }
}
