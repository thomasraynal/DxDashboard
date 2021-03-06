﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Common.Configuration
{
    public interface IDashboardConfiguration
    {
        String ApplicationName { get; }
        StringDictionary KeyValueStore { get; }
        Version Version { get; }
        StringDictionary GetWidgetConfiguration(String widgetName);
        String GetWidgetConfigurationKey(string widgetName, String key);
        List<String> GetWidgetConfigurationItemArray(string widgetName, String key);
        List<String> GetConfigurationItemArray(String key);
    }
}
