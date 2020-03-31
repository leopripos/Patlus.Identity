using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.Common.Presentation.Security;
using Patlus.IdentityManagement.Presentation.Auhtorization.Policies;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOne;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Me
{
    [ApiController]
    [Route("me")]
    [Produces(MediaTypeNames.Application.Json)]
    public class MeController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserResolver _userResolver;

        public MeController(IMediator mediator, IMapper mapper, IUserResolver userResolver)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userResolver = userResolver;
        }

        [HttpGet("profile")]
        [Authorize(Policy = MePolicy.GetProfie)]
        public async Task<ActionResult<ProfileDto>> GetProfile(CancellationToken cancellationToken)
        {
            var command = new GetOneQuery()
            {
                Condition = e => e.Id == _userResolver.Current.Id,
                RequestorId = _userResolver.Current.Id
            };

            var identity = await _mediator.Send(command, cancellationToken);

            return _mapper.Map<ProfileDto>(identity);
        }

        [HttpPut("password")]
        [Authorize(Policy = MePolicy.UpdatePassword)]
        public async Task<ActionResult> UpdateOwnPassword([FromBody] UpdatePasswordForm form, CancellationToken cancellationToken)
        {
            var command = new UpdateOwnPasswordCommand
            {
                OldPassword = form.OldPassword,
                NewPassword = form.NewPassword,
                RetypeNewPassword = form.RetypeNewPassword,
                RequestorId = _userResolver.Current.Id
            };

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
