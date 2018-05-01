using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    [Serializable]
    public class GridLayoutItem
    {
        public String WidgetId { get; set; }
        public byte[] Layout { get; set; }
        public long GridLayoutId { get; set; }
    }
}
