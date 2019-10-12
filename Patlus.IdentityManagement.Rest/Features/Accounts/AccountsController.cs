using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.IdentityManagement.Rest.Policies;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.UseCase.Features.Accounts.Commands.CreateHosted;
using Patlus.IdentityManagement.UseCase.Features.Accounts.Commands.UpdateActiveStatus;
using Patlus.IdentityManagement.UseCase.Features.Accounts.Queries.GetAll;
using Patlus.IdentityManagement.UseCase.Features.Accounts.Queries.GetOneById;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Accounts
{
    [ApiController]
    [Route("pools/{" + PoolResolver.POOL_ID_KEY + "}/accounts")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AccountsController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = AccountPolicy.Read)]
        public async Task<AccountDto[]> GetAll()
        {
            var result = await mediator.Send(new GetAllQuery());

            return mapper.ProjectTo<AccountDto>(result).ToArray();
        }

        [HttpGet("{accountId}")]
        [Authorize(Policy = AccountPolicy.Read)]
        public async Task<AccountDto> GetById(Guid accountId)
        {
            var result = await mediator.Send(new GetOneByIdQuery() { Id = accountId });

            return mapper.Map<AccountDto>(result);
        }

        [HttpPost]
        [Authorize(Policy = AccountPolicy.Create)]
        public async Task<ActionResult<AccountDto>> Create([FromBody] CreateForm form)
        {
            var command = new CreateHostedCommand
            {
                Name = form.Name,
                Password = form.Password,
                RequestorId = null
            };

            var account = await mediator.Send(command);

            return Created(new Uri($"{Request.Path}/{account.Id}", UriKind.Relative), mapper.Map<AccountDto>(account));
        }

        [HttpPut("{accountId}/active")]
        [Authorize(Policy = AccountPolicy.UpdateActiveStatus)]
        public async Task<AccountDto> UpdateActiveStatus(Guid accountId, [FromBody] UpdateActiveStatusForm form)
        {
            var command = new UpdateActiveStatusCommand
            {
                Id = accountId,
                Active = form.Active,
                RequestorId = null
            };

            var account = await mediator.Send(command);

            return mapper.Map<AccountDto>(account);
        }
    }
}
