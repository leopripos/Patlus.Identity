using System.Reflection;

namespace Patlus.IdentityManagement.Presentation
{
    public static class NotificationDispatcherModule
    {
        public static object Assembly { get; internal set; }

        public static Assembly[] GetBundles() => new Assembly[] {
            typeof(NotificationDispatcherModule).GetTypeInfo().Assembly
        };
    }
}
