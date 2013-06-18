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
			createTestWatchList();
			showWatchList();
		}

		public void createTestWatchList()
		{
			WatchList wl = new WatchList();
			wl.ListName = "Default";
			wl.addToList(new Symbol("GOOG"), "Default");
			wl.addToList(new Symbol("GOOG"), "Default");
			wl.addToList(new Symbol("GOOG"), "Default");

			watchlist.InnerText = wl.Items.Count.ToString();
			//WatchListItem wli = new WatchListItem();
			//wli.SymbolName = "TEST";
			//wli.ListName = "Default";
		
			//watchlist.Controls.Clear();
			//watchlist.InnerText = "HELLO WORLD";

			//List<Symbol> symbols = new List<Symbol>();

			//symbols.Add(new Symbol("GOOG"));
			//symbols.Add(new Symbol("AAPL"));
			//symbols.Add(new Symbol("VZ"));
			//symbols.Add(new Symbol("INTC"));
			//symbols.Add(new Symbol("MSFT"));
		}

		public void showWatchList()
		{
			tblWatchList.Controls.Clear();

			TableRow headers = new TableRow();
			List<TableCell> headerCells = new List<TableCell>();
			TableCell companyHeaderCell = new TableCell();
			TableCell priceHeaderCell = new TableCell();
			TableCell changeHeaderCell = new TableCell();
			TableCell changePercentHeaderCell = new TableCell();

			headerCells.Add(companyHeaderCell);
			headerCells.Add(priceHeaderCell);
			headerCells.Add(changeHeaderCell);
			headerCells.Add(changePercentHeaderCell);

			companyHeaderCell.Text = "COMPANY";
			priceHeaderCell.Text = "PRICE";
			changeHeaderCell.Text = "CHANGE";
			changePercentHeaderCell.Text = "CHANGE %";

			foreach (TableCell header in headerCells)
			{
				headers.Cells.Add(header);
			}

			tblWatchList.Rows.Add(headers);

			for (int row = 0; row < 10; row++) // replace with watchlist.length
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

				//replace with variables
				companyCell.Text = "GOOG";
				priceCell.Text = "890.22";
				changeCell.Text = "+ 10.49";
				changePercentCell.Text = "+ 1.19%";

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