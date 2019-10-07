using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;
using System.Linq;

namespace Patlus.Identity.UseCase.Features.Accounts.Queries.GetAll
{
    public class GetAllQuery : IQueryFeature<IQueryable<Account>>
    {
        public Guid? RequestorId { get; set; }
    }
}
