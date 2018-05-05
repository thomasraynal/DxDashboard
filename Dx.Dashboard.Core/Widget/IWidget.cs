using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dx.Dashboard.Core
{
    public interface IWidget: IDockItem
    {
        String ViewModelId { get; }
        String Name { get; }
        String Header { get; set; }
        UserControl View { get; }
        WidgetDescriptor GetDescriptor();
        Task Initialize();
    }
}
