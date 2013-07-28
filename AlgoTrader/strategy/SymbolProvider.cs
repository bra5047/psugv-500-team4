using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using AlgoTrader.watchlist;
using AlgoTrader.portfolio;

namespace AlgoTrader.strategy
{
    public class SymbolProvider
    {
        private Timer _timer;
        private IStrategy _owner;

        public SymbolProvider(IStrategy owner)
        {
            _owner = owner;
            _timer = new Timer(5000);
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
            PortfolioManager pm = new PortfolioManager();
            List<PositionMessage> pos = pm.GetOpenPositions();
            List<string> owned = pos.Select(x => x.SymbolName).ToList<string>();
            _owner.WatchForSell = owned;

            SortedSet<string> watched = new SortedSet<string>();
            IWatchListManager wlm = new WatchListManager();
            List<WatchList> lists = wlm.GetAllWatchLists();
            foreach (WatchList wl in lists)
            {
                foreach (WatchListItem i in wl.Items)
                {
                    watched.Add(i.SymbolName);
                }
            }
            _owner.WatchForBuy = watched.ToList<string>();
        }
    }
}
