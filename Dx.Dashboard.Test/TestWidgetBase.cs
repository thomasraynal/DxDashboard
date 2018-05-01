using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dx.Dashboard.Test
{
    public abstract class TestWidgetBase<TUserControl> : WidgetViewModel<TUserControl, TestWorkspace, TestWorkspaceState>
         where TUserControl : UserControl, new()
    {
        public TestWidgetBase(TestWorkspace host, String cacheID = null) : base(host, cacheID)
        {
        }

    }
}
