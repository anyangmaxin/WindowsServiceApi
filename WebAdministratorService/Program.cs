﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WebAdministratorService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            string ParamTest = "";
            if (args.Length > 0)
            {
                ParamTest = args[0];
            }
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceTest(ParamTest)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
