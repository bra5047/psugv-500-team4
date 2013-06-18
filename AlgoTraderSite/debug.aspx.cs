using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.Interfaces;
using AlgoTrader.portfolio;
using AlgoTraderSite.Portfolio.Client;

namespace AlgoTraderSite
{
    public partial class debug : System.Web.UI.Page
    {
        private PortfolioManagerClient portfolio;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            portfolio = new PortfolioManagerClient();
            Response.Write("Available cash: $" + portfolio.getAvailableCash());
            foreach (PositionMessage p in portfolio.GetOpenPositions())
            {
                Response.Write("<br/>Position: " + p.SymbolName);
                Response.Write("<br/>Price: " + p.Price);
                Response.Write("<br/>Quantity: " + p.Quantity);
                Response.Write("<br/>Status: " + p.Status.ToString());

                foreach (TradeMessage t in p.Trades)
                {
                    Response.Write("<br/>Trade ID: " + t.TradeId);
                    Response.Write("<br/>Timestamp: " + t.Timestamp.ToString());
                    Response.Write("<br/>Type: " + t.Type.ToString());
                    Response.Write("<br/>Price: $" + t.Price.ToString());
                    Response.Write("<br/>Quantity: " + t.Quantity.ToString());
                }
            }
            portfolio.Close();
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            portfolio = new PortfolioManagerClient();
            portfolio.buy("GOOG", 15);
            portfolio.Close();
        }
      
    }
}