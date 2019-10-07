using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;

namespace Patlus.Identity.UseCase.Features.Accounts.Commands.CreateHosted
{
    public class CreateHostedCommand : ICommandFeature<Account>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
