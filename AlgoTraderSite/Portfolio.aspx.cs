using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTraderSite.Portfolio.Client;
using System.IO;

namespace AlgoTraderSite
{
	public partial class MyPortfolio : Page
	{
		private PortfolioManagerClient portfolio = new PortfolioManagerClient();
		private static List<PositionMessage> positions = new List<PositionMessage>();
		private string[] transheaders = { "Date", "Company", "Shares", "Price", "Type" };
		private string[] widths = { "40px", "", "13%", "13%", "13%", "13%" };
		private string[] transwidths = { "20%", "20%", "20", "20%", "20%" };
		private int columns = 6;
		private int tcolumns = 6;
		private int transcolumns = 5;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				generatePositions();
				radioSortType.SelectedIndex = 0;
			}
			update();
		}

		private void generatePositions()
		{
			positions = portfolio.GetOpenPositions().ToList();
		}

		private void showTransactions()
		{
			PortfolioDiv.Controls.Clear();
		}

		private void showPositions()
		{
			PortfolioDiv.Controls.Clear();
			PortfolioDiv.Controls.Add(createPositionHeader());
			positions = sortList();
			foreach (PositionMessage p in positions)
			{
				Table ptbl = createPositionTable(p);
				Table ttbl = createTradeTable(p);

				TableRow containerRow = new TableRow();
				TableCell containerCell = new TableCell(); // create container cell for trade table
				containerCell.ColumnSpan = columns; // make the cell span the width of the positions table
				containerRow.CssClass = "TradeRow";
				//containerRow.Attributes["style"] = "visibility:hidden; display:none";

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

		private Table createPositionHeader()
		{
			string[] pheaders = { "", "Company", "Shares", "Price", "Status", "Actions" };
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

		private Table createPositionTable(PositionMessage pm)
		{
			double lastPrice = pm.Trades.OrderByDescending(x=>x.Timestamp).Select(x=>x.Price).FirstOrDefault();
			string fullName = "Full name"; // TODO replace with real company name

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
			row.Cells[3].Text = String.Format("{0:C}", pm.Price);
			row.Cells[4].Text = pm.Status.ToString();
			row.Cells[4].CssClass = getCssClass(pm.Status.ToString());

			// BUTTONS
			HtmlGenericControl btnToggle = new HtmlGenericControl("div");
			btnToggle.Attributes.Add("class", "toggle icon-plus-sign");
			row.Cells[0].Controls.Add(btnToggle);

			Button btnAction = new Button();
			btnAction.CssClass = "symbol-button";
			btnAction.ToolTip = "Buy/sell";
			btnAction.Text = HttpUtility.HtmlDecode("&#xe015;");
			btnAction.Attributes["SymbolName"] = pm.SymbolName;
			btnAction.Attributes["LastPrice"] = lastPrice.ToString();
			btnAction.Click += new EventHandler(btnClick);
			row.Cells[columns - 1].Controls.Add(btnAction);

			tbl.Rows.Add(row);

			// css stuff
			tbl.CssClass = "main";
			row.CssClass = "main";

			row.Cells[(radioSortType.SelectedIndex / 2) + 1].CssClass = "bold";

			return tbl;
		}

		private Table createTradeTable(PositionMessage pm)
		{
			string[] theaders = { "", "Date", "Shares", "Price", "Type", "" };
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

			foreach (TradeMessage t in pm.Trades.OrderByDescending(x => x.Timestamp))
			{
				TableRow trow = new TableRow();
				for (int i = 0; i < tcolumns; i++)
				{
					TableCell cell = new TableCell();
					trow.Cells.Add(cell);
				}
				trow.Cells[1].Text = String.Format("{0:d} at {0:T}", t.Timestamp);
				trow.Cells[1].Text += new HtmlString(String.Format(" <span class='subtext'>({0})</span>", getTimeSpan(t.Timestamp)));
				trow.Cells[2].Text = String.Format("{0:N0}", t.Quantity);
				trow.Cells[3].Text = String.Format("{0:C}", t.Price);
				trow.Cells[4].Text = t.Type.ToString();
				trow.Cells[4].CssClass = getCssClass(t.Type.ToString());

				tbl.Rows.Add(trow);
			}

			// css stuff
			tbl.CssClass = "sub";

			return tbl;
		}

		private List<PositionMessage> sortList()
		{
			switch (radioSortType.SelectedIndex)
			{
				case 0: return positions.OrderBy(x => x.SymbolName).ToList(); // name asc
				case 1: return positions.OrderByDescending(x => x.SymbolName).ToList(); // name desc
				case 2: return positions.OrderByDescending(x => x.Quantity).ToList(); // shares desc
				case 3: return positions.OrderBy(x => x.Quantity).ToList(); // shares asc
				case 4: return positions.OrderByDescending(x => x.Price).ToList(); // value desc
				case 5: return positions.OrderBy(x => x.Price).ToList(); // value asc
				default: return positions;
			}
		}

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

		private void update()
		{
			switch (radioLists.SelectedIndex)
			{
				case 0: showPositions(); break;
				case 1: showTransactions(); break;
				default: break;
			}
		}

		#region Controls
		protected void btnClick(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			Response.Redirect("BuySell.aspx?s=" + btn.Attributes["SymbolName"] + "&p=" + btn.Attributes["LastPrice"]);
		}

		protected void btnExpand_Click(object sender, EventArgs e)
		{

		}

		protected void radioLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			update();
		}
	
		protected void radioSortType_SelectedIndexChanged(object sender, EventArgs e)
		{
			update();
		}
		#endregion
	}
}