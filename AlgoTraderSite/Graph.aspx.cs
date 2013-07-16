using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTrader.watchlist;
using System.Web.Script.Serialization;
using AlgoTraderSite.Strategy.Client;
using System.IO;

namespace AlgoTraderSite
{
	public partial class Graph : Page
	{
		StrategyClient strategy = new StrategyClient();
		AlgoTraderSite.Strategy.Client.StrategyDetail detail = new AlgoTraderSite.Strategy.Client.StrategyDetail();
		public static double[] metrics = new double[] { 0, 0, 0 };
		public static string symbolName = string.Empty;
		public static double[,] datapoints;
		public static List<PlotPoint> plot = new List<PlotPoint>();
		public JavaScriptSerializer javaSerial = new JavaScriptSerializer();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString.AllKeys.Contains("s"))
			{
				symbolName = Request.QueryString["s"].ToUpper();
				detail = strategy.getDetailedAnalysis(symbolName);
				metrics = new double[3] { detail.Metric_1, detail.Metric_2, detail.Metric_3 };
				//metrics = new double[3] { 450.43, 500.66, 522.63 };

				getDataPoints();
			}
		}

		protected void getDataPoints()
		{
			IWatchListManager wlm = new WatchListManager();
			var query = wlm.GetQuotes(symbolName);
			DateTime dstart = new DateTime(1970, 1, 1);
			//datapoints = new double[query.Count, 2];

			foreach (Quote q in query)
			{
				DateTime qdate = q.timestamp;
				TimeSpan ts = q.timestamp - dstart;
				plot.Add(new PlotPoint(ts.TotalMilliseconds, q.price));
			}

			//for (int i = 0; i < query.Count; i++)
			//{
			//	DateTime qdate = query[i].timestamp;
			//	TimeSpan ts = query[i].timestamp - dstart;
			//	datapoints[i, 0] = ts.TotalMilliseconds - ts.Milliseconds;
			//	datapoints[i, 1] = query[i].price;
			//}
		}

		#region Controls
		protected void btnClick_Back(object sender, EventArgs e)
		{
			Response.Redirect("Watchlist.aspx");
		}
		#endregion
	}

	public class PlotPoint
	{
		public double DateMilliseconds { get; set; }
		public double Price { get; set; }

		public PlotPoint(double ms, double price)
		{
			DateMilliseconds = ms;
			Price = price;
		}
	}
}