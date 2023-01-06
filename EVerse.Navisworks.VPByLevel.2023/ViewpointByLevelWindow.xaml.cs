using EVerse.Navisworks.Plugin.ViewpointByLevel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EVerse.Navisworks.Plugin
{
    /// <summary>
    /// Interaction logic for ViewpointByLevelWindow.xaml
    /// </summary>
    public partial class ViewpointByLevelWindow : Window
    {
        public ViewpointByLevelWindow()
        {
            InitializeComponent();
        }
        public void FillModels(Tools.GridSystems gs)
        {

            //modelsNames.Items.AddRange(gs.Models.ToArray());
        }
    }
}
