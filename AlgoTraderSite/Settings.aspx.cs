using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using System.Web.UI.HtmlControls;
using AlgoTrader.Interfaces;

namespace AlgoTraderSite
{
	public partial class Settings : Page
	{
		string userEmail;
		public int duck1 = 300;
		public int duck2 = 3600;
		public int duck3 = 14400;
		public int movingavg;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				getConfig();
				//InputFirstDuck.ServerChange += new EventHandler(InputFirstDuck_ServerChange);
				//InputSecondDuck.ServerChange += new EventHandler(InputSecondDuck_ServerChange);
				//InputThirdDuck.ServerChange += new EventHandler(InputThirdDuck_ServerChange);
			}
			else
			{

			}
			userEmail = "someone@example.com";
			update();
		}

		protected void update()
		{
			//string value = radioLists.SelectedValue;
			//contentDiv.Controls.Clear();
			//if (value.Equals("General"))
			//{
			//	Label lblEmail = new Label();
			//	lblEmail.Text = "Email: ";
			//	lblEmail.CssClass = "setting";

			//	HtmlInputGenericControl email = new HtmlInputGenericControl("email");
			//	email.Attributes["required"] = "required";
			//	email.Attributes["id"] = "tbEmail";
			//	email.Attributes["style"] = "width: 300px";
			//	email.Value = userEmail;

			//	contentDiv.Controls.Add(lblEmail);
			//	contentDiv.Controls.Add(email);
			//}
			//else if (value.Equals("Strategy"))
			//{
			//	Label lblNFreq = new Label();
			//	lblNFreq.Text = "Notification frequency (mins): ";
			//	lblNFreq.CssClass = "setting";

			//	HtmlInputGenericControl frequency = new HtmlInputGenericControl("number");
			//	frequency.Attributes["required"] = "required";
			//	frequency.Attributes["min"] = "5";
			//	frequency.Attributes["max"] = "60";
			//	frequency.Attributes["id"] = "tbFrequency";
			//	frequency.Value = "5";

			//	Label lblRepeat = new Label();
			//	lblRepeat.Text = "Number of reminders: ";
			//	lblRepeat.CssClass = "setting";

			//	HtmlInputGenericControl repeat = new HtmlInputGenericControl("number");
			//	repeat.Attributes["required"] = "required";
			//	repeat.Attributes["min"] = "1";
			//	repeat.Attributes["max"] = "10";
			//	repeat.Attributes["id"] = "tbRepeat";
			//	repeat.Value = "3";

			//	contentDiv.Controls.Add(lblNFreq);
			//	contentDiv.Controls.Add(frequency);
			//	contentDiv.Controls.Add(lblRepeat);
			//	contentDiv.Controls.Add(repeat);
			//}
		}

		protected void getConfig()
		{
			//TraderContext db = new TraderContext();
			//duck1 = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("FIRST_DUCK_SECONDS")).Select(x => x.Value).FirstOrDefault());
			//duck2 = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("SECOND_DUCK_SECONDS")).Select(x => x.Value).FirstOrDefault());
			//duck3 = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("THIRD_DUCK_SECONDS")).Select(x => x.Value).FirstOrDefault());
			//movingavg = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("MOVING_AVERAGE_WINDOW")).Select(x => x.Value).FirstOrDefault());

			//InputFirstDuck.Value = duck1.ToString();
			//InputSecondDuck.Value = duck2.ToString();
			//InputThirdDuck.Value = duck3.ToString();
			//InputAvgWindow.Value = movingavg.ToString();
		}

		protected string getTimeUnits(string value) 
		{
			string units = "units";
			return units;
		}

		protected string getLabelText(string basetext, string value, string units)
		{
			string label = string.Empty;
			label = String.Format("{0} {1} {2}", basetext, value, units);
			return label;
		}

		public void InputFirstDuck_ServerChange(object sender, EventArgs e)
		{
			string baseText = "First Duck:";
			string units = getTimeUnits(InputFirstDuck.Value);
			LblFirstDuck.InnerText = getLabelText(baseText, "blah", units);
		}

		//void InputSecondDuck_ServerChange(object sender, EventArgs e)
		//{
		//	string baseText = "Second Duck:";
		//	string units = getTimeUnits(InputSecondDuck.Value);
		//	LblSecondDuck.InnerText = getLabelText(baseText, "blah blah", units);
		//}

		//void InputThirdDuck_ServerChange(object sender, EventArgs e)
		//{
		//	string baseText = "Third Duck:";
		//	string units = getTimeUnits(InputThirdDuck.Value);
		//	LblThirdDuck.InnerText = getLabelText(baseText, "blah blah blah", units);
		//}

		#region Controls
		protected void radioLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			update();
		}
		#endregion
	}
}