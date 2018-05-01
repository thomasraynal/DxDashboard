using Dx.Dashboard.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Reflection;

namespace Dx.Dashboard.Test
{
    public class TestConfiguration : IDashboardConfiguration
    {
        public string ApplicationName => TestConstants.ApplicationName;

        public StringDictionary KeyValueStore => throw new NotImplementedException();

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public List<string> GetConfigurationItemArray(string key)
        {
            return null;
        }

        public StringDictionary GetWidgetConfiguration(string widgetName)
        {
            return null;
        }

        public List<string> GetWidgetConfigurationItemArray(string widgetName, string key)
        {
            return null;
        }

        public string GetWidgetConfigurationKey(string widgetName, string key)
        {
            return null;
        }
    }
}
