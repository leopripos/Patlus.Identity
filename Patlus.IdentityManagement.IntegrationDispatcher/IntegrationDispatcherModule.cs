using System.Reflection;

namespace Patlus.IdentityManagement.IntegrationDispatcher
{
    public static class IntegrationDispatcherModule
    {
        public static Assembly[] GetBundles() => new Assembly[] {
            typeof(IntegrationDispatcherModule).GetTypeInfo().Assembly
        };
    }
}
