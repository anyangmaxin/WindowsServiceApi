using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WindowsServiceInvest.ConfigureTest
{

    #region structs

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVICE_DESCRIPTION
    {
        public IntPtr description;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVICE_STATUS
    {
        public int serviceType;
        public int currentState;
        public int controlsAccepted;
        public int win32ExitCode;
        public int serviceSpecificExitCode;
        public int checkPoint;
        public int waitHint;


    }


    #endregion

    #region enums

    public enum ServiceState
    {
        Unknown = -1, // The state cannot be (has not been) retrieved.
        NotFound = 0, // The service is not known on the host server.
        Stopped = 1,
        StartPending = 2,
        StopPending = 3,
        Running = 4,
        ContinuePending = 5,
        PausePending = 6,
        Paused = 7
    }

    public enum SCManagerAccess
    {
        /// <summary>
        /// 0xF0000
        /// </summary>
        StandardRightsRequired = 0xF0000,

        /// <summary>
        /// 0x0001
        /// </summary>
        Connect = 0x0001,
        
        /// <summary>
        /// 0x0002
        /// </summary>
        CreateService = 0x0002,

        /// <summary>
        /// 0x0004
        /// </summary>
        EnumerateService = 0x0004,

        /// <summary>
        /// 0x0008
        /// </summary>
        Lock = 0x0008,

        /// <summary>
        /// 0x0010
        /// </summary>
        QueryLockStatus = 0x0010,

        /// <summary>
        /// 0x0020
        /// </summary>
        ModifyBootConfig = 0x0020,

        /// <summary>
        /// 0xF003F
        /// </summary>
        All = 0xF003F
    }

    public enum ServiceAccess
    {
        /// <summary>
        /// 0xF0000
        /// </summary>
        StandardRightsRequired = 0xF0000,

        /// <summary>
        /// 0x0001
        /// </summary>
        QueryConfig = 0x0001,

        /// <summary>
        /// 0x0002
        /// </summary>
        ChangeConfig = 0x0002,

        /// <summary>
        /// 0x0004
        /// </summary>
        QueryStatus = 0x0004,

        /// <summary>
        /// 0x0008
        /// </summary>
        EnumerateDependents = 0x0008,

        /// <summary>
        /// 0x0010
        /// </summary>
        Start = 0x0010,

        /// <summary>
        /// 0x0020
        /// </summary>
        Stop = 0x0020,

        /// <summary>
        /// 0x0040
        /// </summary>
        PauseContinue = 0x0040,

        /// <summary>
        /// 0x0080
        /// </summary>
        InterRogate = 0x0080,

        /// <summary>
        /// 0x0100
        /// </summary>
        UserDefinedControl = 0x0100,

        /// <summary>
        /// 0xF01FF
        /// </summary>
        All = 0xF01FF
    }

    public enum ServiceType
    {
        /// <summary>
        /// 0x0001
        /// </summary>
        KernelDriver = 0x0001,

        /// <summary>
        /// 0x0002
        /// </summary>
        FileSystemDriver = 0x0002,

        /// <summary>
        /// 0x0004
        /// </summary>
        Adapter = 0x0004,

        /// <summary>
        /// 0x0008
        /// </summary>
        Recognizer_Driver = 0x0008,

        /// <summary>
        /// 0x000F
        /// </summary>
        Driver = 0x000F,

        /// <summary>
        /// 0x0010
        /// </summary>
        Win32OwnProcess = 0x0010,

        /// <summary>
        /// 0x0020
        /// </summary>
        Win32ShareProcess = 0x0020,

        /// <summary>
        /// 0x0030
        /// </summary>
        Win32 = 0x0030,

        /// <summary>
        /// 0x0100
        /// </summary>
        InterActiveProcess = 0x0100,

        /// <summary>
        /// 0x013F
        /// </summary>
        All = 0x013F
    }

    public enum ServiceStartType
    {
        /// <summary>
        /// 0xF0000
        /// </summary>
        BootStart = 0x0000,

        /// <summary>
        /// 0x0001
        /// </summary>
        SystemStart = 0x0001,

        /// <summary>
        /// 0x0002
        /// </summary>
        AutoStart = 0x0002,

        /// <summary>
        /// 0x0003
        /// </summary>
        DemandStart = 0x0003,

        /// <summary>
        /// 0x0004
        /// </summary>
        Disabled = 0x0004

    }

    public enum ServiceErrorControlType
    {
        /// <summary>
        /// 0xF0000
        /// </summary>
        Ignore = 0x0000,

        /// <summary>
        /// 0x0001
        /// </summary>
        Normal = 0x0001,

        /// <summary>
        /// 0x0002
        /// </summary>
        Severe = 0x0002,

        /// <summary>
        /// 0x0003
        /// </summary>
        Critical = 0x0003
    }

    public enum ServiceConfigureAction
    {
        /// <summary>
        /// 0x0001
        /// </summary>
        Description = 0x0001,

        /// <summary>
        /// 0x0002
        /// </summary>
        FailureActions = 0x0002
    }
    #endregion
}
