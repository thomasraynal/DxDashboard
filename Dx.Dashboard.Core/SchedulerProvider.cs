using Dx.Dashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dx.Dashboard.Core
{
    public class SchedulerProvider : ISchedulerProvider
    {

        public IScheduler MainThread { get; }

        public IScheduler Background { get; } = TaskPoolScheduler.Default;

        public SchedulerProvider(Dispatcher dispatcher)
        {
            MainThread = new DispatcherScheduler(dispatcher);
        }

    }
}
