using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Services
{
    public class TokenCacheService : ITokenCacheService
    {
        private readonly IDistributedCache distributedCache;

        public TokenCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public bool HasToken(Guid accountId, Guid tokenId, string authKey)
        {
            var key = $"{accountId.ToString()}:{tokenId.ToString()}";
            var value = distributedCache.GetString(key);

            return value == authKey;
        }

        public Task Set(Guid accountId, Guid tokenId, string authKey, DateTimeOffset expiredTime)
        {
            var key = $"{accountId.ToString()}:{tokenId.ToString()}";

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiredTime
            };

            return distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(authKey), options);
        }

        public Task Remove(Guid accountId, Guid tokenId)
        {
            var key = $"{accountId.ToString()}:{tokenId.ToString()}";

            return distributedCache.RemoveAsync(key);
        }
    }
}
