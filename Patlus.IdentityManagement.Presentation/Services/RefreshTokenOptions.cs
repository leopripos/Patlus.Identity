namespace Patlus.IdentityManagement.Presentation.Services
{
    public class RefreshTokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Version { get; set; }
        public string Key { get; set; }
        public int Duration { get; set; }
    }
}
