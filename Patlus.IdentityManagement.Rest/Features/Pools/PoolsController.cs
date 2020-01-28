using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.IdentityManagement.Rest.Auhtorization.Policies;
using Patlus.IdentityManagement.Rest.Authentication;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetAll;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetOne;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;
using Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus;
using System;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Pools
{
    [ApiController]
    [Route("pools")]
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

        /// <summary>
        /// Get the list of Pool
        /// </summary>
        /// <returns>Array of pool</returns>
        [HttpGet]
        [Authorize(Policy = PoolPolicy.Read)]
        public async Task<PoolDto[]> GetAll()
        {
            var pools = await mediator.Send(new GetAllQuery()
            {
                RequestorId = userResolver.Current.Id
            });

            return mapper.Map<Pool[], PoolDto[]>(pools);
        }

        /// <summary>
        /// Get pool with specific id
        /// </summary>
        /// <param name="poolId">Pool Id</param>
        /// <returns>Pool</returns>
        [HttpGet("{poolId}")]
        [Authorize(Policy = PoolPolicy.Read)]
        public async Task<PoolDto> GetById(Guid poolId)
        {
            var pool = await mediator.Send(new GetOneQuery()
            {
                Condition = (e => e.Id == poolId),
                RequestorId = userResolver.Current.Id
            });

            return mapper.Map<Pool, PoolDto>(pool);
        }

        /// <summary>
        /// Create new Pool
        /// </summary>
        /// <param name="form">New Pool Form</param>
        /// <returns>Created Pool</returns>
        [HttpPost]
        [Authorize(Policy = PoolPolicy.Create)]
        public async Task<ActionResult<PoolDto>> Create([FromBody] CreateForm form)
        {
            if (form.Name is null) throw new ArgumentNullException(nameof(form.Name));
            if (form.Description is null) throw new ArgumentNullException(nameof(form.Description));

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

        /// <summary>
        /// Update the detail of specific pool
        /// </summary>
        /// <remarks>
        /// Updating pool using partial update, so you have to update which field you want to update.
        /// No need to pass all value
        /// </remarks>
        /// <param name="poolId">Pool Id</param>
        /// <param name="form">Updated Pool Form</param>
        /// <returns>Pool Updated</returns>
        [HttpPatch("{poolId}")]
        [Authorize(Policy = PoolPolicy.Update)]
        public async Task<ActionResult<PoolDto>> Update(Guid poolId, [FromBody] UpdateForm form)
        {
            var command = new UpdateCommand()
            {
                Id = poolId,
                RequestorId = userResolver.Current.Id
            };

            if (form.HasName)
            {
                command.Name = form.Name;
            }

            if (form.HasDescription)
            {
                command.Description = form.Description;
            }

            var pool = await mediator.Send(command);

            return Ok(mapper.Map<PoolDto>(pool));
        }

        /// <summary>
        /// Update active status of specific pool
        /// </summary>
        /// <param name="poolId">Pool Id</param>
        /// <param name="form">Update Form</param>
        /// <returns>Pool Updated</returns>
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
