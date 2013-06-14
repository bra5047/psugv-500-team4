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
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ThreeDucksStrategy : IStrategy
    {
        private int FIRST_DUCK_SECONDS;
        private int SECOND_DUCK_SECONDS;
        private int THIRD_DUCK_SECONDS;
        private int AVERAGE_WINDOW;

        private Dictionary<string, List<SmaMetric>> _metrics;
        private Dictionary<string, StrategySignal> _signals;

        public ThreeDucksStrategy()
        {
            _metrics = new Dictionary<string, List<SmaMetric>>();
            _signals = new Dictionary<string, StrategySignal>();

            // these could eventually be constructor params retrieved from the database settings table
            FIRST_DUCK_SECONDS = 300;
            SECOND_DUCK_SECONDS = 3600;
            THIRD_DUCK_SECONDS = 14400;
            AVERAGE_WINDOW = 60;
        }

        public void NewQuote(QuoteMessage quote)
        {
            int buy_ducks = 0;
            int sell_ducks = 0;

            foreach (SmaMetric m in _metrics[quote.SymbolName])
            {
                m.Add(quote.timestamp, quote.price);
                if (quote.price > m.Avg) buy_ducks++;
                if (quote.price < m.Avg) sell_ducks++;
            }
            if (buy_ducks == 3)
            {
                _signals[quote.SymbolName] = StrategySignal.Buy;
            }
            else if (sell_ducks == 3)
            {
                _signals[quote.SymbolName] = StrategySignal.Sell;
            }
            else
            {
                _signals[quote.SymbolName] = StrategySignal.None;
            }
        }

        public bool startWatching(string symbolName)
        {
            if (!_metrics.ContainsKey(symbolName))
            {
                List<SmaMetric> m = new List<SmaMetric>();
                m.Add(new SmaMetric(symbolName, AVERAGE_WINDOW, FIRST_DUCK_SECONDS));
                m.Add(new SmaMetric(symbolName, AVERAGE_WINDOW, SECOND_DUCK_SECONDS));
                m.Add(new SmaMetric(symbolName, AVERAGE_WINDOW, THIRD_DUCK_SECONDS));
                _metrics.Add(symbolName, m);
                _signals.Add(symbolName, StrategySignal.None);
                return true;
            }
            else
            {
                return false;
            }           
        }

        public bool stopWatching(string symbolName)
        {
            if (_metrics.ContainsKey(symbolName))
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public StrategySummary getSummary(string symbolName)
        {
            StrategySummary s = new StrategySummary();
            s.SymbolName = symbolName;
            
            if (_signals.ContainsKey(symbolName))
            {
                s.AsOf = DateTime.Now;
                s.CurrentSignal = _signals[symbolName];
            }
            return s;
        }

        public IStrategyDetail getDetailedAnalysis(ISymbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}
