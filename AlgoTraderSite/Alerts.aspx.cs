using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTraderSite.UserAgent.Client;

namespace AlgoTraderSite
{
    public partial class Alerts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserAgentClient useragent = new UserAgentClient();
            string alertId;
            string action;

			if (Request.QueryString.AllKeys.Contains("id"))
			{
				alertId = Request.QueryString["id"];
				action = Request.QueryString["s"];
				responseCodes r = responseCodes.Pending;
				if (action == "reject") r = responseCodes.Reject;
				if (action == "accept") r = responseCodes.Accept;

				useragent.processAlertResponse(alertId, r, "updated via web interface");
			}

			List<AlertMessage> alerts = new List<AlertMessage>(useragent.getPendingAlerts());
			useragent.Close();

			AlertItemList.DataSource = alerts;
			AlertItemList.DataBind();
        }
    }
}