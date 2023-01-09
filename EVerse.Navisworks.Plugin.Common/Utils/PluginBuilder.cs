using Autodesk.Navisworks.Api.Plugins;
using EVerse.Navisworks.Plugin.Common.Application;

namespace EVerse.Navisworks.Plugin.Common.Utils
{
    class PluginBuilder
    {
        public string pluginId { get; set; }
        public string directoryName { get; set; }
        public string fileName { get; set; }
        public PluginRecord pluginRecord { get; set; }

        public PluginBuilder(string pluginName)
        {
            //pluginId = pluginName + "." + IdentityInformation.DeveloperID;
            //directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //fileName = directoryName + $"\\plugins\\{Assembly.GetExecutingAssembly().GetName().Name}.{pluginName}.dll";
            //Autodesk.Navisworks.Api.Application.Plugins.AddPluginAssembly(fileName);
            //pluginRecord = Autodesk.Navisworks.Api.Application.Plugins.FindPlugin(pluginId);

            pluginId = string.Concat( pluginName, ".",IdentityInformation.DeveloperID);
            pluginRecord = Autodesk.Navisworks.Api.Application.Plugins.FindPlugin(pluginId);
        }
    }
}
