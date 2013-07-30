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
using AlgoTraderSite.Portfolio.Client;
using AlgoTraderSite.Strategy.Client;
using System.IO;

namespace AlgoTraderSite
{
	public partial class WatchListPage : Page
	{
		private static IWatchListManager wlm = new WatchListManager();
		private static StrategyClient strategy = new StrategyClient();
		List<WatchList> watchlists = new List<WatchList>();
		private static List<WatchlistPlusQuote> allitems = new List<WatchlistPlusQuote>();
		private string portfolioName = "My Portfolio";
		string[] headers = { "COMPANY", "PRICE", "CHANGE", "CHANGE %", "ACTIONS" };
		string[] widths = { "32%", "17%", "16%", "15%", "20%" };

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
			PortfolioManagerClient pm = new PortfolioManagerClient();
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
				var quotes = wlm.GetQuotes(item.SymbolName).OrderByDescending(x => x.timestamp).ToList();
				double price1 = 0;
				double price2 = 0;

				price1 = quotes.Select(x => x.price).FirstOrDefault();
				price2 = quotes.Select(x => x.price).Skip(1).FirstOrDefault();

				if (quotes.Count() == 1)
				{
					price2 = price1;
				}

				string companyName = wlm.GetLongName(item.SymbolName);

				DateTime date = quotes.Select(x => x.timestamp).FirstOrDefault();
				allitems.Add(new WatchlistPlusQuote(item.SymbolName, companyName, item.ListName, date, price1, price2));
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
			string prefix = string.Empty;
			string classname = string.Empty;

			for (int i = 0; i < headers.Length; i++)
			{
				TableCell cell = new TableCell();
				row.Cells.Add(cell);
			}

			if (item.PriceChange > 0)
			{
				prefix = "+";
				classname = "green";
			}
			if (item.PriceChange < 0)
			{
				classname = "red";
			}
			row.Cells[0].Text = item.SymbolName + new HtmlString(String.Format(" <span class='subtext'>({0})</span>", item.LongName));
			row.Cells[1].Text = String.Format("{0:C}", item.CurrentPrice);
			row.Cells[1].ToolTip = String.Format("as of {0}", item.Timestamp);
			row.Cells[2].Text = new HtmlString(String.Format("<span class='{0}'>{1:C}</span>", classname, item.PriceChange)).ToString();
			row.Cells[3].Text = new HtmlString(String.Format("<span class='{0}'>{1:P}</span>", classname, item.ChangePercent)).ToString();

			if (isPortfolio()) // create Remove button for each row or a lock for portfolio
			{
				Button btnLocked = new Button();
				btnLocked.CssClass = "symbol-button";
				btnLocked.Text = HttpUtility.HtmlDecode("&#xe016;");
				btnLocked.ToolTip = "This item is locked";
				btnLocked.Enabled = false;
				row.Cells[headers.Length - 1].Controls.Add(btnLocked);
			}
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
			// graph button
			Button btnGraph = new Button();
			btnGraph.CssClass = "symbol-button";
			btnGraph.Attributes.Add("Symbol", item.SymbolName);
			btnGraph.ID = "btnGraph" + item.SymbolName;
			btnGraph.Text = HttpUtility.HtmlDecode("&#xe019;");
			btnGraph.ToolTip = "View graph";
			btnGraph.Click += new EventHandler(btnClick_generateChart);
			row.Cells[headers.Length - 1].Controls.Add(btnGraph);

			// buy/sell strategy signal button
			try
			{
				string summary = strategy.getSummary(item.SymbolName).CurrentSignal.ToString();
				if (summary.Equals("Buy") || summary.Equals("Sell"))
				{
					Button btnSignal = new Button();
					btnSignal.Attributes.Add("Symbol", item.SymbolName);
					btnSignal.ID = "btnSignal" + item.SymbolName;
					btnSignal.Text = summary;
					btnSignal.Click += new EventHandler(btnClick_BuySell);
					row.Cells[headers.Length - 1].Controls.Add(btnSignal);
				}
			}
			catch (Exception ex)
			{
				// meh
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

			row.Cells[radioSortType.SelectedIndex / 2].CssClass = "bold";

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

				//check if all instances of that symbol are removed before strategy stops watching it
				TraderContext db = new TraderContext();
				if (db.WatchListItems.Where(x => x.SymbolName.Equals(symbol)).Count() == 0 && db.Positions.Where(x => x.SymbolName.Equals(symbol) && x.status == AlgoTrader.Interfaces.positionStatus.Open).Count() == 0)
				{
					strategy.stopWatching(symbol);
				}
			}
			updateList(true);
		}

		protected void btnAddToWatchList_Click(object sender, EventArgs e)
		{
			// TODO add validation for entered symbol
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
				strategy.startWatching(symbol);
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

		protected void btnClick_generateChart(object sender, EventArgs e)
		{
			Button Sender = (Button)sender;
			string symbol = Sender.Attributes["Symbol"];
			Response.Redirect("Graph?s=" + symbol);
		}

		protected void btnClick_BuySell(object sender, EventArgs e)
		{
			Button Sender = (Button)sender;
			string symbol = Sender.Attributes["Symbol"];
			Response.Redirect("BuySell?s=" + symbol + "&t="+Sender.Text);
		}
		#endregion
	}

	#region extra classes
	class WatchlistPlusQuote // joined list for watchlist only
	{
		public string SymbolName { get; set; }
		public string LongName { get; set; }
		public string ListName { get; set; }
		public DateTime Timestamp { get; set; }
		public double CurrentPrice { get; set; }
		public double PriceChange { get; set; }
		public double ChangePercent { get; set; }

		public WatchlistPlusQuote(string symbol, string longname, string list, DateTime time, double pricenow, double pricebefore)
		{
			SymbolName = symbol;
			LongName = longname;
			ListName = list;
			Timestamp = time;
			CurrentPrice = pricenow;
			PriceChange = pricenow - pricebefore;
			ChangePercent = (pricenow - pricebefore) / pricebefore;
		}
	}
	#endregion
}