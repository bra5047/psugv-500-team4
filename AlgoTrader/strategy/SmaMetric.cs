using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.strategy
{
    class DataPoint
    {
        public DataPoint(DateTime time, double val)
        {
            Timestamp = time;
            Value = val;
        }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }

    public class SmaMetric
    {
        private static int HISTORY_DATAPOINTS = 50;

        private Queue<DataPoint> _quotes;
        private SortedList<DateTime, double> _datapoints;

        public SmaMetric(string symbol, int windowSize, int intervalSeconds)
        {
            SymbolName = symbol;
            WindowSize = windowSize;
            IntervalSeconds = intervalSeconds;
            _quotes = new Queue<DataPoint>(windowSize);
            _datapoints = new SortedList<DateTime, double>(HISTORY_DATAPOINTS);
        }

        public string SymbolName { get; set; }
        public int WindowSize { get; set; }
        public int IntervalSeconds { get; set; }

        public void Add(DateTime time, double price)
        {
            if (_quotes.Count < 1 || (time - _quotes.Last<DataPoint>().Timestamp).Seconds >= IntervalSeconds)
            {
                _quotes.Enqueue(new DataPoint(time, price)); 
                if (_quotes.Count > WindowSize) _quotes.Dequeue();
                _datapoints.Add(time, Avg);
                if (_datapoints.Count > HISTORY_DATAPOINTS) _datapoints.Remove(_datapoints.First().Key);
            }
        }

        public double Avg
        {
            get
            {
                if (_quotes.Count > 0)
                {
                    return _quotes.Average<DataPoint>(q => q.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
