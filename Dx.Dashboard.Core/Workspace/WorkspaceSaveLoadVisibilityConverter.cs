using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Dx.Dashboard.Core
{
    public class WorkspaceSaveLoadVisibilityConverter : FrameworkElement, IValueConverter
    {
        public IWorkspaceState Workspace
        {
            get { return (IWorkspaceState)GetValue(WorkspaceProperty); }
            set { SetValue(WorkspaceProperty, value); }
        }
        public static readonly DependencyProperty WorkspaceProperty =
            DependencyProperty.Register("Workspace", typeof(IWorkspaceState), typeof(WorkspaceSaveLoadVisibilityConverter));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value) return true;

            var layout = value as WorkspaceLayout;

            return true;;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
