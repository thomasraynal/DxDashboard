using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dx.Dashboard.Core
{
    public static class DispatcherExtensions
    {
        public static async Task ExecuteOnCurrentDispatcherAsync(this object obj, Action action, DispatcherPriority priority = DispatcherPriority.ApplicationIdle)
        {
            await Dispatcher.CurrentDispatcher.BeginInvoke(action, priority);
        }

        public static void ExecuteOnCurrentDispatcher(this object obj, Action action, DispatcherPriority priority = DispatcherPriority.ApplicationIdle)
        {
             Dispatcher.CurrentDispatcher.BeginInvoke(action, priority);
        }
    }
}
