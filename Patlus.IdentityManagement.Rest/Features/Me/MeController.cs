using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patlus.IdentityManagement.Rest.Auhtorization.Policies;
using Patlus.IdentityManagement.Rest.Authentication;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Me
{
    [ApiController]
    [Route("me")]
    [Produces(MediaTypeNames.Application.Json)]
    public class MeController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IUserResolver userResolver;

        public MeController(IMediator mediator, IMapper mapper, IUserResolver userResolver)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.userResolver = userResolver;
        }

        [HttpPut("password")]
        [Authorize(Policy = MePolicy.UpdatePassword)]
        public async Task<ActionResult> UpdateOwnPassword([FromBody] UpdatePasswordForm form)
        {
            var command = new UpdateOwnPasswordCommand
            {
                OldPassword = form.OldPassword,
                NewPassword = form.NewPassword,
                RetypeNewPassword = form.RetypeNewPassword,
                RequestorId = userResolver.Current.Id
            };

            await mediator.Send(command);

            return NoContent();
        }
    }
}
