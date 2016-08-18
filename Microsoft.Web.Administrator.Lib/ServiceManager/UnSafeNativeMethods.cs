using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace WindowsServiceInvest.ConfigureTest
{
    [ComVisible(false), SuppressUnmanagedCodeSecurity]
    public class UnSafeNativeMethods
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern unsafe bool ControlService(IntPtr serviceHandle, int control, SERVICE_STATUS* pStatus);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern unsafe bool QueryServiceStatus(IntPtr serviceHandle, SERVICE_STATUS* pStatus);
        
    }
}
