using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.datamodel;
using log4net;

namespace ATStrategyService
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

            log.Info("Loading configuration");
            Dictionary<string, string> settings = GetConfiguration();
            log.DebugFormat("Loaded {0} configuration values", settings.Count);

            serviceHost = new ServiceHost(new AlgoTrader.strategy.ThreeDucksStrategy(settings));

            //serviceHost = new ServiceHost(typeof(AlgoTrader.strategy.ThreeDucksStrategy));
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

        private Dictionary<string, string> GetConfiguration()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            try
            {
                TraderContext db = new TraderContext();
                var settings = from s in db.SystemSettings where s.Module == "ThreeDuckStrategy" select s;
                foreach (var i in settings)
                {
                    config.Add(i.Name, i.Value);
                    log.DebugFormat("Loaded configuration value: {0}={1}", i.Name, i.Value);
                }
            }
            catch (Exception ex)
            {
                log.WarnFormat("Failed to load configuration: {0}", ex.Message);
                log.Warn(ex.StackTrace);
            }

            return config;
        }
    }
}
