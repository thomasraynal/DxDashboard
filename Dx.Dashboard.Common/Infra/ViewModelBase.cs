using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dx.Dashboard.Common
{
    public class ViewModelBase : ReactiveObject, IDisposable, IViewModel
    {
        private List<Action> _disposableActions;
        private CompositeDisposable _disposable;
        private IPropertyCache _propertyCache;

        public CompositeDisposable CleanUp
        {
            get 
            {
                return _disposable;
            }
        }

        public IPropertyCache PropertyCache
        {
            get
            {
                return _propertyCache;
            }
        }

        public IAppContainer AppContainer
        {
            get
            {
                return AppCore.Instance.Get<IAppContainer>();
            }
        }

        public ISchedulerProvider AppScheduler
        {
            get
            {
                return AppCore.Instance.Get<ISchedulerProvider>();
            }
        }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isProcessing, value);
            }
        }

        public Window Host { get; set; }
        public string ViewModelId { get; private set; }

        protected virtual Task InitializeInternal()
        {
            return Task.CompletedTask;
        }

        public async Task Initialize()
        {
            await InitializeInternal();
        }

        public ViewModelBase(String viewModelId = null)
        {
            ViewModelId = viewModelId ?? Guid.NewGuid().ToString();
            _propertyCache = AppCore.Instance.Get<IPropertyCache>();
            _propertyCache.Initialize(this).Wait();
            _disposable = new CompositeDisposable();
            _disposableActions = new List<Action>();
        }

        public void Cleanup(params IDisposable[] disposables)
        {
            foreach(var disposable in disposables)
            {
                CleanUp.Add(disposable);
            }
        }

        public void RegisterCommandExceptionHandler(params ReactiveCommand[] commands)
        {
            var exeptionsObservable = commands.Select(command => command.ThrownExceptions);

            var disposable = Observable.Merge(exeptionsObservable)
             .Throttle(TimeSpan.FromMilliseconds(250), AppScheduler.MainThread)
             .Subscribe(ex =>
             {
                 throw ex;
             });

            _disposable.Add(disposable);
        }

        protected async Task WithIsProcessing(Func<Task> action)
        {
            try
            {
                IsProcessing = true;
                await action();
                IsProcessing = false;
            }
            catch (Exception)
            {
                IsProcessing = false;
                throw;
            }
        }

        protected void WithIsProcessing(Action action)
        {
            try
            {
                IsProcessing = true;
                action();
                IsProcessing = false;
            }
            catch (Exception)
            {
                IsProcessing = false;
                throw;
            }
        }

        protected ReactiveCommand CreateIsProcessingCommand(Action action)
        {
            return ReactiveCommand.Create(() =>
            {
                try
                {
                    IsProcessing = true;
                    action();
                    IsProcessing = false;
                }
                catch (Exception)
                {
                    IsProcessing = false;
                    throw;
                }
            });
        }

        protected ReactiveCommand CreateIsProcessingCommand<TParameter>(Action<TParameter> action)
        {
            return ReactiveCommand.Create<TParameter>((parameter) =>
            {
                try
                {
                    IsProcessing = true;
                    action(parameter);
                    IsProcessing = false;
                }
                catch (Exception)
                {
                    IsProcessing = false;
                    throw;
                }
            });
        }

        protected ReactiveCommand CreateIsProcessingCommand(Func<Task> action)
        {
            return ReactiveCommand.Create(async () =>
            {
                try
                {
                    IsProcessing = true;
                    await action();
                    IsProcessing = false;
                }
                catch (Exception)
                {
                    IsProcessing = false;
                    throw;
                }
            });
        }

        protected ReactiveCommand CreateIsProcessingCommand<TParameter>(Func<TParameter, Task> action)
        {
            return ReactiveCommand.Create<TParameter>(async (parameter) =>
            {
                try
                {
                    IsProcessing = true;
                    await action(parameter);
                    IsProcessing = false;
                }
                catch (Exception)
                {
                    IsProcessing = false;
                    throw;
                }
            });
        }

        protected virtual void DisposeInternal()
        {
        }

        public void AddDisposable(Func<IDisposable> disposable)
        {
            _disposable.Add(disposable());
        }

        public void AddDisposeAction(Action action)
        {
            _disposableActions.Add(action);
        }

        public void Dispose()
        {
            DisposeInternal();

            _disposableActions.ForEach(action => action());
            _disposable.Dispose();
        }
    }
}
