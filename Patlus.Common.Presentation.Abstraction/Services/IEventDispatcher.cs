using Patlus.Common.Presentation.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Common.Presentation.Services
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<TNotification>(string topic, TNotification notification, Guid? orderGroup, CancellationToken cancellationToken)
            where TNotification : IDto;
    }
}
