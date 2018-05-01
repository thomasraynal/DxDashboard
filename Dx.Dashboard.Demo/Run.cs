using DevExpress.Xpf.Core;
using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dx.Dashboard.Demo
{
    public static class Run
    {
        [STAThread]
        public static void Main(string[] args)
        {
      
            DXGridDataController.DisableThreadingProblemsDetection = true;

            var app = new DashboardApp { ShutdownMode = ShutdownMode.OnLastWindowClose };

            app.InitializeComponent();

            Startup.Run<DashboardView, DashboardViewModel>(app);
        }
    }
}
