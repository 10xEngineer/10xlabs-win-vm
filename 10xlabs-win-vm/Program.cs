using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;

namespace TenxLabsService
{
    class Program : ServiceBase
    {
        private Microcloud microcloud;
        private string nodeName;

        static void Main(string[] args)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            string endpoint = appSettings["endpoint"];
            string nodeName = appSettings["node"];
           
            ServiceBase.Run(new Program(endpoint, nodeName));
        }

        public Program(string endpoint, string nodeName)
        {

            this.ServiceName = "10xLabs Windows VM";

            if (endpoint != null)
            {
                this.microcloud = new Microcloud(endpoint);
            }

            this.nodeName = nodeName;

            if (this.nodeName == null) this.nodeName = "no-node-name-provided-win-vm";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            if (this.microcloud == null)
            {
                Program.logEvent(@"10xLabs endpoint not configured! Stopping.");
                base.Stop();

                return;
            }

            // TODO 
            microcloud.notify("node", this.nodeName, "confirm", null);
        }

        protected override void OnStop()
        {
            base.OnStop();
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

    }
}
