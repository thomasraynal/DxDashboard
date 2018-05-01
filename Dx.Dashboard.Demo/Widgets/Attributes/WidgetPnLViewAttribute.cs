using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dx.Dashboard.Core;

namespace Dx.Dashboard.Demo
{
    public class WidgetPnLViewAttribute : UnboundedWidget
    {
        public override WidgetCategory Category
        {
            get
            {
                return WidgetCategoryProvider.PnLCategory;
            }
        }

        public WidgetPnLViewAttribute(string name, String glyph = null) : base(name, glyph)
        {
        }
    }
}
