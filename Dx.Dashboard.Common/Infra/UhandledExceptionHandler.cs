using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace Dx.Dashboard.Common
{
    public class UhandledExceptionHandler
    {
        private readonly IClientLogger _logger;

        public UhandledExceptionHandler(IClientLogger logger)
        {
            _logger = logger;

            Application.Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            _logger.Error(ex);
        }

        private void CurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = e.Exception;
            _logger.Error(ex);
            e.Handled = true;
        }

    }
}
