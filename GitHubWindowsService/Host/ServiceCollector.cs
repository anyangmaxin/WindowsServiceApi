using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GitHubWindowsService.Rest;

namespace GitHubWindowsService.Host
{
    public static class ServiceCollector
    {
        private static ServiceHost restHost;

        public static void StartServices()
        {
            try
            {
                restHost = new ServiceHost(typeof(Restfull));
                restHost.Open();

                foreach (var channel in restHost.ChannelDispatchers)
                {
                    Console.WriteLine("RestHost is running on address: {0}", channel.Listener.Uri);
                }
            }
            catch (AddressAccessDeniedException e)
            {
                Console.WriteLine(@"For non-admin user permissions need to be setup:");
                Console.WriteLine(@"Example: netsh http add urlacl url=http://+:[PortNumber]/ user=[Domain]\[UserName]");
                Console.WriteLine();
                Console.WriteLine(e.ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine(exception.Message);
                Console.WriteLine();
            }
        }

        public static void StopServices()
        {
            if (restHost != null)
                restHost.Close();
        }
    }
}
