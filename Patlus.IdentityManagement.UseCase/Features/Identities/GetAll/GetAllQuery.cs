using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Linq;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetAll
{
    public class GetAllQuery : IQueryFeature<Identity[]>
    {
        public Guid? PoolId { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
