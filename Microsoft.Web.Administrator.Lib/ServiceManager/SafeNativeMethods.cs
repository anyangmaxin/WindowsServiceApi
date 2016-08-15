using System;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

namespace WindowsServiceInvest.ConfigureTest
{
    internal class SafeNativeMethods
    {
        #region const

        public const System.Int32 STANDARD_RIGHTS_REQUIRED = 0xF0000;
        // Service Control Manager object specific access types  
        public const System.Int32 SC_MANAGER_CONNECT = 0x0001;
        public const System.Int32 SC_MANAGER_CREATE_SERVICE = 0x0002;
        public const System.Int32 SC_MANAGER_ENUMERATE_SERVICE = 0x0004;
        public const System.Int32 SC_MANAGER_LOCK = 0x0008;
        public const System.Int32 SC_MANAGER_QUERY_LOCK_STATUS = 0x0010;
        public const System.Int32 SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020;
        public const System.Int32 SC_MANAGER_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED
                                                            | SC_MANAGER_CONNECT
                                                            | SC_MANAGER_CREATE_SERVICE
                                                            | SC_MANAGER_ENUMERATE_SERVICE
                                                            | SC_MANAGER_LOCK
                                                            | SC_MANAGER_QUERY_LOCK_STATUS
                                                            | SC_MANAGER_MODIFY_BOOT_CONFIG;


        // Service object specific access type  
        public const System.Int32 SERVICE_QUERY_CONFIG = 0x0001;
        public const System.Int32 SERVICE_CHANGE_CONFIG = 0x0002;
        public const System.Int32 SERVICE_QUERY_STATUS = 0x0004;
        public const System.Int32 SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
        public const System.Int32 SERVICE_START = 0x0010;
        public const System.Int32 SERVICE_STOP = 0x0020;
        public const System.Int32 SERVICE_PAUSE_CONTINUE = 0x0040;
        public const System.Int32 SERVICE_INTERROGATE = 0x0080;
        public const System.Int32 SERVICE_USER_DEFINED_CONTROL = 0x0100;
        public const System.Int32 SERVICE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED
                                                        | SERVICE_QUERY_CONFIG
                                                        | SERVICE_CHANGE_CONFIG
                                                        | SERVICE_QUERY_STATUS
                                                        | SERVICE_ENUMERATE_DEPENDENTS
                                                        | SERVICE_START
                                                        | SERVICE_STOP
                                                        | SERVICE_PAUSE_CONTINUE
                                                        | SERVICE_INTERROGATE
                                                        | SERVICE_USER_DEFINED_CONTROL;

        // service type  
        public const System.Int32 SERVICE_KERNEL_DRIVER = 0x00000001;
        public const System.Int32 SERVICE_FILE_SYSTEM_DRIVER = 0x00000002;
        public const System.Int32 SERVICE_ADAPTER = 0x00000004;
        public const System.Int32 SERVICE_RECOGNIZER_DRIVER = 0x00000008;
        public const System.Int32 SERVICE_DRIVER = SERVICE_KERNEL_DRIVER
                                                    | SERVICE_FILE_SYSTEM_DRIVER
                                                    | SERVICE_RECOGNIZER_DRIVER;

        public const System.Int32 SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        public const System.Int32 SERVICE_WIN32_SHARE_PROCESS = 0x00000020;
        public const System.Int32 SERVICE_WIN32 = SERVICE_WIN32_OWN_PROCESS
                                                    | SERVICE_WIN32_SHARE_PROCESS;

        public const System.Int32 SERVICE_INTERACTIVE_PROCESS = 0x00000100;
        public const System.Int32 SERVICE_TYPE_ALL = SERVICE_WIN32
                                                    | SERVICE_ADAPTER
                                                    | SERVICE_DRIVER
                                                    | SERVICE_INTERACTIVE_PROCESS;


        // Start Type  
        public const System.Int32 SERVICE_BOOT_START = 0x00000000;
        public const System.Int32 SERVICE_SYSTEM_START = 0x00000001;
        public const System.Int32 SERVICE_AUTO_START = 0x00000002;
        public const System.Int32 SERVICE_DEMAND_START = 0x00000003;
        public const System.Int32 SERVICE_DISABLED = 0x00000004;


        // Error control type  
        public const System.Int32 SERVICE_ERROR_IGNORE = 0x00000000;
        public const System.Int32 SERVICE_ERROR_NORMAL = 0x00000001;
        public const System.Int32 SERVICE_ERROR_SEVERE = 0x00000002;
        public const System.Int32 SERVICE_ERROR_CRITICAL = 0x00000003;


        // Info levels for ChangeServiceConfig2 and QueryServiceConfig2  
        public const System.Int32 SERVICE_CONFIG_DESCRIPTION = 1;
        public const System.Int32 SERVICE_CONFIG_FAILURE_ACTIONS = 2;


        #endregion

        #region methods

        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="databaseHandle"></param>
        /// <param name="serviceName"></param>
        /// <param name="displayName"></param>
        /// <param name="access"></param>
        /// <param name="serviceType"></param>
        /// <param name="startType"></param>
        /// <param name="errorControl"></param>
        /// <param name="binaryPath"></param>
        /// <param name="loadOrderGroup"></param>
        /// <param name="pTagId"></param>
        /// <param name="dependencies"></param>
        /// <param name="servicesStartName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateService(IntPtr databaseHandle, string serviceName, string displayName, int access, int serviceType, int startType, int errorControl, string binaryPath, string loadOrderGroup, IntPtr pTagId, string dependencies, string servicesStartName, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="databaseName"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenSCManager(string machineName, string databaseName, int access);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr databaseHandle, string serviceName, int access);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeleteService(IntPtr serviceHandle);

        #region ControlService
        [DllImport("advapi32.dll")]
        public static extern int ControlService(IntPtr hService, ServiceAccess dwControl, SERVICE_STATUS lpServiceStatus);
        #endregion

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CloseServiceHandle(IntPtr handle);

        #region QueryServiceStatus
        [DllImport("advapi32.dll")]
        public static extern int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);
        #endregion

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ChangeServiceConfig2(IntPtr serviceHandle, uint infoLevel, ref SERVICE_DESCRIPTION serviceDesc);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumDependentServices(IntPtr serviceHandle, int serviceState, IntPtr bufferOfENUM_SERVICE_STATUS, int bufSize, ref int bytesNeeded, ref int numEnumerated);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumServicesStatus(IntPtr databaseHandle, int serviceType, int serviceState, IntPtr status, int size, out int bytesNeeded, out int servicesReturned, ref int resumeHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumServicesStatusEx(IntPtr databaseHandle, int infolevel, int serviceType, int serviceState, IntPtr status, int size, out int bytesNeeded, out int servicesReturned, ref int resumeHandle, string group);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool QueryServiceConfig(IntPtr serviceHandle, IntPtr query_service_config_ptr, int bufferSize, out int bytesNeeded);

        #region StartService
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);
        #endregion

        #endregion

    }

}
