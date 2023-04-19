using Autodesk.Navisworks.Api.Plugins;
using System.Diagnostics;
using System;

namespace EVerse.Navisworks.ViewpointByLevel.Common.Application
{
    class CommonCommandHandlerPlugin : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters) { return 0; }
        public override bool TryShowCommandHelp(string name)
        {
            Redirect(name);
            return true;
        }
        static void Redirect(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to redirect: " + ex.Message);
            }
        }
    }
}