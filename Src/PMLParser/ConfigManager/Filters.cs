using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser.ConfigManager
{
    abstract public class IFilter
    {
        public const string FiltersRootTagName = "Filters";
        public const string FilterCreateTagName = "Create";
        public const string FilterExecuteTagName = "Execute";
        public const string FilterNameAttribute = "name";
        public const string FilterPropertyNameAttribute = "propertyName";
        public const string FilterAppliesOnAttribute = "appliesOn";
        public const string FilterOperatorAttribute = "operator";

        public IFilter(string name, string propertyName, FilterOperators filterOperator)
        {
            Name = name;
            PropertyName = propertyName;
            FilterOperator = filterOperator;
        }

        public string Name { get; protected set; }
        public string PropertyName { get; protected set; }
        public FilterOperators FilterOperator { get; protected set; }
        public FilterTarget FilterAppliesOn { get; protected set; }

        protected abstract bool Matches(IPMLEntity pmlEntity);

        internal static List<ExecutableFilter> ProcessAppConfig(XDocument xDoc)
        {
            var namedFilters = new Dictionary<string, IFilter>();
            var allFilterNodes = xDoc.Descendants(FilterCreateTagName).DescendantNodes();
            foreach (var filterConfig in allFilterNodes)
            {
                var name = filterConfig.Attribute(FilterNameAttribute).Value;
                var propName = filterConfig.Attribute(FilterPropertyNameAttribute).Value;
                var filterTarget = filterConfig.Attribute(FilterAppliesOnAttribute).Value.StringToEnum<FilterTarget>();
                var filterOperator = filterConfig.Attribute(FilterOperatorAttribute).Value.StringToEnum<FilterOperators>();
                switch (filterTarget)
                {
                    case FilterTarget.Processes:
                        namedFilters[name] = new ProcessFilter(name, propName, filterOperator);
                        break;
                    case FilterTarget.Events:
                        namedFilters[name] = new EventFilter(name, propName, filterOperator);
                        break;
                    case FilterTarget.None:
                    default:
                        throw new Exception("Unable to Process XElement to create a(n) (I)Filter.");
                }
            }
            var allExecutableFilterNodes = xDoc.Descendants();
            var executableFilters = new List<ExecutableFilter>();
            return namedFilters;
        }
    }

    class ProcessFilter : IFilter
    {
        public ProcessFilter(string name, string propertyName, FilterOperators filterOperator) : base(name, propertyName, filterOperator)
        {
            FilterAppliesOn = FilterTarget.Processes;
        }

        protected override bool Matches(IPMLEntity pmlEntity)
        {
            throw new NotImplementedException();
        }
    }

    class EventFilter : IFilter
    {
        public EventFilter(string name, string propertyName, FilterOperators filterOperator) : base(name, propertyName, filterOperator)
        {
            FilterAppliesOn = FilterTarget.Events;
        }

        protected override bool Matches(IPMLEntity pmlEntity)
        {
            throw new NotImplementedException();
        }
    }

    public class ExecutableFilter
    {
        public Inclusions Inclusion { get; private set; }
        public MixinOperators MixinOperator { get; private set; }
        public List<IFilter> FiltersList { get; private set; }

        public ExecutableFilter(List<IFilter> filtersList, string inclusion, string mixinOperator)
        {
            Inclusion = inclusion.StringToEnum<Inclusions>();
            MixinOperator = mixinOperator.StringToEnum<MixinOperators>();
            FiltersList = filtersList ?? throw new ArgumentNullException(nameof(filtersList));
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
