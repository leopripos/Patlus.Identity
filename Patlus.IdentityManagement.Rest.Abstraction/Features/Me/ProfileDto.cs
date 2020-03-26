using System;

namespace Patlus.IdentityManagement.Rest.Features.Me
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; } = null!;
    }
}
