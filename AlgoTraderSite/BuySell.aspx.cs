using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTraderSite.Portfolio.Client;

namespace AlgoTraderSite
{
    public partial class BuySell : System.Web.UI.Page
    {
        private PortfolioManagerClient portfolio;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("s"))
            {
                SymbolLabel.Text = Request.QueryString["s"];
            }
            if (Request.QueryString.AllKeys.Contains("s"))
            {
                PriceLabel.Text = Request.QueryString["p"];
            }
            else
            {
                PriceLabel.Text = "$1.25";
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ExecuteTrade_Click(object sender, EventArgs e)
        {
            portfolio = new PortfolioManagerClient();
            portfolio.buy(SymbolLabel.Text, Int32.Parse(QuantityBox.Text));
            Response.Redirect("Portfolio.aspx");
        }
    }
}