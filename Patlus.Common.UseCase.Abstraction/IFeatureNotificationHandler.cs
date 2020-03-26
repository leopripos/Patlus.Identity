namespace Patlus.Common.UseCase
{
    public interface IFeatureNotificationHandler<in TNotification> : MediatR.INotificationHandler<TNotification>
        where TNotification : IFeatureNotification
    {
    }
}
