using Akavache;
using Dx.Dashboard.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Cache
{
    public class DefaultPersistantCache<TObject> : ICacheProvider<TObject>
    {
        public CacheType CacheType => CacheType.Persistant;

        private IBlobCache CacheInternal
        {
            get
            {
                return BlobCache.UserAccount;
            }
        }

        public DefaultPersistantCache(String name)
        {
            BlobCache.ApplicationName = name;
            BlobCache.EnsureInitialized();
        }

        public async Task Put(string key, TObject obj)
        {
            await CacheInternal.InsertObject(key, obj);
        }

        public async Task<TObject> Get(string key)
        {
            return await CacheInternal.GetObject<TObject>(key).Catch(Observable.Return(default(TObject)));
        }

        public async Task<TObject> GetOrSet(string key, Func<Task<TObject>> action)
        {
            var @object = await Get(key);

            if (null != @object) return @object;

            @object = await action();

            await Put(key, @object);

            return @object;
        }

        public async Task Remove(string key)
        {
            await CacheInternal.Invalidate(key);
        }
    }
}
