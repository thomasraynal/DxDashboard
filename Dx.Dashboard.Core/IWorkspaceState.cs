using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public interface IWorkspaceState : IEquatable<IWorkspaceState>
    {
        String Name { get; }
        String Tag { get; }
    }
}
