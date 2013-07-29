using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using System.Web.UI.HtmlControls;

namespace AlgoTraderSite
{
	public partial class Settings : Page
	{
		string userEmail;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{

			}
			else
			{

			}
			userEmail = "someone@example.com";
			update();
		}

		protected void update()
		{
			string value = radioLists.SelectedValue;
			contentDiv.Controls.Clear();
			if (value.Equals("General"))
			{
				Label lblEmail = new Label();
				lblEmail.Text = "Email: ";
				lblEmail.CssClass = "setting";

				HtmlInputGenericControl email = new HtmlInputGenericControl("email");
				email.Attributes["required"] = "required";
				email.Attributes["id"] = "tbEmail";
				email.Attributes["style"] = "width: 300px";
				email.Value = userEmail;

				contentDiv.Controls.Add(lblEmail);
				contentDiv.Controls.Add(email);
			}
			else if (value.Equals("Alerts"))
			{
				Label lblNFreq = new Label();
				lblNFreq.Text = "Notification frequency (mins): ";
				lblNFreq.CssClass = "setting";

				HtmlInputGenericControl frequency = new HtmlInputGenericControl("number");
				frequency.Attributes["required"] = "required";
				frequency.Attributes["min"] = "5";
				frequency.Attributes["max"] = "60";
				frequency.Attributes["id"] = "tbFrequency";
				frequency.Value = "5";

				Label lblRepeat = new Label();
				lblRepeat.Text = "Number of reminders: ";
				lblRepeat.CssClass = "setting";

				HtmlInputGenericControl repeat = new HtmlInputGenericControl("number");
				repeat.Attributes["required"] = "required";
				repeat.Attributes["min"] = "1";
				repeat.Attributes["max"] = "10";
				repeat.Attributes["id"] = "tbRepeat";
				repeat.Value = "3";

				contentDiv.Controls.Add(lblNFreq);
				contentDiv.Controls.Add(frequency);
				contentDiv.Controls.Add(lblRepeat);
				contentDiv.Controls.Add(repeat);
			}
		}

		#region Controls
		protected void radioLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			update();
		}
		#endregion
	}
}