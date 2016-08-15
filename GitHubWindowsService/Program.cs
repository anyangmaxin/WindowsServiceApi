using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GitHubWindowsService.Host;
using ServiceInstaller = GitHubWindowsService.Installer.ServiceInstaller;

namespace GitHubWindowsService
{
    class Program:ServiceBase
    {
        private const string Name = "Rest Service";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            if (args == null)
                return;
            switch (args[0])
            {
                case null:
                    return;

                case "-Console":
                    {
                        ServiceCollector.StartServices();
                        Console.ReadLine();
                        ServiceCollector.StopServices();
                    }
                    break;

                case "-Service":
                    HostingServiceRunner
                        .Service(Name)
                        .StartAction(ServiceCollector.StartServices)
                        .StopAction(ServiceCollector.StopServices)
                        .Run();
                    break;

                case "-Install":
                    Install(Name);
                    break;

                case "-Uninstall":
                    Uninstall(Name);
                    break;
            }

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);
        }


        public static void Install(string serviceName)
        {
            string fileName = Path.Combine(Environment.CurrentDirectory, Environment.GetCommandLineArgs()[0] + " -Service");

            try
            {
                ServiceInstaller.Install(serviceName, serviceName, fileName);
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public static void Uninstall(string serviceName)
        {
            try
            {
                ServiceInstaller.Uninstall(serviceName);
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
