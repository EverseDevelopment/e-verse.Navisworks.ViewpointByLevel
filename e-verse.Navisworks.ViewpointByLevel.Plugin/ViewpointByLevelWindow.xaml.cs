using Autodesk.Navisworks.Api;
using EVerse.Navisworks.Plugin.ViewpointByLevel.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static EVerse.Navisworks.Plugin.ViewpointByLevel.Utils.Tools;

namespace EVerse.Navisworks.ViewpointByLevel.Plugin
{
    /// <summary>
    /// Interaction logic for ViewpointByLevelWindow.xaml
    /// </summary>
    public partial class ViewpointByLevelWindow : Window
    {
        private const string NO_REVIT_MODEL_MESSAGE = "No revit model available";
        private const string SELECT_REVIT_MODEL_MESSAGE = "Select a revit model";
        private int SelectedUnits { get; set; }

        public ViewpointByLevelWindow()
        {
            InitializeComponent();
        }

        private void Apply_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.CutOffset = Convert.ToDouble(textBox.Text);
                Tools.SelectedUnits = (UnitsEnum)SelectedUnits;
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
            if (gs.Models.Count > 0)
            {
                foreach (string model in gs.Models)
                {
                    modelsNames.Items.Add(model);
                }
                OffOn(true, SELECT_REVIT_MODEL_MESSAGE, Colors.Gray);
            }
            else OffOn(false, NO_REVIT_MODEL_MESSAGE, Colors.Red);
        }
        public void FillUnits()
        {
            modelUnits.Items.Add(Units.Meters.ToString());
            modelUnits.Items.Add(Units.Feet.ToString());
            modelUnits.Items.Add(Units.Inches.ToString());

        }
        private void OffOn(bool toggle, string message, System.Windows.Media.Color color)
        {
            modelsNames.IsEnabled = toggle;
            modelsNames.IsHitTestVisible = toggle;
            applyButton.IsEnabled = toggle;
            textBox.IsEnabled = toggle;
            notificationField.Content = message;
            notificationField.Foreground = new SolidColorBrush(color);
        }
        private void Model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tools.SelectedSystem = modelsNames.SelectedIndex;
        }
        private void Units_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUnits = modelUnits.SelectedIndex;
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