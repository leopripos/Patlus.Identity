using System;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Services
{
    public interface IDbActivatorService {
        Task<IDbInfo> Create(Guid id);
    }
}
