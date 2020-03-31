using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Create
{
    public class CreateCommand : ICommandFeature<Token>
    {
        public Guid? PoolId { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
