using Autodesk.Navisworks.Api.Plugins;
using EVerse.Navisworks.Plugin.Common.Application;
using EVerse.Navisworks.Plugin.Common.Utils;
using EVerse.Navisworks.Plugin.ViewpointByLevel;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace EVerse.Navisworks.Plugin.Common
{
    [Plugin("VPByLevelRibbon", "EVS", DisplayName ="Viewpoint by Level")]
    [RibbonLayout("PluginRibbon.xaml")]
    [RibbonTab("Viewpoint by Level")]
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