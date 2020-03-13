using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.Rest.Responses.Content;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;
using System;
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
        /// <param name="form">Create form</param>
        /// <returns>Authentication Token</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Create([FromBody] CreateForm form)
        {
            var command = new CreateCommand()
            {
                PoolId = form.PoolId,
                Name = form.Name,
                Password = form.Password
            };

            try
            {
                var token = await _mediator.Send(command).ConfigureAwait(false);

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
        /// <param name="form">Refresh Form</param>
        /// <returns>Authentication token</returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Refresh([FromBody] RefreshForm form)
        {
            var command = new RefreshCommand()
            {
                RefreshToken = form.RefreshToken
            };

            try
            {
                var token = await _mediator.Send(command).ConfigureAwait(false);

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
