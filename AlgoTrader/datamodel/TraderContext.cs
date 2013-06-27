using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.Entity;

namespace AlgoTrader.datamodel
{
    public class TraderContext : DbContext
    {
        public TraderContext()
            : base("name=AlgoTraderDb")
        {
        }

        public TraderContext(DbConnection connection)
            : base(connection, true)
        {
        }

        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<WatchListItem> WatchListItems { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public Quote FindLastQuoteFor(Symbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");
            return Quotes.Where(x => x.SymbolName == symbol.name).OrderByDescending(y => y.timestamp).FirstOrDefault();
        }

        public Quote FindLastQuoteFor(string symbolName)
        {
            Symbol s = Symbols.Where(x => x.name == symbolName).FirstOrDefault();
            if (s == null) throw new ArgumentException("Unable to locate symbol.", "symbolName");
            return FindLastQuoteFor(s);
        }
    }
}