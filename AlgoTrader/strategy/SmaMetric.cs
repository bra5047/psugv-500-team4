using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using log4net;

namespace AlgoTrader.strategy
{
    // utility class used to track the quote history
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

    // this class calculates a simple moving average with a specified size and time interval
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

        // this defines how many data points are included in the average
        public int WindowSize { get; set; }

        // this defines how frequently the metric will sample
        public int IntervalSeconds { get; set; }

        // submits a new data point to the metric. May or may not be used depending on the specified interval.
        public void Add(DateTime time, double price)
        {
            ILog log = LogManager.GetLogger(typeof(SmaMetric));

            // only use this data point if enough time has passed since the last quote
            if (_quotes.Count < 1 || (time - _quotes.Last<DataPoint>().Timestamp).TotalSeconds >= IntervalSeconds)
            {
                // add the new point and trim the queue if we've reached our max size
                _quotes.Enqueue(new DataPoint(time, price));
                if (_quotes.Count > WindowSize) _quotes.Dequeue();
                // record the new average for historical tracking
                _datapoints.Add(time, Avg);
                if (_datapoints.Count > HISTORY_DATAPOINTS) _datapoints.Remove(_datapoints.First().Key);
            }
            else
            {
                // debugging removed to decrease log file size
                //log.DebugFormat("Data point added: symbol {0} interval {1} time {2} price {3} count {4} elapsed {5}", SymbolName, IntervalSeconds, time, price, _quotes.Count, (time - _quotes.Last<DataPoint>().Timestamp).TotalSeconds);
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

        // produces a set of datapoints showing how this metric has changed over time
        public SortedList<DateTime, double> History
        {
            get
            {
                return _datapoints;
            }
        }

        // produces a string that can be used to identify this metric on a graph
        // includes "SMA" and the time interval in minutes or hours
        public string Label
        {
            get
            {
                string label = string.Empty;
                TimeSpan t = new TimeSpan(0, 0, IntervalSeconds);
                if (t.Hours > 0)
                {
                    label = String.Format("{0:d}h SMA", t.Hours);
                }
                else
                {
                    label = String.Format("{0:d}m SMA", t.Minutes);
                }
                return label;
            }
        }
    }
}
