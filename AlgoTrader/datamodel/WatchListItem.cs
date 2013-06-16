using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    public class WatchListItem
    {
        [Column(Order=0), Key]
        public string SymbolName { get; set; }
        [Column(Order = 1), Key]
        public string ListName { get; set; }

        [ForeignKey("SymbolName")]
        public virtual Symbol Symbol { get; set; }
        [ForeignKey("ListName")]
        public virtual WatchList WatchList { get; set; }

		public WatchListItem(ISymbol symbol, string listName)
		{
			SymbolName = symbol.ToString();
			ListName = listName;
		}

        public WatchListItem()
        {
            // needs to have a zero-argument constructor
        }
    }
}