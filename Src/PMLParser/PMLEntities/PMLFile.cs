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
        public PMLFile(string xmlFilePath, PMLProcess[] procs, PMLEvent[] evts)
        {
            OutputXMLFilePath = xmlFilePath;
            this.processes = procs;
            this.events = evts;
#if DEBUG
            Console.WriteLine("# of Processes that match the criteria {0}.", this.processes.Length);
            Console.WriteLine("# of Events that match the criteria {0}.", this.events.Length);
#endif
        } 
        #endregion

        #region Properties
        internal string OwnerPMLFilePath { get; private set; }
        internal string OutputXMLFilePath { get; private set; }
        #endregion
    }
}
