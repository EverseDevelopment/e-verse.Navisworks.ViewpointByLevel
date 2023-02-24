using Autodesk.Navisworks.Api;
using EVerse.Navisworks.ViewpointByLevel.Common;
using EVerse.Navisworks.ViewpointByLevel.Plugin.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static EVerse.Navisworks.ViewpointByLevel.Plugin.Utils.Tools;

namespace EVerse.Navisworks.ViewpointByLevel.Plugin.Windows
{
    /// <summary>
    /// Interaction logic for ViewpointByLevelWindow.xaml
    /// </summary>
    public partial class ViewpointByLevelWindow : Window
    {
        private const string NO_REVIT_MODEL_MESSAGE = "No revit model available";
        private const string SELECT_REVIT_MODEL_MESSAGE = "Select a revit model";
        //private const string ADDIN_IMAGE_PATH = "Images\\VL_32.jpg";
        private const string HEART_IMAGE_PATH = "Images\\Heart.jpg";

        private int SelectedUnits { get; set; }

        public ViewpointByLevelWindow()
        {
            InitializeComponent();
        }

        private void LoadImage(Image image, string imagePath)
        {
            string commonProjectDirectory = System.IO.Path.GetDirectoryName(typeof(PluginRibbon).Assembly.Location);
            string fullPath = System.IO.Path.Combine(commonProjectDirectory, imagePath);
            Uri uri = new Uri(fullPath);
            image.Source = new BitmapImage(uri);
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
                textBox.Text = "0";
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
        private void FinDisclaimerButtonChildImage(object sender, RoutedEventArgs e)
        {
            Button disclaimerButton = sender as Button;
            if (disclaimerButton != null)
            {
                Image heartImage = disclaimerButton.Template.FindName("heartImage", disclaimerButton) as Image;
                if (heartImage != null)
                {
                    LoadImage(heartImage, HEART_IMAGE_PATH);
                }
            }
        }
        private void OffOn(bool toggle, string message, System.Windows.Media.Color color)
        {
            modelsNames.IsEnabled = toggle;
            modelsNames.IsHitTestVisible = toggle;
            modelUnits.IsEnabled = toggle;
            applyButton.IsEnabled = toggle;
            textBox.IsEnabled = toggle;
            notificationField.Content = message;
            notificationField.Foreground = new SolidColorBrush(color);
        }
        private void Model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tools.SelectedSystem = modelsNames.SelectedIndex;
            OffOn(true, "", Colors.Gray);
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
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Title_Link(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://e-verse.com/");
        }

    }
}