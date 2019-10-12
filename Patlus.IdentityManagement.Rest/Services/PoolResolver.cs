using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetOneById;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Services
{
    public class PoolResolver : IPoolResolver
    {
        public const string POOL_ID_KEY = "poolId";

        public Pool Current { get; private set; }

        public PoolResolver(IHttpContextAccessor httpContextAccesor, IMediator mediator)
        {
            var task = Resolve(httpContextAccesor, mediator);

            if (task.Result == null)
            {
                httpContextAccesor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else {
                Current = task.Result;
            }
        }

        private async Task<Pool> Resolve(IHttpContextAccessor httpContextAccesor, IMediator mediator)
        {
            var httpContext = httpContextAccesor.HttpContext;
            var routeData = httpContext.GetRouteData();

            if (Guid.TryParse(routeData.Values[POOL_ID_KEY].ToString(), out Guid poolId))
            {
                var pool = await mediator.Send(new GetOneByIdQuery() { Id = poolId });

                return pool;
            }

            return null;
        }
    }
}
