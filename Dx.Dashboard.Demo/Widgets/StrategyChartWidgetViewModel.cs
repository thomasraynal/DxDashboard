using Dx.Dashboard;
using DynamicData;
using DynamicData.ReactiveUI;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{


    [WidgetStrategyChart("Strategy Chart", "GlobalColorScheme_16x16.png")]
    public class StrategyChartWidgetViewModel : DemoWidgetViewModelBase<StrategyChartWidget>
    {
            
        private ReactiveList<StrategyChartItemViewModel> _strategyChartItems;
        public ReactiveList<StrategyChartItemViewModel> StrategyChartItems
        {
            get { return _strategyChartItems; }
            set { this.RaiseAndSetIfChanged(ref _strategyChartItems, value); }
        }

        private IStrategy _strategy;
        public IStrategy Strategy
        {
            get { return _strategy; }
            set { this.RaiseAndSetIfChanged(ref _strategy, value); }
        }

        public StrategyChartWidgetViewModel(DemoWorkspaceViewModel host, String cacheID) : base(host, cacheID)
        {
            StrategyChartItems = new ReactiveList<StrategyChartItemViewModel>();

            this.WhenAnyValue(vm => vm.Workspace)
            .Where(obj => null != obj)
            .Subscribe((obs) =>
            {
                Strategy = obs.State.Strategy;
            });


            this.WhenAnyValue(vm => vm.Strategy)
                .Where(chg=> chg!= null)
            .Subscribe(chg =>
            {
                var tradeObservable = Strategy.Trades
                .Connect()
                .Group(trade => trade.Counterparty)
                .Transform(group => new StrategyChartItemViewModel(group))
                .Bind(StrategyChartItems)
                .DisposeMany()
                .Subscribe();

                Cleanup(tradeObservable);

            });

            this.WhenAnyObservable(vm => vm.Strategy.Trades.CountChanged)
              .Subscribe((obs) =>
              {
                  Header = String.Format("{0} Counterparties", Strategy.Trades.Items.Select(trade => trade.Counterparty).Distinct().Count());

                  //force chart to redraw
                  this.RaisePropertyChanged("StrategyChartItems");
              });
        }
    }
}
