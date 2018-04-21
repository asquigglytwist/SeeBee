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

        protected override bool Matches(IPMLEntity pmlEntity)
        {
            throw new NotImplementedException();
        }
    }
}
