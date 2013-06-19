using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using System.IO;

namespace AlgoTraderSite
{
	public partial class WatchListPage : Page
	{
		private static WatchList wl = new WatchList();
		private List<QuoteMessage> quotes = new List<QuoteMessage>();
		private static bool showing = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			watchlistdiv.InnerText = string.Empty;
			createTestWatchList();
			showWatchList(wl);
		}

		private void createTestWatchList()
		{
			if (!showing)
			{
				wl.ListName = "Default";
				wl.addToList(new Symbol("GOOG"), "Default");
				wl.addToList(new Symbol("AAPL"), "Default");
				wl.addToList(new Symbol("VZ"), "Default");
				wl.addToList(new Symbol("INTC"), "Default");
				wl.addToList(new Symbol("MSFT"), "Default");
				wl.addToList(new Symbol("HP"), "Default");

				quotes.Add(new QuoteMessage(890.22, new DateTime(2013, 06, 18), "GOOG"));
				quotes.Add(new QuoteMessage(870.47, new DateTime(2013, 06, 17), "GOOG"));
				quotes.Add(new QuoteMessage(840.47, new DateTime(2013, 06, 16), "GOOG"));
				quotes.Add(new QuoteMessage(830.47, new DateTime(2013, 06, 15), "GOOG"));
				quotes.Add(new QuoteMessage(438.89, new DateTime(2013, 06, 18), "AAPL"));
				quotes.Add(new QuoteMessage(441.03, new DateTime(2013, 06, 17), "AAPL"));
				quotes.Add(new QuoteMessage(50.53, new DateTime(2013, 06, 18), "VZ"));
				quotes.Add(new QuoteMessage(50.14, new DateTime(2013, 06, 17), "VZ"));
				quotes.Add(new QuoteMessage(25.01, new DateTime(2013, 06, 18), "INTC"));
				quotes.Add(new QuoteMessage(24.53, new DateTime(2013, 06, 17), "INTC"));
				quotes.Add(new QuoteMessage(35.47, new DateTime(2013, 06, 18), "MSFT"));
				quotes.Add(new QuoteMessage(35.67, new DateTime(2013, 06, 17), "MSFT"));
				quotes.Add(new QuoteMessage(25.44, new DateTime(2013, 06, 18), "MSFT"));

				showing = true;
			}
		}

		private void showWatchList(WatchList watchlist)
		{
			tblWatchList.Controls.Clear();

			TableRow headers = new TableRow();
			TableCell companyHeaderCell = new TableCell();
			TableCell priceHeaderCell = new TableCell();
			TableCell changeHeaderCell = new TableCell();
			TableCell changePercentHeaderCell = new TableCell();
			TableCell actionHeaderCell = new TableCell();

			companyHeaderCell.Text = "COMPANY";
			priceHeaderCell.Text = "PRICE";
			changeHeaderCell.Text = "CHANGE";
			changePercentHeaderCell.Text = "CHANGE %";
			actionHeaderCell.Text = "ACTIONS";

			headers.Cells.Add(companyHeaderCell);
			headers.Cells.Add(priceHeaderCell);
			headers.Cells.Add(changeHeaderCell);
			headers.Cells.Add(changePercentHeaderCell);
			headers.Cells.Add(actionHeaderCell);

			tblWatchList.Rows.Add(headers);

			for (int row = 0; row < watchlist.Items.Count; row++)
			{
				string symbolName = watchlist.Items[row].SymbolName;
				string listName = watchlist.Items[row].ListName;
				double priceNow = 0;
				double priceBefore = 0;
				DateTime date = new DateTime(1, 1, 1);

				TableRow tblrow = new TableRow();
				List<TableCell> cells = new List<TableCell>();
				TableCell companyCell = new TableCell();
				TableCell priceCell = new TableCell();
				TableCell changeCell = new TableCell();
				TableCell changePercentCell = new TableCell();
				TableCell actionCell = new TableCell();

				Button removeBtn = new Button();
				removeBtn.Attributes["Symbol"] = symbolName;
				removeBtn.Attributes["ListName"] = listName;
				removeBtn.Text = "Remove";
				removeBtn.Click += new EventHandler(btnRemove_Click);

				cells.Add(companyCell);
				cells.Add(priceCell);
				cells.Add(changeCell);
				cells.Add(changePercentCell);
				actionCell.Controls.Add(removeBtn);
				cells.Add(actionCell);

				companyCell.Text = watchlist.Items[row].SymbolName;
				foreach (QuoteMessage q in quotes)
				{
					List<QuoteMessage> _quotes = new List<QuoteMessage>();
					_quotes = _quotes.Where(x => x.SymbolName == symbolName).OrderByDescending(x => x.timestamp).ToList();

					watchlistdiv.InnerText = _quotes.Count.ToString();
				
					//date = (from quote in quotes
					//		where (quote.SymbolName == symbolName)
					//		orderby quote.timestamp descending
					//		select quote.timestamp).Skip(1).First();
					//priceNow = (from _q in _quotes
					//			select _q.Price).First();
					//priceBefore = (from _q in _quotes
					//			   select _q.Price).Skip(1).First();
				}
				
				priceCell.Text = "890.22"; //replace with quote price
				changeCell.Text = (priceNow - priceBefore).ToString() + " since " + date.ToShortDateString(); // replace with quote change
				changePercentCell.Text = "+ 1.19%"; // replace with quote percent change

				foreach (TableCell cell in cells)
				{
					cell.Width = 300;
					tblrow.Cells.Add(cell);
				}
				tblWatchList.Rows.Add(tblrow);
			}
		}

		private void removeFromWatchList(WatchList watchlist, ISymbol symbol, string listName)
		{
			bool success = false;
			success = watchlist.removeFromList(symbol, listName);
			showWatchList(watchlist);
			
			if (success)
			{
				watchlistdiv.InnerText = symbol.name + " from list " + listName + " removed successfully.";
			}
		}

		private void addToWatchList(WatchList watchlist, ISymbol symbol, string listName)
		{
			watchlist.addToList(symbol, listName);
			showWatchList(watchlist);
		}

		protected void btnRemove_Click(object sender, EventArgs e)
		{
			string symbol = ((Button)sender).Attributes["Symbol"];
			string listname = ((Button)sender).Attributes["ListName"];
			watchlistdiv.InnerText = symbol + " " + listname;
			removeFromWatchList(wl, new Symbol(symbol), listname);
		}

		protected void btnAddToWatchList_Click(object sender, EventArgs e)
		{
			string symbol = this.tbAddToWatchList.Text.ToUpper();
			if (symbol.Length > 0)
			{
				addToWatchList(wl, new Symbol(symbol), "Default");
			}
		}
	}
}