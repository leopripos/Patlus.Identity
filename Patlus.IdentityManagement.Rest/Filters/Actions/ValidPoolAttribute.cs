using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Patlus.IdentityManagement.Rest.Filters.Actions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ValidPoolAttribute : Attribute, IFilterFactory
    {
        private readonly string _routeKey;

        public bool IsReusable => false;

        public ValidPoolAttribute(string routeKey)
        {
            _routeKey = routeKey;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            if (!(serviceProvider.GetService(typeof(ValidPoolFilter)) is ValidPoolFilter filter)) throw new ArgumentNullException(nameof(filter), "ValidPoolFilter service not found");

            filter.RouteKey = _routeKey;

            return filter;
        }
    }
}
