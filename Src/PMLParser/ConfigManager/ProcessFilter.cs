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

        public override bool Matches(IPMLEntity pmlEntity)
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
            return CompareStringValuesAsPerFilterOperator(actualValue, this);
        }
    }
}
