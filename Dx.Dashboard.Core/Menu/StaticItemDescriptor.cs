using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public class StaticItemDescriptor : BaseItemDescriptor
    {
        public StaticItemDescriptor(string caption, ImageSource glyph = null) : base(caption, glyph)
        {
        }
    }
}
