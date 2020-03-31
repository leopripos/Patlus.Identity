using MediatR;

namespace Patlus.Common.UseCase
{
    public interface ICommandFeatureHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommandFeature<TResult>
    { }
}
