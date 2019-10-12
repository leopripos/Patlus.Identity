using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Accounts.Commands.UpdateActiveStatus
{
    public class UpdateActiveStatusCommand : ICommandFeature<Account>
    {
        public Guid? Id { get; set; }
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
