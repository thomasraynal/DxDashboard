using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Reflection;
using Akavache;
using Dx.Dashboard.Common.Configuration;
using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace Dx.Dashboard.Demo
{
    public class DemoConfiguration : IDashboardConfiguration
    {
        public DemoConfiguration() : base()
        {
            BlobCache.ApplicationName = ApplicationName;
        }

   
        public string ApplicationName => "DEMO DASHBOARD";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;


        private const String configFile = "config.xml";


        public List<String> GetConfigurationItemArray(String key)
        {
            return (KeyValueStore.ContainsKey(key)) ? KeyValueStore[key].Split(';').Select(x => x.Trim()).ToList() : null;
        }

        public StringDictionary GetWidgetConfiguration(String widgetName)
        {
            var config = GetConfigFile(configFile);
            var result = new StringDictionary();

            var element = config.Root.XPathSelectElement("//widgets").Elements().FirstOrDefault(x => x.Name.LocalName.ToLower() == widgetName.ToLower());

            if (null != element)
            {
                foreach (var item in element.Elements())
                {
                    result.Add(item.Name.LocalName, item.Value);
                }
            }

            return result;

        }

        public StringDictionary KeyValueStore
        {
            get
            {
                var config = GetConfigFile(configFile);

                var result = new StringDictionary();

                var elements = config.Root.XPathSelectElement("//configuration").Elements().ToList();
                elements.ForEach(x => result.Add(x.Name.ToString(), x.Value));

                return result;
            }
        }

        protected XDocument GetConfigFile(String key)
        {

            XDocument config;

            var path = TryFindFile(key);
            if (null == path) throw new Exception(String.Format("Unable to find configuration file {0}", key));

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var xr = XmlReader.Create(fs))
            {
                config = XDocument.Load(xr);
            }

            return config;
        }

        public String TryFindFile(String filename)
        {
            var path = string.Empty;

            //root
            if (File.Exists(filename))
            {
                path = filename;
            }
            //parent
            else if (File.Exists(Path.Combine("../", filename)))
            {
                path = Path.Combine("../", filename);
            }
            else
            {
                //try find in all children...
                path = FindFileRecursise(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename);

                if (String.IsNullOrEmpty(path))
                {
                    //give up
                    path = null;
                }
            }

            if (null != path)
            {
                var fileInfo = new FileInfo(path);
                path = fileInfo.FullName;
            }

            return path;

        }

        private String FindFileRecursise(String directory, String fileName)
        {
            var result = String.Empty;

            if (File.Exists(Path.Combine(directory, fileName))) return Path.Combine(directory, fileName);

            foreach (var dir in Directory.GetDirectories(directory))
            {
                result = FindFileRecursise(dir, fileName);
                if (!String.IsNullOrEmpty(result)) break;
            }

            return result;
        }

        public List<string> GetWidgetConfigurationItemArray(string widgetName, String key)
        {
            var widgetConfiguration = GetWidgetConfiguration(widgetName);
            if (null == widgetConfiguration) return null;

            return widgetConfiguration[key].Split(';').Select(x => x.Trim()).ToList();
        }

        public string GetWidgetConfigurationKey(string widgetName, string key)
        {
            var widgetConfiguration = GetWidgetConfiguration(widgetName);
            if (null == widgetConfiguration) return null;
            return widgetConfiguration[key];

        }
    }

}
 
