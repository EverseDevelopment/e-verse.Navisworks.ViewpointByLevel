﻿using Autodesk.Navisworks.Api.Plugins;
using EVerse.Navisworks.ViewpointByLevel.Common.Application;

namespace EVerse.Navisworks.ViewpointByLevel.Common.Utils
{
    class PluginBuilder
    {
        public string pluginId { get; set; }
        public string directoryName { get; set; }
        public string fileName { get; set; }
        public PluginRecord pluginRecord { get; set; }

        public PluginBuilder(string pluginName)
        {
            pluginId = string.Concat( pluginName, ".",IdentityInformation.DeveloperID);
            pluginRecord = Autodesk.Navisworks.Api.Application.Plugins.FindPlugin(pluginId);
        }
    }
}
