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
		private static IWatchList wl;
		private static List<Quote> quotes = new List<Quote>();
		string[] headers = { "COMPANY", "PRICE", "CHANGE", "CHANGE %", "ACTIONS" };
		string[] widths = { "40%", "20%", "15%", "15%", "10%" };

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				listWatchLists();
			}
			showWatchList();
		}

		public void listWatchLists()
		{
			ddlistWatchLists.Items.Clear();
			List<WatchList> watchlists = new List<WatchList>();
			watchlists = wlm.GetAllWatchLists().OrderBy(x => x.ListName).ToList();

			foreach (WatchList item in watchlists)
			{
				ListItem li = new ListItem();
				li.Text = item.ListName;
				li.Value = item.ListName;
				ddlistWatchLists.Items.Add(li);
			}
		}

		public void showWatchList()
		{
			string lName = ddlistWatchLists.SelectedValue;
			WatchlistDiv.Controls.Clear();

			Table htbl = createHeader();
			WatchlistDiv.Controls.Add(htbl);

			wl = wlm.GetWatchList(lName);
			wl.items.OrderBy(x => x.SymbolName);

			foreach (WatchListItem item in wl.items)
			{
				Table wltbl = createWatchlistTable(item);
				WatchlistDiv.Controls.Add(wltbl);
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

			// get the quotes
			quotes = wlm.GetQuotes(item.SymbolName);
			quotes.OrderBy(x => x.timestamp);

			foreach (Quote q in quotes)
			{
				date = quotes.Select(x => x.timestamp).FirstOrDefault();
				currentPrice = quotes.Select(x => x.price).FirstOrDefault();
				previousPrice = quotes.Select(x => x.price).Skip(1).FirstOrDefault();
			}

			// create the row and cells
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
				string style = "color:red";
				row.Cells[2].Text = prefix;
				row.Cells[2].Attributes["style"] = style;
				row.Cells[3].Text = prefix;
				row.Cells[3].Attributes["style"] = style;
			}

			row.Cells[2].Text += Math.Abs(priceChange).ToString("N2");
			row.Cells[3].Text += Math.Abs(priceChange / previousPrice * 100).ToString("N2") + "%";

			// create Remove button for each row
			Button btnRemove = new Button();
			btnRemove.Attributes["Symbol"] = item.SymbolName;
			btnRemove.Attributes["ListName"] = item.ListName;
			btnRemove.Text = "Remove";
			btnRemove.ID = "btn" + item.SymbolName;
			btnRemove.Click += new EventHandler(btnRemove_Click);
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

		protected void setStatus(string msg)
		{
			statusMessage.Controls.Clear();
			statusMessage.InnerText = msg;
		}

		protected void updateList()
		{
			showWatchList();
		}

		#region Controls
		protected void btnRemove_Click(object sender, EventArgs e)
		{
			bool success = false;
			string symbol = ((Button)sender).Attributes["Symbol"];
			string listName = ((Button)sender).Attributes["ListName"];

			success = wl.RemoveFromList(new Symbol(symbol), listName);

			if (success)
			{
				setStatus(String.Format("{0} removed from list {1}.", symbol, listName));
			}
			updateList();
		}

		protected void btnAddToWatchList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string symbol = tbAddToWatchList.Text.Trim().ToUpper();
			string listName = ddlistWatchLists.SelectedValue;

			if (symbol.Length > 0)
			{
				success = wl.AddToList(new Symbol(symbol), listName);
				if (success)
				{
					setStatus(String.Format("{0} added to list {1}.", symbol, listName));
				}
				else
				{
					setStatus(String.Format("{0} could not be added to {1}.", symbol, listName));
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
			}
			updateList();
		}

		protected void ddlistWatchLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateList();
		}

		protected void btnDeleteList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string listName = ddlistWatchLists.SelectedValue;

			foreach (WatchListItem w in wl.items.Where(x => x.SymbolName.Equals(listName)))
			{
				wl.RemoveFromList(w.Symbol, w.ListName);
			}

			success = wlm.DeleteWatchList(listName);

			if (success)
			{
				setStatus(String.Format("List {0} deleted successfully.", listName));
			}
			else
			{
				setStatus(String.Format("List {0} could not be deleted.", listName));
			}
			listWatchLists();
			updateList();
		}

		protected void btnAddList_Click(object sender, EventArgs e)
		{
			bool success = false;
			string listName = tbAddList.Text;
			success = wlm.AddWatchList(listName);

			if (success)
			{
				setStatus(String.Format("Added list '{0}'", listName));
			} else {
				setStatus(String.Format("'{0}' could not be added.", listName));
			}
			listWatchLists();
		}
		#endregion


	}
}