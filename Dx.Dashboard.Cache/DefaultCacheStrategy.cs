using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using Dasein.Configuration;

namespace Dasein.Cache.DefaultPersistant
{
    public class DefaultCacheStrategy<TObject> : ICacheStrategy<TObject>
    {
        private ICacheProvider<TObject> _persistantCache;
        private ICacheProvider<TObject> _memoryCache;

        public DefaultCacheStrategy(IConfiguration configuration)
        {
            _persistantCache = new DefaultPersistantCache<TObject>(configuration.ApplicationName);
            _memoryCache = new DefaultMemoryCache<TObject>(configuration.ApplicationName);
        }

        public ICacheProvider<TObject> PersistantCache => _persistantCache;

        public ICacheProvider<TObject> MemoryCache => _memoryCache;

        public async Task Clear(CacheType cacheType)
        {
            if (cacheType == CacheType.Volatile) await MemoryCache.Clear();
            if (cacheType == CacheType.Persistant) await PersistantCache.Clear();
        }

        public Task Clear(CacheType cacheType, string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CacheStatus> GetStatus()
        {
            throw new NotImplementedException();
        }

        public Task InvalidateWhenKeyContains(string key)
        {
            throw new NotImplementedException();
        }
    }
}
