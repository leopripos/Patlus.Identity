using System;

#nullable enable

namespace Patlus.IdentityManagement.Rest.Features.Tokens
{
    public class CreateForm
    {
        public Guid? PoolId { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
