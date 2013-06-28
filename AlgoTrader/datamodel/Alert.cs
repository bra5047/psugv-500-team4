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
    public class Alert : IAlert
    {
        [Key]
        public Guid AlertId { get; set; }
        public DateTime Timestamp { get; set; }
        public string SymbolName { get; set; }
        [ForeignKey("SymbolName")]
        public virtual Symbol Symbol { get; set; }
        public tradeTypes Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string SentTo { get; set; }
        public responseCodes? ResponseCode { get; set; }
        public string Response { get; set; }

        ISymbol IAlert.Symbol
        {
            get { return Symbol; }
            set { throw new NotImplementedException(); }
        }
    }
}
