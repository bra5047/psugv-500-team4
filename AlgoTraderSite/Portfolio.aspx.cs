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
			portfolio = new PortfolioManagerClient();

			Table tblHeader = new Table();
			tblHeader.ID = "Header";
			tblHeader.Width = new Unit("100%");
			TableHeaderRow pheader = new TableHeaderRow();
			for (int i = 0; i < columns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				cell.Text = pheaders[i];
				cell.Width = new Unit(widths[i]);
				pheader.Cells.Add(cell);
			}
			tblHeader.Rows.Add(pheader);
			PortfolioDiv.Controls.Add(tblHeader);

			foreach (PositionMessage p in portfolio.GetOpenPositions().OrderBy(x => x.SymbolName))
			{
				double lastPrice = 0;

				// create a new table for each position
				Table positionTable = new Table();
				positionTable.ID = p.SymbolName;
				positionTable.Width = new Unit("100%");

				// position row
				TableRow prow = new TableRow();
				for (int i = 0; i < columns; i++)
				{
					TableCell cell = new TableCell();
					cell.Width = new Unit(widths[i]);
					prow.Cells.Add(cell);
				}

				// TODO replace with real company name
				string fullName = " (" + "Full name" + ")";
				string fullNameStyle = "style='color:gray; font-weight:300'";

				// TODO replace with expander image or text;
				//prow.Cells[0].Text = "+";
				prow.Cells[1].Text = p.SymbolName;
				prow.Cells[1].Text += new HtmlString("<span " + fullNameStyle + ">" + fullName + "</span>");
				prow.Cells[2].Text = p.Quantity.ToString();
				prow.Cells[3].Text = "$" + p.Price.ToString();
				prow.Cells[4].Text = p.Status.ToString();


				// TRADE TABLE STUFF
				// make a table for the trades, to be nested in positions table
				Table ttbl = new Table();
				ttbl.Width = new Unit("100%");
				TableRow tContainerRow = new TableRow();
				TableCell tContainerCell = new TableCell(); // create container cell for trade table
				tContainerCell.ColumnSpan = columns; // make the cell span the width of the positions table

				// trade header
				TableHeaderRow theader = new TableHeaderRow();
				for (int i = 0; i < tcolumns; i++)
				{
					TableHeaderCell cell = new TableHeaderCell();
					cell.Text = theaders[i];
					cell.Width = new Unit(widths[i]);
					theader.Cells.Add(cell);
				}
				ttbl.Rows.Add(theader);

				// trade rows
				foreach (TradeMessage t in p.Trades.OrderByDescending(x => x.Timestamp))
				{
					TableRow trow = new TableRow();
					for (int i = 0; i < tcolumns; i++)
					{
						TableCell cell = new TableCell();
						trow.Cells.Add(cell);
					}

					string style = "style='color:gray; font-weight: 300'";
					trow.Cells[1].Text = t.Timestamp.ToString();
					trow.Cells[1].Text += new HtmlString(String.Format(" <span {0}>({1})</span>", style, getTimeSpan(t.Timestamp)));
					trow.Cells[2].Text = t.Quantity.ToString();
					trow.Cells[3].Text = "$" + t.Price.ToString();
					trow.Cells[4].Text = t.Type.ToString();
					ttbl.Rows.Add(trow);
					lastPrice = t.Price;
				}

				// Buttons
				Button btnToggle = new Button();
				btnToggle.Text = "+";
				btnToggle.Attributes["SymbolName"] = p.SymbolName;
				prow.Cells[0].Controls.Add(btnToggle);

				Button btnAction = new Button();
				btnAction.Text = "Buy / Sell";
				btnAction.Attributes["SymbolName"] = p.SymbolName;
				btnAction.Attributes["LastPrice"] = lastPrice.ToString();
				btnAction.Click += new EventHandler(btnClick);
				prow.Cells[columns - 1].Controls.Add(btnAction);

				positionTable.Rows.Add(prow);
				tContainerCell.Controls.Add(ttbl);
				tContainerRow.Cells.Add(tContainerCell);
				positionTable.Rows.Add(tContainerRow);
				PortfolioDiv.Controls.Add(positionTable);
			}

		}

		private string getTimeSpan(DateTime timestamp)
		{
			TimeSpan timespan = (DateTime.Now - timestamp);
			string output = null;
			string single = " ago";
			string plural = "s ago";

			if (timespan.TotalSeconds < 60) // less than 1 minute
			{
				if (timespan.Seconds == 1)
				{
					output = single;
				}
				else
				{
					output = plural;
				}
				return timespan.Seconds + "second" + output;
			}
			else if (timespan.TotalMinutes < 60) // less than 1 hour
			{
				if (timespan.Minutes == 1)
				{
					output = single;
				}
				else
				{
					output = plural;
				}
				return timespan.Minutes + " minute" + output;
			}
			else if (timespan.TotalHours < 24) // less than 24 hours
			{
				if (timespan.Hours == 1)
				{
					output = single;
				}
				else
				{
					output = plural;
				}
				return timespan.Hours + " hour" + output;
			}
			else // over a day
			{
				if (timespan.Days == 1)
				{
					output = single;
				}
				else
				{
					output = plural;
				}

				return timespan.Days + " day" + output;
			}
		}

		private void toggleTable(object sender, EventArgs e)
		{
			Table tbl = (Table)sender;
			if (tbl.Rows[1].Visible == true)
			{
				tbl.Rows[1].Visible = false;
				
			}
			else
			{
				tbl.Rows[1].Visible = true;
			}
		}

		protected void btnClick(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			Response.Redirect("BuySell.aspx?s=" + btn.Attributes["SymbolName"] + "&p=" + btn.Attributes["LastPrice"]);
		}
	}
}