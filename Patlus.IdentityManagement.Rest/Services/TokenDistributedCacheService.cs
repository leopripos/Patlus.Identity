using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Services
{
    public class TokenDistributedCacheService : ITokenCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public TokenDistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public bool HasToken(Guid tokenId, Guid authKey)
        {
            var key = $"{tokenId}";
            var value = _distributedCache.GetString(key);

            return value == authKey.ToString();
        }

        public Task Set(Guid tokenId, Guid authKey, DateTimeOffset expiredTime)
        {
            var key = $"{tokenId}";

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiredTime
            };

            return _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(authKey.ToString()), options);
        }

        public Task Remove(Guid tokenId)
        {
            var key = $"{tokenId}";

            return _distributedCache.RemoveAsync(key);
        }
    }
}
