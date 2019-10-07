using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;

namespace Patlus.Identity.UseCase.Features.Accounts.Queries.GetOneBySecret
{
    public class GetOneBySecretQuery : IQueryFeature<Account>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
