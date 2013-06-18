using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using System.IO;

namespace AlgoTraderSite
{
	public partial class WatchListPage : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			showTestWatchList();
		}

		public WatchList createTestWatchList()
		{
			WatchList wl = new WatchList("Default");
			wl.addToList(new Symbol("GOOG"), "Default");
			wl.addToList(new Symbol("AAPL"), "Default");
			wl.addToList(new Symbol("VZ"), "Default");
			wl.addToList(new Symbol("INTC"), "Default");
			wl.addToList(new Symbol("MSFT"), "Default");
			wl.addToList(new Symbol("HP"), "Default");

			return wl;
		}

		public void showTestWatchList()
		{
			WatchList wl = createTestWatchList();
			showWatchList(wl);
		}

		public void showWatchList(WatchList watchlist)
		{
			tblWatchList.Controls.Clear();

			TableRow headers = new TableRow();
			List<TableCell> headerCells = new List<TableCell>();
			TableCell companyHeaderCell = new TableCell();
			TableCell priceHeaderCell = new TableCell();
			TableCell changeHeaderCell = new TableCell();
			TableCell changePercentHeaderCell = new TableCell();

			companyHeaderCell.Text = "COMPANY";
			priceHeaderCell.Text = "PRICE";
			changeHeaderCell.Text = "CHANGE";
			changePercentHeaderCell.Text = "CHANGE %";

			headerCells.Add(companyHeaderCell);
			headerCells.Add(priceHeaderCell);
			headerCells.Add(changeHeaderCell);
			headerCells.Add(changePercentHeaderCell);

			foreach (TableCell header in headerCells)
			{
				headers.Cells.Add(header);
			}

			tblWatchList.Rows.Add(headers);

			for (int row = 0; row < watchlist.Items.Count; row++)
			{
				TableRow tblrow = new TableRow();
				List<TableCell> cells = new List<TableCell>();

				TableCell companyCell = new TableCell();
				TableCell priceCell = new TableCell();
				TableCell changeCell = new TableCell();
				TableCell changePercentCell = new TableCell();

				cells.Add(companyCell);
				cells.Add(priceCell);
				cells.Add(changeCell);
				cells.Add(changePercentCell);

				companyCell.Text = watchlist.Items[row].SymbolName;
				priceCell.Text = "890.22"; //replace with quote price
				changeCell.Text = "+ 10.49"; // replace with quote change
				changePercentCell.Text = "+ 1.19%"; // replace with quote percent change

				foreach (TableCell cell in cells)
				{
					cell.Width = 300;
					tblrow.Cells.Add(cell);
				}
				tblWatchList.Rows.Add(tblrow);
			}
		}
	}
}