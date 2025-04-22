using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using EVerse.Navisworks.ViewpointByLevel.Plugin.Utils;
using EVerse.Navisworks.ViewpointByLevel.Common.Application;
using System;
using System.IO;
using System.Linq;
using EVerse.Navisworks.ViewpointByLevel.Plugin.Windows;
using System.Globalization;

namespace EVerse.Navisworks.ViewpointByLevel.Plugin
{
    [Plugin("VPByLevel", IdentityInformation.DeveloperID, ToolTip = "Viewpoint by Level", DisplayName = "Veronica")]
    public class ViewpointByLevelPlugin : CustomPlugin
    {
        public int Execute(params string[] parameters)
        {
            Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            View aView = oDoc.ActiveView;

            GridSystemCollection gSystems = oDoc.Grids.Systems;
            Tools.GridSystems gs = new Tools.GridSystems(gSystems);
            Tools.ModelUnits mu = new Tools.ModelUnits(oDoc);
            
            if (gs.Models.Count == 0)
            {
                MessageWindow.Show("Alert", "Please load a Revit model first");
                return 0;
            }

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
                    double offset = Tools.CutOffset;
                    double recomputedOffset = (double)ComputeOffsetValue(mu, offset);

                    //Model Origin
                    double originZ = gSystems[Tools.SelectedSystem].Origin.Z;

                    foreach (GridLevel level in gSystems[Tools.SelectedSystem].Levels)
                    {
                        double elevation = -originZ - level.Elevation - recomputedOffset;
                        string elevationString = jview((elevation).ToString(CultureInfo.InvariantCulture));
                        viewpointByLevelWindow.CreateViewpoint(elevationString, level.DisplayName, aView);
                    }
                    t.Commit();
                }
                MessageWindow.Show("Success", $"{gSystems[Tools.SelectedSystem].Levels.Count} viewpoints created succesfully");
            }
            return 0;
        }

        private static double ComputeOffsetValue(Tools.ModelUnits mu, double offset)
        {
            // get the selected units
            Enum.TryParse(Tools.SelectedUnits.ToString(), out Units selectedUnits);
            // get the model units
            Units modelUnits = mu.units.Values.First();
            // calculate the scale factor between two scales
            double scaleFactor = UnitConversion.ScaleFactor(modelUnits, selectedUnits);
            // calculate the new offset value
            double computedOffset = offset / scaleFactor;
            return computedOffset;
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