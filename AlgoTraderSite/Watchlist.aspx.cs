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
		private int numColumns = 5;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				listWatchLists();
			}
			else
			{

			}
			statusMessage.InnerText = string.Empty;
			showWatchList();
		}

        public void listWatchLists()
        {
            ddlistWatchLists.Items.Clear();
            List<WatchList> watchlists = new List<WatchList>();
            watchlists = wlm.GetAllWatchLists().OrderBy(x=>x.ListName).ToList();

            foreach (WatchList w in watchlists)
            {
                ddlistWatchLists.Items.Add(w.ListName);
            }
        }

		public void showWatchList()
		{
			string lName = ddlistWatchLists.SelectedValue;

			tblWatchList.Controls.Clear();
            wl = wlm.GetWatchList(lName);
			wl.items.OrderBy(x => x.SymbolName);

			TableHeaderRow headers = new TableHeaderRow();
			string[] headerNames = { "COMPANY", "PRICE", "CHANGE", "CHANGE %", "ACTIONS" };
			for (int i = 0; i < numColumns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				headers.Cells.Add(cell);
				headers.Cells[i].Text = headerNames[i];
			}
			tblWatchList.Rows.Add(headers);

			foreach (WatchListItem item in wl.items)
			{
				double currentPrice = 0;
				double previousPrice = 0;
				double priceChange = 0;
				DateTime date = new DateTime();

				// get the quotes
				quotes = wlm.GetQuotes(item.SymbolName);
				quotes.OrderBy(x=>x.timestamp);

				foreach (Quote q in quotes)
				{
					date = quotes.Select(x => x.timestamp).FirstOrDefault();
					currentPrice = quotes.Select(x => x.price).FirstOrDefault();
					previousPrice = quotes.Select(x => x.price).Skip(1).FirstOrDefault();
				}

				// create the row and cells
				TableRow tr = new TableRow();
				for (int i = 0; i < numColumns; i++)
				{
					TableCell cell = new TableCell();
					tr.Cells.Add(cell);
				}

				// TODO get long name info from quote manager - should it be stored in the database?
				string fullName = "long name goes here";
				string fullNameStyle = "style='color:gray; font-weight:300'";
				tr.Cells[0].Text = item.SymbolName;
				tr.Cells[0].Text += new HtmlString(String.Format(" <span {0}>({1})</span>", fullNameStyle, fullName));
				tr.Cells[1].Text = currentPrice.ToString("N2") + " as of " + date.ToShortDateString();
				priceChange = currentPrice - previousPrice;

				if (priceChange > 0)
				{
					string prefix = "+ ";
					string style = "color:green";

					tr.Cells[2].Text = prefix;
					tr.Cells[2].Attributes["style"] = style;
					tr.Cells[3].Text = prefix;
					tr.Cells[3].Attributes["style"] = style;
				}
				if (priceChange < 0)
				{
					string prefix = "- ";
					string style = "color:red";

					tr.Cells[2].Text = prefix;
					tr.Cells[2].Attributes["style"] = style;
					tr.Cells[3].Text = prefix;
					tr.Cells[3].Attributes["style"] = style;
				}

				tr.Cells[2].Text += Math.Abs(priceChange).ToString("N2");
				tr.Cells[3].Text += Math.Abs(priceChange / previousPrice * 100).ToString("N2") + "%";

				// create Remove button for each row
				// TODO fix the double click required for page updates; OnPreRender? ViewState? Ajax?
				Button btnRemove = new Button();
				btnRemove.Attributes["Symbol"] = item.SymbolName;
				btnRemove.Attributes["ListName"] = item.ListName;
				btnRemove.Text = "Remove";
				btnRemove.Click += new EventHandler(btnRemove_Click);
				tr.Cells[numColumns - 1].Controls.Add(btnRemove);

				// set widths
				tr.Cells[0].Width = new Unit("40%");
				tr.Cells[1].Width = new Unit("20%");

				tblWatchList.Rows.Add(tr);
			}
		}

        protected void updateList()
        {
            showWatchList();
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
			string listName = this.ddlistWatchLists.SelectedValue;

			if (symbol.Length > 0)
			{
				wl.AddToList(new Symbol(symbol), listName);
				statusMessage.InnerText = symbol + " added to list " + listName + ".";

				// TODO remove this later
				Quote q1 = new Quote();
				q1.price = new Random().NextDouble() * (400 - 100) + 100;
				q1.SymbolName = symbol;
				q1.timestamp = DateTime.Now;
				quotes.Add(q1);

				Quote q2 = new Quote();
				q2.price = new Random().NextDouble() * (400 - 100) + 100;
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