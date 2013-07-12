using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using Qt = AlgoTrader.StockService;
using System.Xml;

namespace AlgoTrader.quote
{
    class QuoteAsync
    {
        public Quote getQoute(Symbol symbol)
        {
            //the pupose of this is to get an up to minute stock update without having to do a sql query with a where clause looking for the current time
            //first time will take a second to run after that it will speed up
            Quote StockQt = new Quote();
            Qt.StockQuote SQ = new Qt.StockQuote();
            XmlDocument doc = new XmlDocument();

            string UpdatedQoute = SQ.GetQuote(symbol.ToString());
            doc.LoadXml(UpdatedQoute);
            StockQt.price = Convert.ToDouble(doc.DocumentElement.SelectSingleNode("//Stock/Last").InnerText);
            StockQt.SymbolName = doc.DocumentElement.SelectSingleNode("//Stock/Name").InnerText;
            StockQt.timestamp = Convert.ToDateTime(doc.DocumentElement.SelectSingleNode("//Stock/Date").InnerText + " " + doc.DocumentElement.SelectSingleNode("//Stock/Time").InnerText);
            StockQt.Symbol = symbol; 
            return StockQt;
        }
    }
}
