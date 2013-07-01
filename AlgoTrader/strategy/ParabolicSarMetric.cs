using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;

namespace AlgoTrader.strategy
{
    public class ParabolicSarMetric
    {
        public double AccelerationFactor { get; set; }
        public double StepIncrement { get; set; }
        public double MaximumStep { get; set; }
        public int Period { get; set; }

        //private double _prior_period_low;
        //private double _2prior_period_low;
        private double _extreme_point;
        private double _sar;
        private SortedList<DateTime, double> _previous_sar;
        private Queue<DataPoint> _period_lows;

        public ParabolicSarMetric(int period, double _prior_low, double _prior_high)
        {
            AccelerationFactor = 0.02;
            StepIncrement = 0.02;
            MaximumStep = 0.2;
            Period = period;
            _extreme_point = _prior_high;

            DateTime period_start = DateTime.Now;
            _period_lows = new Queue<DataPoint>(2);
            _period_lows.Enqueue(new DataPoint(period_start.AddSeconds(0-Period), _prior_low));
            _period_lows.Enqueue(new DataPoint(period_start, _prior_low));


            //_prior_period_low = _prior_low;
            //_2prior_period_low = _prior_low;
            _sar = _prior_low;
            _previous_sar = new SortedList<DateTime, double>();
            _previous_sar.Add(DateTime.Now, _sar);
        }






        public double SAR
        {
            get
            {
                return _sar;
            }
        }
    }
}
