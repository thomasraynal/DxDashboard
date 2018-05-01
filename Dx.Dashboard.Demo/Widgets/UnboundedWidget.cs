using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public abstract class UnboundedWidget : WorkspaceAwareWidgetAttribute
    {
        public override abstract WidgetCategory Category { get; }

        public UnboundedWidget(string name, string glyph = null) : base(name, glyph)
        {
        }

        public override bool IsVisible(DemoWorkspaceState state)
        {
            return true;
        }
    }
}
