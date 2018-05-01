using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public interface ITradeViewModel
    {
        double Amount { get; }
        string Asset { get; }
        string Counterparty { get; }
        double PnL { get; set; }
        double Price { get; }
        long Timestamp { get; }
        ITrade Trade { get; set; }
        BuySell Way { get; }
    }
}
