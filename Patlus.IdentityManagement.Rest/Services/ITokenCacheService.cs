using System;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Services
{
    public interface ITokenCacheService
    {
        bool HasToken(Guid tokenId, Guid authKey);
        Task Set(Guid tokenId, Guid authKey, DateTimeOffset expiredTime);
        Task Remove(Guid tokenId);
    }
}
