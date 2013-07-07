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
	// TODO disable delete button on portfolio
	// TODO populate portfolio watchlist
	// TODO switch images on sort buttons
	public partial class WatchListPage : Page
	{
		private static IWatchListManager wlm = new WatchListManager();
		private static IWatchList wl = new WatchList();
		private static List<Quote> quotes = new List<Quote>();
		private string portfolioName = "My Portfolio";
		string[] headers = { "COMPANY", "PRICE", "CHANGE", "CHANGE %", "ACTIONS" };
		string[] widths = { "40%", "20%", "15%", "15%", "10%" };

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				listWatchLists();
			}
			updateList();		
		}

		public void listWatchLists()
		{
			string value = radioLists.SelectedValue;
			List<WatchList> watchlists = new List<WatchList>();
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

		public void showWatchList(string listName)
		{
			string lName = listName;
			WatchlistDiv.Controls.Clear();

			wl = wlm.GetWatchList(lName);
			if (wl.items.Count > 0)
			{
				wl.items.OrderBy(x => x.SymbolName);

				Table htbl = createHeader();
				WatchlistDiv.Controls.Add(htbl);

				foreach (WatchListItem item in wl.items)
				{
					Table wltbl = createWatchlistTable(item);
					WatchlistDiv.Controls.Add(wltbl);
				}
			}
			else
			{
				HtmlGenericControl empty = new HtmlGenericControl("h2");
				empty.InnerText = "This list is empty. Why not add a symbol to watch?";
				WatchlistDiv.Controls.Add(empty);
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

		private Table createWatchlistTable(WatchListItem item)
		{
			Table tbl = new Table();
			double currentPrice = 0;
			double previousPrice = 0;
			double priceChange = 0;
			DateTime date = new DateTime();

			quotes = wlm.GetQuotes(item.SymbolName);
			quotes.OrderBy(x => x.timestamp);

			foreach (Quote q in quotes)
			{
				date = quotes.Select(x => x.timestamp).FirstOrDefault();
				currentPrice = quotes.Select(x => x.price).FirstOrDefault();
				previousPrice = quotes.Select(x => x.price).Skip(1).FirstOrDefault();
			}

			TableRow row = new TableRow();
			for (int i = 0; i < headers.Length; i++)
			{
				TableCell cell = new TableCell();
				row.Cells.Add(cell);
			}

			// TODO get long name info from quote manager - should it be stored in the database?
			string fullName = "long name goes here";
			row.Cells[0].Text = item.SymbolName;
			row.Cells[0].Text += new HtmlString(String.Format(" <span class='subtext'>({0})</span>", fullName));
			row.Cells[1].Text = currentPrice.ToString("N2") + " as of " + date.ToShortDateString();
			priceChange = currentPrice - previousPrice;

			if (priceChange > 0)
			{
				string prefix = "+ ";
				row.Cells[2].Text = prefix;
				row.Cells[2].CssClass = "green";
				row.Cells[3].Text = prefix;
				row.Cells[3].CssClass = "green";
			}
			if (priceChange < 0)
			{
				string prefix = "- ";
				row.Cells[2].Text = prefix;
				row.Cells[2].CssClass = "red";
				row.Cells[3].Text = prefix;
				row.Cells[3].CssClass = "red";
			}

			row.Cells[2].Text += Math.Abs(priceChange).ToString("N2");
			row.Cells[3].Text += Math.Abs(priceChange / previousPrice * 100).ToString("N2") + "%";

			// create Remove button for each row
			Button btnRemove = new Button();
			btnRemove.Attributes.Add("Symbol", item.SymbolName);
			btnRemove.Attributes.Add("ListName", item.ListName);
			btnRemove.ID = "btnRemove" + item.SymbolName;
			btnRemove.Text = "Remove";
			btnRemove.Click += btnRemove_Click;
			row.Cells[headers.Length - 1].Controls.Add(btnRemove);

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

		private void updateList()
		{
			showWatchList(radioLists.SelectedValue);
		}

		#region Controls
		protected void btnRemove_Click(object sender, EventArgs e)
		{
			bool success = false;
			string symbol = ((Button)sender).Attributes["Symbol"];
			string listName = ((Button)sender).Attributes["ListName"];

			if (symbol.Length > 0 && listName.Length > 0)
			{
				success = wl.RemoveFromList(new Symbol(symbol), listName);

				if (success)
				{
					setStatus(String.Format("{0} removed from list {1}.", symbol, listName), true);
				}
				
			}
			updateList();
		}

		protected void btnAddToWatchList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string symbol = tbAddToWatchList.Text.Trim().ToUpper();
			string listName = radioLists.SelectedValue;

			if (symbol.Length > 0)
			{
				success = wl.AddToList(new Symbol(symbol), listName);
				if (success)
				{
					setStatus(String.Format("{0} added to list {1}.", symbol, listName), true);
				}
				else
				{
					setStatus(String.Format("{0} could not be added to {1}.", symbol, listName), false);
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
				updateList();
			}
			else
			{
				setStatus("Stock symbol cannot be blank", false);
			}
		}

		protected void btnDeleteList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string listName = radioLists.SelectedValue;

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
					setStatus(String.Format("List {0} deleted successfully.", listName), true);
				}
				else
				{
					setStatus(String.Format("List {0} could not be deleted.", listName), false);
				}
				listWatchLists();
				updateList();
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
						setStatus(String.Format("Added list '{0}'", listName), true);
					}
					else
					{
						setStatus(String.Format("'{0}' already exists.", listName), false);
					}
					listWatchLists();
				}
				else
				{
					setStatus("Sorry! You can't have a custom list with that name.", false);
				}
			}
		}

		protected void radioLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateList();
		}
		#endregion
	}
}