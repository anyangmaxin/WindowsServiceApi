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
        private struct AccountInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public ServiceAccount Account { get; set; }
        }

        #region 安装服务

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
        private static ServiceController CreateService(string serviceName, string displayName, string binPath,string description, ServiceStartType serviceStartType,
            ServiceAccount serviceAccount, string dependencies, bool startAfterRun)
        {
            if (CheckServiceExist(serviceName))
            {
                return new ServiceController(serviceName);
                // throw new InvalidOperationException("Windows Service:" + serviceName + " has existed!");
            }

            IntPtr databaseHandle = SafeNativeMethods.OpenSCManager(null, null, (int) SCManagerAccess.All);
            IntPtr zero = IntPtr.Zero;
            if (databaseHandle == zero)
            {
                throw new Win32Exception();
            }

            string servicesStartName = null;
            string password = null;
            switch (serviceAccount)
            {
                case ServiceAccount.LocalService:
                {
                    servicesStartName = @"NT AUTHORITY\LocalService";
                    break;
                }
                case ServiceAccount.LocalSystem:
                {
                    servicesStartName = null;
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
                zero = SafeNativeMethods.CreateService(databaseHandle, serviceName, displayName, (int) ServiceAccess.All,
                    (int) ServiceType.Win32OwnProcess,
                    (int) serviceStartType, (int) ServiceErrorControlType.Ignore, binPath, null, IntPtr.Zero,
                    dependencies, servicesStartName, password);

                if (zero == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                if (description != null && description.Length > 0)
                {
                    SERVICE_DESCRIPTION serviceDesc = new SERVICE_DESCRIPTION();
                    serviceDesc.description = Marshal.StringToHGlobalUni(description);
                    bool flag = SafeNativeMethods.ChangeServiceConfig2(zero, (int) ServiceErrorControlType.Normal,
                        ref serviceDesc);
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
                    SafeNativeMethods.CloseServiceHandle(zero);//关闭
                }
                SafeNativeMethods.CloseServiceHandle(databaseHandle);
            }

            if (zero != IntPtr.Zero)
            {
                ServiceController sc = new ServiceController(serviceName);
                if (startAfterRun &&
                    (sc.Status == ServiceControllerStatus.Stopped || sc.Status == ServiceControllerStatus.Paused))
                {
                    sc.Start();
                }
                return sc;
            }
            return null;
        }

        private static ServiceController CreateService(string serviceName, string displayName, string binPath,
            string description, ServiceStartType serviceStartType, ServiceAccount serviceAccount, bool startAfterRun)
        {
            return CreateService(serviceName, displayName, binPath, description, serviceStartType, serviceAccount, null,
                startAfterRun);
        }


        public static ServiceController CreateService(string serviceName, string displayName, string binPath,
            bool startAfterRun)
        {
            return CreateService(serviceName, displayName, binPath, null, ServiceStartType.AutoStart,
                ServiceAccount.LocalSystem, startAfterRun);
        }

        #endregion


        #region 启动服务

        /// <summary>
        /// Takes a service name and starts it
        /// </summary>
        /// <param name="serviceName">The service name</param>
        public static string StartService(string serviceName)
        {
            IntPtr scm = SafeNativeMethods.OpenSCManager(null,null,(int)SafeNativeMethods.SC_MANAGER_CONNECT);

            try
            {
                //获取服务实例
                IntPtr service = SafeNativeMethods.OpenService(scm, serviceName, SafeNativeMethods.SC_MANAGER_ALL_ACCESS);
                if (service == IntPtr.Zero)
                {
                    return "服务不存在";
                }

                try
                {
                    return StartService(service) ? "成功" : "失败";
                }
                finally
                {
                    SafeNativeMethods.CloseServiceHandle(service);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                SafeNativeMethods.CloseServiceHandle(scm);
            }
        }


        /// <summary>
        /// Stars the provided windows service
        /// </summary>
        /// <param name="service">The handle to the windows service</param>
        private static bool StartService(IntPtr service)
        {
            var status = new SERVICE_STATUS();
            int result =SafeNativeMethods.StartService(service,0,0);
            return result > 0;
            //if (result == 0)
            //    throw new ApplicationException("Unable to start service");
            //var changedStatus = WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running);
            //if (!changedStatus)
            //    throw new ApplicationException("Unable to start service");
        }
        #endregion

        #region 卸载服务
        /// <summary>
        /// Takes a service name and tries to stop and then uninstall the windows serviceError
        /// </summary>
        /// <param name="serviceName">The windows service name to uninstall</param>
        public static string Uninstall(string serviceName)
        {
            IntPtr scm =SafeNativeMethods.OpenSCManager(null,null,(int)SafeNativeMethods.SC_MANAGER_ALL_ACCESS);

            try
            {
                IntPtr service = SafeNativeMethods.OpenService(scm, serviceName, SafeNativeMethods.SC_MANAGER_ALL_ACCESS);
                if (service == IntPtr.Zero) return "服务尚未安装";
                // throw new ApplicationException("Service not installed.");

                try
                {
                    StopService(service);
                    if (!SafeNativeMethods.DeleteService(service))
                        return "Could not delete service " + Marshal.GetLastWin32Error();
                    //throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error());

                    return "卸载成功";
                }
                finally
                {
                    SafeNativeMethods.CloseServiceHandle(service);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
               SafeNativeMethods.CloseServiceHandle(scm);
            }
        }
        #endregion

        #region

        /// <summary>
        /// Stops the provided windows service
        /// </summary>
        /// <param name="service">The handle to the windows service</param>
        private static void StopService(IntPtr service)
        {
            var status = new SERVICE_STATUS();
          var handler=  SafeNativeMethods.ControlService(service, ServiceAccess.Stop, status);
            if (handler <= 0)
            {
                throw new ApplicationException("Service not exists!");
            }
            var changedStatus = WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped);
            if (!changedStatus)
                throw new ApplicationException("Unable to stop service");
        }
        #endregion


        #region  WaitForServiceStatus
        /// <summary>
        /// Returns true when the service status has been changes from wait status to desired status
        /// ,this method waits around 10 seconds for this operation.
        /// </summary>
        /// <param name="service">The handle to the service</param>
        /// <param name="waitStatus">The current state of the service</param>
        /// <param name="desiredStatus">The desired state of the service</param>
        /// <returns>bool if the service has successfully changed states within the allowed timeline</returns>
        private static bool WaitForServiceStatus(IntPtr service, ServiceState waitStatus, ServiceState desiredStatus)
        {
            var status = new SERVICE_STATUS();

          SafeNativeMethods.QueryServiceStatus(service, status);
            if (status.currentState == (int) desiredStatus) return true;

            int dwStartTickCount = Environment.TickCount;
            int dwOldCheckPoint = status.checkPoint;

            while (status.currentState == (int) waitStatus)
            {
                // Do not wait longer than the wait hint. A good interval is
                // one tenth the wait hint, but no less than 1 second and no
                // more than 10 seconds.

                int dwWaitTime = status.waitHint / 10;

                if (dwWaitTime < 1000) dwWaitTime = 1000;
                else if (dwWaitTime > 10000) dwWaitTime = 10000;

                Thread.Sleep(dwWaitTime);

                // Check the status again.

                if (SafeNativeMethods.QueryServiceStatus(service, status) == 0) break;

                if (status.checkPoint > dwOldCheckPoint)
                {
                    // The service is making progress.
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = status.checkPoint;
                }
                else
                {
                    if (Environment.TickCount - dwStartTickCount > status.waitHint)
                    {
                        // No progress made within the wait hint
                        break;
                    }
                }
            }
            return (status.currentState == (int) desiredStatus);
        }
        #endregion

        /// <summary>
        /// 根据服务名称获取服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
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
                        accountInfo.UserName = null;
                        accountInfo.Password = null;
                        accountInfo.Account = ServiceAccount.LocalSystem;
                        break;

                    case ServiceInstallerDialogResult.Canceled:
                        throw new InvalidOperationException("Must select Account");
                }
            }

            return accountInfo;

        }

        /// <summary>
        /// 检测服务是否已经存在
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
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
