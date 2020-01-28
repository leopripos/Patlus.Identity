using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Patlus.IdentityManagement.Rest.Responses.Content;
using Patlus.IdentityManagement.UseCase.Features.Pools.Exist;
using System;

namespace Patlus.IdentityManagement.Rest.Filters.Actions
{
    public class ValidPoolFilter : ActionFilterAttribute, IResourceFilter
    {
        public string RouteKey = "poolId";

        private readonly IMediator mediator;

        public ValidPoolFilter(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        { }

        public async void OnResourceExecuting(ResourceExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var routeData = httpContext.GetRouteData();

            var isValid = false;

            if (Guid.TryParse(routeData.Values[RouteKey].ToString(), out Guid poolId))
            {
                isValid = await mediator.Send(new ExistQuery()
                {
                    Condition = (e => e.Id == poolId)
                });
            }

            if (!isValid)
            {
                context.Result = new NotFoundObjectResult(
                    new NotFoundResultContent($"Pool with id `{(routeData.Values[RouteKey])}` not found.")
                );
            }
        }
    }
}
