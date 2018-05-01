using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Dx.Dashboard.Demo
{
    public static class DashboardDemoHelper
    {

        public static BitmapFrame GetIconAsFrame()
        {
            try
            {
                var uri = new Uri("icon.ico", UriKind.Relative);
                var ico = BitmapFrame.Create(uri);
                return ico;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
