using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public abstract class BaseWorkspaceState : IWorkspaceState
    {
        public String Name { get; }
        public string Tag { get; }

        public abstract bool Equals(IWorkspaceState other);
    }
}
