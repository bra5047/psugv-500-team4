using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTrader.watchlist;
using AlgoTrader.portfolio;
using System.IO;

namespace AlgoTraderSite
{
	// TODO implement strategy signal into watchlist
	public partial class WatchListPage : Page
	{
		private static IWatchListManager wlm = new WatchListManager();
		List<WatchList> watchlists = new List<WatchList>();
		private static List<WatchlistPlusQuote> allitems = new List<WatchlistPlusQuote>();

		private string portfolioName = "My Portfolio";
		string[] headers = { "COMPANY", "PRICE", "CHANGE", "CHANGE %", "ACTIONS" };
		string[] widths = { "40%", "20%", "15%", "15%", "10%" };

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				listWatchLists();
				generateWatchLists();
				radioSortType.SelectedIndex = 0;
			}
			showWatchList();
		}

		#region List Management Stuff
		public void listWatchLists()
		{
			string value = radioLists.SelectedValue;
			watchlists.Add(new WatchList(portfolioName));
			watchlists.AddRange(wlm.GetAllWatchLists().OrderBy(x => x.ListName).ToList());
			radioLists.DataSource = watchlists;
			radioLists.DataBind();

			if (value.Length > 0)
			{
				try
				{
					radioLists.SelectedValue = value;
				}
				catch
				{
					radioLists.SelectedIndex = 0;
				}
			}
			else
			{
				radioLists.SelectedIndex = 0;
			}

			radioLists.Items[0].Text += new HtmlString(" <span class='icon-lock float-right' style='opacity:.5; line-height: 1.5em'></span>");
		}

		public void generateWatchLists() // gets all the data ready
		{
			IWatchList wl = new WatchList();
			PortfolioManager pm = new PortfolioManager();
			allitems.Clear();

			foreach (WatchList w in watchlists) // add all the real watchlist items
			{
				wl.items.AddRange(wlm.GetWatchList(w.ListName).items);
			}
			foreach (PositionMessage msg in pm.GetOpenPositions()) // add the portfolio items
			{
				wl.items.Add(new WatchListItem(new Symbol(msg.SymbolName), portfolioName));
			}
			foreach (WatchListItem item in wl.items) // join all the items together into a list of WatchlistPlusQuote objects
			{
				var quotes = wlm.GetQuotes(item.SymbolName).OrderBy(x => x.timestamp).Take(2).ToList();
				double price1 = quotes.Select(x => x.price).FirstOrDefault();
				double price2 = quotes.Select(x => x.price).Skip(1).FirstOrDefault();
				DateTime date = quotes.Select(x => x.timestamp).FirstOrDefault();
				allitems.Add(new WatchlistPlusQuote(item.SymbolName, item.ListName, date, price1, price2));
			}
		}

		public void showWatchList()
		{
			string listName = radioLists.SelectedValue;
			WatchlistDiv.Controls.Clear();
			emptyDiv.Controls.Clear();

			if (isPortfolio())
			{
				inputGroupLeft.Visible = false;
			}
			else
			{
				inputGroupLeft.Visible = true;
			}

			if (allitems.Where(x => x.ListName.Equals(listName)).Count() > 0)
			{
				WatchlistDiv.Controls.Add(createHeader());
				List<WatchlistPlusQuote> subitems = allitems.Where(x => x.ListName.Equals(listName)).ToList();
				subitems = sortList(subitems);
				foreach (WatchlistPlusQuote w in subitems)
				{
					WatchlistDiv.Controls.Add(createWatchlistTable(w));
				}
			}
			else
			{
				emptyDiv.InnerHtml = new HtmlString("<h1>This list is empty :(</h1><h2>Why not add a symbol to watch?</h2>").ToString();
			}
		}

		private List<WatchlistPlusQuote> sortList(List<WatchlistPlusQuote> unsorted)
		{
			switch (radioSortType.SelectedIndex)
			{
				case 0: return unsorted.OrderBy(x => x.SymbolName).ToList(); // name asc
				case 1: return unsorted.OrderByDescending(x => x.SymbolName).ToList(); // name desc
				case 2: return unsorted.OrderBy(x => x.CurrentPrice).ToList(); // price asc
				case 3: return unsorted.OrderByDescending(x => x.CurrentPrice).ToList(); // price desc
				case 4: return unsorted.OrderByDescending(x => x.PriceChange).ToList(); // highest change
				case 5: return unsorted.OrderBy(x => x.PriceChange).ToList(); // lowest change
				case 6: return unsorted.OrderByDescending(x => x.ChangePercent).ToList(); // highest % change
				case 7: return unsorted.OrderBy(x => x.ChangePercent).ToList(); // lowest % change
				default: return unsorted;
			}
		}

		private Table createHeader()
		{
			Table tbl = new Table();
			TableHeaderRow header = new TableHeaderRow();

			for (int i = 0; i < headers.Length; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				header.Cells.Add(cell);
				header.Cells[i].Text = headers[i];
			}
			tbl.Rows.Add(header);

			for (int i = 0; i < widths.Length - 1; i++)
			{
				header.Cells[i].Width = new Unit(widths[i]);
			}
			return tbl;
		}

		private Table createWatchlistTable(WatchlistPlusQuote item)
		{
			Table tbl = new Table();
			TableRow row = new TableRow();
			// TODO get long name info from quote manager - should it be stored in the database?
			string fullName = item.SymbolName + "'s long name goes here";
			string currentPrice = item.CurrentPrice.ToString("N2");
			double priceChange = item.PriceChange;
			double changePercentage = item.ChangePercent;
			DateTime date = item.Timestamp;
			string prefix = string.Empty;

			for (int i = 0; i < headers.Length; i++)
			{
				TableCell cell = new TableCell();
				row.Cells.Add(cell);
			}

			if (priceChange > 0)
			{
				prefix = "+";
				row.Cells[2].CssClass = "green";
				row.Cells[3].CssClass = "green";
			}
			if (priceChange < 0)
			{
				prefix = "-";
				row.Cells[2].CssClass = "red";
				row.Cells[3].CssClass = "red";
			}
			row.Cells[0].Text = item.SymbolName + new HtmlString(String.Format(" <span class='subtext'>({0})</span>", fullName));
			row.Cells[1].Text = currentPrice + " as of " + date.ToShortDateString();
			row.Cells[2].Text = String.Format("{0}{1:N2}", prefix, Math.Abs(priceChange));
			row.Cells[3].Text = String.Format("{0}{1:N2}%", prefix, Math.Abs(changePercentage));

			if (isPortfolio()) // create Remove button for each row or a lock for portfolio
			{
				row.Cells[headers.Length - 1].Text = new HtmlString("<span class='icon-lock' title='This item is locked.'></span>").ToString();
			}
			// TODO add a button for strategy buy/sell
			else // otherwise create a remove button
			{
				Button btnRemove = new Button();
				btnRemove.CssClass = "delete symbol-button";
				btnRemove.Attributes.Add("Symbol", item.SymbolName);
				btnRemove.Attributes.Add("ListName", item.ListName);
				btnRemove.ID = "btnRemove" + item.SymbolName;
				btnRemove.Text = HttpUtility.HtmlDecode("&#xe007;");
				btnRemove.ToolTip = "Remove from list";
				btnRemove.Click += btnRemove_Click;
				row.Cells[headers.Length - 1].Controls.Add(btnRemove);
			}

			// set widths
			for (int i = 0; i < widths.Length - 1; i++)
			{
				row.Cells[i].Width = new Unit(widths[i]);
			}
			//css stuff
			tbl.CssClass = "main";
			row.CssClass = "main";
			tbl.Rows.Add(row);
			return tbl;
		}

		protected void setStatus(string msg, bool type)
		{
			statusMessage.Controls.Clear();
			HtmlGenericControl message = new HtmlGenericControl("span");

			if (type)
			{
				message.Attributes.Add("class", "message-success");
				message.InnerHtml = new HtmlString("<span class='icon-ok-sign'></span> " + msg).ToString();
			}
			else
			{
				message.Attributes.Add("class", "message-fail");
				message.InnerHtml = new HtmlString("<span class='icon-remove-sign'></span> " + msg).ToString();
			}
			statusMessage.Controls.Add(message);
		}

		private void updateList(bool fullUpdate)
		{
			if (fullUpdate)
			{
				listWatchLists();
				generateWatchLists();
				showWatchList();
			}
			else
			{
				showWatchList();
			}
		}

		private bool isPortfolio()
		{
			return radioLists.SelectedIndex == 0;
		}
		#endregion

		#region Controls
		protected void btnRemove_Click(object sender, EventArgs e)
		{
			bool success = false;
			string symbol = ((Button)sender).Attributes["Symbol"];
			string listName = ((Button)sender).Attributes["ListName"];

			if (symbol.Length > 0 && listName.Length > 0)
			{
				IWatchList wl = new WatchList();
				success = wl.RemoveFromList(new Symbol(symbol), listName);

				if (success)
				{
					setStatus(String.Format("{0} removed from list \"{1}.\"", symbol, listName), true);
				}
			}
			updateList(true);
		}

		protected void btnAddToWatchList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string symbol = tbAddToWatchList.Text.Trim().ToUpper();
			string listName = radioLists.SelectedValue;

			if (symbol.Length > 0)
			{
				IWatchList wl = new WatchList();
				success = wl.AddToList(new Symbol(symbol), listName);
				if (success)
				{
					setStatus(String.Format("{0} added to list \"{1}.\"", symbol, listName), true);
				}
				else
				{
					setStatus(String.Format("{0} could not be added to \"{1}.\"", symbol, listName), false);
				}

				TraderContext context = new TraderContext();
				Random rand = new Random();
				// TODO remove this later
				Quote q1 = new Quote();
				q1.price = rand.NextDouble() * (400 - 100) + 100;
				q1.SymbolName = symbol;
				q1.timestamp = DateTime.Now;
				context.Quotes.Add(q1);

				Quote q2 = new Quote();
				q2.price = rand.NextDouble() * (400 - 100) + 100;
				q2.SymbolName = symbol;
				q2.timestamp = DateTime.Now.AddDays(-1);
				context.Quotes.Add(q2);

				context.SaveChanges();
				updateList(true);
			}
			else
			{
				setStatus("Stock symbol cannot be blank.", false);
			}
		}

		protected void btnDeleteList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string listName = radioLists.SelectedValue;
			IWatchList wl = new WatchList();
			if (listName.Length > 0)
			{
				List<WatchListItem> items = wl.items.Where(x => x.ListName.Equals(listName)).ToList();
				foreach (WatchListItem w in items)
				{
					wl.RemoveFromList(w.Symbol, w.ListName);
				}

				success = wlm.DeleteWatchList(listName);
				if (success)
				{
					setStatus(String.Format("List \"{0}\" deleted successfully.", listName), true);
				}
				else
				{
					setStatus(String.Format("List \"{0}\" could not be deleted.", listName), false);
				}
				updateList(true);
			}
		}

		protected void btnAddList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string listName = tbAddList.Text.Trim();
			if (listName.Length > 0)
			{
				if (!listName.ToLower().Equals(portfolioName.ToLower()))
				{
					success = wlm.AddWatchList(listName);

					if (success)
					{
						setStatus(String.Format("List \"{0}\" added successfully.", listName), true);
					}
					else
					{
						setStatus(String.Format("List \"{0}\" already exists.", listName), false);
					}
					listWatchLists();
				}
				else
				{
					setStatus("Sorry! You can't have a custom list with that name.", false);
				}
			}
			else
			{
				setStatus(String.Format("List name cannot be blank."), false);
			}
		}

		protected void radioLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateList(false);
		}

		protected void radioSortType_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateList(false);
		}
		#endregion
	}

	class WatchlistPlusQuote // joined list for watchlist only
	{
		public string SymbolName { get; set; }
		public string ListName { get; set; }
		public DateTime Timestamp { get; set; }
		public double CurrentPrice { get; set; }
		public double PriceChange { get; set; }
		public double ChangePercent { get; set; }

		public WatchlistPlusQuote(string symbol, string list, DateTime time, double pricenow, double pricebefore)
		{
			SymbolName = symbol;
			ListName = list;
			Timestamp = time;
			CurrentPrice = pricenow;
			PriceChange = pricenow - pricebefore;
			ChangePercent = pricenow / pricebefore * 100;
		}
	}
}