using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.strategy
{
    public class SmaMetric
    {
        private static int HISTORY_DATAPOINTS = 50;

        private Queue<IQuote> _quotes;
        private SortedList<DateTime, double> _datapoints;

        public SmaMetric(ISymbol symbol, int windowSize, int intervalSeconds)
        {
            Symbol = symbol;
            WindowSize = windowSize;
            IntervalSeconds = intervalSeconds;
            _quotes = new Queue<IQuote>(windowSize);
            _datapoints = new SortedList<DateTime, double>(HISTORY_DATAPOINTS);
        }

        public ISymbol Symbol { get; set; }
        public int WindowSize { get; set; }
        public int IntervalSeconds { get; set; }

        public void Add(IQuote quote)
        {
            if (_quotes.Count < 1 || (quote.timestamp - _quotes.Last<IQuote>().timestamp).Seconds >= IntervalSeconds)
            {
                _quotes.Enqueue(quote);
                if (_quotes.Count > WindowSize) _quotes.Dequeue();
                _datapoints.Add(quote.timestamp, Avg);
                if (_datapoints.Count > HISTORY_DATAPOINTS) _datapoints.Remove(_datapoints.First().Key);
            }
        }

        public double Avg
        {
            get
            {
                return _quotes.Average<IQuote>(q => q.price);
            }
        }
    }
}
