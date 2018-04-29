using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public class ButtonItemDescriptor : BaseItemDescriptor
    {

        public ButtonItemDescriptor(string caption, ImageSource glyph = null) : base(caption, glyph)
        {
        }

        public ReactiveCommand Command { get; set; }
    }
}
