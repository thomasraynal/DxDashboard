using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public class ListItemDescriptor : BaseItemDescriptor
    {
        private DataTemplateSelector _selector;

        public ListItemDescriptor(string caption, ImageSource glyph = null) : base(caption, glyph)
        {
            Items = new List<IMenuItemDescriptor>();
            _selector = new MenuItemTemplateSelector();
        }

        public DataTemplateSelector Selector
        {
            get
            {
                return _selector;
            }
        }

        private IEnumerable<IMenuItemDescriptor> InternalGetChildrenMenuItems(IMenuItemDescriptor item, Func<IMenuItemDescriptor, bool> predicate)
        {
            if (item is ListItemDescriptor) return
                    (item as ListItemDescriptor).Items.Select(menuItem => InternalGetChildrenMenuItems(menuItem, predicate))
                    .Aggregate((enum1, enum2) => enum1.Concat(enum2));

            if(predicate(item)) return new[] { item };

            return Enumerable.Empty<IMenuItemDescriptor>();
        }

        public IEnumerable<IMenuItemDescriptor> GetChildrenMenuItems(Func<IMenuItemDescriptor, bool> predicate)
        {
            if (null == predicate) predicate = (item) => true;

            var results = Items.Select(item => InternalGetChildrenMenuItems(item, predicate));

            if (results.Count() == 0) return Enumerable.Empty<IMenuItemDescriptor>();

            return results.Aggregate((enum1, enum2) => enum1.Concat(enum2));
        }

        public List<IMenuItemDescriptor> Items { get; set; }
    }
}
