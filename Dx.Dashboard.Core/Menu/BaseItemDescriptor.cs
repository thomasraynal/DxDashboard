using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public abstract class BaseItemDescriptor : ReactiveObject, IMenuItemDescriptor
    {
        private string _caption;
        private ImageSource _glyph;

        public BaseItemDescriptor(string caption = "", ImageSource glyph= null)
        {
            _caption = caption;
            _glyph = glyph;
            IsEnabled = true;
        }

        public String Caption => _caption;
        public ImageSource Glyph => _glyph;
    
        public bool _isEnable;
        public bool IsEnabled
        {
            get { return _isEnable; }
            set { this.RaiseAndSetIfChanged(ref _isEnable, value); }
        }
    }
}
