using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using DynamicData;
using DynamicData.ReactiveUI;
using System.Reactive.Linq;
using Dx.Dashboard.Core;
using Dx.Dashboard.Common;

namespace Dx.Dashboard.Demo
{
    [WidgetPnLView("PnL", "GlobalColorScheme_16x16.png")]
    public class PnLWidgetViewModel : DemoWidgetViewModelBase<PnLWidget>
    {

        private ReactiveList<StrategyWidgetViewModel> _strategies;
        public ReactiveList<StrategyWidgetViewModel> Strategies
        {
            get { return _strategies; }
            set { this.RaiseAndSetIfChanged(ref _strategies, value); }
        }

        private ReactiveList<StrategyChartItemViewModel> _strategyChartItems;
        public ReactiveList<StrategyChartItemViewModel> StrategyChartItems
        {
            get { return _strategyChartItems; }
            set { this.RaiseAndSetIfChanged(ref _strategyChartItems, value); }
        }

        private StrategyWidgetViewModel _strategy;
        public StrategyWidgetViewModel SelectedStrategy
        {
            get { return _strategy; }
            set { this.RaiseAndSetIfChanged(ref _strategy, value); }
        }

        private int _tradesCount;
        public int TradesCount
        {
            get { return _tradesCount; }
            set { this.RaiseAndSetIfChanged(ref _tradesCount, value); }
        }

        private double _pnlGlobal;
        public double PnLGlobal
        {
            get { return _pnlGlobal; }
            set { this.RaiseAndSetIfChanged(ref _pnlGlobal, value); }
        }

        private int _strategiesCount;
        public int StrategiesCount
        {
            get { return _strategiesCount; }
            set { this.RaiseAndSetIfChanged(ref _strategiesCount, value); }
        }

        public ReactiveCommand OpenStrategyWorkspace { get; private set; }

        public PnLWidgetViewModel(DemoWorkspaceViewModel host, String cacheID = null) : base(host, cacheID)
        {
            Strategies = new ReactiveList<StrategyWidgetViewModel>();

            this.WhenAnyObservable(vm => vm.Strategies.CountChanged).Subscribe(change => StrategiesCount = change);

            var strategyObservable = StrategyService.All.Connect()
                .Transform(strategy => new StrategyWidgetViewModel(Workspace)
                {
                    Strategy = strategy
                })
                .Bind(_strategies)
                .DisposeMany()
                .Subscribe();

            var pricesObservable = PriceService.All
                .Connect()
                .ObserveOn(AppScheduler.MainThread)
                .Do(changes =>
                {
                    PnLGlobal = Strategies.Sum(strategy => strategy.Trades.Sum(trade => trade.PnL));
                })
                .DisposeMany()
                .Subscribe();

            OpenStrategyWorkspace = ReactiveCommand.Create<StrategyWidgetViewModel>((vm) =>
            {
                
                var state = this.Workspace.Dashboard.GetState();
                this.Workspace.Dashboard.CreateWorkspace(new DemoWorkspaceState(vm.Name, state.Date, DemoWorkspaceType.Strategy)
                {
                    Strategy = vm.Strategy
                },true);
            });

            this.WhenAny(vm => vm.TradesCount, vm => vm.StrategiesCount, vm => vm.PnLGlobal, (trd, str, pnl) => String.Format("PnL viewer - {0} Strategies - {1} Trades(s) - PnL [{2}] ", StrategiesCount, TradesCount, PnLGlobal))
                .Subscribe(header =>
                {
                    Header = header;
                });

            var tradesCountObservable = Observable
                .Interval(TimeSpan.FromMilliseconds(500))
                .ObserveOnDispatcher()
                .Subscribe(observer =>
                {
                    TradesCount = Strategies.Sum(strategy => strategy.TradesCount);
                });

            Cleanup(tradesCountObservable, strategyObservable, pricesObservable);
        }

   
    }
}
