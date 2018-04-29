using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public interface IMenuItemDescriptor
    {
        string Caption { get;  }
        ImageSource Glyph { get; }
        bool IsEnabled { get; set; }
    }
}