using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class Price : IPrice
    {
        private long _timestamp;
        private Asset _asset;
        private double _value;

        public Price(long timestamp, Asset asset, double value)
        {
            _timestamp = timestamp;
            _asset = asset;
            _value = value;
        }

        public long Timestamp => _timestamp;

        public Asset Asset => _asset;

        public string EntityCacheKey => String.Format("{0}-{1}-{2}", Timestamp, Asset.Name, Value);

        public double Value => _value;
    }
}
