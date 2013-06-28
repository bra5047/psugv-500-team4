using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;

namespace AlgoTrader.strategy
{
    public class QuoteProvider
    {
        private Timer _timer;
        private DateTime _lastPeek;
        private IStrategy _owner;

        public QuoteProvider(IStrategy owner)
        {
            _owner = owner;
            _lastPeek = new DateTime(1900, 1, 1);
            _timer = new Timer(1000);
            _timer.Elapsed += new ElapsedEventHandler(OnTick);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void OnTick(object source, ElapsedEventArgs e)
        {
            TraderContext db = new TraderContext();
            List<Quote> quotes = db.Quotes.Include("Symbol").Where(x => x.timestamp > _lastPeek).OrderBy(y => y.timestamp).ToList<Quote>();
            _lastPeek = quotes.Max(x => x.timestamp);
            foreach (Quote q in quotes)
            {
                _owner.NewQuote(new QuoteMessage(q));
            }
            db.Dispose();
        }
    }
}
