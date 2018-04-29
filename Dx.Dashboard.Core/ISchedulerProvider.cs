using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public interface ISchedulerProvider
    {
        IScheduler MainThread { get; }
        IScheduler Background { get; }
    }
}
