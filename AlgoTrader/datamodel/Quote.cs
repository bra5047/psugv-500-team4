using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    public class Quote
    {
        [Key]
        public int QuoteId { get; set; }
        public double price { get; set; }
        public DateTime timestamp { get; set; }

        public string SymbolName { get; set; }
        [ForeignKey("SymbolName")]
        public virtual Symbol Symbol { get; set; }

    }
    public class Qoutemanager : IQuoteManager
    {


    }
}