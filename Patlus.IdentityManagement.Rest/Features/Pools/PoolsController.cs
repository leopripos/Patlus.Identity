using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.IdentityManagement.Rest.Authentication;
using Patlus.IdentityManagement.Rest.Policies;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetAll;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetOneById;
using Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Pools
{
    [ApiController]
    [Route("pools")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class PoolsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IUserResolver userResolver;

        public PoolsController(IMediator mediator, IMapper mapper, IUserResolver userResolver)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.userResolver = userResolver;
        }

        [HttpGet]
        [Authorize(Policy = PoolPolicy.Read)]
        public async Task<PoolDto[]> GetAll()
        {
            var pools = await mediator.Send(new GetAllQuery() { 
                RequestorId = userResolver.Current.Id
            });

            return mapper.Map<Pool[], PoolDto[]>(pools);
        }

        [HttpGet("{poolId}")]
        [Authorize(Policy = PoolPolicy.Read)]
        public async Task<PoolDto> GetById(Guid poolId)
        {
            var pool = await mediator.Send(new GetOneByIdQuery() { 
                Id = poolId,
                RequestorId = userResolver.Current.Id
            });

            return mapper.Map<Pool, PoolDto>(pool);
        }

        [HttpPost]
        [Authorize(Policy = PoolPolicy.Create)]
        public async Task<ActionResult<PoolDto>> Create([FromBody] CreateForm form)
        {
            var command = new CreateCommand
            {
                Active = form.Active,
                Name = form.Name,
                Description = form.Description,
                RequestorId = userResolver.Current.Id
            };

            var pool = await mediator.Send(command);

            return Created(new Uri($"{Request.Path}/{pool.Id}", UriKind.Relative), mapper.Map<PoolDto>(pool));
        }

        [HttpPut("{poolId}/active")]
        [Authorize(Policy = PoolPolicy.UpdateActiveStatus)]
        public async Task<PoolDto> UpdateActiveStatus(Guid poolId, [FromBody] UpdateActiveStatusForm form)
        {
            var command = new UpdateActiveStatusCommand
            {
                Id = poolId,
                Active = form.Active,
                RequestorId = userResolver.Current.Id
            };

            var pool = await mediator.Send(command);

            return mapper.Map<Pool, PoolDto>(pool);
        }
    }
}
