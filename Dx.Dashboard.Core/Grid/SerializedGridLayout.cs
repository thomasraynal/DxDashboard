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
    public class SerializedGridLayout
    {
        public List<String> Layout { get; set; }
        public List<ColumnSettingDescriptor> Mappings { get; set; }

        public static SerializedGridLayout GetLayout(byte[] layout)
        {

            var file = Path.GetTempFileName();

            using (var fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                var memoryStrema = new MemoryStream(layout);
                memoryStrema.CopyTo(fileStream);
            }

            var gridLayout = XElement.Load(file);

            var columns = gridLayout.XPathSelectElement("//*[@name='Columns']");

            if (null != columns)
            {

                var elements = gridLayout.XPathSelectElement("//*[@name='Columns']").Elements().Where(x =>
                {
                    var visibility = x.Elements().FirstOrDefault(z => z.Attribute("name").Value == "Visible");
                    return ((visibility == null || visibility.Value == "true"));
                }).OrderBy(x =>
                {
                    var position = x.Elements().FirstOrDefault(z => z.Attribute("name").Value == "VisibleIndex").Value;
                    return double.Parse(position);

                });

                var results = elements
                    .Select(x =>
                    {
                        return x.Elements().First(z => z.Attribute("name").Value == "FieldName").Value;

                    }).Where(x => null != x).ToList();


                //mappings
                var settings = gridLayout.XPathSelectElement("//*[@name='ColumnEditSettingDescriptors']");
                List<ColumnSettingDescriptor> settingDescriptor = null;
                if (null != settings)
                {

                    settingDescriptor = settings.Elements().Select(x =>
                    {
                        var childs = x.Elements();

                        if (null == childs) return null;

                        var fieldName = childs.FirstOrDefault(z => z.Attribute("name").Value == "FieldName");
                        var settingsMaskType = childs.FirstOrDefault(z => z.Attribute("name").Value == "SettingsMaskType");
                        var settingsMask = childs.FirstOrDefault(z => z.Attribute("name").Value == "SettingsMask");

                        return new ColumnSettingDescriptor() { FieldName = fieldName.Value, SettingsMaskType = settingsMaskType.Value, SettingsMask = settingsMask.Value };

                    }).Where(x => x != null).ToList();

                }


                return new SerializedGridLayout()
                {
                    Layout = results.ToList(),
                    Mappings = settingDescriptor
                };
            };

            return null;
        }
    }

}
