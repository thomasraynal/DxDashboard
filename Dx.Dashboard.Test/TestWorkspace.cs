using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dx.Dashboard.Cache;

namespace Dx.Dashboard.Test
{
    public class TestWorkspace : BaseWorkspace<TestWorkspaceState>
    {
        public TestWorkspace(ICacheStrategy<WorkspaceLayout> cacheStrategy, IDashboard<TestWorkspaceState> host, bool loadLayout = true) : base(cacheStrategy, host, loadLayout)
        {
        }

        protected override Task InitializeWorkspaceInternal()
        {
            return Task.CompletedTask;
        }
    }
}
