using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret
{
    public class GetOneBySecretQuery : IQueryFeature<Identity>
    {
        public Guid? PoolId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
