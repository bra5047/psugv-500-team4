using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace DbTestingApp
{
    class DbTesting
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing the entity framework...");
            TraderContext db = new TraderContext();

            
            Console.WriteLine("Clearing the database...");
            var d_quotes = from q in db.Quotes select q;
            foreach (var i in d_quotes) db.Quotes.Remove(i);
            var d_symbol = from s in db.Symbols select s;
            foreach (var i in d_symbol) db.Symbols.Remove(i);
            var d_position = from p in db.Positions select p;
            foreach (var i in d_position) db.Positions.Remove(i);
            var d_trade = from t in db.Trades select t;
            foreach (var i in d_trade) db.Trades.Remove(i);


            Console.WriteLine("Adding records to database...");
            db.Symbols.Add(new Symbol("GOOG"));
            db.Symbols.Add(new Symbol("LNC"));

            db.SaveChanges();

            Quote q1 = new Quote();
            q1.timestamp = DateTime.Now;
            q1.price = 50.47;
            q1.Symbol = db.Symbols.First();
            db.Quotes.Add(q1);

            Position p1 = new Position();
            p1.SymbolName = "GOOG";
            p1.price = 5000;
            p1.quantity = 150;
            p1.status = positionStatus.Open;
            db.Positions.Add(p1);

            db.SaveChanges();

            Trade t1 = new Trade();
            t1.timestamp = DateTime.Now;
            t1.quantity = 40;
            t1.price = 10.10;
            t1.type = tradeTypes.Buy;
            t1.SymbolName = "GOOG";
            db.Trades.Add(t1);


            var q_addtopos = from p in db.Positions.Include("Trades") where p.SymbolName == "GOOG" select p;
            Position goog_pos = q_addtopos.FirstOrDefault();
            goog_pos.Trades.Add(t1);

            db.SaveChanges();
            
            Console.WriteLine("Retrieving records from database...");
            Console.WriteLine("\nSymbols");
            var query = from s in db.Symbols orderby s.name select s;
            foreach (var i in query)
            {
                Console.WriteLine("Symbol: " + i.name);
            }

            Console.WriteLine("\nQuotes");
            var q_search = from q in db.Quotes select q;
            foreach (var i in q_search)
            {
                Console.WriteLine("Record: " + i.QuoteId.ToString());
                Console.WriteLine("Symbol: " + i.Symbol.name);
                Console.WriteLine("Timestamp: " + i.timestamp.ToString());
                Console.WriteLine("Price: " + i.price.ToString());
            }

            Console.WriteLine("\nPositions");
            var p_search = from p in db.Positions select p;
            foreach (var i in p_search)
            {
                Console.WriteLine("Record: " + i.PositionId.ToString());
                Console.WriteLine("Symbol: " + i.Symbol.name);
                Console.WriteLine("Trade count: " + i.Trades.Count.ToString());
                foreach (var t in i.Trades)
                {
                    Console.WriteLine("\tTrade Id: " + t.TradeId.ToString());
                    Console.WriteLine("\tType: " + t.type.ToString());
                    Console.WriteLine("\tPrice: " + t.price.ToString());
                    Console.WriteLine("\tQuantity: " + t.quantity.ToString());
                    Console.WriteLine("\tTimestamp: " + t.timestamp.ToString());
                }
            }

            Console.WriteLine("\nTesting complete.");
            Console.ReadLine();
            db.Dispose();
        }
    }
}
