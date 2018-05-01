using Dx.Dashboard;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Dx.Dashboard.Demo;

namespace Dx.Dashboard.Demo
{
    internal static class StrategyIdProvider
    {
        private static int stretagyIndex = 0;

        public static int GetID()
        {
            return stretagyIndex++;
        }
    }

    public class Strategy :  IStrategy, IDisposable
    {
        private string _name;

        private ISourceCache<ITrade, Guid> _tradesCache;
        private IObservableCache<ITrade, Guid> _trades;

        private CompositeDisposable _cleanUp;

        public Strategy()
        {
            _name = String.Format("Strategy - [{0}]", StrategyIdProvider.GetID());

            _tradesCache = new SourceCache<ITrade, Guid>(trade => trade.TradeId);
            _trades = _tradesCache.AsObservableCache();

            _cleanUp = new CompositeDisposable();

            var generator = Observable
              .Interval(TimeSpan.FromMilliseconds(TestHelper.Rand.Next(500,3000)))
              .Delay(TimeSpan.FromSeconds(1))
              .Subscribe(time =>
              {
                  _tradesCache.AddOrUpdate(TradeService.Instance.CreateTrade(this));
              });

            _cleanUp.Add(generator);
        }

        public IObservableCache<ITrade, Guid> Trades
        {
            get
            {
                return _trades;
            }
        }

        public string EntityCacheKey
        {
            get
            {
                return _name;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }

}
