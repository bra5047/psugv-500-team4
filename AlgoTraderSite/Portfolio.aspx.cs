using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;
using AlgoTrader.portfolio;
using AlgoTraderSite.Portfolio.Client;
using System.IO;

namespace AlgoTraderSite
{
	public partial class MyPortfolio : Page
	{
        private PortfolioManagerClient portfolio;

		protected void Page_Load(object sender, EventArgs e)
		{
            TreeNode root = new TreeNode("Portfolio");
            portfolio = new PortfolioManagerClient();

            foreach (PositionMessage p in portfolio.GetOpenPositions())
            {
                string s = String.Format("{0}     {1}     ${2}     {3}", p.SymbolName, p.Quantity, p.Price, p.Status);
                TreeNode n = new TreeNode(s);

                foreach (TradeMessage t in p.Trades)
                {
                    string tstr = String.Format("{0}     {1}     ${2}     {3}", t.Timestamp.ToString(), t.Quantity.ToString(), t.Price.ToString(), t.Type.ToString());
                    n.ChildNodes.Add(new TreeNode(tstr));
                }
                root.ChildNodes.Add(n);
            }
            PortfolioTree.Nodes.Clear();
            PortfolioTree.Nodes.Add(root);
		}
	}
}