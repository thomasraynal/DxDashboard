using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dx.Dashboard;
using ReactiveUI;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.ReactiveUI;
using Dx.Dashboard.Core.Extensions;

namespace Dx.Dashboard.Demo
{
    [WidgetStrategy("Strategy View", "GlobalColorScheme_16x16.png")]
    public class StrategyWidgetViewModel : DemoWidgetViewModelBase<StrategyWidget>
    {
        private IStrategy _strategy;
        public IStrategy Strategy
        {
            get { return _strategy; }
            set { this.RaiseAndSetIfChanged(ref _strategy, value); }
        }

        private ReactiveList<TradeViewModel> _trades;
        public ReactiveList<TradeViewModel> Trades
        {
            get { return _trades; }
            set { this.RaiseAndSetIfChanged(ref _trades, value); }
        }

        private double _strategyPnL;
        public double StrategyPnL
        {
            get { return _strategyPnL; }
            set { this.RaiseAndSetIfChanged(ref _strategyPnL, value); }
        }

        private ObservableAsPropertyHelper<int> _tradesCount;
        public int TradesCount
        {
            get { return this._tradesCount.Value; }
        }

        private double _marketValue;
        public double MarketValue
        {
            get { return _marketValue; }
            set { this.RaiseAndSetIfChanged(ref _marketValue, value); }
        }

        public StrategyWidgetViewModel(DemoWorkspaceViewModel host, String cacheId = null) : base(host, cacheId)
        {

            var pricesObservable = PriceService.All
             .Connect()
             .ObserveOn(AppScheduler.MainThread)
             .Do(changes =>
             {

                 changes.ForEach(change =>
                 {
                     var pnl = 0.0;

                     for (var i = 0; i < Trades.Count; i++)
                     {
                         if (Trades[i].Trade.Asset == change.Current.Asset.Name)
                         {
                             Trades[i].PnL = Trades[i].Trade.Price - change.Current.Value;
                         }

                         pnl += Trades[i].PnL;
                     }

                     StrategyPnL = pnl;

                 });
             })
             .DisposeMany()
             .Subscribe();

            Cleanup(pricesObservable);


            this.WhenAnyValue(vm => vm.Workspace)
              .Where(obj => null != obj)
              .Subscribe((obs) =>
              {
                  Strategy = obs.State.Strategy;
              });

            this.WhenAnyValue(vm => vm.Strategy)
              .Where(obj => null != obj)
              .Subscribe((obs) =>
              {
                  Trades = new ReactiveList<TradeViewModel>();

                  var tradeObservable = _strategy.Trades
                  .Connect()
                  .Transform(trade => new TradeViewModel(trade))
                  .Bind(_trades)
                  .DisposeMany()
                  .Subscribe();

                  Cleanup(tradeObservable);
              });

            _tradesCount = this.WhenAnyObservable(vm => vm.Trades.CountChanged)
                 .ToProperty(this, vm => vm.TradesCount);

            this.WhenAnyObservable(vm => vm.Trades.CountChanged)
                   .Select(change => Trades.Sum(trade => trade.Price + trade.PnL))
                   .Subscribe(chg =>
                   {
                       MarketValue = chg;
                   });


            this.WhenAny(vm => vm.TradesCount, vm => vm.StrategyPnL, (trd, pnl) => String.Format("Strategy viewer - {0} Trades - PnL [{1}] ", TradesCount, StrategyPnL))
            .Subscribe(header =>
            {
                Header = header;
            });

        }

    }
}
