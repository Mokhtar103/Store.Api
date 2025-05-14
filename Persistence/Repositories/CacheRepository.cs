using Domain.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connectionMultiplexer) : ICacheRepository
    {

        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<string?> GetAsync(string cachekey)
            => await _database.StringGetAsync(cachekey);
              

        public async Task SetAsync(string cachekey, string value, TimeSpan timeToLive)
            => await _database.StringSetAsync(cachekey, value, timeToLive);
    }
}
