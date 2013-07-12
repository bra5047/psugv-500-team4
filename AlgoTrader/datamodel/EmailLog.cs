using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    class EmailLog : IEmailLog 
    {
        [Key]
        public int EmailID { get; set; }
        public string UserEmail{ get; set; }
        public int Quantity { get; set; }
        public string Symbol { get; set; }
        public DateTime timestamp { get; set; }
        public tradeTypes TradeType { get; set; }
        public string Approved { get; set; }


    }
}
