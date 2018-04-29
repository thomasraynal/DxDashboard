using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public static class TaskExtensions
    {
        public static async Task<T> ExecuteAndThrowOnMainThread<T>(this Task<T> task)
        {
            try
            {
                return await task;
            }
            //mandatory - async command are not catched view ThrowExceptions
            //https://github.com/reactiveui/ReactiveUI/issues/1072
            catch (Exception ex)
            {
                var mainScheduler = AppCore.Instance.Get<ISchedulerProvider>();

                mainScheduler.MainThread.Schedule(() => throw ex);

                throw;
            }
        }

    }
}
