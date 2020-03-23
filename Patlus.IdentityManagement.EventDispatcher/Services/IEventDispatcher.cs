using System;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.EventDispatcher.Services
{
    public interface IEventDispatcher
    {
        Task Dispatch<TNotification>(string topic, TNotification notification, Guid? orderGroup = null);
    }
}
