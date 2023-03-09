using EVerse.Navisworks.ViewpointByLevel.Plugin.Windows;
using System;
using System.IO;
using WixSharp;
using WixSharp.Nsis;

namespace EVerse.Navisworks.ViewpointByLevel.Installer
{
    internal class Program
    {
        private static void Main()
        {
            // Get the directory path of the solution
            // Get the directory path of the current executable
            var exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Get the solution directory path by finding the first .sln file in the parent directories
            var solutionDirectory = Directory.GetParent(exeDirectory).Parent.Parent.FullName;

            // Get the folder path by combining the solution directory path with the folder name
            var sourceFolder = Path.Combine(solutionDirectory, "e-verse.Navisworks.ViewpointByLevel.bundle");

            var files = Files.FromBuildDir(@sourceFolder, ".dll|.xml|.json|.jpg|.png|.xaml|.addin");
            files.Attributes.Add("Component:SharedDllRefCount", "yes");

            var destinationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Autodesk", "ApplicationPlugins");

            // Create the project
            var project = new ManagedProject("e-verse.Navisworks.ViewpointByLevel", new Dir(@destinationDirectory, new Dir(@"e-verse.Navisworks.ViewpointByLevel.bundle", files)))
            {
                GUID = new Guid("CA920446-C503-493F-8B3D-714340249796"),
                Version = new Version(ViewpointByLevelWindow.PRODUCT_VERSION),
            };

            project.ControlPanelInfo.Manufacturer = "e-verse";

            // project.ManagedUI = ManagedUI.DefaultWpf; // all stock UI dialogs

            //custom set of UI WPF dialogs
            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs.Add<EVerse.Navisworks.ViewpointByLevel.Installer.WelcomeDialog>()
                                            .Add<EVerse.Navisworks.ViewpointByLevel.Installer.LicenceDialog>()
                                            .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ProgressDialog>()
                                            .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ExitDialog>();

            project.ManagedUI.ModifyDialogs.Add<EVerse.Navisworks.ViewpointByLevel.Installer.MaintenanceTypeDialog>()
                                           .Add<EVerse.Navisworks.ViewpointByLevel.Installer.FeaturesDialog>()
                                           .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ProgressDialog>()
                                           .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ExitDialog>();

            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            var msiFile = project.BuildMsi();

            var bootstrapper = new NsisBootstrapper
            {
                Primary = { FileName = msiFile },

                OutputFile = "e-verse.Navisworks.ViewpointByLevel.exe",
                IconFile = "Resources\\logo.ico",

                VersionInfo = new VersionInformation("1.0.0.0")
                {
                    ProductName = "Test Application",
                    LegalCopyright = "Copyright Test company",
                    FileDescription = "Test Application",
                    FileVersion = "1.0.0",
                    CompanyName = "Test company",
                    InternalName = "setup.exe",
                    OriginalFilename = "setup.exe"
                },
            };

            bootstrapper.Build();

            //#endif
        }
    }
}