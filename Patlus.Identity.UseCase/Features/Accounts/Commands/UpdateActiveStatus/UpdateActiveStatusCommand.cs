using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;

namespace Patlus.Identity.UseCase.Features.Accounts.Commands.UpdateActiveStatus
{
    public class UpdateActiveStatusCommand : ICommandFeature<Account>
    {
        public Guid? Id { get; set; }
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
