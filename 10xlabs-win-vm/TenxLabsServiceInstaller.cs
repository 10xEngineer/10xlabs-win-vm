using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;

namespace TenxLabsService
{
    [RunInstaller(true)]
    public class TenxLabsServiceInstaller : Installer
    {
        public TenxLabsServiceInstaller()
        {
            var processInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            var serviceInstaller = new System.ServiceProcess.ServiceInstaller();

            processInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;

            // service details
            serviceInstaller.DisplayName = "10xLabs Windows VM";
            serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;

            // Same as Program.ServiceName
            serviceInstaller.ServiceName = "10xLabs Windows VM";

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }

    }
}
