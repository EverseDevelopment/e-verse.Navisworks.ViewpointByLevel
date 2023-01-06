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
            var AView = oDoc.ActiveView;

            GridSystemCollection GSystems = oDoc.Grids.Systems;
            Tools.GridSystems gs = new Tools.GridSystems(GSystems);

            ViewpointByLevelWindow thisForm = new ViewpointByLevelWindow();

            thisForm.FillModels(gs);
            thisForm.ShowDialog();
            //Exit plugin if user clicks cancel
            if (thisForm.DialogResult == false)
            { return 0; }

            //If user selects Apply create viewpoints
            if (thisForm.DialogResult == true)
            {
                using (Transaction t = new Transaction(oDoc, "Cutting Planes"))
                {
                    //Offset for level
                    double offset = Tools.CutOffset;
                    //Model Origin
                    var originZ = GSystems.FirstOrDefault().Origin.Z;

                    foreach (var level in GSystems[Tools.SelectedSystem].Levels)
                    {
                        var elev = jview((-originZ - level.Elevation - offset).ToString());

                        AView.TrySetClippingPlanes(elev);

                        Console.Write(elev + Environment.NewLine);
                        var newViewpoint = new SavedViewpoint(oDoc.CurrentViewpoint);
                        newViewpoint.DisplayName = level.DisplayName;
                        oDoc.SavedViewpoints.AddCopy(newViewpoint);
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