using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser.PMLEntities
{
    /// <summary>
    /// Represents an input PML file
    /// </summary>
    class PMLFile
    {
        internal PMLProcess[] processes;
        internal PMLEvent[] events;

        internal PMLFile(PMLProcess[] procs, PMLEvent[] evts)
        {
            this.processes = procs;
            this.events = evts;
        }
    }
}
