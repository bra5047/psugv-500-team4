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
			string[] symbols = { "AAPL", "VZ", "INTC", "MSFT", "HP", "PANL", "NVDA", "QCOM", "AMD", "FB", "LNKD", "ZNGA" };
			string[] watchlists = { "", "Other", "Test List", "Future Purchases" };
			Random rand = new Random();

			for (int i = 0; i < watchlists.Length; i++)
			{
				WatchList w = new WatchList(watchlists[i]);
				context.WatchLists.Add(w);
			}
			context.SaveChanges();

			for (int i = 0; i < symbols.Length; i++)
			{
				Symbol symbol = new Symbol(symbols[i]);
				context.Symbols.Add(symbol);

				WatchListItem wli = new WatchListItem(symbol, watchlists[rand.Next(0, watchlists.Length)]);
				context.WatchListItems.Add(wli);

				Quote quote1 = new Quote();
				quote1.price = Math.Round((rand.NextDouble() * (1000 - 5) + 5), 2);
				quote1.timestamp = DateTime.Now;
				quote1.SymbolName = symbol.name;
				context.Quotes.Add(quote1);

				Quote quote2 = new Quote();
				quote2.price = Math.Round((rand.NextDouble() * (1000 - 5) + 5), 2);
				quote2.timestamp = DateTime.Now.AddMinutes(-15);
				quote2.SymbolName = symbol.name;
				context.Quotes.Add(quote2);

				Position p = new Position();
				p.status = positionStatus.Open;
				p.Symbol = symbol;
				p.Portfolio = portfolio;

				for (int j = 0; j < 5; j++)
				{
					Trade t = new Trade();
					t.Symbol = symbol;
					t.Position = p;
					t.price = rand.Next(10, 100);
					t.quantity = rand.Next(1, 50);
					t.type = tradeTypes.Buy;
					t.TransactionId = Guid.NewGuid().ToString();
					t.timestamp = DateTime.Now.AddSeconds(rand.Next(1, (60 * 60 * 24 * 60) + 1) * -1); // any time between 60 days ago
					p.price += t.price;
					context.Trades.Add(t);
				}
				p.Recalculate();
				context.Positions.Add(p);
			}
			context.SaveChanges();
		}
	}
}
