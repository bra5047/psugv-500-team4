using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace AlgoTrader.datamodel
{
    public class TraderContext : DbContext
    {
        public TraderContext()
            : base("name=AlgoTraderDb")
        {
        }

        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<WatchListItem> WatchListItems { get; set; }
    }
}