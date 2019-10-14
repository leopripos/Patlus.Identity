using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.IdentityManagement.Rest.Auhtorization.Policies;
using Patlus.IdentityManagement.Rest.Authentication;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetAll;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneById;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Identities
{
    [ApiController]
    [Route("pools/{" + PoolResolver.POOL_ID_KEY + "}/identities")]
    [Produces(MediaTypeNames.Application.Json)]
    public class IdentitiesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IUserResolver userResolver;
        private readonly IPoolResolver poolResolver;

        public IdentitiesController(IMediator mediator, IMapper mapper, IUserResolver userResolver, IPoolResolver poolResolver)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.userResolver = userResolver;
            this.poolResolver = poolResolver;
        }

        [HttpGet]
        [Authorize(Policy = IdentityPolicy.Read)]
        public async Task<IdentityDto[]> GetAll()
        {
            var identities = await mediator.Send(new GetAllQuery() { 
                PoolId = poolResolver.Current.Id,
                RequestorId = userResolver.Current.Id
            });;

            return mapper.Map<Identity[], IdentityDto[]>(identities);
        }

        [HttpGet("{identityId}")]
        [Authorize(Policy = IdentityPolicy.Read)]
        public async Task<IdentityDto> GetById(Guid identityId)
        {
            var identity = await mediator.Send(new GetOneByIdQuery() { 
                Id = identityId,
                PoolId = poolResolver.Current.Id,
                RequestorId = userResolver.Current.Id
            });

            return mapper.Map<Identity, IdentityDto>(identity);
        }

        [HttpPost]
        [Authorize(Policy = IdentityPolicy.Create)]
        public async Task<ActionResult<IdentityDto>> Create([FromBody] CreateForm form)
        {
            var command = new CreateHostedCommand
            {
                PoolId = poolResolver.Current.Id,
                Name = form.Name,
                Password = form.Password,
                RequestorId = userResolver.Current.Id
            };

            var identity = await mediator.Send(command);

            return Created(new Uri($"{Request.Path}/{identity.Id}", UriKind.Relative), mapper.Map<IdentityDto>(identity));
        }

        [HttpPut("{identityId}/active")]
        [Authorize(Policy = IdentityPolicy.UpdateActiveStatus)]
        public async Task<IdentityDto> UpdateActiveStatus(Guid identityId, [FromBody] UpdateActiveStatusForm form)
        {
            var command = new UpdateActiveStatusCommand
            {
                PoolId = poolResolver.Current.Id,
                Id = identityId,
                Active = form.Active,
                RequestorId = userResolver.Current.Id
            };

            var identity = await mediator.Send(command);

            return mapper.Map<Identity, IdentityDto>(identity);
        }
    }
}
