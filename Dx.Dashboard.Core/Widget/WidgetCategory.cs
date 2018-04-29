using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public class WidgetCategory
    {
        private string _path;
        private ImageSource _glyph;

        public WidgetCategory(String path, ImageSource glyph)
        {
            _path = path;
            _glyph = glyph;
        }

        public String Path => _path;
        public ImageSource Glyph => _glyph;
    }
}
