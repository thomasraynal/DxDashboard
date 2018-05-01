using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class WidgetStrategyChartAttribute : StrategyBoundedWidget
    {
        public override WidgetCategory Category
        {
            get
            {
                return WidgetCategoryProvider.StrategyChartCategory;
            }
        }

        public WidgetStrategyChartAttribute(string name, String glyph = null) : base(name, glyph)
        {
        }
    }
}
