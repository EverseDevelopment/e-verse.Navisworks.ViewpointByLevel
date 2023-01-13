using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using EVerse.Navisworks.Plugin.ViewpointByLevel.Utils;
using EVerse.Navisworks.ViewpointByLevel.Common.Application;
using System;
using System.IO;
using System.Linq;

namespace EVerse.Navisworks.ViewpointByLevel.Plugin
{
    [Plugin("VPByLevel", IdentityInformation.DeveloperID, ToolTip = "Viewpoints by Level", DisplayName = "Viewpoints by Level")]
    public class ViewpointByLevelPlugin : CustomPlugin
    {
        public int Execute(params string[] parameters)
        {
            Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            View aView = oDoc.ActiveView;

            GridSystemCollection gSystems = oDoc.Grids.Systems;
            Tools.GridSystems gs = new Tools.GridSystems(gSystems);
            Tools.ModelUnits mu = new Tools.ModelUnits(oDoc);

            ViewpointByLevelWindow viewpointByLevelWindow = new ViewpointByLevelWindow();

            viewpointByLevelWindow.FillModels(gs);
            viewpointByLevelWindow.FillUnits();
            viewpointByLevelWindow.ShowDialog();
            //Exit plugin if user clicks cancel
            if (viewpointByLevelWindow.DialogResult == false)
            { return 0; }

            //If user selects Apply create viewpoints
            if (viewpointByLevelWindow.DialogResult == true)
            {
                using (Transaction t = new Transaction(oDoc, "Cutting Planes"))
                {
                    double offset = ComputeOffsetValue(mu);

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

        private static double ComputeOffsetValue(Tools.ModelUnits mu)
        {
            //Offset for level
            double offset = Tools.CutOffset;
            // get the selected units
            Enum.TryParse(Tools.SelectedUnits.ToString(), out Units selectedUnits);
            // get the model units
            Units modelUnits = mu.units.Values.First();
            // calculate the scale factor between two scales
            double scaleFactor = UnitConversion.ScaleFactor(modelUnits, selectedUnits);
            // calculate the new offset value
            return offset *= scaleFactor;
        }

        private const string _jsonFileName = "clipPlaneTemplate.json";
        private static string jview(string elevation)
        {
            string assemblyLocation = typeof(ViewpointByLevelPlugin).Assembly.Location;
            string clipPlaneTextFilePath = Path.Combine(Path.GetDirectoryName(assemblyLocation), _jsonFileName);
            string clipPlane = string.Empty;
            using (StreamReader reader = new StreamReader(clipPlaneTextFilePath))
            {
                clipPlane = reader.ReadToEnd();
            }
            string output = (clipPlane).Replace("[ 0 ]", elevation);

            return output;
        }
    }
}