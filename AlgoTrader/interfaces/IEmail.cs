using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.interfaces
{
    interface IEmail
    {
        void sendEmail(string Recipient, string SymbolName);

    }
}
