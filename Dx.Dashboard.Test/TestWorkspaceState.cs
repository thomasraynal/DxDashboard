using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Test
{
    public class TestWorkspaceState : IWorkspaceState
    {
        public TestWorkspaceState(String name, String tag)
        {
            Name = name;
            Tag = tag;
        }
        public string Name { get; private set; }

        public string Tag { get; private set; }

        public bool Equals(IWorkspaceState other)
        {
            return this.Name == other.Name;
        }
    }
}
