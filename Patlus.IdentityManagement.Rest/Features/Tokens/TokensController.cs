using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.Common.Presentation.Responses.Errors;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Tokens
{
    [ApiController]
    [Route("tokens")]
    public class TokensController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TokensController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Create authentication token by passing user credential
        /// </summary>
        /// <returns>Authentication Token</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Create([FromBody] CreateForm form, CancellationToken cancellationToken)
        {
            var command = new CreateCommand()
            {
                PoolId = form.PoolId,
                Name = form.Name,
                Password = form.Password
            };

            try
            {
                var token = await _mediator.Send(command, cancellationToken);

                return _mapper.Map<TokenDto>(token);
            }
            catch (NotFoundException)
            {
                return BadRequest(new ValidationErrorDto("Invalid name or password."));
            }
        }

        /// <summary>
        /// Refresh the token by passing refresh token
        /// </summary>
        /// <returns>Authentication token</returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Refresh([FromBody] RefreshForm form, CancellationToken cancellationToken)
        {
            var command = new RefreshCommand()
            {
                RefreshToken = form.RefreshToken
            };

            try
            {
                var token = await _mediator.Send(command, cancellationToken);

                return _mapper.Map<TokenDto>(token);
            }
            catch (Exception e) when (
                    (e is ArgumentException argumentException && argumentException.ParamName == nameof(command.RefreshToken))
                    || (e is NotFoundException)
                )
            {
                ModelState.AddModelError(nameof(form.RefreshToken), "Invalid refresh token.");
            }

            return ValidationProblem(ModelState);
        }
    }
}
