using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.useragent;
using AlgoTrader.portfolio;

namespace AlgoTrader.strategy
{
    public interface ISignalAlerter
    {
        void BuyAlert(string symbolName, int quantity, double price);
        void SellAlert(string symbolName, double price);
    }

    public class SignalAlerter : ISignalAlerter
    {
        public void BuyAlert(string symbolName, int quantity, double price)
        {
            PortfolioManager pm = new PortfolioManager();
            IUserAgent ua = new UserAgent();
            ua.generateAlert(symbolName, tradeTypes.Buy, pm.DefaultBuySize(price), price);
        }

        public void SellAlert(string symbolName, double price)
        {
            IPortfolioManager pm = new PortfolioManager();
            PositionMessage pos = pm.GetPosition(symbolName);
            UserAgent ua = new UserAgent();
            ua.generateAlert(symbolName, tradeTypes.Sell, pos.Quantity, price);
        }
    }
}
