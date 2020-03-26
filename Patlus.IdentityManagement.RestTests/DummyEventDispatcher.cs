using Patlus.Common.Presentation;
using Patlus.Common.Presentation.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.RestTests
{
    public class DummyEventDispatcher : IEventDispatcher
    {
        public Task DispatchAsync<TNotification>(string topic, TNotification notification, Guid? orderGroup, CancellationToken cancellationToken) where TNotification : IDto
        {
            return Task.CompletedTask;
        }
    }
}
