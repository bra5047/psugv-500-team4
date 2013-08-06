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
        public string symbolName;
		public string m1Label;
		public string m2Label;
		public string m3Label;
		public double m1;
		public double m2;
		public double m3;
		public double[,] datapoints;
        public List<PlotPoint> plot;
        public JavaScriptSerializer javaSerial;
		public string data;

		protected void Page_Load(object sender, EventArgs e)
		{
            symbolName = string.Empty;
			
            datapoints = new double[1, 1];
            plot = new List<PlotPoint>();
            javaSerial = new JavaScriptSerializer();

            StrategyClient strategy = new StrategyClient();
            AlgoTraderSite.Strategy.Client.StrategyDetail detail;

            // TODO: fail gracefully (EndpointNotFoundException)
			if (Request.QueryString.AllKeys.Contains("s"))
			{
				symbolName = Request.QueryString["s"].ToUpper();
				try
				{
					detail = strategy.getDetailedAnalysis(symbolName);
					m1 = detail.Metric_1;
					m2 = detail.Metric_2;
					m3 = detail.Metric_3;
					m1Label = detail.Metric_1_Label;
					m2Label = detail.Metric_2_Label;
					m3Label = detail.Metric_3_Label;

					getDataPoints();
				}
				catch (Exception ex)
				{
				}
			}
            strategy.Close();
		}

		protected void getDataPoints()
		{
			IWatchListManager wlm = new WatchListManager();
			var query = wlm.GetQuotes(symbolName);
			DateTime dstart = new DateTime(1970, 1, 1);
			query = query.OrderBy(x => x.timestamp).ToList();

			data = "[";
			foreach (Quote q in query)
			{
				DateTime qdate = q.timestamp;
				TimeSpan ts = q.timestamp - dstart;
				//plot.Add(new PlotPoint(ts.TotalMilliseconds, q.price));
				data += "[" + (ts.TotalMilliseconds - ts.Milliseconds-15000) + ", " + q.price + "],";
			}
			data = data.TrimEnd(',');
			data += "]";
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