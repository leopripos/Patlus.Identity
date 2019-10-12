using System.Reflection;

namespace Patlus.IdentityManagement.UseCase
{
    public class ModuleProfile
    {
        public static Assembly[] GetBundles() => new Assembly[] {
            typeof(ModuleProfile).GetTypeInfo().Assembly
        };
    }
}
