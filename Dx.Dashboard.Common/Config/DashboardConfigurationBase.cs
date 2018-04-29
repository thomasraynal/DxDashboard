using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Dx.Dashboard.Common.Configuration
{
    public abstract class DashboardConfigurationBase : IDashboardConfiguration
    {
        public abstract StringDictionary KeyValueStore { get; }
        public abstract string ApplicationName { get; }
        public abstract StringDictionary GetWidgetConfiguration(string widgetName);
        public abstract String GetWidgetConfigurationKey(string widgetName, String key);
        public abstract List<string> GetWidgetConfigurationItemArray(string widgetName, String key);
        public abstract List<String> GetConfigurationItemArray(String key);
        
    }
}
