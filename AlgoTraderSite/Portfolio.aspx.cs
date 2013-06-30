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
		private string[] pheaders = { "COMPANY", "QUANTITY", "PRICE", "STATUS", "ACTIONS" };
		private string[] theaders = { "DATE", "QUANTITY", "PRICE", "TYPE" };
		private int columns = 5;
		private int tcolumns = 4;

		protected void Page_Load(object sender, EventArgs e)
		{
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

			portfolio = new PortfolioManagerClient();

			TableHeaderRow pheader = new TableHeaderRow();
			for (int i = 0; i < columns; i++)
			{
				TableHeaderCell cell = new TableHeaderCell();
				cell.Text = pheaders[i];
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

				string fullName = " (" + "Full name" + ")";
				string fullNameStyle = "style='color:gray; font-weight:300'";
				prow.Cells[0].Text = p.SymbolName;
				prow.Cells[0].Text += new HtmlString("<span " + fullNameStyle + ">" + fullName + "</span>");
				prow.Cells[1].Text = p.Quantity.ToString();
				prow.Cells[2].Text = "$" + p.Price.ToString();
				prow.Cells[3].Text = p.Status.ToString();

				// make a table for the trades, to be nested in positions table
				Table ttbl = new Table();
				TableRow tContainerRow = new TableRow();
				TableCell tContainerCell = new TableCell(); // create container cell for trade table
				tContainerRow.Width = new Unit("100%");
				tContainerCell.ColumnSpan = columns; // make the cell span the width of the positions table

				TableHeaderRow theader = new TableHeaderRow();
				for (int i = 0; i < tcolumns; i++)
				{
					TableHeaderCell cell = new TableHeaderCell();
					cell.Text = theaders[i];
					theader.Cells.Add(cell);
				}
				
				foreach (TradeMessage t in p.Trades)
				{
					TableRow trow = new TableRow();
					for (int i = 0; i < tcolumns; i++)
					{
						TableCell cell = new TableCell();
						trow.Cells.Add(cell);
					}

					trow.Cells[0].Text = t.Timestamp.ToString();
					trow.Cells[1].Text = t.Quantity.ToString();
					trow.Cells[2].Text = t.Price.ToString();
					trow.Cells[3].Text = t.Type.ToString();
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
				PortfolioTable.Rows.Add(theader);
				tContainerCell.Controls.Add(ttbl);
				tContainerRow.Cells.Add(tContainerCell);
				PortfolioTable.Rows.Add(tContainerRow);
			}
		}

		protected void btnClick(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			Response.Redirect("BuySell.aspx?s=" + btn.Attributes["SymbolName"] + "&p=" + btn.Attributes["LastPrice"]);
		}
	}
}