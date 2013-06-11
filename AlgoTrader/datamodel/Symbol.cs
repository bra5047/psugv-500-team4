using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    public class Symbol : ISymbol
    {
        [Key]
        public string name { get; set; }

        public Symbol()
        {
            name = string.Empty;
        }

        public Symbol(string symbol_name)
        {
            name = symbol_name;
        }
    }
}
