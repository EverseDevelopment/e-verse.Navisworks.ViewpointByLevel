using Autodesk.Navisworks.Api.Plugins;

namespace EVerse.Navisworks.Plugin.Common.Application
{
    class CommonCommandHandlerPlugin : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters) { return 0; }
    }
}
