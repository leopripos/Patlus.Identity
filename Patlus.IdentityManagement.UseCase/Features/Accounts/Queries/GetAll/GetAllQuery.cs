using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Linq;

namespace Patlus.IdentityManagement.UseCase.Features.Accounts.Queries.GetAll
{
    public class GetAllQuery : IQueryFeature<IQueryable<Account>>
    {
        public Guid? RequestorId { get; set; }
    }
}
