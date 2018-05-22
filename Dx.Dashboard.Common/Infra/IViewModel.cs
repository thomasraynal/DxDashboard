using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dx.Dashboard.Common
{
    public interface IViewModel : IDisposable
    {
        String ViewModelUniqueId { get; }
        Window Host { get;  set; }
        IAppContainer AppContainer { get; }
        Task Initialize();
        ISchedulerProvider AppScheduler { get; }
        CompositeDisposable CleanUp { get; }    
        void RegisterCommandExceptionHandler(params ReactiveCommand[] commands);
    }
}
