using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.StockService;
using System.Xml;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.qoute
{
    public class QuoteManager : IQuoteManager 
    {
        public bool startWatching(string SymbolName)
        {
            StockQuote SQ = new StockQuote();
            XmlDocument Doc = new XmlDocument();
            string Check;
            XmlNode XmlCheck;

            string QouteUpdate = SQ.GetQuote(SymbolName);
            Doc.LoadXml(QouteUpdate);

            //Check the stock to see if it has ever had an opening price
            XmlCheck = Doc.DocumentElement.SelectSingleNode("//Stock/Open");
            Check = XmlCheck.InnerText;

            if (Check == "N/A")
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
