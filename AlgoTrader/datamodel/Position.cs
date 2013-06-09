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
    public class Position
    {
        [Key]
        public int PositionId { get; set; }
        public string SymbolName { get; set; }
        [ForeignKey("SymbolName")]
        public virtual Symbol Symbol { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public positionStatus status { get; set; }
        public virtual List<Trade> Trades { get; set; }
    }
}
