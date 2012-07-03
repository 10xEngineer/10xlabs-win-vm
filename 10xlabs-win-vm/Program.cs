using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceProcess;

namespace TenxLabsService
{
    class Program : ServiceBase
    {
        private Microcloud microcloud;

        static void Main(string[] args)
        {
            ServiceBase.Run(new Program());
        }

        public Program()
        {
            this.ServiceName = "10xLabs Windows VM";
            this.microcloud = new Microcloud();

            this.log("10xlabs created");


        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            this.log("onStart");

            microcloud.notify("node", "sharp-vm", "confirm", null);

        }

        protected override void OnStop()
        {
            base.OnStop();

            this.log("10xlab stopped");
        }

        protected void log(string something)
        {
            var log = File.AppendText("c:\\10x.log");
            log.WriteLine(DateTime.Now);
            log.WriteLine(something);
            log.Close();
        }

    }
}
