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
		private string[] pheaders = { "", "COMPANY", "QUANTITY", "PRICE", "STATUS", "ACTIONS" };
		private string[] theaders = { "", "DATE", "QUANTITY", "PRICE", "TYPE", "" };
		private string[] widths = { "5%", "41%", "11%", "11%", "11%", "11%" };
		private int columns = 6;
		private int tcolumns = 6;

		protected void Page_Load(object sender, EventArgs e)
		{
			#region old code
			//TreeNode root = new TreeNode("Portfolio");
			//portfolio = new PortfolioManagerClient();

			//foreach (PositionMessage p in portfolio.GetOpenPositions())
			//{
			//	string s = String.Format("{0}     {1}     ${2}     {3}", p.SymbolName, p.Quantity, p.Price, p.Status);
			//	TreeNode n = new TreeNode(s);

			//	double lastPrice = 0;

			//	foreach (TradeMessage t in p.Trades)
			//	{
			//		string tstr = String.Format("{0}     {1}     ${2}     {3}", t.Timestamp.ToString(), t.Quantity.ToString(), t.Price.ToString(), t.Type.ToString());
			//		n.ChildNodes.Add(new TreeNode(tstr));
			//		lastPrice = t.Price;
			//	}
			//	n.NavigateUrl = "BuySell.aspx?s=" + p.SymbolName + "&p=" + lastPrice.ToString();
			//	root.ChildNodes.Add(n);
			//}
			//PortfolioTree.Nodes.Clear();
			//PortfolioTree.Nodes.Add(root);
			#endregion

			portfolio = new PortfolioManagerClient();

			TableHeaderRow pheader = new TableHeaderRow();
			for (int i = 0; i < columns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				cell.Text = pheaders[i];
				cell.Width = new Unit(widths[i]);
				pheader.Cells.Add(cell);
			}
			PortfolioTable.Rows.Add(pheader);

			foreach (PositionMessage p in portfolio.GetOpenPositions().OrderBy(x => x.SymbolName))
			{
				double lastPrice = 0;

				TableRow prow = new TableRow();
				for (int i = 0; i < columns; i++)
				{
					TableCell cell = new TableCell();
					prow.Cells.Add(cell);
				}
				// TODO replace with real company name
				string fullName = " (" + "Full name" + ")";
				string fullNameStyle = "style='color:gray; font-weight:300'";
				// TODO replace with expander image or text;
				prow.Cells[0].Text = "+";
				prow.Cells[1].Text = p.SymbolName;
				prow.Cells[1].Text += new HtmlString("<span " + fullNameStyle + ">" + fullName + "</span>");
				prow.Cells[2].Text = p.Quantity.ToString();
				prow.Cells[3].Text = "$" + p.Price.ToString();
				prow.Cells[4].Text = p.Status.ToString();

				// make a table for the trades, to be nested in positions table
				Table ttbl = new Table();
				ttbl.Width = new Unit("100%");
				TableRow tContainerRow = new TableRow();
				TableCell tContainerCell = new TableCell(); // create container cell for trade table
				//tContainerRow.Width = new Unit("100%");
				tContainerCell.ColumnSpan = columns; // make the cell span the width of the positions table - 1

				// Add trade header row
				TableHeaderRow theader = new TableHeaderRow();
				for (int i = 0; i < tcolumns; i++)
				{
					TableHeaderCell cell = new TableHeaderCell();
					cell.Text = theaders[i];
					cell.Width = new Unit(widths[i]);
					theader.Cells.Add(cell);
				}
				ttbl.Rows.Add(theader);

				// Add trade data rows
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

				// create button to buy/sell stuff
				Button btnAction = new Button();
				btnAction.Text = "Buy / Sell";
				btnAction.Attributes["SymbolName"] = p.SymbolName;
				btnAction.Attributes["LastPrice"] = lastPrice.ToString();
				btnAction.Click += new EventHandler(btnClick);
				prow.Cells[columns - 1].Controls.Add(btnAction);

				PortfolioTable.Rows.Add(prow);
				tContainerCell.Controls.Add(ttbl);
				tContainerRow.Cells.Add(tContainerCell);
				PortfolioTable.Rows.Add(tContainerRow);
			}
		}

		private string getTimeSpan(DateTime timestamp)
		{
			TimeSpan timespan = (DateTime.Now - timestamp);

			if (timespan.TotalSeconds < 60) // less than 1 minute
			{
				return timespan.Seconds + " seconds ago";
			}
			else if (timespan.TotalMinutes < 60) // less than 1 hour
			{
				return timespan.Minutes + " minutes ago";
			}
			else if (timespan.TotalHours < 24) // less than 24 hours
			{
				return timespan.Hours + " hours ago";
			}
			else // over a day
			{
				return timespan.Days + " days ago";
			}
		}

		protected void btnClick(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			Response.Redirect("BuySell.aspx?s=" + btn.Attributes["SymbolName"] + "&p=" + btn.Attributes["LastPrice"]);
		}
	}
}