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
		string name = string.Empty;
		string useremail = string.Empty;
		public int duck1 = 300;
		public int duck2 = 3600;
		public int duck3 = 14400;
		public int movingavg;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				getConfig();
			}
			else
			{

			}
			
			update();
		}

		/// <summary>
		/// Updates the page if there are multiple settings tabs.
		/// </summary>
		protected void update()
		{
			
		}

		/// <summary>
		/// Gets the system settings configuration and displays it on the page.
		/// </summary>
		protected void getConfig()
		{
			TraderContext db = new TraderContext();
			duck1 = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("FIRST_DUCK_SECONDS")).Select(x => x.Value).FirstOrDefault());
			duck2 = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("SECOND_DUCK_SECONDS")).Select(x => x.Value).FirstOrDefault());
			duck3 = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("THIRD_DUCK_SECONDS")).Select(x => x.Value).FirstOrDefault());
			movingavg = int.Parse(db.SystemSettings.Where(x => x.Name.Equals("MOVING_AVERAGE_WINDOW")).Select(x => x.Value).FirstOrDefault());
			useremail = db.SystemSettings.Where(x => x.Name.Equals("ALERTS_EMAIL_ADDRESS_TO")).Select(x => x.Value).FirstOrDefault();
			name = db.SystemSettings.Where(x => x.Name.Equals("USERNAME")).Select(x => x.Value).FirstOrDefault();

			InputFirstDuck.Value = duck1.ToString();
			InputSecondDuck.Value = duck2.ToString();
			InputThirdDuck.Value = duck3.ToString();
			InputAvgWindow.Value = movingavg.ToString();
			InputName.Value = name;
			InputEmail.Value = useremail;
		}

		/// <summary>
		/// Sets the text of the status message displayed on the page.
		/// </summary>
		/// <param name="msg">string, the message text</param>
		/// <param name="type">bool, the type of the message (success or fail)</param>
		protected void setStatus(string msg, bool type)
		{
			statusMessage.Controls.Clear();
			HtmlGenericControl message = new HtmlGenericControl("span");

			if (type)
			{
				message.Attributes.Add("class", "message-success");
				message.InnerHtml = new HtmlString("<span class='icon-ok-sign'></span> " + msg).ToString();
			}
			else
			{
				message.Attributes.Add("class", "message-fail");
				message.InnerHtml = new HtmlString("<span class='icon-remove-sign'></span> " + msg).ToString();
			}
			statusMessage.Controls.Add(message);
		}

		#region Controls
		/// <summary>
		/// Updates the page if the selected index is changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void radioLists_SelectedIndexChanged(object sender, EventArgs e)
		{
			update();
		}

		/// <summary>
		/// Saves the changes made to the settings.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				string module = "ThreeDuckStrategy";

				// Remove the old settings
				TraderContext db = new TraderContext();
				var query = db.SystemSettings.Where(x => x.Module.Equals(module)).Select(x => x);
				foreach (SystemSetting setting in query)
				{
					db.SystemSettings.Remove(setting);
				}

				var q_username = db.SystemSettings.Where(x => x.Name.Equals("USERNAME")).Select(x=>x);
				if (q_username.Count() > 0)
				{
					SystemSetting s = q_username.FirstOrDefault();
					db.SystemSettings.Remove(s);
				}

				// Add the new settings
				SystemSetting _duck1 = new SystemSetting(module, "FIRST_DUCK_SECONDS", InputFirstDuck.Value);
				SystemSetting _duck2 = new SystemSetting(module, "SECOND_DUCK_SECONDS", InputSecondDuck.Value);
				SystemSetting _duck3 = new SystemSetting(module, "THIRD_DUCK_SECONDS", InputThirdDuck.Value);
				SystemSetting _movingavg = new SystemSetting(module, "MOVING_AVERAGE_WINDOW", InputAvgWindow.Value);
				SystemSetting _username = new SystemSetting("User", "USERNAME", InputName.Value);
				db.SystemSettings.Add(_duck1);
				db.SystemSettings.Add(_duck2);
				db.SystemSettings.Add(_duck3);
				db.SystemSettings.Add(_movingavg);
				db.SystemSettings.Add(_username);
				db.SaveChanges();

				setStatus("Settings saved successfully.", true);
			}
			catch
			{
				setStatus("Settings could not be saved. Please try again.", false);
			}
		}
		
		protected void btnResetDefault_Click(object sender, EventArgs e)
		{
			try
			{
				string module = "ThreeDuckStrategy";

				// Remove the old settings
				TraderContext db = new TraderContext();
				var query = db.SystemSettings.Where(x => x.Module.Equals(module)).Select(x => x);
				foreach (SystemSetting setting in query)
				{
					db.SystemSettings.Remove(setting);
				}

				var q_username = db.SystemSettings.Where(x => x.Name.Equals("USERNAME")).Select(x => x);
				if (q_username.Count() > 0)
				{
					SystemSetting s = q_username.FirstOrDefault();
					db.SystemSettings.Remove(s);
				}

				// Restore the default settings
				SystemSetting _duck1 = new SystemSetting(module, "FIRST_DUCK_SECONDS", "300");
				SystemSetting _duck2 = new SystemSetting(module, "SECOND_DUCK_SECONDS", "3600");
				SystemSetting _duck3 = new SystemSetting(module, "THIRD_DUCK_SECONDS", "14400");
				SystemSetting _movingavg = new SystemSetting(module, "MOVING_AVERAGE_WINDOW", "60");
				db.SystemSettings.Add(_duck1);
				db.SystemSettings.Add(_duck2);
				db.SystemSettings.Add(_duck3);
				db.SystemSettings.Add(_movingavg);
				db.SaveChanges();

				// Display success message
				setStatus("Settings saved successfully.", true);
			}
			catch
			{
				// Display fail message
				setStatus("Settings could not be saved. Please try again.", false);
			}
		}
		#endregion
	}
}