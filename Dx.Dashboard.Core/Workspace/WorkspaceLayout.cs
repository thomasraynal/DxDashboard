using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Dx.Dashboard.Core
{

    [Serializable]
    public class WorkspaceLayout
    {

        public byte[] DockingLayout { get; set; }
        public List<WidgetDescriptor> Widgets { get; set; }
        public String Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(WorkspaceLayout) && (obj as WorkspaceLayout).GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            if (String.IsNullOrEmpty(Name))
            {
                return Int32.MinValue;
            }
            return Name.GetHashCode();
        }

        public SerializedLayout ToSerializedLayout()
        {
            if (null == DockingLayout) return null;

            var file = Path.GetTempFileName();

            using (var fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                var memoryStrema = new MemoryStream(DockingLayout);
                memoryStrema.CopyTo(fileStream);
            }

            var xmlLayout = XElement.Load(file);
            var elements = xmlLayout.XPathSelectElement("//*[@name='Items']").Elements();
            var serializedLayout = SerializedLayout.CreateLayoutTree(elements);

            if (null != Widgets)
            {
                foreach (var widget in Widgets)
                {
                    var target = serializedLayout.FindRecursiveByName(widget.ParentName);

                    //hidden widgets
                    if (null == target) continue;

                    target.Widget = new SerializedWidget()
                    {
                        Id = widget.ViewModelId,
                        SerializedGridLayouts = widget.GridsLayout.Any() ? widget.GridsLayout.Select(y => SerializedGridLayout.GetLayout(y.Layout)).ToList() : null
                    };
                }
            }

            return serializedLayout;

        }
    }
}
