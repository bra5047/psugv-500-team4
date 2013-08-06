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

            SystemSetting emailaddress = new SystemSetting();
            emailaddress.Module = "UserAgent";
            emailaddress.Name = "ALERTS_EMAIL_ADDRESS_TO";
            emailaddress.Value = "AlgTrader500@Gmail.com";
            context.SystemSettings.Add(emailaddress);
            context.SaveChanges();

            //ThreeDuckStrategy
            context.SystemSettings.Add(new SystemSetting("ThreeDuckStrategy", "FIRST_DUCK_SECONDS", "604800"));
            context.SystemSettings.Add(new SystemSetting("ThreeDuckStrategy", "SECOND_DUCK_SECONDS", "86400"));
            context.SystemSettings.Add(new SystemSetting("ThreeDuckStrategy", "THIRD_DUCK_SECONDS", "43200"));
            context.SystemSettings.Add(new SystemSetting("ThreeDuckStrategy", "MOVING_AVERAGE_WINDOW", "10"));
            context.SaveChanges();

			// Adam's stuff to fill in db tables
			string[] symbols = { "AAPL", "VZ", "INTC", "MSFT", "HP", "PANL", "NVDA", "QCOM", "AMD", "FB", "LNKD", "ZNGA"};
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
				symbol.CompanyName = "Random Company Name, Inc.";
				context.Symbols.Add(symbol);

				WatchListItem wli = new WatchListItem(symbol, watchlists[rand.Next(0, watchlists.Length)]);
				context.WatchListItems.Add(wli);

				for (int k = 0; k < 10; k++)
				{
					Quote quote1 = new Quote();
					quote1.price = Math.Round((rand.NextDouble() * (200 - 5) + 5), 2);
					quote1.timestamp = DateTime.Now.AddDays(-k);
					quote1.SymbolName = symbol.name;
					context.Quotes.Add(quote1);
				}

				Alert alert1 = new Alert();
				alert1.AlertId = Guid.NewGuid();
				alert1.Symbol = symbol;
				alert1.Timestamp = DateTime.Now.AddDays(-2);
				alert1.Type = tradeTypes.Buy;
				alert1.Quantity = 100;
				alert1.SentTo = "bra5047@psu.edu";
				alert1.Price = 20.00;
				alert1.ResponseCode = responseCodes.Pending;
				context.Alerts.Add(alert1);

				Alert alert2 = new Alert();
				alert2.AlertId = Guid.NewGuid();
				alert2.Symbol = symbol;
				alert2.Timestamp = DateTime.Now;
				alert2.Type = tradeTypes.Buy;
				alert2.Quantity = 100;
				alert2.SentTo = "bra5047@psu.edu";
				alert2.Price = 20.00;
				alert2.ResponseCode = responseCodes.Pending;
				context.Alerts.Add(alert2);

				Alert alert3 = new Alert();
				alert3.AlertId = Guid.NewGuid();
				alert3.Symbol = symbol;
				alert3.Timestamp = DateTime.Now.AddHours(-1);
				alert3.Type = tradeTypes.Buy;
				alert3.Quantity = 100;
				alert3.SentTo = "bra5047@psu.edu";
				alert3.Price = 20.00;
				alert3.ResponseCode = responseCodes.Pending;
				context.Alerts.Add(alert3);
				context.SaveChanges();

				Position p = new Position();
				p.status = positionStatus.Open;
				p.Symbol = symbol;
				p.Portfolio = portfolio;

				for (int j = 0; j < new Random().Next(1, 10); j++)
				{
					Trade t = new Trade();
					t.Symbol = symbol;
					t.Position = p;
					t.price = Math.Round((rand.NextDouble() * (200 - 5) + 5), 2);
					t.quantity = rand.Next(1, 20);
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
