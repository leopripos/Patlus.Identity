using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.Common.Presentation.Security;
using Patlus.IdentityManagement.Presentation.Auhtorization.Policies;
using Patlus.IdentityManagement.Rest.Filters.Actions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetAll;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOne;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Identities
{
    [ApiController]
    [ValidPool("poolId")]
    [Route("pools/{poolId}/identities")]
    [Authorize]
    public class IdentitiesController : ApiControllerBase
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
        public async Task<IdentityDto[]> GetAll(Guid poolId, CancellationToken cancellationToken)
        {
            var query = new GetAllQuery()
            {
                Condition = (e => e.PoolId == poolId),
                RequestorId = _userResolver.Current.Id
            };

            var identities = await _mediator.Send(query, cancellationToken); ;

            return _mapper.Map<Identity[], IdentityDto[]>(identities);
        }

        /// <summary>
        /// Get specific identity in specific pool
        /// </summary>
        /// <returns>Requested Identity</returns>
        [HttpGet("{identityId}")]
        [Authorize(Policy = IdentityPolicy.Read)]
        public async Task<ActionResult<IdentityDto>> GetById(Guid poolId, Guid identityId, CancellationToken cancellationToken)
        {
            var query = new GetOneQuery()
            {
                Condition = (e => e.PoolId == poolId && e.Id == identityId),
                RequestorId = _userResolver.Current.Id
            };

            var identity = await _mediator.Send(query, cancellationToken);

            if (identity == null)
            {
                return NotFound();
            }

            return _mapper.Map<Identity, IdentityDto>(identity);
        }

        /// <summary>
        /// Create new identity 
        /// </summary>
        /// <returns>Created Identity</returns>
        [HttpPost]
        [Authorize(Policy = IdentityPolicy.Create)]
        public async Task<ActionResult<IdentityDto>> Create(Guid poolId, [FromBody] CreateForm form, CancellationToken cancellationToken)
        {
            var command = new CreateHostedCommand
            {
                PoolId = poolId,
                Name = form.Name,
                AccountName = form.AccountName,
                AccountPassword = form.AccountPassword,
                Active = true,
                RequestorId = _userResolver.Current.Id
            };

            var identity = await _mediator.Send(command, cancellationToken);

            return Created(new Uri($"{Request.Path}/{identity.Id}", UriKind.Relative), _mapper.Map<IdentityDto>(identity));
        }

        /// <summary>
        /// Update active status of specific identity
        /// </summary>
        /// <returns>Updated Identity</returns>
        [HttpPut("{identityId}/active")]
        [Authorize(Policy = IdentityPolicy.UpdateActiveStatus)]
        public async Task<IdentityDto> UpdateActiveStatus(Guid poolId, Guid identityId, [FromBody] UpdateActiveStatusForm form, CancellationToken cancellationToken)
        {
            var command = new UpdateActiveStatusCommand
            {
                PoolId = poolId,
                Id = identityId,
                Active = form.Active,
                RequestorId = _userResolver.Current.Id
            };

            var identity = await _mediator.Send(command, cancellationToken);

            return _mapper.Map<Identity, IdentityDto>(identity);
        }
    }
}
