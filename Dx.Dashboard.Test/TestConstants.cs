using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Test
{
    public static class TestConstants
    {
        public static String ApplicationName = "TEST";
        public static String WidgetMenuName = "My Widgets";
        public static WidgetCategory TestCategory = new WidgetCategory("Features", DevExpressHelper.GetGlyph("ExportToXLS_16x16.png"));
    }
}