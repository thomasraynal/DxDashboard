using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Test
{
    [TestWidgetOne]
    public class TestWidget1 : TestWidgetBase<Void>
    {
        public TestWidget1(TestWorkspace host, string cacheID = null) : base(host, cacheID)
        {
        }
    }
}
