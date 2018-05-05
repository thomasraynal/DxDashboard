using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public class Widget
    {
        public String Name { get; set; }
        public String Categories { get; set; }
        public Type View { get; set; }
        public bool Hide { get; set; }

        public Widget(string categories, string name, Type view, bool hide = false)
        {
            View = view;
            Categories = categories;
            Hide = hide;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is Widget && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ((Categories == null) ? string.Empty.GetHashCode() : Categories.GetHashCode());
                hashCode = (hashCode * 397) ^ (View != null ? View.FullName.GetHashCode() : 0);
                return hashCode;
            }
        }

    }
}
