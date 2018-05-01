using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public interface IStrategy : IEntity
    {
        string Name { get; }
        IObservableCache<ITrade, Guid> Trades { get; }
    }

}
