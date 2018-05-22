using Akavache;
using Dx.Dashboard.Common;
using Dx.Dashboard.Common.Configuration;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dx.Dashboard.Cache
{
    ////https://github.com/flagbug/Lager
    //needed to create an abstraction layer...
    public class ReactiveSettingsStorage : PropertyCacheBase
    {
        private IBlobCache _blobCache;
        private Dictionary<string, object> _cache;
        private ReaderWriterLockSlim _cacheLock;

        public ReactiveSettingsStorage(IDashboardConfiguration configuration)
        {
            BlobCache.ApplicationName = configuration.ApplicationName;
            _blobCache = AppCore.Instance.Get<IBlobCache>();
        }

        protected override Task InitializeInternal()
        {
            _blobCache = _blobCache ?? BlobCache.UserAccount;

            _cache = new Dictionary<string, object>();
            _cacheLock = new ReaderWriterLockSlim();

            foreach (var property in GetType().GetRuntimeProperties())
            {
                property.GetValue(this);
            }

            return Task.CompletedTask;

        }

       public override T GetOrCreate<T>(T defaultValue, [CallerMemberName] string key = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _cacheLock.EnterReadLock();

            try
            {
                if (_cache.TryGetValue(key, out object value))
                {
                    return (T)value;
                }
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

            T returnValue = _blobCache.GetOrCreateObject($"{_cacheKey}:{key}", () => defaultValue)
                .Do(x => AddToInternalCache(key, x)).Wait();

            return returnValue;
        }

        public override void SetOrCreate<T>(T value, [CallerMemberName] string key = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            AddToInternalCache(key, value);

            // Fire and forget, we retrieve the value from the in-memory cache from now on
            _blobCache.InsertObject($"{_cacheKey}:{key}", value).Wait();

            _model.RaisePropertyChanged(key);
        }

        private void AddToInternalCache(string key, object value)
        {
            _cacheLock.EnterWriteLock();

            _cache[key] = value;

            _cacheLock.ExitWriteLock();
        }
    }
}
