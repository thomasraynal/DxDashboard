using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class Asset
    {
        private string _name;
        private double _startingPrice;

        public Asset(String name, double startingPrice)
        {
            _name = name;
            _startingPrice = startingPrice;
        }

        public String Name => _name;
        public double StartingPrice => _startingPrice;

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Asset && obj.GetHashCode() == this.GetHashCode();
        }
    }

}
