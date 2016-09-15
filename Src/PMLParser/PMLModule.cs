using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    public class PMLModule
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

        internal long TimeStamp { get; private set; }
        internal long BaseAddress { get; private set; }
        internal long Size { get; private set; }
        internal string Path { get; private set; }
        internal string Version { get; private set; }
        internal string Company { get; private set; }
        internal string Description { get; private set; }
    }
}
