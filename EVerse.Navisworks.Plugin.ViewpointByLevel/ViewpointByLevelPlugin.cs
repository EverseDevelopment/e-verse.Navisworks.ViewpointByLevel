using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using EVerse.Navisworks.Plugin.Common.Application;
using EVerse.Navisworks.Plugin.ViewpointByLevel.Utils;
using System;
using System.Linq;

namespace EVerse.Navisworks.Plugin.ViewpointByLevel
{
    [PluginAttribute("VPByLevel", IdentityInformation.DeveloperID, ToolTip = "Viewpoints by Level", DisplayName = "Viewpoints by Level")]
    //[AddInPluginAttribute(AddInLocation.None)]
    public class ViewpointByLevelPlugin : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            View aView = oDoc.ActiveView;

            GridSystemCollection gSystems = oDoc.Grids.Systems;
            Tools.GridSystems gs = new Tools.GridSystems(gSystems);

            ViewpointByLevelWindow viewpointByLevelWindow = new ViewpointByLevelWindow();

            viewpointByLevelWindow.FillModels(gs);
            viewpointByLevelWindow.ShowDialog();
            //Exit plugin if user clicks cancel
            if (viewpointByLevelWindow.DialogResult == false)
            { return 0; }

            //If user selects Apply create viewpoints
            if (viewpointByLevelWindow.DialogResult == true)
            {
                using (Transaction t = new Transaction(oDoc, "Cutting Planes"))
                {
                    //Offset for level
                    double offset = Tools.CutOffset;
                    //Model Origin
                    double originZ = gSystems.FirstOrDefault().Origin.Z;

                    foreach (GridLevel level in gSystems[Tools.SelectedSystem].Levels)
                    {
                        string elev = jview((-originZ - level.Elevation - offset).ToString());
                        viewpointByLevelWindow.CreateViewpoint(elev, level.DisplayName, aView);
                    }
                    t.Commit();
                }
            }
            return 0;
        }
        private static string jview(string elevation)
        {
            var output = (@"{""Type"":""ClipPlaneSet"",""Version"":1,""Planes"":[{""Type"":""ClipPlane"",""Version"":1,""Normal"":[-0,-0,-1],""Distance"":{0},""Enabled"":true}],""Linked"":false,""Enabled"":true}").Replace("{0}", elevation);

            return output;
        }
    }
}