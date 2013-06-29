using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTrader.watchlist;
using System.IO;

namespace AlgoTraderSite
{
	public partial class WatchListPage : Page
	{
		private static IWatchListManager wlm = new WatchListManager();
		private static IWatchList wl = new WatchList("Default");
		private static List<Quote> quotes = new List<Quote>();
        private List<WatchList> watchlists = new List<WatchList>();
		public static bool showing = false;
		public int numColumns = 5;

		protected void Page_Load(object sender, EventArgs e)
		{
			statusMessage.InnerText = string.Empty; // status string above the list
            listWatchLists();

			if (!showing)
			{
				
                quotes = wlm.GetQuotes("GOOG");
				showing = true;
			}
			showWatchList(ddlistWatchLists.SelectedValue);
		}

        public void listWatchLists()
        {
            watchlists.Clear();
            watchlists = wlm.GetAllWatchLists();
            List<WatchList> ordered = watchlists.OrderBy(x=>x.ListName).ToList();
            for (int i = 0; i < ordered.Count; i++)
            {
                ddlistWatchLists.Items.Add(ordered[i].ListName);
            }
        }

		public void showWatchList(string lname)
		{
            wl = wlm.GetWatchList(lname);
			tblWatchList.Controls.Clear(); // clear the page
			wl.items = wl.items.OrderBy(a => a.SymbolName).ToList();

			TableHeaderRow headers = new TableHeaderRow();
			for (int i = 0; i < numColumns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				headers.Cells.Add(cell);
			}

			headers.Cells[0].Text = "COMPANY";
			headers.Cells[1].Text = "PRICE";
			headers.Cells[2].Text = "CHANGE";
			headers.Cells[3].Text = "CHANGE %";
			headers.Cells[4].Text = "ACTIONS";
			tblWatchList.Rows.Add(headers);

			for (int row = 0; row < wl.items.Count; row++)
			{
				string symbolName = wl.items[row].SymbolName;
				string listName = wl.items[row].ListName;
				double currentPrice = 0;
				double previousPrice = 0;
				double priceChange = 0;
				DateTime date = new DateTime();

				// create the row and cells
				TableRow tblRow = new TableRow();
				for (int j = 0; j < numColumns; j++)
				{
					TableCell cell = new TableCell();
					cell.Width = 300;
					tblRow.Cells.Add(cell);
				}

				// create remove button for each row
				// need to fix double click required for page update; OnPreRender?
				Button btnRemove = new Button();
				btnRemove.Attributes["Symbol"] = symbolName;
				btnRemove.Attributes["ListName"] = listName;
				btnRemove.Text = "Remove";
				btnRemove.Click += new EventHandler(btnRemove_Click);
				tblRow.Cells[numColumns - 1].Controls.Add(btnRemove);

				foreach (Quote q in quotes)
				{
					date = (from quote in quotes
							where quote.SymbolName.Equals(symbolName)
							orderby quote.timestamp descending
							select quote.timestamp).FirstOrDefault();
					currentPrice = (from quote in quotes
									where quote.SymbolName.Equals(symbolName)
									orderby quote.timestamp descending
									select quote.price).FirstOrDefault();
					previousPrice = (from quote in quotes
									 where quote.SymbolName.Equals(symbolName)
									 orderby quote.timestamp descending
									 select quote.price).Skip(1).FirstOrDefault();
				}

				tblRow.Cells[0].Text = symbolName;
				tblRow.Cells[1].Text = currentPrice.ToString("N2") + " as of " + date.ToShortDateString();
				priceChange = currentPrice - previousPrice;

				// set color and prefix of price change column
				if (priceChange > 0 && priceChange != 0)
				{
					tblRow.Cells[2].Text = "+ ";
					tblRow.Cells[2].Attributes["style"] = "color:green";
					tblRow.Cells[3].Text = "+ ";
					tblRow.Cells[3].Attributes["style"] = "color:green";
				}
				if (priceChange < 0 && priceChange != 0)
				{
					tblRow.Cells[2].Text = "- ";
					tblRow.Cells[2].Attributes["style"] = "color:red";
					tblRow.Cells[3].Text = "- ";
					tblRow.Cells[3].Attributes["style"] = "color:red";
				}

				tblRow.Cells[2].Text += Math.Abs(priceChange).ToString("N2");
				tblRow.Cells[3].Text = Math.Abs(priceChange / previousPrice * 100).ToString("N2") + "%";

				tblWatchList.Rows.Add(tblRow); // finally add the row to the page
			}	
		}

        protected void updateList()
        {
            showWatchList(ddlistWatchLists.SelectedValue);
        }

		protected void btnRemove_Click(object sender, EventArgs e)
		{
			bool success = false;
			string symbol = ((Button)sender).Attributes["Symbol"];
			string listName = ((Button)sender).Attributes["ListName"];
			success = wl.RemoveFromList(new Symbol(symbol), listName);
			
			if (success)
			{
				statusMessage.InnerText = symbol + " from list " + listName + " removed successfully.";
			}

            updateList();
		}

		protected void btnAddToWatchList_Click(object sender, EventArgs e)
		{
			string symbol = this.tbAddToWatchList.Text.Trim().ToUpper();
			if (symbol.Length > 0)
			{
				wl.AddToList(new Symbol(symbol), "Default");
				statusMessage.InnerText = symbol + " added to list " + "Default" + ".";

				//REMOVE
				//THIS
				//LATER
				Quote q1 = new Quote();
				q1.price = 500.00;
				q1.SymbolName = symbol;
				q1.timestamp = DateTime.Now;
				quotes.Add(q1);

				Quote q2 = new Quote();
				q2.price = 400.00;
				q2.SymbolName = symbol;
				q2.timestamp = DateTime.Now.AddDays(-1);
				quotes.Add(q2);
			}
            updateList();
		}

        protected void ddlistWatchLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateList();
        }
	}
}