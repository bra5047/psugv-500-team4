using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.qoute
{
    class QuoteManager:IQuoteManager 
    {
        public bool startWatching(string SymbolName)
        {
            TraderContext db = new TraderContext();
            var ReturnValue = (from x in db.Symbols select x.name).ToList();
            if (ReturnValue.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
