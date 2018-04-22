using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeBee.PMLParser.PMLEntities;

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

        public bool SatisfiesCondition(IPMLEntity pMLEntity)
        {
            if (pMLEntity == null)
            {
                throw new ArgumentNullException(nameof(pMLEntity));
            }
            bool comparisonResult = false;
            switch (MixinOperator)
            {
                case MixinOperators.Only:
                    return FiltersList.First().Matches(pMLEntity);
                case MixinOperators.And:
                    var andResult = true;
                    foreach (var filter in FiltersList)
                    {
                        andResult = andResult && filter.Matches(pMLEntity);
                        if (!andResult)
                        {
                            //comparisonResult = false;
                            break;
                        }
                    }
                    comparisonResult = andResult;
                    break;
                case MixinOperators.Or:
                    var orResult = false;
                    foreach (var filter in FiltersList)
                    {
                        orResult = orResult || filter.Matches(pMLEntity);
                        if (orResult)
                        {
                            //comparisonResult = true;
                            break;
                        }
                    }
                    comparisonResult = orResult;
                    break;
                case MixinOperators.None:
                    throw new Exception("(Mixin) Operator cannot be empty.");
                default:
                    throw new Exception(string.Format("Unidentified MixinOperator {0}.", MixinOperator.ToString()));
            }
            return comparisonResult;
        }
    }
}
