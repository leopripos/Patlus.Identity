using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh
{
    public class RefreshCommand : ICommandFeature<Token>
    {
        public string? RefreshToken { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
