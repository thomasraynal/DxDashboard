using DevExpress.Xpf.Core;
using Dx.Dashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Dx.Dashboard.Demo
{
    public static class Startup
    {

        public static void Run<TUserControl, TViewModel>(Application app, Type splashScreen = null, Action afterInit = null)
            where TUserControl : UserControl
            where TViewModel : IViewModel
        {
            var logger = AppCore.Instance.Get<IClientLogger>();

            try
            {
                if (null != splashScreen) DXSplashScreen.Show(splashScreen);

                var exceptionHandler = AppCore.Instance.Get<IExceptionHandler>();
                var appStateScheduler = AppCore.Instance.Get<ISchedulerProvider>();
                var winFactory = AppCore.Instance.Get<WindowFactory>();

                app.DispatcherUnhandledException += exceptionHandler.Handle;

                AppDomain.CurrentDomain.UnhandledException += exceptionHandler.Handle;

                afterInit?.Invoke();

                var mainWindow = winFactory.CreateWindow<TUserControl, TViewModel>(true).Result;
       
                if (null != splashScreen) DXSplashScreen.Close();

                app.Run();


            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                app.Shutdown();
            }

            finally
            {
            }

        }
    }
}
