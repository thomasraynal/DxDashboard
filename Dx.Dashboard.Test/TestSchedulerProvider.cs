using Dx.Dashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Test
{
    public class TestSchedulerProvider : ISchedulerProvider
    {
        public IScheduler MainThread => Scheduler.Default;

        public IScheduler Background => Scheduler.Default;
    }
}
