using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    [Serializable]
    public class ColumnSettingDescriptor
    {
        public String FieldName { get; set; }
        public String SettingsMaskType { get; set; }
        public String SettingsMask { get; set; }

    }
}
