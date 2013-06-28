﻿using System;
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

            TraderContext db = DbContext;
            Symbol s = db.Symbols.Where(x => x.name == symbolName).FirstOrDefault();
            Alert a = new Alert();
            a.Symbol = s;
            a.Type = type;
            a.Quantity = quantity;
            a.Price = price;
            a.ResponseCode = responseCodes.Pending;
            db.Alerts.Add(a);
            db.SaveChanges();
            db.Dispose();
        }

        public void processAlertResponse(string alertID, responseCodes alertResponseCode, string alertResponse)
        {
            ILog log = Logger;
            log.DebugFormat("Alert generated: {0} {1}", alertID, alertResponse);
            TraderContext db = DbContext;
            Guid alertGuid = Guid.Parse(alertID);
            Alert alert = db.Alerts.Where(x => x.AlertId == alertGuid).FirstOrDefault();
            if (alert == null)
            {
                log.WarnFormat("Alert not found: {0}", alertID);
                return;
            }
            alert.ResponseCode = alertResponseCode;
            alert.Response = alertResponse;
            db.SaveChanges();
            db.Dispose();
        }

        public List<AlertMessage> getPendingAlerts()
        {
            List<AlertMessage> pending = new List<AlertMessage>();
            TraderContext db = DbContext;
            foreach (IAlert a in db.Alerts.Include("Symbol").Where(x => x.ResponseCode == responseCodes.Pending))
            {
                pending.Add(new AlertMessage(a));
            }
            return pending;
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

        private TraderContext _dbContext;

        protected TraderContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }
                else
                {
                    return new TraderContext();
                }
            }
        }

        public UserAgent()
        {
            // no-arg constructor
        }
    }
}
