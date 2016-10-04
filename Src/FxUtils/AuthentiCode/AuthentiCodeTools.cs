using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

// [BIB]:  http://stackoverflow.com/a/6597017
// [BIB]:  https://github.com/kg/shootblues/blob/master/SignatureCheck.cs
// [BIB]:  http://www.pinvoke.net/default.aspx/wintrust.winverifytrust
namespace SeeBee.FxUtils.AuthentiCode
{
    #region AuthenticodeTools
    public static class AuthentiCodeTools
    {
        [DllImport("Wintrust.dll", PreserveSig = true, SetLastError = false)]
        private static extern uint WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWinTrustData);

        private static uint WinVerifyTrust(string fileName)
        {

            Guid wintrust_action_generic_verify_v2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");
            uint result = 0;
            using (WINTRUST_FILE_INFO fileInfo = new WINTRUST_FILE_INFO(fileName,
                                                                        Guid.Empty))
            using (UnmanagedPointer guidPtr = new UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid))),
                                                                   AllocMethod.HGlobal))
            using (UnmanagedPointer wvtDataPtr = new UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WINTRUST_DATA))),
                                                                      AllocMethod.HGlobal))
            {
                WINTRUST_DATA data = new WINTRUST_DATA(fileInfo);
                IntPtr pGuid = guidPtr;
                IntPtr pData = wvtDataPtr;
                Marshal.StructureToPtr(wintrust_action_generic_verify_v2,
                                       pGuid,
                                       true);
                Marshal.StructureToPtr(data,
                                       pData,
                                       true);
                result = WinVerifyTrust(IntPtr.Zero,
                                        pGuid,
                                        pData);
            }
            return result;
        }

        public static bool IsTrusted(string fileName)
        {
            return WinVerifyTrust(fileName) == 0;
        }
    }
    #endregion
}
