using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Dx.Dashboard.Core
{
    public abstract class WidgetAttribute : Attribute
    {
        public String Name { get; private set; }

        public abstract WidgetCategory Category { get; }

        public BitmapImage Glyph { get; private set; }

        public WidgetAttribute(String name, String glyph = null)
        {
            Name = name;
            Glyph = DevExpressHelper.GetGlyph(glyph);
        }
    }
}
