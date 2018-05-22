using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dx.Dashboard.Core
{
    public static class ReactiveUiExtensions
    {
        private static IObservable<bool> ExecuteIfPossibleInternal(this ReactiveCommand cmd, object param = null)
        {
            if (cmd.CanExecute.FirstAsync().Any(can => can).Wait())
            {
                (cmd as ICommand).Execute(param);
            }

            return Observable.Return(true);
        }

        public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd)
        {
            return ExecuteIfPossibleInternal(cmd);
        }

        public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd, TParam param)
        {
            return ExecuteIfPossibleInternal(cmd, param);
        }

        public static IObservable<bool> ExecuteIfPossible<TParam>(this ReactiveCommand cmd, TParam param)
        {
            return ExecuteIfPossibleInternal(cmd, param);
        }

        public static IObservable<bool> ExecuteIfPossible(this ReactiveCommand cmd)
        {
            return ExecuteIfPossibleInternal(cmd);
        }

        public static bool CanExecute(this ReactiveCommand cmd)
        {
            return (cmd as ICommand).CanExecute(null);
        }
    }
}
