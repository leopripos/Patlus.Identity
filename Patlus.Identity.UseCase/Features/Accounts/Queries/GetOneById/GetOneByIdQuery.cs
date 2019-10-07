using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;

namespace Patlus.Identity.UseCase.Features.Accounts.Queries.GetOneById
{
    public class GetOneByIdQuery : IQueryFeature<Account>
    {
        public Guid? Id { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
