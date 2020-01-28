using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Patlus.IdentityManagement.Rest.Filters.Actions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidPoolAttribute : Attribute, IFilterFactory
    {
        private readonly string routeKey;

        public bool IsReusable => false;

        public ValidPoolAttribute(string routeKey)
        {
            this.routeKey = routeKey;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService(typeof(ValidPoolFilter)) as ValidPoolFilter;

            if (filter is null) throw new ArgumentNullException(nameof(filter), "ValidPoolFilter service not found");

            filter.RouteKey = this.routeKey;

            return filter;
        }
    }
}
