using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Accounts.Queries.GetOneById
{
    public class GetOneByIdQuery : IQueryFeature<Account>
    {
        public Guid? Id { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
