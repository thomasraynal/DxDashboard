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
    public static class DispatcherExtensions
    {
        private static Dispatcher Dispatcher
        {
            get
            {
                return AppCore.Instance.Get<Dispatcher>();
            }
        }

        public static void ExecuteOnCurrentDispatcher(this object obj, Action action, DispatcherPriority priority = DispatcherPriority.ApplicationIdle)
        {
             Dispatcher.BeginInvoke(action, priority);
        }
    }
}
