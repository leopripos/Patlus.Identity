using MediatR;
using System;

namespace Patlus.Common.UseCase
{
    public interface IFeatureNotification : INotification
    {
        Guid OrderingGroup { get; }
        DateTimeOffset Time { get; }
    }
}
