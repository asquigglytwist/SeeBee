using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser.ConfigManager
{
    internal class AppConfig
    {
        List<ExecutableFilter> ExecFilters;

        internal AppConfig(string configFile)
        {
            var xDoc = XDocument.Load(configFile);
            ExecFilters = IFilter.ProcessAppConfig(xDoc);
        }

        public bool ShouldInclude(IPMLEntity pMLEntity)
        {
            bool include = true;
            foreach(var execFilter in ExecFilters)
            {
                include = include && execFilter.SatisfiesCondition(pMLEntity);
                if (!include)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
