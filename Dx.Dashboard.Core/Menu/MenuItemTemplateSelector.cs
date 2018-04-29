using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace Dx.Dashboard.Core
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var menuItemTemplate = new ResourceDictionary() { Source = new Uri("/Dx.Dashboard.Core;component/Menu/ItemTemplateDictionnary.xaml", UriKind.Relative) };

            if (item is StaticItemDescriptor) return (DataTemplate)menuItemTemplate["barStaticItemTemplate"];
            if (item is ButtonItemDescriptor) return (DataTemplate)menuItemTemplate["barButtonItemTemplate"];
            if (item is CheckButtonDescriptor) return (DataTemplate)menuItemTemplate["checkButtonItemTemplate"];
            if (item is SeparatorItemDescriptor) return (DataTemplate)menuItemTemplate["separatorItemTemplate"];
            if (item is ListItemDescriptor) return (DataTemplate)menuItemTemplate["listItemTemplate"];
            if (item is CustomItemDescriptor) return (DataTemplate)menuItemTemplate["customItemTemplate"];
            if (item is DateItemDescriptor) return (DataTemplate)menuItemTemplate["dateItemTemplate"];
            return null;
        }
    }
}
