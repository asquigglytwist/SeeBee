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
            // TODO:  Right now the Include filter doesn't make sense; But once an "Exclude *" filter is supported, Include will take effect.
            //bool include = true;
            foreach(var execFilter in ExecFilters)
            {
                var thisFilterOutput = execFilter.SatisfiesCondition(pMLEntity);
                //include = include || thisFilterOutput;
                if (thisFilterOutput && execFilter.Inclusion == Inclusions.Exclude)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
