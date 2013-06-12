using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace ATStrategySvc
{
    [RunInstaller(true)]
    public partial class ATStrategySvcInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller _process;
        private ServiceInstaller _service;

        public ATStrategySvcInstaller()
        {
            InitializeComponent();
            _process = new ServiceProcessInstaller();
            _process.Account = ServiceAccount.LocalSystem;
            _service = new ServiceInstaller();
            _service.ServiceName = "ATStrategySvc";
            Installers.Add(_process);
            Installers.Add(_service);
        }
    }
}
