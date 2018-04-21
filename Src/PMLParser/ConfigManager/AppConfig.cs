using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.ConfigManager
{
    internal class AppConfig
    {
        internal AppConfig(string configFile)
        {
            var xDoc = XDocument.Load(configFile);
            var filters = IFilter.ProcessAppConfig(xDoc);
        }
    }
}
