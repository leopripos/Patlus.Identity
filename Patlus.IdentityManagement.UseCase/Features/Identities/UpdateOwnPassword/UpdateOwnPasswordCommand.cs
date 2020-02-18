using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword
{
    public class UpdateOwnPasswordCommand : ICommandFeature<Identity>
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string RetypeNewPassword { get; set; } = null!;
        public Guid? RequestorId { get; set; }
    }
}
