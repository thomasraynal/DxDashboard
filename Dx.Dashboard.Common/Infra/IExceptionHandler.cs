using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dx.Dashboard.Common
{
    public interface IExceptionHandler
    {
        void Handle(object sender, DispatcherUnhandledExceptionEventArgs e);
        void Handle(object sender, UnhandledExceptionEventArgs e);
        void Handle(Exception e);
    }
}
