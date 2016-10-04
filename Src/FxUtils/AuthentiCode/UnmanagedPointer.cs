using System;
using System.Runtime.InteropServices;

namespace SeeBee.FxUtils.AuthentiCode
{
    #region UnmanagedPointer
    internal sealed class UnmanagedPointer : IDisposable
    {
        private IntPtr m_ptr;
        private AllocMethod m_meth;

        internal UnmanagedPointer(IntPtr ptr, AllocMethod method)
        {
            m_meth = method;
            m_ptr = ptr;
        }

        ~UnmanagedPointer()
        {
            Dispose(false);
        }

        #region IDisposable Members
        private void Dispose(bool disposing)
        {
            if (m_ptr != IntPtr.Zero)
            {
                if (m_meth == AllocMethod.HGlobal)
                {
                    Marshal.FreeHGlobal(m_ptr);
                }
                else if (m_meth == AllocMethod.CoTaskMem)
                {
                    Marshal.FreeCoTaskMem(m_ptr);
                }
                m_ptr = IntPtr.Zero;
            }

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        public static implicit operator IntPtr(UnmanagedPointer ptr)
        {
            return ptr.m_ptr;
        }
    }
    #endregion
}
