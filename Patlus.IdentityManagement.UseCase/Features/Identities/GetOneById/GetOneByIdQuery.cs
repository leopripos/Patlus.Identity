using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetOneById
{
    public class GetOneByIdQuery : IQueryFeature<Identity>
    {
        public Guid? PoolId { get; set; }
        public Guid? Id { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
