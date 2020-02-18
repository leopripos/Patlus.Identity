using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.IdentityManagement.Rest.Auhtorization.Policies;
using Patlus.IdentityManagement.Rest.Authentication;
using Patlus.IdentityManagement.Rest.Filters.Actions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetAll;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOne;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus;
using System;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Identities
{
    [ApiController]
    [ValidPool("poolId")]
    [Route("pools/{poolId}/identities")]
    [Authorize]
    public class IdentitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserResolver _userResolver;

        public IdentitiesController(IMediator mediator, IMapper mapper, IUserResolver userResolver)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userResolver = userResolver;
        }

        /// <summary>
        /// Get all identities in specific pool
        /// </summary>
        /// <returns>List of identities</returns>
        [HttpGet]
        [Authorize(Policy = IdentityPolicy.Read)]
        public async Task<IdentityDto[]> GetAll(Guid poolId)
        {
            var query = new GetAllQuery()
            {
                Condition = (e => e.PoolId == poolId),
                RequestorId = _userResolver.Current.Id
            };

            var identities = await _mediator.Send(query); ;

            return _mapper.Map<Identity[], IdentityDto[]>(identities);
        }

        /// <summary>
        /// Get specific identity in specific pool
        /// </summary>
        /// <param name="poolId">Pool Id</param>
        /// <param name="identityId">Identity Id</param>
        /// <returns>Requested Identity</returns>
        [HttpGet("{identityId}")]
        [Authorize(Policy = IdentityPolicy.Read)]
        public async Task<IdentityDto> GetById(Guid poolId, Guid identityId)
        {
            var identity = await _mediator.Send(new GetOneQuery()
            {
                Condition = (e => e.PoolId == poolId && e.Id == identityId),
                RequestorId = _userResolver.Current.Id
            });

            return _mapper.Map<Identity, IdentityDto>(identity);
        }

        /// <summary>
        /// Create new identity 
        /// </summary>
        /// <param name="poolId">Pool Id</param>
        /// <param name="form">Create Form</param>
        /// <returns>Created Identity</returns>
        [HttpPost]
        [Authorize(Policy = IdentityPolicy.Create)]
        public async Task<ActionResult<IdentityDto>> Create(Guid poolId, [FromBody] CreateForm form)
        {
            var command = new CreateHostedCommand
            {
                PoolId = poolId,
                Name = form.Name,
                AccountName = form.AccountName,
                AccountPassword = form.AccountPassword,
                RequestorId = _userResolver.Current.Id
            };

            var identity = await _mediator.Send(command);

            return Created(new Uri($"{Request.Path}/{identity.Id}", UriKind.Relative), _mapper.Map<IdentityDto>(identity));
        }

        /// <summary>
        /// Update active status of specific identity
        /// </summary>
        /// <param name="poolId">Pool Id</param>
        /// <param name="identityId">Identity Id</param>
        /// <param name="form">Update Active Status Form</param>
        /// <returns>Updated Identity</returns>
        [HttpPut("{identityId}/active")]
        [Authorize(Policy = IdentityPolicy.UpdateActiveStatus)]
        public async Task<IdentityDto> UpdateActiveStatus(Guid poolId, Guid identityId, [FromBody] UpdateActiveStatusForm form)
        {
            var command = new UpdateActiveStatusCommand
            {
                PoolId = poolId,
                Id = identityId,
                Active = form.Active,
                RequestorId = _userResolver.Current.Id
            };

            var identity = await _mediator.Send(command);

            return _mapper.Map<Identity, IdentityDto>(identity);
        }
    }
}
