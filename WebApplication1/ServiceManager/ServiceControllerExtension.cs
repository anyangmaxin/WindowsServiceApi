using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.ServiceProcess;
using System.ServiceProcess.Design;
using System.ComponentModel;
using System.Threading;

namespace WindowsServiceInvest.ConfigureTest
{
    public class ServiceControllerExtension
    {
        internal struct AccountInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public ServiceAccount Account { get; set; }
        }
  
        /// <summary>
        /// Invocate the Windows API CreateService to create a windows service
        /// </summary>
        /// <param name="serviceName">The service name that indentified by Machine</param>
        /// <param name="displayName">The Display Name in Service Manager Control </param>
        /// <param name="binPath">The Full Name(include path) of Service Application </param>
        /// <param name="description">The Description displayed in Service Manager Control</param>
        /// <param name="serviceStartType">The Start Type displayed in Service Manager Control</param>
        /// <param name="serviceAccount">The Start Account displayed in Service Manager Control</param>
        /// <param name="dependencies">The other services' name which the creating service dependes on, service name joined by space,like: "service1 service2 service3" </param>
        /// <returns></returns>
        public static ServiceController CreateService(string serviceName,string displayName,string binPath ,string description,ServiceStartType serviceStartType ,
             ServiceAccount serviceAccount,string dependencies,bool startAfterRun)
        {
            if (CheckServiceExist(serviceName))
            {
                throw new InvalidOperationException("Windows Service:" + serviceName + " has existed!");
            }

            IntPtr databaseHandle = SafeNativeMethods.OpenSCManager(null, null, (int)SCManagerAccess.All );
            IntPtr zero = IntPtr.Zero;
            if (databaseHandle == zero)
            {
                throw new Win32Exception ();
            }

            string servicesStartName=null  ;
            string password = null  ;
            switch (serviceAccount)
            {
                case ServiceAccount.LocalService:
                    {
                        servicesStartName = @"NT AUTHORITY\LocalService";
                        break;
                    }
                case ServiceAccount.LocalSystem:
                    {
                        servicesStartName =null ;
                        password = null;
                        break;
                    }
                case ServiceAccount.NetworkService:
                    {
                        servicesStartName = @"NT AUTHORITY\NetworkService";
                        break;
                    }
                case ServiceAccount.User:
                    {
                        AccountInfo accountInfo = GetLoginInfo();
                        serviceAccount = accountInfo.Account;
                        password = accountInfo.Password;
                        servicesStartName = accountInfo.UserName;
                        break;
                    }
            }
 
            try
            {
                zero = SafeNativeMethods.CreateService(databaseHandle, serviceName, displayName, (int)ServiceAccess.All, (int)ServiceType.Win32OwnProcess,
                    (int)serviceStartType, (int)ServiceErrorControlType.Ignore, binPath, null, IntPtr.Zero, dependencies, servicesStartName, password);

                if (zero == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                if (description != null && description.Length > 0)
                {
                    SERVICE_DESCRIPTION serviceDesc = new SERVICE_DESCRIPTION();
                    serviceDesc.description = Marshal.StringToHGlobalUni(description);
                    bool flag = SafeNativeMethods.ChangeServiceConfig2(zero, (int)ServiceErrorControlType.Normal, ref serviceDesc);
                    Marshal.FreeHGlobal(serviceDesc.description);

                    if (!flag)
                    {
                        throw new Win32Exception();
                    }
                }
            }
            finally 
            {
                if (zero != IntPtr.Zero)
                {
                    SafeNativeMethods.CloseServiceHandle(zero);
                }
                SafeNativeMethods.CloseServiceHandle(databaseHandle ); 
            }

            if (zero != IntPtr.Zero)
            {
                ServiceController sc = new ServiceController(serviceName);
                if (startAfterRun)
                    sc.Start();
                return sc;
            }
            else
            {
                return null;
            }
        }

        public static ServiceController CreateService(string serviceName, string displayName, string binPath, string description, ServiceStartType serviceStartType, ServiceAccount serviceAccount, bool startAfterRun)
        {
            return CreateService(serviceName, displayName, binPath, description, serviceStartType, serviceAccount,null,startAfterRun  );
        }

        public static ServiceController CreateService(string serviceName, string displayName, string binPath,bool startAfterRun)
        {
            return CreateService(serviceName, displayName, binPath, null, ServiceStartType.AutoStart, ServiceAccount.LocalSystem, startAfterRun);
        }

        public static bool DeleteService(string serviceName)
        {
            if (!CheckServiceExist(serviceName))
            {
                throw new InvalidOperationException("Windows Service:"+serviceName +" doesn't exist!");
            }

            bool result = false;
            IntPtr databaseHandle = IntPtr.Zero;
            IntPtr zero = IntPtr.Zero;

            databaseHandle = SafeNativeMethods.OpenSCManager(null, null, (int)SCManagerAccess.All);
            if (databaseHandle == zero)
            {
                throw new Win32Exception();
            }

            try
            {
                zero = SafeNativeMethods.OpenService(databaseHandle, serviceName, (int)ServiceAccess.All);
                if (zero == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }
                result = SafeNativeMethods.DeleteService(zero);
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    SafeNativeMethods.CloseServiceHandle(zero);
                }
                SafeNativeMethods.CloseServiceHandle(databaseHandle);
            }


            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        sc.Stop();
                        sc.Refresh();
                        int num = 10;
                        while (sc.Status != ServiceControllerStatus.Stopped && num > 0)
                        {
                            Thread.Sleep(0x3e8);
                            sc.Refresh();
                            num--;
                        }

                    }
                }
            }
            catch { }

            return result; 
        }

        public static ServiceController GetService(string serviceName)
        {
            return new ServiceController(serviceName);
        }

        private static AccountInfo GetLoginInfo()
        {
            AccountInfo accountInfo = new AccountInfo();
           using (ServiceInstallerDialog dialog = new ServiceInstallerDialog())
            {                
                dialog.ShowDialog();
                switch (dialog.Result)
                {
                    case ServiceInstallerDialogResult.OK:
                        accountInfo.UserName = dialog.Username;
                        accountInfo.Password = dialog.Password;
                        break;

                    case ServiceInstallerDialogResult.UseSystem:
                        accountInfo.UserName  = null;
                        accountInfo.Password  = null;
                        accountInfo.Account  = ServiceAccount.LocalSystem;
                        break ;

                    case ServiceInstallerDialogResult.Canceled:
                        throw new InvalidOperationException("Must select Account");
                } 
            }

           return accountInfo;
            
        }

        private static bool CheckServiceExist(string serviceName)
        {
            bool exist = false;

            ServiceController sc = new ServiceController(serviceName);
            try
            {
                exist = sc.CanPauseAndContinue || sc.CanStop || true;
            }
            catch
            {
                exist = false;
            }
            finally
            {
                sc.Dispose();
            }
            return exist;
        }
    }
}
