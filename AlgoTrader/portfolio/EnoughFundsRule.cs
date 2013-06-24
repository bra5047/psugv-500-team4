using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;

namespace AlgoTrader.portfolio
{
    public class EnoughFundsRule : PortfolioRule
    {
        public bool Apply(IPortfolio p, ITrade t)
        {
            if (t.type != tradeTypes.Buy) return true;
            if (t.price * t.quantity > p.Cash)
            {
                InsufficientFunds ex = new InsufficientFunds();
                ex.Data.Add("AvailableFunds", p.Cash);
                ex.Data.Add("TransactionAmount", t.price * t.quantity);
                throw ex;
            }
            return true;
        }
    }

    public class InsufficientFunds : Exception
    {
        public InsufficientFunds() : base("Insufficient funds available to complete the requested transaction.") { }
    } 
}
