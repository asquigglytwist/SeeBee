using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeBee.PMLParser.ConfigManager
{
    public class ExecutableFilter
    {
        public Inclusions Inclusion { get; private set; }
        public MixinOperators MixinOperator { get; private set; }
        public List<IFilter> FiltersList { get; private set; }

        public ExecutableFilter(List<IFilter> filtersList, Inclusions inclusion, MixinOperators mixinOperator)
        {
            Inclusion = inclusion;
            MixinOperator = mixinOperator;
            FiltersList = filtersList ?? throw new ArgumentNullException(nameof(filtersList));
        }
    }
}
