using Dx.Dashboard;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class StrategyChartItemViewModel : ReactiveObject, IDisposable
    {
        private String _counterparty;
        public String Counterparty
        {
            get { return _counterparty; }
            set { this.RaiseAndSetIfChanged(ref _counterparty, value); }
        }

        private readonly IDisposable _cleanUp;

        public StrategyChartItemViewModel(IGroup<ITrade, Guid, string> tradesbyCounterparty)
        {
            Counterparty = tradesbyCounterparty.Key;
            _cleanUp = tradesbyCounterparty.Cache.Connect()
             .QueryWhenChanged(query =>
             {
                 return query.Items.Sum(trade => trade.PnL + trade.Price);
             })
             .Subscribe(position => MarketValue = position);
        }

        private double _marketValue;
        public double MarketValue
        {
            get { return _marketValue; }
            set { this.RaiseAndSetIfChanged(ref _marketValue, value); }
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
