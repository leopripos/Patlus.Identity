namespace Patlus.IdentityManagement.Rest.Policies
{
    public static class AccountPolicy
    {
        public const string Read = "IdentityService/Account/Read";
        public const string Create = "IdentityService/Account/Create";
        public const string UpdateActiveStatus = "IdentityService/Account/UpdateStatus";
    }
}
