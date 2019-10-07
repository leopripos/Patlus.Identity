using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Patlus.Identity.UseCase.Entities;
using Patlus.Identity.UseCase.Features.Pools.Queries.GetOneById;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Patlus.Identity.Rest.Services
{
    public class PoolResolver : IPoolResolver
    {
        public const string POOL_ID_KEY = "poolId";

        public Pool Current { get; private set; }

        public PoolResolver(IHttpContextAccessor httpContextAccesor, IMediator mediator, IMapper mapper)
        {
            var task = Resolve(httpContextAccesor, mediator, mapper);

            if (task.Result == null)
            {
                httpContextAccesor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else {
                Current = task.Result;
            }
        }

        private async Task<Pool> Resolve(IHttpContextAccessor httpContextAccesor, IMediator mediator, IMapper mapper)
        {
            var httpContext = httpContextAccesor.HttpContext;
            var routeData = httpContext.GetRouteData();

            Pool pool = null;
            if (Guid.TryParse(routeData.Values[POOL_ID_KEY].ToString(), out Guid poolId))
            {
                var query = await mediator.Send(new GetOneByIdQuery() { Id = poolId });

                pool = query.Include(e => e.Database).FirstOrDefault();
            }

            return pool;
        }
    }
}
