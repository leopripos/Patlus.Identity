namespace Patlus.Common.UseCase.Services
{
    public interface IPerformaceTrackingService<TFeature, TResponse> where TFeature : IFeature
    {
        IPerformaceTracker Start(string context, string name);
    }

    public interface IPerformaceTracker
    {
        void Stop();
    }
}
