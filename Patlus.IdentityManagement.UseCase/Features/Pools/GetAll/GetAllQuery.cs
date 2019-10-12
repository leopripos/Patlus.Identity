using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.GetAll
{
    public class GetAllQuery : IQueryFeature<Pool[]>
    {
        public Guid? RequestorId { get; set; }
    }
}
