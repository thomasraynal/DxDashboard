using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public interface IPrice : IEntity
    {
        long Timestamp { get; }
        Asset Asset { get; }
        double Value { get; }
    }
}
