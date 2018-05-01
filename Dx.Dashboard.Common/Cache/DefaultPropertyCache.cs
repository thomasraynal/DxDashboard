using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Common.Cache
{
    public class DefaultPropertyCache : PropertyCacheBase
    {
        private Dictionary<String, object> _cache;

        public DefaultPropertyCache()
        {
            _cache = new Dictionary<string, object>();
        }

        public override T GetOrCreate<T>(T defaultValue, [CallerMemberName] string key = null)
        {
            if (_cache.ContainsKey(key)) return (T)_cache[key];
            return defaultValue;
        }

        public override void SetOrCreate<T>(T value, [CallerMemberName] string key = null)
        {
            _cache[key] = value;
        }
    }
}
