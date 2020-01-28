namespace Patlus.IdentityManagement.Rest.Features.Tokens
{
    public class TokenDto
    {
        public string Scheme { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
