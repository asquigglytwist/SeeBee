using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser.ConfigManager
{
    class EventFilter : IFilter
    {
        public EventFilter(string name, string propertyName, FilterOperators filterOperator, string[] filterValue)
            : base(name, propertyName, filterOperator, filterValue)
        {
            FilterAppliesOn = FilterTarget.Events;
        }

        public override bool Matches(IPMLEntity pmlEntity)
        {
            var evt = pmlEntity as PMLEvent;
            var actualValue = string.Empty;
            switch (PropertyName)
            {
                case "Operation":
                    actualValue = evt.Operation;
                    break;
                case "Result":
                    actualValue = evt.Result;
                    break;
                case "PID":
                    actualValue = evt.PID.ToString();
                    break;
                case "TID":
                    actualValue = evt.TID.ToString();
                    break;
                //case "Session":
                //    actualValue = evt.ProcessIndex;
                //    break;
                case "Path":
                    actualValue = evt.Path;
                    break;
                case "Detail":
                    actualValue = evt.Detail;
                    break;
                case "StackFramePath":
                    if (FilterOperator != FilterOperators.Contains)
                    {
                        throw new Exception(string.Format("Filter Operator {0} is invalid when PropertyName is \"StackFramePath\"", FilterOperator.ToString()));
                    }
                    var sbStackFramePaths = new StringBuilder();
                    foreach(var frame in evt.CallStack)
                    {
                        if(!string.IsNullOrWhiteSpace(frame.Path))
                        {
                            sbStackFramePaths.Append(frame.Path).Append(Environment.NewLine);
                        }
                    }
                    actualValue = sbStackFramePaths.ToString();
                    break;
                case "StackFrameLocation":
                    if (FilterOperator != FilterOperators.Contains)
                    {
                        throw new Exception(string.Format("Filter Operator {0} is invalid when PropertyName is \"StackFramePath\"", FilterOperator.ToString()));
                    }
                    var sbStackFrameLocations = new StringBuilder();
                    foreach (var frame in evt.CallStack)
                    {
                        if (!string.IsNullOrWhiteSpace(frame.Location))
                        {
                            sbStackFrameLocations.Append(frame.Location).Append(Environment.NewLine);
                        }
                    }
                    actualValue = sbStackFrameLocations.ToString();
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
