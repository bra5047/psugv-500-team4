using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Interfaces;
using AlgoTrader.datamodel;
using log4net;

namespace AlgoTrader.useragent
{
    public class UserAgent : IUserAgent
    {
        public void generateAlert(string symbolName, tradeTypes type, int quantity, double price)
        {
            ILog log = Logger;
            log.DebugFormat("Alert generated: {0} {1} {2} {3}", symbolName, type.ToString(), quantity.ToString(), price.ToString());
        }

        public void processAlertResponse(string alertID, string alertResponse)
        {
            ILog log = Logger;
            log.DebugFormat("Alert generated: {0} {1}", alertID, alertResponse);
        }

        private ILog _logger;
        public ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    return LogManager.GetLogger(typeof(AlgoTrader.useragent.UserAgent));
                }
                else
                {
                    return _logger;
                }
            }
            set
            {
                _logger = value;
            }
        }

        public UserAgent()
        {
            // no-arg constructor
        }
    }
}
