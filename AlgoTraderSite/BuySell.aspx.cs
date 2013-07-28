using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTraderSite.Portfolio.Client;

namespace AlgoTraderSite
{
    public partial class BuySell : System.Web.UI.Page
    {
        private PortfolioManagerClient portfolio;
        private FaultException f;

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
			if (Request.QueryString.AllKeys.Contains("t"))
			{
				if (Request.QueryString["t"].Equals("Buy"))
				{
					BuySellPicker.SelectedIndex = 0;
				}
				if (Request.QueryString["t"].Equals("Sell"))
				{
					BuySellPicker.SelectedIndex = 1;
				}
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
            if (BuySellPicker.SelectedValue == "Buy")
            {
                try
                {
                    portfolio.buy(SymbolLabel.Text, Int32.Parse(QuantityBox.Text));
                }
                catch (FaultException<AlgoTraderSite.Portfolio.Client.InsufficientFundsFault> ex)
                {
                    ErrorMsg.Text = String.Format("Insufficient funds error. Available: ${0}  Requested: ${1}", ex.Detail.AvailableAmount, ex.Detail.TransactionAmount);
                    ErrorMsg.Visible = true;
                    return;
                }
                catch (FaultException<AlgoTraderSite.Portfolio.Client.AllocationViolationFault> ex)
                {
                    ErrorMsg.Text = ex.Detail.FaultMessage;
                    ErrorMsg.Visible = true;
                    return;
                }
            }
            else
            {
                try
                {
                    portfolio.sell(SymbolLabel.Text, Int32.Parse(QuantityBox.Text));
                }
                catch (FaultException<AlgoTraderSite.Portfolio.Client.InsufficientQuantityFault> ex)
                {
                    ErrorMsg.Text = String.Format("Not enough shares to sell. Available {0} Requested: {1}", ex.Detail.AvailableQuantity, ex.Detail.RequestedQuantity);
                    ErrorMsg.Visible = true;
                    return;
                }
            }
            Response.Redirect("Portfolio.aspx");
        }
    }
}