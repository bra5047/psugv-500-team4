﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTraderSite.UserAgent.Client;
using AlgoTrader.datamodel;
using System.Web.UI.HtmlControls;

namespace AlgoTraderSite
{
	public partial class _Default : Page
	{
		UserAgentClient useragent;
		private static List<AlertMessage> alerts;
		protected void Page_Load(object sender, EventArgs e)
		{
			useragent = new UserAgentClient();
			if (!IsPostBack)
			{
				generateAlerts();
			}
			showAlerts();
		}

		protected void generateAlerts()
		{
			alerts = new List<AlertMessage>(useragent.getPendingAlerts().OrderByDescending(x => x.Timestamp));
		}

		protected void showAlerts()
		{
			AlertBox.Controls.Clear();
			if (alerts.Count() > 0)
			{
				AlertBox.InnerText = string.Empty;
				foreach (AlertMessage am in alerts)
				{
					TimeSpan span = new TimeSpan();
					string classname = string.Empty;
					span = DateTime.Now - am.Timestamp;

					if (am.Type == AlgoTraderSite.UserAgent.Client.tradeTypes.Buy)
					{
						classname = "green";
					}
					else
					{
						classname = "red";
					}

					Table tbl = new Table();
					tbl.CssClass = "alert-table";
					TableRow row1 = new TableRow();
					// symbol cell
					TableCell cellCompany = new TableCell();
					cellCompany.Text = am.SymbolName;
					cellCompany.CssClass = "alert-company";
					// date cell
					TableCell cellDate = new TableCell();
					cellDate.Text = am.Timestamp.ToShortTimeString();
					cellDate.CssClass = "alert-date";
					if (span.Days > 0)
					{
						cellDate.Text = am.Timestamp.ToShortDateString() + " at " + cellDate.Text;
					}
					row1.Cells.Add(cellCompany);
					row1.Cells.Add(cellDate);
					tbl.Rows.Add(row1);

					TableRow row2 = new TableRow();
					TableCell cellDetail = new TableCell();
					cellDetail.ColumnSpan = 2;
					cellDetail.Text = new HtmlString(String.Format("<span class='{3}'>{0}</span> {1} shares at {2:C}", am.Type, am.Quantity, am.Price, classname)).ToString();
					cellDetail.CssClass = "alert-detail";
					row2.Cells.Add(cellDetail);
					tbl.Rows.Add(row2);

					TableRow row3 = new TableRow();
					TableCell cellActions = new TableCell();
					cellActions.ColumnSpan = 2;
					cellActions.CssClass = "alert-action";

					HtmlGenericControl actionDiv = new HtmlGenericControl("div");
					actionDiv.Attributes["class"] = "alert-actiondiv";
					actionDiv.Attributes["style"] = "visibility: hidden; display: none;";

					// accept button
					Button btnAccept = new Button();
					btnAccept.Text = "Accept";
					btnAccept.CssClass = "accept";
					btnAccept.Attributes["alertID"] = am.AlertId;
					btnAccept.Click += new EventHandler(btnAccept_Click);
					// reject button
					Button btnReject = new Button();
					btnReject.Text = "Reject";
					btnReject.CssClass = "reject";
					btnReject.Attributes["alertID"] = am.AlertId;
					btnReject.Click += new EventHandler(btnAccept_Click);

					actionDiv.Controls.Add(btnAccept);
					actionDiv.Controls.Add(btnReject);

					cellActions.Controls.Add(actionDiv);
					row3.Cells.Add(cellActions);
					tbl.Rows.Add(row3);

					//HtmlGenericControl alert = new HtmlGenericControl("div");
					//alert.Attributes["class"] = "alert-message";
					//alert.InnerText = " Price: " + am.Price + " Type: " + am.Type;
					AlertBox.Controls.Add(tbl);
				}
			}
			else
			{
				Table tbl = new Table();
				TableRow row = new TableRow();
				TableCell cell = new TableCell();
				cell.Text = "No new alerts.";
				cell.CssClass = "alert-company";
				row.Cells.Add(cell);
				tbl.Rows.Add(row);
				AlertBox.Controls.Add(tbl);
			}
		}

		protected void update()
		{
			showAlerts();
		}

		#region Controls
		void btnAccept_Click(object sender, EventArgs e)
		{
			Button Sender = (Button)sender;
			string alertID = Sender.Attributes["alertID"];
			string action = Sender.Text;
			responseCodes r = responseCodes.Pending;

			if (action == "Accept") r = responseCodes.Accept;
			if (action == "Reject") r = responseCodes.Reject;
			useragent.processAlertResponse(alertID, r, "updated via web interface");
			alerts.RemoveAll(x => x.AlertId.Equals(alertID));
			update();
		}
		#endregion
	}
}