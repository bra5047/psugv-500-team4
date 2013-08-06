using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.qoute
{
    public class QuoteManager : IQuoteManager 
    {
        public bool startWatching(string SymbolName)
        {
            //Check DB to see if the Symbol is being watched already or not
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
