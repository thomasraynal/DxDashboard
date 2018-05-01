using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Test
{
    [TestWidgetTwo]
    public class TestWidget2 : TestWidgetBase<Void>
    {
        public TestWidget2(TestWorkspace host, string cacheID = null) : base(host, cacheID)
        {
        }
    }
}
