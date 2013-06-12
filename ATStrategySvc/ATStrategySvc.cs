using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ATStrategySvc
{
    public partial class ATStrategySvc : ServiceBase
    {
        public ServiceHost serviceHost = null;
        private ILog log;

        public ATStrategySvc()
        {
            InitializeComponent();
            ServiceName = "ATStrategySvc";
        }

        protected override void OnStart(string[] args)
        {
            log = LogManager.GetLogger(typeof(ATStrategySvc));

            if (serviceHost != null)
            {
                log.Info("Service restarting");
                serviceHost.Close();
            }

            serviceHost = new ServiceHost(typeof(AlgoTrader.strategy.ThreeDucksStrategy));
            log.Info("Service starting");
            serviceHost.Open();
            log.Info("Service started");
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                log.Info("Service stopping");
                serviceHost.Close();
                log.Info("Service stopped");
                serviceHost = null;
            }
        }
    }
}
