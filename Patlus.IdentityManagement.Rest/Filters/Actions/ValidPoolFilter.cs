using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Patlus.IdentityManagement.Rest.Authentication;
using Patlus.IdentityManagement.Rest.Responses.Content;
using Patlus.IdentityManagement.UseCase.Features.Pools.Exist;
using System;

namespace Patlus.IdentityManagement.Rest.Filters.Actions
{
    public sealed class ValidPoolFilter : ActionFilterAttribute, IResourceFilter
    {
        private readonly IMediator _mediator;
        private readonly IUserResolver _userResolver;

        public string RouteKey = "poolId";

        public ValidPoolFilter(IMediator mediator, IUserResolver userResolver)
        {
            _mediator = mediator;
            _userResolver = userResolver;
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
                isValid = await _mediator.Send(new ExistQuery()
                {
                    Condition = (e => e.Id == poolId),
                    RequestorId = _userResolver.Current.Id
                });
            }

            if (!isValid)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new NotFoundObjectResult(
                    new NotFoundResultDto($"Pool with id `{(routeData.Values[RouteKey])}` not found.")
                );
            }
        }
    }
}
