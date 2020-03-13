using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword
{
    public class UpdateOwnPasswordCommand : ICommandFeature<Identity>
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? RetypeNewPassword { get; set; } 
        public Guid? RequestorId { get; set; }
    }
}
