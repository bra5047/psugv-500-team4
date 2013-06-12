using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.strategy
{
    public enum StrategySignal { Buy, Sell, None }
    
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ThreeDucksStrategy : IStrategy
    {
        private List<SmaMetric> _metrics;

        public ThreeDucksStrategy()
        {
            _metrics = new List<SmaMetric>();
            _metrics.Add(new SmaMetric(new Symbol("GOOG"), 5, 5));
        }
        public void NewQuote(QuoteMessage quote)
        {
            //this would be a good place to add logging support
        }

        public bool startWatching(ISymbol symbol)
        {
            throw new NotImplementedException();
        }

        public bool stopWatching(ISymbol symbol)
        {
            throw new NotImplementedException();
        }

        public IStrategyDetail getDetailedAnalysis(ISymbol symbol)
        {
            throw new NotImplementedException();
        }

        public IStrategySummary getSummary(ISymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
