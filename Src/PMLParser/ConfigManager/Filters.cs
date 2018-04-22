using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser.ConfigManager
{
    abstract public class IFilter
    {
        public const string FilterTagName = "Filter";
        public const string FilterNameAttribute = "name";
        public const string FilterPropertyAttribute = "property";
        public const string FilterAppliesOnAttribute = "appliesOn";
        public const string FilterOperatorAttribute = "operator";
        public const string ConditionTagName = "Condition";
        public const string ConditionActionAttribute = "action";
        public const string ConditionOperatorAttribute = "operator";

        public IFilter(string name, string propertyName, FilterOperators filterOperator, string[] filterValue)
        {
            Name = name;
            PropertyName = propertyName;
            FilterOperator = filterOperator;
            FilterExpectedValue = filterValue;
        }

        public string Name { get; protected set; }
        public string PropertyName { get; protected set; }
        public FilterOperators FilterOperator { get; protected set; }
        public FilterTarget FilterAppliesOn { get; protected set; }
        public string[] FilterExpectedValue { get; protected set; }

        public abstract bool Matches(IPMLEntity pmlEntity);

        internal static List<ExecutableFilter> ProcessAppConfig(XDocument xDoc)
        {
            var namedFilters = new Dictionary<string, IFilter>();
            var allFilterNodes = xDoc.Descendants(FilterTagName);
            foreach (var filterConfig in allFilterNodes)
            {
                var name = filterConfig.Attribute(FilterNameAttribute).Value;
                var propName = filterConfig.Attribute(FilterPropertyAttribute).Value;
                var filterTarget = filterConfig.Attribute(FilterAppliesOnAttribute).Value.StringToEnum<FilterTarget>();
                var filterOperator = filterConfig.Attribute(FilterOperatorAttribute).Value.StringToEnum<FilterOperators>();
                var filterValue = filterConfig.Value.CSVSplit();
                switch (filterTarget)
                {
                    case FilterTarget.Processes:
                        namedFilters[name] = new ProcessFilter(name, propName, filterOperator, filterValue);
                        break;
                    case FilterTarget.Events:
                        namedFilters[name] = new EventFilter(name, propName, filterOperator, filterValue);
                        break;
                    case FilterTarget.None:
                    default:
                        throw new Exception("Unable to Process XElement to create a(n) (I)Filter.");
                }
            }
            var executableFilters = new List<ExecutableFilter>();
            var allConditionNodes = xDoc.Descendants(ConditionTagName);
            foreach(var includeFilter in allConditionNodes)
            {
                var filters = includeFilter.Value.CSVSplit();
                var inclusion = includeFilter.Attribute(ConditionActionAttribute).Value.StringToEnum<Inclusions>();
                var mixinOperator = includeFilter.Attribute(ConditionOperatorAttribute).Value.StringToEnum<MixinOperators>();
                var filterList = new List<IFilter>();
                for(int i = 0; i < filters.Length; i++)
                {
                    var name = filters[i].Trim();
                    if (namedFilters.TryGetValue(name, out var filteredByName))
                    {
                        filterList.Add(filteredByName);
                    }
                    else
                    {
                        throw new Exception(string.Format("Unidentified filter name {0}.{1}Complete string for reference:{1}\"{2}\"", name, Environment.NewLine, includeFilter.Value));
                    }
                }
                executableFilters.Add(new ExecutableFilter(filterList, inclusion, mixinOperator));
            }
            return executableFilters;
        }

        internal static bool CompareStringValuesAsPerFilterOperator(string actualValue, IFilter filter)
        {
            foreach (var expectedValue in filter.FilterExpectedValue)
            {
                switch (filter.FilterOperator)
                {
                    case FilterOperators.Equals:
                        if (!actualValue.Equals(expectedValue, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return false;
                        }
                        break;
                    case FilterOperators.NotEquals:
                        if (actualValue.Equals(expectedValue, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return false;
                        }
                        break;
                    case FilterOperators.StartsWith:
                        if (!actualValue.StartsWith(expectedValue, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return false;
                        }
                        break;
                    case FilterOperators.EndsWith:
                        if (!actualValue.EndsWith(expectedValue, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return false;
                        }
                        break;
                    case FilterOperators.Contains:
                        if (!actualValue.Contains(expectedValue))
                        {
                            return false;
                        }
                        break;
                    case FilterOperators.GreaterThan:
                        break;
                    case FilterOperators.GreaterThanOrEqualsTo:
                        break;
                    case FilterOperators.LesserThan:
                        break;
                    case FilterOperators.LesserThanOrEqualsTo:
                        break;
                    case FilterOperators.None:
                        throw new Exception("FilterOperator cannot be empty.");
                    default:
                        throw new Exception(string.Format("Unidentified FilterOperator {0}.", filter.FilterOperator.ToString()));
                }
            }
            return true;
        }
    }

    public enum FilterOperators
    {
        None,
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqualsTo,
        LesserThan,
        LesserThanOrEqualsTo,
        StartsWith,
        EndsWith,
        Contains
    }

    public enum FilterTarget
    {
        None,
        Processes,
        Events
    }

    public enum MixinOperators
    {
        None,
        Only,
        And,
        Or
    }

    public enum Inclusions
    {
        None,
        Include,
        Exclude
    }
}
