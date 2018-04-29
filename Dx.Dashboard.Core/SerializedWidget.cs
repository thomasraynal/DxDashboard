using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    [Serializable]
    public class SerializedWidget
    {
        public IEnumerable<SerializedGridLayout> SerializedGridLayouts { get; set; }
        public String Name { get; set; }
        public String Id { get; set; }
    }
}
