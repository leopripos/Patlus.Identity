using System;

namespace Patlus.IdentityManagement.Rest.Features.Tokens
{
    public class TokenDto
    {
        public string Scheme { get; set; } = null!;
        public string Access { get; set; } = null!;
        public string Refresh { get; set; } = null!;
        public DateTimeOffset CreatedTime { get; set; }
    }
}
