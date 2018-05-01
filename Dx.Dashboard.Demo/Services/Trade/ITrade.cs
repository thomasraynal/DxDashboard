using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public interface ITrade
    {
        Guid TradeId { get; }
        long Timestamp { get; }
        double Amount { get; }
        double Price { get; }
        double PnL { get; set; }
        String Asset { get; }
        BuySell Way { get; }
        String Counterparty { get; }
    }
}
