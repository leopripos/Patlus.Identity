
namespace Patlus.IdentityManagement.Rest.Features.Me
{
    public class UpdatePasswordForm
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? RetypeNewPassword { get; set; }
    }
}
