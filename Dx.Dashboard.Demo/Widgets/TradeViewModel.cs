using Dx.Dashboard.Common;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class TradeViewModel : ViewModelBase, ITradeViewModel
    {
        private double _pnl;
        public double PnL
        {
            get { return _pnl; }
            set { this.RaiseAndSetIfChanged(ref _pnl, value); }
        }

        public double Amount => _trade.Amount;

        public double Price => _trade.Price;

        public string Asset => _trade.Asset;

        public string Counterparty => _trade.Counterparty;

        public BuySell Way => _trade.Way;

        public long Timestamp => _trade.Timestamp;

        private ITrade _trade;
        public ITrade Trade
        {
            get { return _trade; }
            set
            {
                this.RaiseAndSetIfChanged(ref _trade, value);
            }
        }

        public TradeViewModel(ITrade tarde)
        {
            _trade = tarde;
        }
    }
}

