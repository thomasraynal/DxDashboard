using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dx.Dashboard.Demo;
using DynamicData;

namespace Dx.Dashboard.Demo
{
    public class StrategyService : IEntityService<IStrategy>
    {
        private readonly SourceCache<IStrategy, string> _availableEntitiesCache;
        private readonly IObservableCache<IStrategy, string> _availableEntities;

        public IObservableCache<IStrategy, string> All => _availableEntities;

        public StrategyService()
        {
            _availableEntitiesCache = new SourceCache<IStrategy, String>(price => price.EntityCacheKey);
            _availableEntities = _availableEntitiesCache.AsObservableCache();

            _availableEntitiesCache.Edit(innerCache =>
            {
                innerCache.AddOrUpdate(Enumerable.Range(0, 5).Select(_ => new Strategy()));
            });

        }

        public Task<bool> CreateOrUpdate(IStrategy entity)
        {
            _availableEntitiesCache.AddOrUpdate(entity);
            return Task.FromResult(true);
        }

        public Task<bool> Delete(IStrategy entity)
        {
            _availableEntitiesCache.Remove(entity);
            return Task.FromResult(true);
        }
    }
}
