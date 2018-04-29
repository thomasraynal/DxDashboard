using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dx.Dashboard.Core
{
    public static class XElementExtension
    {
        public static String GetAttribute(this XElement elt, String name)
        {
            var attr = elt.Attributes().Where(x => x.Name.LocalName == name);
            if (attr.Any())
                return attr.First().Value;

            return null;
        }

        public static String GetChild(this XElement elt, String name)
        {
            var attr = elt.Elements().Where(x => x.Name.LocalName == name);
            if (attr.Any())
                return attr.First().Value;

            return null;
        }

        public static String GetChild(this XElement elt, Func<XElement, bool> predicate)
        {
            var attr = elt.Elements().FirstOrDefault(predicate);
            return null != attr ? attr.Value : null;
        }

    }
}
