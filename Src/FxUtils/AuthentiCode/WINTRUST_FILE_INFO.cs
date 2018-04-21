using System;
using System.Runtime.InteropServices;

namespace SeeBee.FxUtils.AuthentiCode
{
    #region WINTRUST_FILE_INFO
    internal struct WINTRUST_FILE_INFO : IDisposable
    {
        public WINTRUST_FILE_INFO(string fileName, Guid subject)
        {
            cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
            pcwszFilePath = fileName;
            if (subject != Guid.Empty)
            {
                pgKnownSubject = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid)));
                Marshal.StructureToPtr(subject, pgKnownSubject, true);
            }
            else
            {
                pgKnownSubject = IntPtr.Zero;
            }
            hFile = IntPtr.Zero;
        }

        public uint cbStruct;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pcwszFilePath;
        public IntPtr hFile;
        public IntPtr pgKnownSubject;

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (pgKnownSubject != IntPtr.Zero)
            {
                Marshal.DestroyStructure(pgKnownSubject, typeof(Guid));
                Marshal.FreeHGlobal(pgKnownSubject);
            }
        }
        #endregion
    }
    #endregion
}
