using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public interface IDockItem : IDisposable
    {
        bool AllowGridSaveLayout { get; }
        string DockId { get; set; }

        List<GridLayoutItem> GridsLayout { get; set; }
        List<GridLayoutItem> PivotGridsLayout { get; set; }

        string ParentName { get; set; }
    }
}
