using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTraderSite.Strategy.Client;

namespace AlgoTraderSite
{
    public partial class TestStrategyDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StrategyClient sc = new StrategyClient();
            StrategyDetail d = sc.getDetailedAnalysis("GOOG");

            Response.Write(String.Format("{0} <br/>", d.SymbolName));
            Response.Write(String.Format("{0} {1}<br/>", d.Metric_1_Label, d.Metric_1));
            Response.Write(String.Format("{0} {1}<br/>", d.Metric_2_Label, d.Metric_2));
            Response.Write(String.Format("{0} {1}<br/>", d.Metric_3_Label, d.Metric_3));

            sc.Close();
        }
    }
}