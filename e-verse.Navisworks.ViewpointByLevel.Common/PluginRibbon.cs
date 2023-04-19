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
    [Plugin("VPByLevelRibbon", IdentityInformation.DeveloperID, DisplayName = "Viewpoint by Level")]
    [RibbonLayout("PluginRibbon.xaml")]
    [RibbonTab("VPByLevel")]
    [Command("VPByLevel", LargeIcon = "VL_32.jpg", ToolTip = "Create viewpoint by level\n\nVeronica is a Viewpoint by Level add-in for Autodesk® Navisworks®. It allows users to quickly and easily create viewpoints based on a selected level of a Revit model.", DisplayName = "Viewpoint by Level")]

    class PluginRibbon : CommonCommandHandlerPlugin
    {
        public const string VERONICA = "veronica";
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginFileName = directoryName + $"\\{Assembly.GetExecutingAssembly().GetName().Name}.dll";
            Autodesk.Navisworks.Api.Application.Plugins.AddPluginAssembly(pluginFileName);
            switch (name)
            {
                case "VPByLevel":
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

        public override bool TryShowCommandHelp(string name)
        {
            bool result = base.TryShowCommandHelp($"https://e-verse.com/{VERONICA}");
            return result;
        }
    }
}