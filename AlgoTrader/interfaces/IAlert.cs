using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Interfaces
{
    public interface IAlert
    {
        Guid AlertId { get; set; }
        DateTime Timestamp { get; set; }
        ISymbol Symbol { get; set; }
        tradeTypes Type { get; set; }
        int Quantity { get; set; }
        double Price { get; set; }
        string SentTo { get; set; }
        responseCodes? ResponseCode { get; set; }
        string Response { get; set; }
    }
}
