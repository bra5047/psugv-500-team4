using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTraderSite.Portfolio.Client;
using System.IO;

namespace AlgoTraderSite
{
	public partial class MyPortfolio : Page
	{
		private PortfolioManagerClient portfolio;
		private string[] pheaders = { "", "Company", "Quantity", "Price", "Status", "Actions" };
		private string[] theaders = { "", "Date", "Quantity", "Price", "Type", "" };
		private string[] widths = { "5%", "41%", "11%", "11%", "11%", "11%" };
		private int columns = 6;
		private int tcolumns = 6;

		protected void Page_Load(object sender, EventArgs e)
		{
			showPositions();
		}

		private void showPositions()
		{
			portfolio = new PortfolioManagerClient();
			PortfolioDiv.Controls.Add(createHeader());

			foreach (PositionMessage p in portfolio.GetOpenPositions().OrderBy(x => x.SymbolName))
			{
				Table ptbl = createPositionTable(p);
				Table ttbl = createTradeTable(p);

				TableRow containerRow = new TableRow();
				TableCell containerCell = new TableCell(); // create container cell for trade table
				containerCell.ColumnSpan = columns; // make the cell span the width of the positions table
				containerRow.CssClass = "TradeRow";
				containerRow.Attributes["style"] = "visibility:hidden; display:none";

				containerCell.Controls.Add(ttbl); // add trade table to cell
				containerRow.Cells.Add(containerCell); // add cell to row
				ptbl.Rows.Add(containerRow); // add row to position table
				PortfolioDiv.Controls.Add(ptbl); // add position table to div
			}
		}

		private Table createHeader()
		{
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
			tbl.Width = new Unit("100%");

			TableRow row = new TableRow();
			for (int i = 0; i < columns; i++)
			{
				TableCell cell = new TableCell();
				cell.Width = new Unit(widths[i]);
				row.Cells.Add(cell);
			}

			row.Cells[0].Text = "+";// TODO replace with expander image or text;
			row.Cells[1].Text += new HtmlString(pm.SymbolName + " <span class='subtext'>(" + fullName + ")</span>");
			row.Cells[2].Text = String.Format("{0:N0}", pm.Quantity);
			row.Cells[3].Text = String.Format("{0:C}", pm.Price);
			row.Cells[4].Text = pm.Status.ToString();
			row.Cells[4].CssClass = getCssClass(pm.Status.ToString());

			// BUTTONS
			Button btnToggle = new Button();
			btnToggle.Text = "+";
			btnToggle.Width = new Unit("80%");
			btnToggle.OnClientClick = "return false";
			btnToggle.CssClass = "toggle";
			btnToggle.UseSubmitBehavior = false;
			row.Cells[0].Controls.Add(btnToggle);

			Button btnAction = new Button();
			btnAction.Text = "Buy / Sell";
			btnAction.Attributes["SymbolName"] = pm.SymbolName;
			btnAction.Attributes["LastPrice"] = lastPrice.ToString();
			btnAction.UseSubmitBehavior = false;
			btnAction.Click += new EventHandler(btnClick);
			row.Cells[columns - 1].Controls.Add(btnAction);

			tbl.Rows.Add(row);

			return tbl;
		}

		private Table createTradeTable(PositionMessage pm)
		{
			Table tbl = new Table();
			tbl.Width = new Unit("100%");

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
				trow.Cells[1].Text = t.Timestamp.ToString();
				trow.Cells[1].Text += new HtmlString(String.Format(" <span class='subtext'>({0})</span>", getTimeSpan(t.Timestamp)));
				trow.Cells[2].Text = String.Format("{0:N0}", t.Quantity);
				trow.Cells[3].Text = String.Format("{0:C}", t.Price);
				trow.Cells[4].Text = t.Type.ToString();
				trow.Cells[4].CssClass = getCssClass(t.Type.ToString());

				tbl.Rows.Add(trow);
			}

			return tbl;
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

		protected void btnClick(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			Response.Redirect("BuySell.aspx?s=" + btn.Attributes["SymbolName"] + "&p=" + btn.Attributes["LastPrice"]);
		}
	}
}