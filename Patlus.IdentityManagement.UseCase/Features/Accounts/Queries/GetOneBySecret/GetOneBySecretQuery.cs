using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Accounts.Queries.GetOneBySecret
{
    public class GetOneBySecretQuery : IQueryFeature<Account>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
