using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class Trade : ITrade
    {
        private double _amount;
        private string _asset;
        private double _price;
        private string _counterparty;
        private Guid _id;
        private BuySell _way;
        private long _timestamp;
      
        public Trade(double amount, double price, string asset, string counterparty, BuySell way)
        {
            _amount = amount;
            _asset = asset;
            _price = price;
            _counterparty = counterparty;
            _way = way;
            _timestamp = DateTime.Now.Ticks;
            _id = Guid.NewGuid();
        
        }

        public double Amount => _amount;

        public double Price => _price;
        
        public string Asset => _asset;

        public string Counterparty => _counterparty;

        public Guid TradeId => _id;

        public BuySell Way => _way;

        public double PnL { get; set; }

        public long Timestamp => _timestamp;
    }
}
