using Patlus.Common.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Entities
{
    public class Token : IEntity
    {
        public Guid Id { get; set; }
        public string Scheme { get; set; } = null!;
        public string Access { get; set; } = null!;
        public string Refresh { get; set; } = null!;
        public DateTimeOffset CreatedTime { get; set; }
    }
}
