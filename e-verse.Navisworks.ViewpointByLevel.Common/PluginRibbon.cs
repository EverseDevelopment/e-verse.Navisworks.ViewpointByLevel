using Autodesk.Navisworks.Api.Plugins;
using EVerse.Navisworks.ViewpointByLevel.Common.Application;
using EVerse.Navisworks.ViewpointByLevel.Common.Utils;
using EVerse.Navisworks.ViewpointByLevel.Plugin;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace EVerse.Navisworks.ViewpointByLevel.Common
{
    [Plugin("VPByLevelRibbon", IdentityInformation.DeveloperID, DisplayName ="Viewpoint by Level")]
    [RibbonLayout("PluginRibbon.xaml")]
    [RibbonTab("VPByLevel")]
    [Command("ID_Button_1", LargeIcon ="VL_32.png", ToolTip="Viewpoint by level", DisplayName ="Viewpoint by Level")]
    class PluginRibbon : CommonCommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginFileName = directoryName + $"\\{Assembly.GetExecutingAssembly().GetName().Name}.dll";
            Autodesk.Navisworks.Api.Application.Plugins.AddPluginAssembly(pluginFileName);
            switch (name)
            {
                case "ID_Button_1":
                    try
                    {
                        if (!Autodesk.Navisworks.Api.Application.IsAutomated)
                        {
                            PluginBuilder pluginBuilder = new PluginBuilder("VPByLevel");
                            if (pluginBuilder.pluginRecord is CustomPluginRecord && pluginBuilder.pluginRecord.IsEnabled)
                            {
                                ViewpointByLevelPlugin vpByLvlPlugin = (ViewpointByLevelPlugin)(pluginBuilder.pluginRecord.LoadedPlugin ?? pluginBuilder.pluginRecord.LoadPlugin());
                                vpByLvlPlugin.Execute();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ups, something went wrong" + Environment.NewLine + ex.Message);
                    }
                    break;
            }
            return 0;
        }
    }
}