using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dx.Dashboard.Core
{
    public class SerializedLayout
    {
        private String _name;
        public String Name
        {
            get
            {
                return _name;
            }
        }

        public String LayoutOrientation { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public String LayoutType { get; set; }
        public List<SerializedLayout> Children { get; set; }
        public String Parent { get; set; }
        public SerializedWidget Widget { get; set; }
        public bool IsActive { get; private set; }

        public SerializedLayout FindByName(String name)
        {
            if (Name == name) return this;

            foreach (var child in Children)
            {
                var result = child.FindByName(name);
                if (null != result) return result;
            }

            return null;
        }

        public static SerializedLayout Create(IEnumerable<XElement> elements)
        {

            var main = elements.FirstOrDefault(x => x.Elements().Any(y => y.GetAttribute("name") == "ParentName" && y.Value == string.Empty));

            var name = main.GetAttribute("Name");
            var orientation = main.GetAttribute("Orientation");

            if (main == null) return null;

            return new SerializedLayout(name, orientation, elements);

        }

        public SerializedLayout(String name, String orientation, IEnumerable<XElement> elements)
        {
            _name = name;
            LayoutOrientation = orientation;
            Children = GetChildren(elements).ToList();
        }

        private IEnumerable<SerializedLayout> GetChildren(IEnumerable<XElement> elements)
        {

            var children = elements.Where(x => x.Elements().Any(y => y.GetAttribute("name") == "ParentName" && y.Value == this.Name));

            return children
                .Select(child =>
                {
                    var width = child.GetAttribute("ItemWidth");
                    var height = child.GetAttribute("ItemHeight");
                    var name = child.GetAttribute("Name");
                    var type = child.GetAttribute("TypeName");
                    var orientation = child.GetAttribute("Orientation");
                    var isActive = child.GetAttribute("IsActive");

                    var layout = new SerializedLayout(name, orientation, elements)
                    {
                        Width = width == null || width == "*" ? 0.0 : double.Parse(width.Replace("*", ""), CultureInfo.InvariantCulture),
                        Height = height == null || height == "*" ? 0.0 : double.Parse(height.Replace("*", ""), CultureInfo.InvariantCulture),
                        Parent = this.Name,
                        IsActive = isActive == null ? false : Boolean.Parse(isActive),
                        LayoutType = type
                    };

                    return layout;

                });
        }
    }

}
