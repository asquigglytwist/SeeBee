using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    internal class PMLModule
    {
        internal PMLModule(long timeStamp, long baseAddress, long size, string path, string version, string company, string description)
        {
            TimeStamp = timeStamp;
            BaseAddress = baseAddress;
            Size = size;
            Path = path;
            Version = version;
            Company = company;
            Description = description;
        }

        internal long TimeStamp { internal get; private set; }
        internal long BaseAddress { internal get; private set; }
        internal long Size { internal get; private set; }
        internal string Path { internal get; private set; }
        internal string Version { internal get; private set; }
        internal string Company { internal get; private set; }
        internal string Description { internal get; private set; }
    }
}
