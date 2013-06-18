﻿using System;
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
    public class WatchList
    {
        [Key]
        public string ListName { get; set; }
        public virtual List<WatchListItem> Items { get; set; }

		public WatchList(string listName)
		{
			ListName = listName;
			Items = new List<WatchListItem>();
		}

		public WatchList()
		{
			Items = new List<WatchListItem>();
		}

        public bool addToList(ISymbol symbol, string listName)
        {
			foreach (WatchListItem item in Items)
			{
				if (item.SymbolName == symbol.name && item.ListName == listName)
					return false;
			}

			WatchListItem newItem = new WatchListItem(symbol, listName);
			Items.Add(newItem);
			return true;
		}

        public bool removeFromList(ISymbol symbol, string listName)
        {
			if (Items.Contains(new WatchListItem(symbol, listName)))
			{
				Items.RemoveAll(item => (item.SymbolName == symbol.ToString() && item.ListName == listName));
				return true;
			}
            return false;
        }

        public List<ISymbol> symbols
        {
            get
            {
                List<ISymbol> result = new List<ISymbol>();
                foreach (WatchListItem i in Items)
                {
                    result.Add(i.Symbol);
                }
                return result;
            }
        }
    }
}