using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.Identity.Rest.Policies;
using Patlus.Identity.UseCase.Features.Pools.Commands.Create;
using Patlus.Identity.UseCase.Features.Pools.Commands.UpdateActiveStatus;
using Patlus.Identity.UseCase.Features.Pools.Queries.GetList;
using Patlus.Identity.UseCase.Features.Pools.Queries.GetOneById;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Patlus.Identity.Rest.Features.Pools
{
    [ApiController]
    [Route("pools")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class PoolsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public PoolsController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = PoolPolicy.Read)]
        public async Task<PoolDto[]> GetAll()
        {
            var query = await mediator.Send(new GetListQuery());

            return mapper.ProjectTo<PoolDto>(query).ToArray();
        }

        [HttpGet("{poolId}")]
        [Authorize(Policy = PoolPolicy.Read)]
        public async Task<PoolDto> GetById(Guid poolId)
        {
            var query = await mediator.Send(new GetOneByIdQuery() { Id = poolId });

            return mapper.Map<PoolDto>(query);
        }

        [HttpPost]
        [Authorize(Policy = PoolPolicy.Create)]
        public async Task<ActionResult<PoolDto>> Create([FromBody] CreateForm form)
        {
            var command = new CreateCommand
            {
                Id = form.Id,
                Active = form.Active,
                RequestorId = null
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
                RequestorId = null
            };

            var account = await mediator.Send(command);

            return mapper.Map<PoolDto>(account);
        }
    }
}
