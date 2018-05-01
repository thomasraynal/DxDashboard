using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public abstract class WorkspaceAwareWidgetAttribute : WidgetAttribute
    {
        public override abstract WidgetCategory Category { get; }

        public WorkspaceAwareWidgetAttribute(string name, string glyph = null) : base(name, glyph)
        {
        }

        public abstract bool IsVisible(DemoWorkspaceState state);
    }
}
