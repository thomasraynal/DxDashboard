using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    [Serializable]
    public class WidgetDescriptor
    {
        public WidgetDescriptor()
        {
            GridsLayout = new List<GridLayoutItem>();
            PivotGridsLayout = new List<GridLayoutItem>();
        }

        public String DockId { get; set; }
        public String Parent { get; set; }
        public String Type { get; set; }
        public List<GridLayoutItem> PivotGridsLayout { get; set; }
        public List<GridLayoutItem> GridsLayout { get; set; }
        public string ParentName { get; set; }
        public bool IsFloating { get; set; }
        public string UniqueId { get; set; }
    }
}
