using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using app = Autodesk.Navisworks.Api.Application;

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

        public static void createViewpoints(Document doc)
        {
            var view = doc.ActiveView;

            string text =
            @"{ ""Type"": ""ClipPlaneSet"",
	                ""Version"": 1,
	                ""Planes"": [{
		                ""Type"": ""ClipPlane"",
		                ""Version"": 1,
		                ""Normal"": [0, 0, -1],
		                ""Distance"": {1},
		                ""Enabled"": true
	                    }, {
		                    ""Type"": ""ClipPlane"",
		                    ""Version"": 1,
		                    ""Normal"": [0, 0, 1],
		                    ""Distance"": {0},
		                    ""Enabled"": true
	                    }],
	                    ""Linked"": false,
	                    ""Enabled"": true
                    }";

            //ugly as shit
            if (SelectedLevels.Count == 2)
            {
                var clip = text.Replace("{0}", SelectedLevels[0].ToString()).Replace("{1}", SelectedLevels[1].ToString());
                view.SetClippingPlanes(clip);
                var viewp = view.CreateViewpointCopy();
                viewp.CreateCopy();
            }

        }

    }

}
