using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Layout.Core;
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
    public class WindowFactory
    {
        private List<DXWindow> _windowsRegistry;

        public WindowFactory()
        {
            _windowsRegistry = new List<DXWindow>();
        }

        private DXWindow CreateWindowInternal<TUserControl>(String title = "", WindowStyle style = WindowStyle.SingleBorderWindow, double height = 750, double width = 1200, double minHeight = 750, double minWidth = 1200) where TUserControl : UserControl
        {
            var control = (UserControl)Activator.CreateInstance(typeof(TUserControl));

            var window = new DXWindow()
            {
                MinWidth = minWidth,
                MinHeight = minHeight,
                Width = width,
                Height = height,
                Margin = new Thickness(0, 0, 0, 0),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Maximized,
                ShowIcon = false,
                Title = title,
                WindowStyle = style,
            };
         
            window.Content = control;

            return window;
        }
        private async Task<TViewModel> HandleCreateWindow<TUserControl, TViewModel>(DXWindow window, bool isModal = false) where TUserControl : UserControl where TViewModel : IViewModel
        {
            var model = AppCore.Instance.Get<TViewModel>();

            await model.Initialize();

            model.Host = window;

            window.DataContext = null;
            window.DataContext = model;

            window.Closing += (sender, e) =>
            {
                var win = (DXWindow)sender;
                var todispose = win.DataContext as IDisposable;
                todispose?.Dispose();
                _windowsRegistry.Remove(win);
            };

            window.Show();
            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
            window.Focus();

            _windowsRegistry.Add(window);

            return model;

        }
        public async Task<TViewModel> CreateWindow<TUserControl, TViewModel>(bool singleton, string title = "", WindowStyle style = WindowStyle.SingleBorderWindow, bool closeOnOutsideClick = false) where TUserControl : UserControl where TViewModel : IViewModel
        {
            var match = _windowsRegistry.FirstOrDefault(win => win.Content is TUserControl);

            var window = (singleton) ? match ?? CreateWindowInternal<TUserControl>(title, style) : CreateWindowInternal<TUserControl>(title, style);

            return await HandleCreateWindow<TUserControl, TViewModel>(window);

        }


    }
}
