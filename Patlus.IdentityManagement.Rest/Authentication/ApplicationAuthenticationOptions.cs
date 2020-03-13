namespace Patlus.IdentityManagement.Rest.Authentication
{
    public class ApplicationAuthenticationOptions
    {
        public PasswordOptions Password { get; set; } = new PasswordOptions();
        public AccessTokenOptions AccessToken { get; set; } = new AccessTokenOptions();
        public RefreshTokenOptions RefreshToken { get; set; } = new RefreshTokenOptions();
    }

    public class PasswordOptions
    {
        public string Salt { get; set; } = "DefaultSalt";
    }

    public class AccessTokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Version { get; set; }
        public string Key { get; set; }
        public int Duration { get; set; }
    }

    public class RefreshTokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Version { get; set; }
        public string Key { get; set; }
        public int Duration { get; set; }
    }
}
