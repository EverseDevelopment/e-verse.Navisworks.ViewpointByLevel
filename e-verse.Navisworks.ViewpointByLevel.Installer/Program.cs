﻿using EVerse.Navisworks.ViewpointByLevel.Plugin.Windows;
using System;
using WixSharp;
using WixSharp.Nsis;

namespace EVerse.Navisworks.ViewpointByLevel.Installer
{
    internal class Program
    {
        private static void Main()
        {
            var project = new ManagedProject("e-verse.Navisworks.ViewpointByLevel",
                new Dir(@"%AppData%\Autodesk\ApplicationPlugins",
                new Dir(@"Veronica.ViewpointByLevel.bundle",
                new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\PackageContents.xml"),
                new Dir(@"Contents", new Dir(@"dlls",

                    new Dir(@"2018",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2018\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*"))),

                    new Dir(@"2019",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2019\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*"))),

                    new Dir(@"2020",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2020\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*"))),

                    new Dir(@"2021",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2021\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*"))),

                    new Dir(@"2022",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2022\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*"))),

                    new Dir(@"2023",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2023\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*"))),

                    new Dir(@"2024",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2024\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*"))),

                    new Dir(@"2026",
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.2026\bin\Release\Veronica.ViewpointByLevel.dll"),
                        new File(@"..\e-verse.Navisworks.ViewpointByLevel.Plugin\ClipPlaneTemplate.json"),
                        new Dir(@"en-US",
                            new File(@"..\e-verse.Navisworks.ViewpointByLevel.Common\en-US\PluginRibbon.xaml")),
                        new Dir(@"Images",
                            new Files(@"..\e-verse.Navisworks.ViewpointByLevel.Common\Images\*.*")))
                )))));

            project.GUID = new Guid("949A974F-2B58-40FE-BA53-0C74D5870368");
            project.Version = new Version(ViewpointByLevelWindow.PRODUCT_VERSION);

            project.ControlPanelInfo.Manufacturer = "e-verse";

            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.WelcomeDialog>()
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.LicenceDialog>()
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ProgressDialog>()
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ExitDialog>();

            project.ManagedUI.ModifyDialogs
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.MaintenanceTypeDialog>()
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.FeaturesDialog>()
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ProgressDialog>()
                .Add<EVerse.Navisworks.ViewpointByLevel.Installer.ExitDialog>();

            var msiFile = project.BuildMsi();

            var bootstrapper = new NsisBootstrapper
            {
                Primary = { FileName = msiFile },
                OutputFile = "Veronica.ViewpointByLevel.exe",
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
        }
    }
}
