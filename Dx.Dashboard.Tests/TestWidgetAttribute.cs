using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Tests
{
    public abstract class TestWidgetAttribute : WidgetAttribute
    {
        public override WidgetCategory Category => TestConstants.TestCategory;

        public TestWidgetAttribute(string name, string glyph = null) : base(name, glyph)
        {
        }
    }

    public class TestWidgetOneAttribute : WidgetAttribute
    {
        public override WidgetCategory Category => TestConstants.TestCategory;

        public TestWidgetOneAttribute() : base("I am Two", null)
        {
        }
    }

    public class TestWidgetTwoAttribute : WidgetAttribute
    {
        public override WidgetCategory Category => TestConstants.TestCategory;

        public TestWidgetTwoAttribute() : base("I am Two", null)
        {
        }
    }
}
