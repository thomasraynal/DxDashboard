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
        public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd) =>
            cmd.CanExecute.FirstAsync().Where(can => can).Do(async _ => await cmd.Execute());

        public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd, TParam param) =>
            cmd.CanExecute.FirstAsync().Where(can => can).Do(async _ => await cmd.Execute(param));

        public static IObservable<bool> ExecuteIfPossible(this ReactiveCommand cmd) =>
            cmd.CanExecute.FirstAsync().Where(can => can).Do(async _ => await (cmd as ReactiveCommand<Unit, Unit>).Execute());

        public static IObservable<bool> ExecuteIfPossible<TParam>(this ReactiveCommand cmd, TParam param) =>
            cmd.CanExecute.FirstAsync().Where(can => can).Do(async _ => await (cmd as ReactiveCommand<TParam, Unit>).Execute(param));
    }
}
