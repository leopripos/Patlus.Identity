using System;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Services
{
    public interface ITokenCacheService
    {
        bool HasToken(Guid accountId, Guid tokenId, string authKey);
        Task Set(Guid accountId, Guid tokenId, string authKey, DateTime expiredTime);
        Task Remove(Guid accountId, Guid tokenId);
    }
}
