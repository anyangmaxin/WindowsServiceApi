using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace GitHubWindowsService.Host
{
    internal class HostingService : ServiceBase
    {
        private readonly HostingServiceRunner serviceRunner;

        public HostingService(HostingServiceRunner serviceRunner)
        {
            if (serviceRunner == null)
                throw new ArgumentNullException("serviceRunner");

            ServiceName = serviceRunner.ServiceName;
            this.serviceRunner = serviceRunner;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            serviceRunner.OnStart();
        }

        protected override void OnStop()
        {
            serviceRunner.OnStop();
            base.OnStop();
        }
    }
}
