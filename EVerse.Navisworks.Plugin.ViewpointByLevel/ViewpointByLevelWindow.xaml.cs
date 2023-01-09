using Autodesk.Navisworks.Api;
using EVerse.Navisworks.Plugin.ViewpointByLevel.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        private void Apply_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.CutOffset = Convert.ToDouble(textBox.Text);
            }
            catch
            {
                MessageBox.Show("Error with offset" + Environment.NewLine + "Using 1");
                Tools.CutOffset = 1;
            }
            this.DialogResult = true;
        }

        public void FillModels(Tools.GridSystems gs)
        {
            foreach (string model in gs.Models)
            {
                modelsNames.Items.Add(model);
            }
        }

        private void Model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tools.SelectedSystem = modelsNames.SelectedIndex;
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void CreateViewpoint(string elev, string displayName, View aView)
        {
            Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            aView.TrySetClippingPlanes(elev);
            Console.Write(elev + Environment.NewLine);
            SavedViewpoint newViewpoint = new SavedViewpoint(oDoc.CurrentViewpoint);
            newViewpoint.DisplayName = displayName;
            oDoc.SavedViewpoints.AddCopy(newViewpoint);
        }
    }
}