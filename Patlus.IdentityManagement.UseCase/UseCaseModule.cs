using System.Reflection;

namespace Patlus.IdentityManagement.UseCase
{
    public static class UseCaseModule
    {
        public static Assembly[] GetBundles() => new Assembly[] {
            typeof(UseCaseModule).GetTypeInfo().Assembly
        };
    }
}
