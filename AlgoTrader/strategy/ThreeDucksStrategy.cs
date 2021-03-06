﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using log4net;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.strategy
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ThreeDucksStrategy : IStrategy
    {
        // these settings can be initialized in the constructor
        private int FIRST_DUCK_SECONDS;
        private int SECOND_DUCK_SECONDS;
        private int THIRD_DUCK_SECONDS;
        private int AVERAGE_WINDOW;
        private int DEFAULT_BUY_SIZE;

        private Dictionary<string, List<SmaMetric>> _metrics;
        private Dictionary<string, StrategySignal> _signals;

        private List<string> _watchForBuy;
        private List<string> _watchForSell;

        private QuoteProvider _quoteProvider;
        private SymbolProvider _symbolProvider;
        private ISignalAlerter _alerter;

        // this is used to block service requests while the class is initializing from the database
        private Object _init_lock;

        public ThreeDucksStrategy()
        {
            _metrics = new Dictionary<string, List<SmaMetric>>();
            _signals = new Dictionary<string, StrategySignal>();
            _alerter = new SignalAlerter();
            _init_lock = new Object();

            _watchForBuy = new List<string>();
            _watchForSell = new List<string>();

            FIRST_DUCK_SECONDS = 300;
            SECOND_DUCK_SECONDS = 3600;
            THIRD_DUCK_SECONDS = 14400;
            AVERAGE_WINDOW = 60;
            DEFAULT_BUY_SIZE = 100;
        }

        public ThreeDucksStrategy(Dictionary<string,string> settings)
            : this()
        {
            foreach (string s in settings.Keys)
            {
                switch (s)
                {
                    case "FIRST_DUCK_SECONDS":
                        Int32.TryParse(settings[s], out FIRST_DUCK_SECONDS);
                        break;
                    case "SECOND_DUCK_SECONDS":
                        Int32.TryParse(settings[s], out SECOND_DUCK_SECONDS);
                        break;
                    case "THIRD_DUCK_SECONDS":
                        Int32.TryParse(settings[s], out THIRD_DUCK_SECONDS);
                        break;
                    case "MOVING_AVERAGE_WINDOW":
                        Int32.TryParse(settings[s], out AVERAGE_WINDOW);
                        break;
                }
            }

            _quoteProvider = new QuoteProvider(this);
            _quoteProvider.Start();
            _symbolProvider = new SymbolProvider(this);
            _symbolProvider.Start();
        }

        public int First_Duck_Seconds { get { return FIRST_DUCK_SECONDS; } }
        public int Second_Duck_Seconds { get { return SECOND_DUCK_SECONDS; } }
        public int Third_Duck_Seconds { get { return THIRD_DUCK_SECONDS; } }
        public int Moving_Average_Window { get { return AVERAGE_WINDOW; } }

        // this method loads bulk quotes without triggering excessive alerts
        public void NewQuotes(List<QuoteMessage> quotes)
        {
            ILog log = LogManager.GetLogger(typeof(ThreeDucksStrategy));
            log.DebugFormat("Bulk quote list received. Size: {0}", quotes.Count);

            lock (_init_lock)
            {
                // we attack it one symbol at a time
                SortedSet<string> symbols = new SortedSet<string>(quotes.Select(x => x.SymbolName));
                log.DebugFormat("Bulk quote list contained {0} symbols", symbols.Count);

                foreach (string s in symbols)
                {
                    log.DebugFormat("Processing quotes for {0}", s);
                    if (!_metrics.ContainsKey(s))
                    {
                        log.DebugFormat("Starting to watch {0}", s);
                        startWatching(s);
                    }
                    // pull out the quotes for the symbol we are processing and run them in order
                    List<QuoteMessage> qs = quotes.OrderBy(x => x.timestamp).Where(y => y.SymbolName == s).ToList<QuoteMessage>();
                    log.DebugFormat("Extracted {0} quotes for symbol {1}", qs.Count, s);
                    foreach (QuoteMessage q in qs)
                    {
                        NewQuote(q);
                    }
                    CheckSignals(qs.Last());
                }
                log.DebugFormat("Done processing symbols.");
            }
            log.DebugFormat("Lock released.");
        }

        // this is the method used to process real-time streaming quotes
        public void NewQuote(QuoteMessage quote)
        {
            ILog log = LogManager.GetLogger(typeof(ThreeDucksStrategy));
            log.DebugFormat("Quote received: {0} {1} {2}", quote.SymbolName, quote.timestamp.ToString(), quote.Price);

            foreach (SmaMetric m in _metrics[quote.SymbolName])
            {
                m.Add(quote.timestamp, quote.Price);
            }
        }

        // determines if we need to raise any alerts as a result of the most recent quote
        public void CheckSignals(QuoteMessage quote)
        {
            int buy_ducks = 0;
            int sell_ducks = 0;

            foreach (SmaMetric m in _metrics[quote.SymbolName])
            {
                if (quote.Price > m.Avg) buy_ducks++;
                if (quote.Price < m.Avg) sell_ducks++;
            }
            if (buy_ducks == 3)
            {
                _signals[quote.SymbolName] = StrategySignal.Buy;
                if (_watchForBuy.Contains(quote.SymbolName))
                {
                    _alerter.BuyAlert(quote.SymbolName, DEFAULT_BUY_SIZE, quote.Price);
                }
            }
            else if (sell_ducks == 3)
            {
                _signals[quote.SymbolName] = StrategySignal.Sell;
                if (_watchForSell.Contains(quote.SymbolName))
                {
                    _alerter.SellAlert(quote.SymbolName, quote.Price);
                }
            }
            else
            {
                _signals[quote.SymbolName] = StrategySignal.None;
            }
        }

        public List<string> WatchForBuy
        {
            set
            {
                _watchForBuy = value;
            }
        }

        public List<string> WatchForSell
        {
            set
            {
                _watchForSell = value;
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

        // this method is called by the UI to display buy/sell recommendations
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

        // this method is called by the UI to generate detailed graphs
        public StrategyDetail getDetailedAnalysis(string symbolName)
        {
            ILog log = LogManager.GetLogger(typeof(ThreeDucksStrategy));
            log.DebugFormat("Detail request for {0}", symbolName);

            StrategyDetail s = new StrategyDetail();
            s.SymbolName = symbolName;
            if (!_metrics.ContainsKey(symbolName))
            {
                log.DebugFormat("Symbol not found in metrics list: {0}", symbolName);
                return s;
            }

            lock (_init_lock)
            {
                try
                {
                    List<SmaMetric> m = _metrics[symbolName];
                    s.Metric_1 = m[0].Avg;
                    s.Metric_1_Label = m[0].Label;
                    s.History_Series_1 = m[0].History;
                    s.Metric_2 = m[1].Avg;
                    s.Metric_2_Label = m[1].Label;
                    s.History_Series_2 = m[1].History;
                    s.Metric_3 = m[2].Avg;
                    s.Metric_3_Label = m[2].Label;
                    s.History_Series_3 = m[2].History;
                }
                catch (Exception ex)
                {
                    log.Warn(ex.Message);
                    log.Debug(ex.StackTrace);
                }
            }
            return s;
        }
    }
}
