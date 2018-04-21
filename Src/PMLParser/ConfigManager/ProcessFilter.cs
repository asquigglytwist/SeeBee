using SeeBee.PMLParser.ManagedLists;
using SeeBee.PMLParser.PMLEntities;
using System;
using System.Text;

namespace SeeBee.PMLParser.ConfigManager
{
    class ProcessFilter : IFilter
    {
        public ProcessFilter(string name, string propertyName, FilterOperators filterOperator, string[] filterValue)
            : base(name, propertyName, filterOperator, filterValue)
        {
            FilterAppliesOn = FilterTarget.Processes;
        }

        protected override bool Matches(IPMLEntity pmlEntity)
        {
            var proc = pmlEntity as PMLProcess;
            var actualValue = string.Empty;
            switch (PropertyName)
            {
                case "ProcessName":
                    actualValue = ProcessNameList.GetProcessName(proc.ProcessNameIndex);
                    break;
                case "ImagePath":
                    actualValue = ModuleList.GetModulePath(proc.ImageIndex);
                    break;
                case "FinishTime":
                    actualValue = proc.FinishTime.ToString();
                    break;
                case "Modules":
                    if (FilterOperator != FilterOperators.Contains)
                    {
                        throw new Exception(string.Format("Filter Operator {0} is invalid when PropertyName is \"Modules\"", FilterOperator.ToString()));
                    }
                    var sbModules = new StringBuilder();
                    foreach (var i in proc.LoadedModuleList)
                    {
                        sbModules.Append(ModuleList.GetModulePath(i)).Append(Environment.NewLine);
                    }
                    actualValue = sbModules.ToString();
                    break;
                case "":
                    throw new Exception("PropertyName cannot be empty.");
                default:
                    throw new Exception(string.Format("Unidentified PropertyName {0}.", PropertyName));
            }
            foreach (var expectedValue in FilterValue)
            {
                switch (FilterOperator)
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
                        throw new Exception(string.Format("Unidentified FilterOperator {0}.", FilterOperator.ToString()));
                }
            }
            return true;
        }
    }
}
