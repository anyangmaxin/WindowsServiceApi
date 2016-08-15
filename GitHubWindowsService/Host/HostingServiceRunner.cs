using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GitHubWindowsService.Host
{
    public class HostingServiceRunner
    {
        public Action OnStart { get; private set; }
        public Action OnStop { get; private set; }
        public string ServiceName { get; private set; }

        private HostingServiceRunner(string serviceName)
        {
            ServiceName = serviceName;
        }

        public static HostingServiceRunner Service(string serviceName)
        {
            return new HostingServiceRunner(serviceName);
        }

        public HostingServiceRunner StartAction(Action startAction)
        {
            OnStart = startAction;
            return this;
        }

        public HostingServiceRunner StopAction(Action stopAction)
        {
            OnStop = stopAction;
            return this;
        }

        public void Run()
        {
            using (var service = new HostingService(this))
            {
                ServiceBase.Run(service);
            }
        }
    }
}
