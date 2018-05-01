using Dx.Dashboard.Cache;
using Dx.Dashboard.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dx.Dashboard.Cache
{
    public class DefaultCacheStrategy<TObject> : ICacheStrategy<TObject>
    {
        private ICacheProvider<TObject> _persistantCache;
        private ICacheProvider<TObject> _memoryCache;

        public DefaultCacheStrategy(IDashboardConfiguration configuration)
        {
            _persistantCache = new DefaultPersistantCache<TObject>(configuration.ApplicationName);
            _memoryCache = new DefaultMemoryCache<TObject>(configuration.ApplicationName);
        }

        public ICacheProvider<TObject> PersistantCache => _persistantCache;

        public ICacheProvider<TObject> MemoryCache => _memoryCache;

    }
}
