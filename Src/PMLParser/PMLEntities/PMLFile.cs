using System;

namespace SeeBee.PMLParser.PMLEntities
{
    /// <summary>
    /// Represents an input PML file
    /// </summary>
    internal class PMLFile
    {
        #region Members
        private PMLProcess[] processes;
        private PMLEvent[] events; 
        #endregion

        #region Constructor
        public PMLFile(PMLProcess[] procs, PMLEvent[] evts)
        {
            this.processes = procs;
            this.events = evts;
#if DEBUG
            Console.WriteLine("# of Processes that match the criteria {0}.", this.processes.Length);
            Console.WriteLine("# of Events that match the criteria {0}.", this.events.Length);
#endif
        } 
        #endregion
    }
}
