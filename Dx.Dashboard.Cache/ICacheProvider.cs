﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Cache
{
    public interface ICacheProvider<TObject>
    {
        Task Put(string key, TObject obj);
        Task<TObject> Get(string key);
        Task<TObject> GetOrSet(String cacheKey, Func<Task<TObject>> action);
        Task Remove(string key);
        CacheType CacheType { get; }
    }
}
