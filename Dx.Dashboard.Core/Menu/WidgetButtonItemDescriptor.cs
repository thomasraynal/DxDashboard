using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public class WidgetButtonItemDescriptor : ButtonItemDescriptor
    {
        private WidgetAttribute _widget;

        public WidgetButtonItemDescriptor(string caption, WidgetAttribute widget, ImageSource glyph = null) : base(caption, glyph)
        {
            _widget = widget;
        }

        public WidgetAttribute Widget
        {
            get
            {
                return _widget;
            }
        }

    }
}
