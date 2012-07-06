using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Reflection;

namespace TenxLabsService
{
    class Program : ServiceBase
    {
        private Microcloud microcloud;
        private dynamic data;
        private string nodeName;

        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                string param = string.Concat(args);
                switch (param)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--test":
                        Console.WriteLine(Ec2Metadata.getKey("instance-id"));
                        break;
                }
            }
            else
            {
                ServiceBase.Run(new Program());
            }
        }

        public Program()
        {
            this.ServiceName = "10xLabs Windows VM";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            this.nodeName = Ec2Metadata.getKey("instance-id");
            var hostname = Ec2Metadata.getKey("public-hostname");

            if (hostname == null)
            {
                hostname = Ec2Metadata.getKey("public-ipv4");
            }

            // check if running on EC2
            if (this.nodeName == null) {
                Program.logEvent(@"10xLabs Win VM most likely not running on Amazon EC2! Stopping.");
                base.Stop();

                return;
            }

            Dictionary<string, string> node = new Dictionary<string, string>();
            node.Add("hostname", hostname);

            Microcloud mc = getMicrocloud();
            mc.notify("node", this.nodeName, "confirm", node);
        }

        protected override void OnStop()
        {
            base.OnStop();

            Microcloud mc = getMicrocloud();
            mc.notify("node", this.nodeName, "stop", null);
        }

        public static void logEvent(string evn) 
        {
            string sSource = "10xlabs-win-vm";
            string sLog = "Application";
            
            if (!EventLog.SourceExists(sSource))
            {
                EventLog.CreateEventSource(sSource, sLog);
            }

            EventLog.WriteEntry(sSource, evn);
        }

        private Microcloud getMicrocloud()
        {
            if (this.microcloud != null) { return this.microcloud; }

            this.microcloud = new Microcloud(getData()["endpoint"]);

            return this.microcloud;
        }

        private dynamic getData()
        {
            if (this.data == null)
            {
                this.data = Ec2Metadata.getCustomData();
            }

            return this.data;
        }
   

    }
}
