using Patlus.Identity.UseCase.Entities;

namespace Patlus.Identity.Rest.Services
{
    public interface IPoolResolver
    {
        Pool Current { get; }
    }
}
