
namespace Patlus.IdentityManagement.Rest.Features.Me
{
    public class UpdatePasswordForm
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string RetypeNewPassword { get; set; } = null!;
    }
}
