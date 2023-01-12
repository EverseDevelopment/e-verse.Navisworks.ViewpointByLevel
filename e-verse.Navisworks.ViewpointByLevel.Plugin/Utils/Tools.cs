using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using app = Autodesk.Navisworks.Api.Application;
using System.IO;

namespace EVerse.Navisworks.Plugin.ViewpointByLevel.Utils
{

    public static class Tools
    {

        public static List<Grids> GridList { get; set; }

        public static List<double> SelectedLevels { get; set; }

        /// <summary>
        /// Integer from selected item at model list in form
        /// </summary>
        public static int SelectedSystem { get; set; }

        /// <summary>
        /// Class that stores a list of models, index will match the loop 
        /// </summary>
        public class GridSystems
        {
            public GridSystems(GridSystemCollection gsc)
            {
                Models = new List<string>();
                foreach (var item in gsc)
                {
                    Models.Add(item.DisplayName);
                }
            }

            public List<string> Models { get; set; }

        }
        public class ModelUnits
        {
            public List<Units> units;

            public ModelUnits(Document document)
            {
                units = new List<Units>();
                foreach (var item in document.Models)
                {
                    units.Add(item.Units);
                }
            }
        }
        public class Grids
        {
            public string name;
            public GridLevelCollection levels;
            public OrderedDictionary lev;
            public Grids(string n, GridLevelCollection l)
            {

                name = n;
                levels = l;
                lev = new OrderedDictionary();
                foreach (var item in levels)
                {
                    lev[item.DisplayName] = item.Elevation;

                }
            }

        }
        /// <summary>
        /// Saves the offset selected by the user
        /// </summary>
        public static double CutOffset { get; set; }

        public static void GridLevel(Document doc)
        {
            GridList = new List<Grids>();
            var gridSystem = doc.Grids.Systems;

            foreach (var system in gridSystem)
            {
                var g = new Grids(system.DisplayName, system.Levels);
                GridList.Add(g);
            }

        }
    }

}
