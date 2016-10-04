using System;
using System.Runtime.InteropServices;

namespace SeeBee.FxUtils.AuthentiCode
{
    #region WINTRUST_DATA
    [StructLayout(LayoutKind.Sequential)]
    internal struct WINTRUST_DATA : IDisposable
    {
        public WINTRUST_DATA(WINTRUST_FILE_INFO fileInfo)
        {
            this.cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_DATA));
            pInfoStruct = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WINTRUST_FILE_INFO)));
            Marshal.StructureToPtr(fileInfo, pInfoStruct, false);
            this.dwUnionChoice = UnionChoice.File;
            pPolicyCallbackData = IntPtr.Zero;
            pSIPCallbackData = IntPtr.Zero;
            dwUIChoice = UiChoice.NoUI;
            fdwRevocationChecks = RevocationCheckFlags.None;
            dwStateAction = StateAction.Ignore;
            hWVTStateData = IntPtr.Zero;
            pwszURLReference = IntPtr.Zero;
            dwProvFlags = TrustProviderFlags.Safer;
            dwUIContext = UIContext.Execute;
        }

        public uint cbStruct;
        public IntPtr pPolicyCallbackData;
        public IntPtr pSIPCallbackData;
        public UiChoice dwUIChoice;
        public RevocationCheckFlags fdwRevocationChecks;
        public UnionChoice dwUnionChoice;
        public IntPtr pInfoStruct;
        public StateAction dwStateAction;
        public IntPtr hWVTStateData;
        private IntPtr pwszURLReference;
        public TrustProviderFlags dwProvFlags;
        public UIContext dwUIContext;

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (dwUnionChoice == UnionChoice.File)
            {
                WINTRUST_FILE_INFO info = new WINTRUST_FILE_INFO();
                Marshal.PtrToStructure(pInfoStruct, info);
                info.Dispose();
                Marshal.DestroyStructure(pInfoStruct, typeof(WINTRUST_FILE_INFO));
            }
            Marshal.FreeHGlobal(pInfoStruct);
        }
        #endregion
    }
    #endregion
}
