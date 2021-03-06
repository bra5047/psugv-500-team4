﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTraderSite.Portfolio.Client;
using AlgoTrader.watchlist;
using System.IO;

namespace AlgoTraderSite
{
	public partial class MyPortfolio : Page
	{
		private PortfolioManagerClient portfolio = new PortfolioManagerClient();
		private static List<PositionMessage> openpositions = new List<PositionMessage>();
		private static List<PositionMessage> allpositions = new List<PositionMessage>();
		private static List<Trade> transactions = new List<Trade>();
		private string[] widths = { "40px", "", "10%", "25%", "10%", "10%", "10%" };
		private int columns = 7;
		private int tcolumns = 7;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				generatePositions();
				radioSortType.SelectedIndex = 0;
				update();
			}
			update();
		}

		/// <summary>
		/// Gets the open positions and transactions.
		/// </summary>
		private void generatePositions()
		{
			openpositions.Clear();
			allpositions.Clear();
			transactions.Clear();
			openpositions = portfolio.GetOpenPositions().ToList();
			AvailableCash.Text = String.Format("{0:C}", portfolio.getAvailableCash());

			TraderContext db = new TraderContext();
			var query = db.Positions.Select(x => x.SymbolName);
			foreach (string s in query)
			{
				PositionMessage msg = portfolio.GetPosition(s);
				allpositions.Add(msg);
			}

			// get the transactions
			var transquery = db.Trades.Select(x => x);
			foreach (Trade t in transquery)
			{
				transactions.Add(t);
			}
		}

		/// <summary>
		/// Creates a table for each transaction and displays them on the page.
		/// </summary>
		private void showTransactions()
		{
			transactions = transactions.OrderByDescending(x => x.timestamp).ToList();
			string[] transheaders = { "Date", "Company", "Shares", "Price", "Fees", "Type" };
			string[] transwidths = { "20%", "16%", "16%", "16%", "16%", "16%" };
			int transcolumns = 6;

			PortfolioDiv.Controls.Clear();
			Table htbl = new Table();
			TableHeaderRow hr = new TableHeaderRow();
			for (int i = 0; i < transcolumns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				cell.Text = transheaders[i];
				cell.Width = new Unit(transwidths[i]);
				hr.Cells.Add(cell);
			}
			htbl.Rows.Add(hr);
			PortfolioDiv.Controls.Add(htbl);

			foreach (Trade t in transactions)
			{
				Table tbl = new Table();
				TableRow tr = new TableRow();
				for (int i = 0; i < transcolumns; i++)
				{
					TableCell cell = new TableCell();
					cell.Width = new Unit(transwidths[i]);
					tr.Cells.Add(cell);
				}
				tr.Cells[0].Text = t.timestamp.ToString();
				tr.Cells[1].Text = t.SymbolName;
				tr.Cells[2].Text = t.InitialQuantity.ToString();
				tr.Cells[3].Text = String.Format("{0:C}", t.price);
				tr.Cells[4].Text = String.Format("{0:C}", t.PaidCommission);
				tr.Cells[5].Text = t.type.ToString();
				tbl.Rows.Add(tr);
				tbl.CssClass = "main";
				PortfolioDiv.Controls.Add(tbl);
			}
		}

		/// <summary>
		/// Creates a table for each position and displays them on the page.
		/// </summary>
		private void showPositions()
		{
			PortfolioDiv.Controls.Clear();
			PortfolioDiv.Controls.Add(createPositionHeader());
			openpositions = sortList();
			foreach (PositionMessage p in openpositions)
			{
				Table ptbl = createPositionTable(p);
				Table ttbl = createTradeTable(p);

				TableRow containerRow = new TableRow();
				TableCell containerCell = new TableCell(); // create container cell for trade table
				containerCell.ColumnSpan = columns; // make the cell span the width of the positions table
				containerRow.CssClass = "TradeRow";

				HtmlGenericControl div = new HtmlGenericControl("div");
				div.Attributes.Add("class", "TradeDiv");
				div.Attributes.Add("style", "visibility: hidden; display: none");
				div.Controls.Add(ttbl);
				containerCell.Controls.Add(div); // add trade table to cell
				containerRow.Cells.Add(containerCell); // add cell to row
				ptbl.Rows.Add(containerRow); // add row to position table
				PortfolioDiv.Controls.Add(ptbl); // add position table to div
			}
		}

		/// <summary>
		/// Creates the Positions header at the top of the page.
		/// </summary>
		/// <returns>Table</returns>
		private Table createPositionHeader()
		{
			string[] pheaders = { "", "Company", "Shares", "Price / Gain / Gain %", "Fees", "Status", "Actions" };
			Table tbl = new Table();
			tbl.Width = new Unit("100%");
			TableHeaderRow header = new TableHeaderRow();
			for (int i = 0; i < columns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				cell.Text = pheaders[i];
				cell.Width = new Unit(widths[i]);
				header.Cells.Add(cell);
			}
			tbl.Rows.Add(header);

			return tbl;
		}

		/// <summary>
		/// Creates the position table.
		/// </summary>
		/// <param name="pm">PositionMessage</param>
		/// <returns>Table</returns>
		private Table createPositionTable(PositionMessage pm)
		{
			string fullName = string.Empty;
			double gain = 0;
			double gainPercent = 0;
			string classname = string.Empty;

			IWatchListManager wlm = new WatchListManager();
			fullName = wlm.GetLongName(pm.SymbolName);

			TraderContext db = new TraderContext();
			double latestQuote = db.Quotes.Where(x => x.SymbolName.Equals(pm.SymbolName)).OrderByDescending(x => x.timestamp).Select(x => x.price).FirstOrDefault();
			gain = (latestQuote * pm.Quantity) - pm.Price;
			gainPercent = gain / (pm.Price);

			if (gain > 0)
			{
				classname = "green";
			}
			if (gain < 0)
			{
				classname = "red";
			}

			Table tbl = new Table();
			TableRow row = new TableRow();

			for (int i = 0; i < columns; i++)
			{
				TableCell cell = new TableCell();
				cell.Width = new Unit(widths[i]);
				row.Cells.Add(cell);
			}

			row.Cells[1].Text += new HtmlString(String.Format("{0} <span class='subtext'>({1})</span>", pm.SymbolName, fullName));
			row.Cells[2].Text = String.Format("{0:N0}", pm.Quantity);
			row.Cells[3].Text = new HtmlString(String.Format("{0:C} / <span class='{3}'>{1:C}</span> / <span class='{3}'>{2:P2}</span>", pm.Price, gain, gainPercent, classname)).ToString();
			row.Cells[4].Text = String.Format("{0:C}", pm.Trades.Sum(x => x.PaidCommission));
			row.Cells[5].Text = pm.Status.ToString();
			row.Cells[5].CssClass = getCssClass(pm.Status.ToString());

			// BUTTONS
			HtmlGenericControl btnToggle = new HtmlGenericControl("div");
			btnToggle.Attributes.Add("class", "toggle icon-plus-sign");
			row.Cells[0].Controls.Add(btnToggle);

			Button btnAction = new Button();
			btnAction.CssClass = "symbol-button";
			btnAction.ToolTip = "Buy/sell";
			btnAction.Text = HttpUtility.HtmlDecode("&#xe015;");
			btnAction.Attributes["SymbolName"] = pm.SymbolName;
			btnAction.Click += new EventHandler(btnClick);
			row.Cells[columns - 1].Controls.Add(btnAction);

			// css stuff
			tbl.CssClass = "main";
			row.CssClass = "main";
			row.Cells[(radioSortType.SelectedIndex / 2) + 1].CssClass = "bold";

			tbl.Rows.Add(row);
			return tbl;
		}

		/// <summary>
		/// Creates a table for all of the Trades associated with a specific Position.
		/// </summary>
		/// <param name="pm">PositionMessage</param>
		/// <returns>Table</returns>
		private Table createTradeTable(PositionMessage pm)
		{
			string[] theaders = { "", "Date", "Shares", "Price / Gain / Gain %", "Fees", "Type", "" };
			Table tbl = new Table();

			TableHeaderRow header = new TableHeaderRow();
			for (int i = 0; i < tcolumns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				cell.Text = theaders[i];
				cell.Width = new Unit(widths[i]);
				header.Cells.Add(cell);
			}
			tbl.Rows.Add(header);

			TraderContext db = new TraderContext();
			double latestQuote = db.Quotes.Where(x => x.SymbolName.Equals(pm.SymbolName)).OrderByDescending(x => x.timestamp).Select(x => x.price).FirstOrDefault();
			foreach (TradeMessage t in pm.Trades.OrderByDescending(x => x.Timestamp).Where((x) => x.Quantity > 0 && x.Type==Portfolio.Client.tradeTypes.Buy))
			{
				double gain = 0;
				double gainPercent = 0;
				string classname = string.Empty;

				TableRow trow = new TableRow();
				for (int i = 0; i < tcolumns; i++)
				{
					TableCell cell = new TableCell();
					trow.Cells.Add(cell);
				}

				gain = latestQuote - t.Price;
				gainPercent = gain / t.Price;
				if (gain > 0)
				{
					classname = "green";
				}
				if (gain < 0)
				{
					classname = "red";
				}

				trow.Cells[1].Text = String.Format("{0:d} at {0:T}", t.Timestamp); // symbol name
				trow.Cells[1].Text += new HtmlString(String.Format(" <span class='subtext'>({0})</span>", getTimeSpan(t.Timestamp))); // date
				trow.Cells[2].Text = String.Format("{0:N0}", t.Quantity); // quantity
				trow.Cells[3].Text = new HtmlString(String.Format("{0:C} / <span class='{3}'>{1:C}</span> / <span class='{3}'>{2:P2}</span>", t.Price, gain, gainPercent, classname)).ToString();
				trow.Cells[4].Text = String.Format("{0:C}", t.PaidCommission); // broker fee
				trow.Cells[5].Text = t.Type.ToString();
				trow.Cells[5].CssClass = getCssClass(t.Type.ToString());

				tbl.Rows.Add(trow);
			}

			// css stuff
			tbl.CssClass = "sub";

			return tbl;
		}
		
		/// <summary>
		/// Sorts the list of positions based on the selected index of the sort radioList input.
		/// </summary>
		/// <returns>List of PositionMessage objects</returns>
		private List<PositionMessage> sortList()
		{
			switch (radioSortType.SelectedIndex)
			{
				case 0: return openpositions.OrderBy(x => x.SymbolName).ToList(); // name asc
				case 1: return openpositions.OrderByDescending(x => x.SymbolName).ToList(); // name desc
				case 2: return openpositions.OrderByDescending(x => x.Quantity).ToList(); // shares desc
				case 3: return openpositions.OrderBy(x => x.Quantity).ToList(); // shares asc
				case 4: return openpositions.OrderByDescending(x => x.Price).ToList(); // value desc
				case 5: return openpositions.OrderBy(x => x.Price).ToList(); // value asc
				default: return openpositions;
			}
		}

		/// <summary>
		/// Gets the time difference between the current DateTime and the input DateTime.
		/// </summary>
		/// <param name="timestamp">The timestamp that is being compared to the current time.</param>
		/// <returns>A string that says the time difference.</returns>
		private string getTimeSpan(DateTime timestamp)
		{
			TimeSpan timespan = (DateTime.Now - timestamp);
			int time = 0;
			string unit = string.Empty;
			string plural = "s";

			if (timespan.TotalSeconds < 60) // less than 1 minute
			{
				time = timespan.Seconds;
				unit = "second";
			}
			else if (timespan.TotalMinutes < 60) // less than 1 hour
			{
				time = timespan.Minutes;
				unit = "minute";
			}
			else if (timespan.TotalHours < 24) // less than 24 hours
			{
				time = timespan.Hours;
				unit = "hour";
			}
			else // over a day
			{
				time = timespan.Days;
				unit = "day";
			}

			if (time == 1)
			{
				plural = string.Empty;
			}

			return String.Format("{0} {1}{2} ago", time, unit, plural);
		}

		/// <summary>
		/// Gets the appropriate CSS class name based on the input string.
		/// </summary>
		/// <param name="s">string</param>
		/// <returns>string</returns>
		private string getCssClass(string s)
		{
			if (s.Equals("Buy") || s.Equals("Open"))
			{
				return "green";
			}
			else
			{
				return "red";
			}
		}

		/// <summary>
		/// Updates the page based on the selected index.
		/// </summary>
		private void update()
		{
			switch (radioLists.SelectedIndex)
			{
				case 0:
					InputGroup.Visible = true;
					showPositions(); 
					break;
				case 1:
					InputGroup.Visible = false;
					showTransactions(); 
					break;
				default: break;
			}
		}

		#region Controls
		/// <summary>
		/// Redirects to the Buy/Sell page based on the Button's "SymbolName" attribute.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnClick(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			Response.Redirect("BuySell.aspx?s=" + btn.Attributes["SymbolName"]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnExpand_Click(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Updates the page if the selected index changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void radioLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			update();
		}

		/// <summary>
		/// Updates the page if the sort type changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void radioSortType_SelectedIndexChanged(object sender, EventArgs e)
		{
			update();
		}

		/// <summary>
		/// Button event that adds cash to the user's available cash.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAddCash_Click(object sender, EventArgs e)
		{
			double value = double.Parse(InputAddCash.Value);
			TraderContext db = new TraderContext();
			db.Portfolios.FirstOrDefault().Cash += value;
			AvailableCash.Text = String.Format("{0:C}", (double.Parse(AvailableCash.Text.TrimStart('$')) + value));
			db.SaveChanges();
			update();
		}
		#endregion
	}
}