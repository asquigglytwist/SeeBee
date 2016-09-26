using System;

namespace SeeBee.PMLParser
{
    internal class PMLStackFrame
    {
        int pathIndex;

        #region Properties
        internal long Address { get; private set; }
        internal string Path
        {
            get
            {
                return PMLAnalyzer.GetFilePath(pathIndex);
            }
        }
        internal string Location { get; private set; }
        #endregion

        #region System.Object
        public override int GetHashCode()
        {
            return (this.Address+this.Location).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj);
        }

        public bool Equals(PMLStackFrame otherStackFrame)
        {
            return (this.Address==otherStackFrame.Address)
                && (this.pathIndex==otherStackFrame.pathIndex)
                && (this.Location.Equals(otherStackFrame.Location, StringComparison.CurrentCultureIgnoreCase));
        }

        public override string ToString()
        {
            return string.Format("{0} at {1}.", this.Path, this.Address);
        }
        #endregion
    }
}
