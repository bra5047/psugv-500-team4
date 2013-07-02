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

        private double _extreme_point;
        private double _sar;
        private SortedList<DateTime, double> _previous_sar;
        private SortedList<DateTime, double> _period_lows;

        public ParabolicSarMetric(int period, double _prior_low, double _prior_high)
        {
            AccelerationFactor = 0.02;
            StepIncrement = 0.02;
            MaximumStep = 0.2;
            Period = period;
            _extreme_point = _prior_high;

            DateTime period_start = DateTime.Now;
            _period_lows = new SortedList<DateTime, double>(3); // 2prior, prior, current
            _period_lows.Add(period_start.AddSeconds(0-Period), _prior_low);
            _period_lows.Add(period_start, _prior_low);

            _sar = _prior_low;
            _previous_sar = new SortedList<DateTime, double>();
            _previous_sar.Add(DateTime.Now, _sar);
        }

        public void Add(DateTime time, double price)
        {
            if (price > _extreme_point)
            {
                _extreme_point = price;
                if (AccelerationFactor < MaximumStep) AccelerationFactor += StepIncrement;
            }
            // we've entered a new period, start recalculating stuff
            if ((time - _previous_sar.Last().Key).TotalSeconds >= Period)
            {
                // first cycle the tracking lists
                _period_lows.Remove(_period_lows.First().Key);
                _period_lows.Add(time, price);
                _previous_sar.Add(time, _sar);
                double _last_sar = _sar;
                double _new_sar = _last_sar + (AccelerationFactor * (_extreme_point - _last_sar));
                _sar = Math.Min(_new_sar, _period_lows.Min(x => x.Value));
            }
            else
            {
                // just track if you've seen a new low
                if (price < _period_lows.Last().Value)
                {
                    _period_lows.Remove(_period_lows.Last().Key);
                    _period_lows.Add(time, price);
                }
            }
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
