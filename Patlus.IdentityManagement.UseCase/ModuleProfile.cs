using System.Reflection;

namespace Patlus.IdentityManagement.UseCase
{
    public static class ModuleProfile
    {
        public static Assembly[] GetBundles() => new Assembly[] {
            typeof(ModuleProfile).GetTypeInfo().Assembly
        };
    }
}
