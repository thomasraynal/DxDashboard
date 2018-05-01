using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Test
{
    public class TestDashboard : BaseDashboard<TestWorkspaceState>
    {

        public TestDashboard()
        {
        }

        public override TestWorkspaceState GetCurrentState()
        {
            return new TestWorkspaceState(CurrentWorkspace.State.Name, CurrentWorkspace.State.Tag);
        }

    }
}
