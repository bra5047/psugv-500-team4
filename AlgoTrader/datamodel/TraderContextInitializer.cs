using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    class TraderContextInitializer : DropCreateDatabaseAlways<TraderContext>
    {
        protected override void Seed(TraderContext context)
        {
            Portfolio portfolio = new Portfolio();
            portfolio.Cash = 10000;
            context.Portfolios.Add(portfolio);
            context.SaveChanges();

            Symbol s = new Symbol("GOOG");
            context.Symbols.Add(s);
            context.SaveChanges();

            Position pos1 = new Position();
            pos1.price = 100;
            pos1.quantity = 5;
            pos1.status = positionStatus.Open;
            pos1.Symbol = s;
            pos1.Portfolio = portfolio;
            context.Positions.Add(pos1);
            context.SaveChanges();

            Trade t1 = new Trade();
            t1.Symbol = s;
            t1.Position = pos1;
            t1.price = 20;
            t1.quantity = 5;
            t1.type = tradeTypes.Buy;
            t1.TransactionId = Guid.NewGuid().ToString();
            t1.timestamp = DateTime.Now;
            context.Trades.Add(t1);
            context.SaveChanges();

            Quote q1 = new Quote();
            q1.Symbol = s;
            q1.timestamp = DateTime.Now;
            q1.price = 20.15;
            context.Quotes.Add(q1);
            context.SaveChanges();

            Alert a1 = new Alert();
            a1.AlertId = Guid.NewGuid();
            a1.Symbol = s;
            a1.Timestamp = DateTime.Now;
            a1.Type = tradeTypes.Buy;
            a1.Quantity = 100;
            a1.SentTo = "bra5047@psu.edu";
            a1.Price = 10.45;
            a1.ResponseCode = responseCodes.Pending;
            context.Alerts.Add(a1);
            context.SaveChanges();

            // Adam's stuff to fill in db tables
            Symbol s1 = new Symbol("AAPL");
            Symbol s2 = new Symbol("VZ");
            Symbol s3 = new Symbol("INTC");
            Symbol s4 = new Symbol("MSFT");
            Symbol s5 = new Symbol("HP");
            Symbol s6 = new Symbol("AMD");
            Symbol s7 = new Symbol("NVDA");
            Symbol s8 = new Symbol("QCOM");
            Symbol s9 = new Symbol("PANL");
            Symbol s10 = new Symbol("ALSDKJALKSDJ");
            context.Symbols.Add(s1);
            context.Symbols.Add(s2);
            context.Symbols.Add(s3);
            context.Symbols.Add(s4);
            context.Symbols.Add(s5);
            context.Symbols.Add(s6);
            context.Symbols.Add(s7);
            context.Symbols.Add(s8);
            context.Symbols.Add(s9);
            context.Symbols.Add(s10);
            context.SaveChanges();

            WatchList w1 = new WatchList();
            WatchList w2 = new WatchList("Other");
            context.WatchLists.Add(w1);
            context.WatchLists.Add(w2);
            context.SaveChanges();

            WatchListItem wl1 = new WatchListItem(new Symbol("GOOG"), "Default");
            WatchListItem wl2 = new WatchListItem(new Symbol("AAPL"), "Default");
            WatchListItem wl3 = new WatchListItem(new Symbol("VZ"), "Default");
            WatchListItem wl4 = new WatchListItem(new Symbol("INTC"), "Default");
            WatchListItem wl5 = new WatchListItem(new Symbol("MSFT"), "Default");
            WatchListItem wl6 = new WatchListItem(new Symbol("HP"), "Default");
            WatchListItem wl7 = new WatchListItem(new Symbol("AMD"), "Default");
            WatchListItem wl8 = new WatchListItem(new Symbol("NVDA"), "Default");
            WatchListItem wl9 = new WatchListItem(new Symbol("QCOM"), "Default");
            WatchListItem wl10 = new WatchListItem(new Symbol("PANL"), "Default");
            context.WatchListItems.Add(wl1);
            context.WatchListItems.Add(wl2);
            context.WatchListItems.Add(wl3);
            context.WatchListItems.Add(wl4);
            context.WatchListItems.Add(wl5);
            context.WatchListItems.Add(wl6);
            context.WatchListItems.Add(wl7);
            context.WatchListItems.Add(wl8);
            context.WatchListItems.Add(wl9);
            context.WatchListItems.Add(wl10);
            context.SaveChanges();

            Quote q = new Quote();
            q.price = 890.22;
            q.timestamp = new DateTime(2013, 06, 18);
            q.SymbolName = "GOOG";
            context.Quotes.Add(q);

            Quote q2 = new Quote();
            q2.price = 760.47;
            q2.timestamp = new DateTime(2013, 06, 17);
            q2.SymbolName = "GOOG";
            context.Quotes.Add(q2);

            Quote q3 = new Quote();
            q3.price = 438.89;
            q3.timestamp = new DateTime(2013, 06, 18);
            q3.SymbolName = "AAPL";
            context.Quotes.Add(q3);

            Quote q4 = new Quote();
            q4.price = 441.03;
            q4.timestamp = new DateTime(2013, 06, 17);
            q4.SymbolName = "AAPL";
            context.Quotes.Add(q4);

            Quote q5 = new Quote();
            q5.price = 50.53;
            q5.timestamp = new DateTime(2013, 06, 18);
            q5.SymbolName = "VZ";
            context.Quotes.Add(q5);

            Quote q6 = new Quote();
            q6.price = 49.14;
            q6.timestamp = new DateTime(2013, 06, 17);
            q6.SymbolName = "VZ";
            context.Quotes.Add(q6);

            Quote q7 = new Quote();
            q7.price = 25.01;
            q7.timestamp = new DateTime(2013, 06, 18);
            q7.SymbolName = "INTC";
            context.Quotes.Add(q7);

            Quote q8 = new Quote();
            q8.price = 24.53;
            q8.timestamp = new DateTime(2013, 06, 17);
            q8.SymbolName = "INTC";
            context.Quotes.Add(q8);

            Quote q9 = new Quote();
            q9.price = 35.47;
            q9.timestamp = new DateTime(2013, 06, 18);
            q9.SymbolName = "MSFT";
            context.Quotes.Add(q9);

            Quote q10 = new Quote();
            q10.price = 35.67;
            q10.timestamp = new DateTime(2013, 06, 17);
            q10.SymbolName = "MSFT";
            context.Quotes.Add(q10);

            Quote q11 = new Quote();
            q11.price = 20.12;
            q11.timestamp = new DateTime(2013, 06, 18);
            q11.SymbolName = "HP";
            context.Quotes.Add(q11);

            Quote q12 = new Quote();
            q12.price = 22.43;
            q12.timestamp = new DateTime(2013, 06, 17);
            q12.SymbolName = "HP";
            context.Quotes.Add(q12);

            Quote q13 = new Quote();
            q13.price = 25.10;
            q13.timestamp = new DateTime(2013, 06, 18);
            q13.SymbolName = "NVDA";
            context.Quotes.Add(q13);

            Quote q14 = new Quote();
            q14.price = 25.10;
            q14.timestamp = new DateTime(2013, 06, 14);
            q14.SymbolName = "NVDA";
            context.Quotes.Add(q14);

            Quote q15 = new Quote();
            q15.price = 130.40;
            q15.timestamp = new DateTime(2013, 06, 18);
            q15.SymbolName = "QCOM";
            context.Quotes.Add(q15);

            Quote q16 = new Quote();
            q16.price = 118.27;
            q16.timestamp = new DateTime(2013, 06, 16);
            q16.SymbolName = "QCOM";
            context.Quotes.Add(q16);

            Quote q17 = new Quote();
            q17.price = 83.61;
            q17.timestamp = new DateTime(2013, 06, 18);
            q17.SymbolName = "PANL";
            context.Quotes.Add(q17);

            Quote q18 = new Quote();
            q18.price = 90.43;
            q18.timestamp = new DateTime(2013, 06, 16);
            q18.SymbolName = "PANL";
            context.Quotes.Add(q18);

            Quote q19 = new Quote();
            q19.price = 4.14;
            q19.timestamp = new DateTime(2013, 06, 18);
            q19.SymbolName = "AMD";
            context.Quotes.Add(q19);

            Quote q20 = new Quote();
            q20.price = 4.08;
            q20.timestamp = new DateTime(2013, 06, 17);
            q20.SymbolName = "AMD";
            context.Quotes.Add(q20);
            context.SaveChanges();

        }
    }
}