using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Cache
{
    public class DefaultMemoryCache<TObject> : ICacheProvider<TObject>
    {
        private readonly MemoryCache _cache;
        private readonly TimeSpan cacheDuration = new TimeSpan(0, 30, 0);

        public CacheType CacheType => CacheType.Volatile;

        public DefaultMemoryCache(String applicationName)
        {
            _cache = new MemoryCache(applicationName);
        }

        public Task<TObject> Get(string key)
        {
            var result = _cache.Get(key);
            TObject @object;

            if (null == result) @object = default(TObject);
            else
            {
                @object = (TObject)result;
            }

            return Task.FromResult(@object);
        }

        public async Task<TObject> GetOrSet(string key, Func<Task<TObject>> action)
        {
            var @object = await Get(key);

            if (null != @object) return @object;

            @object = await action();

            await Put(key, @object);

            return @object;
        }

        public Task Remove(string key)
        {
            _cache.Remove(key);

            return Task.CompletedTask;
        }

        public Task Put(string key, TObject obj)
        {
            _cache.Set(key, obj, new DateTimeOffset(DateTime.Now, cacheDuration));

            return Task.CompletedTask;
        }
    }
}
