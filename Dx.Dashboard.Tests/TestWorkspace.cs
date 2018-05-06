using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dx.Dashboard.Cache;

namespace Dx.Dashboard.Tests
{
    public class TestWorkspace : BaseWorkspace<TestWorkspaceState>
    {
        public TestWorkspace(bool loadLayout = true) : base(loadLayout)
        {
        }

        protected override Task InitializeWorkspaceInternal()
        {
            return Task.CompletedTask;
        }
    }
}
