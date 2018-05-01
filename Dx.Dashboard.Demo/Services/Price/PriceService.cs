using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using System.Reactive.Linq;
using Dx.Dashboard.Core.Extensions;

namespace Dx.Dashboard.Demo
{
    public class PriceService : IEntityService<IPrice>
    {
        private readonly SourceCache<IPrice, string> _availableEntitiesCache;
        private readonly IObservableCache<IPrice, string> _availableEntities;

        public IObservableCache<IPrice, string> All => _availableEntities;

        private IPrice CreatePrice()
        {
            var asset = TestHelper.Random(TestHelper.Assets);
            var way = TestHelper.Rand.Next(2) == 0 ? -1.0 : 1.0;
            var last = All.Items.Last(price => price.Asset.Name == asset.Name);
            var newPrice = last.Value + (way * last.Value * 0.05);
            return new Price(DateTime.Now.Ticks, asset, newPrice);
        }

        public PriceService()
        {
            _availableEntitiesCache = new SourceCache<IPrice, String>(price => price.EntityCacheKey);
            _availableEntities = _availableEntitiesCache.AsObservableCache();

            TestHelper.Assets.ForEach(asset => _availableEntitiesCache.AddOrUpdate(new Price(DateTime.Now.Ticks, asset, asset.StartingPrice)));

            Observable
             .Interval(TimeSpan.FromMilliseconds(500))
             .Subscribe((_) =>
             {
                 var price = CreatePrice();
                 _availableEntitiesCache.AddOrUpdate(price);
             });
        }

        public Task<bool> CreateOrUpdate(IPrice entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(IPrice entity)
        {
            throw new NotImplementedException();
        }
    }
}
