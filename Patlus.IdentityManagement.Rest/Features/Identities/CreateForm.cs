namespace Patlus.IdentityManagement.Rest.Features.Identities
{
    public class CreateForm
    {
        public string Name { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string AccountPassword { get; set; } = null!;
    }
}
