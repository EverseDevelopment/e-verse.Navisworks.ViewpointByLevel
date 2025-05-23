﻿using Caliburn.Micro;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using WixSharp;
using WixSharp.UI.Forms;
using WixSharp.UI.WPF;
using IO = System.IO;

namespace EVerse.Navisworks.ViewpointByLevel.Installer
{
    /// <summary>
    /// The standard ExitDialog.
    /// <para>Follows the design of the canonical Caliburn.Micro View (MVVM).</para>
    /// <para>See https://caliburnmicro.com/documentation/cheat-sheet</para>
    /// </summary>
    /// <seealso cref="WixSharp.UI.WPF.WpfDialog" />
    /// <seealso cref="WixSharp.IWpfDialog" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ExitDialog : WpfDialog, IWpfDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitDialog"/> class.
        /// </summary>
        public ExitDialog()
        {
            InitializeComponent();
            DialogDescription.TextInput += DialogDescription_TextInput;
        }

        private void DialogDescription_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {  
            string text = e.Text;
            MessageBox.Show(text, text);
        }

        private System.Windows.Point _dragStartPoint;
        private bool _isDragging;

        /// <summary>
        /// This method is invoked by WixSHarp runtime when the custom dialog content is internally fully initialized.
        /// This is a convenient place to do further initialization activities (e.g. localization).
        /// </summary>
        public void Init()
        {
            UpdateTitles(ManagedFormHost.Runtime.Session);

            var container = ManagedFormHost;
            var parent = container.Parent as Form;
            parent.FormBorderStyle = FormBorderStyle.None;
            PreviewMouseLeftButtonDown += (s, e) =>
            {
                _dragStartPoint = e.GetPosition(this);
                _isDragging = true;
            };

            PreviewMouseLeftButtonUp += (s, e) => _isDragging = false;
            PreviewMouseMove += (s, e) =>
            {
                if (!_isDragging)
                    return;

                System.Windows.Point currentPoint = e.GetPosition(this);
                double deltaX = currentPoint.X - _dragStartPoint.X;
                double deltaY = currentPoint.Y - _dragStartPoint.Y;

                parent.Left += Convert.ToInt32(deltaX);
                parent.Top += Convert.ToInt32(deltaY);
            };
            container.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e3df");

            parent.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, parent.Width, parent.Height, 20, 20));
                var host = new ExitDialogModel { Host = ManagedFormHost };
                ViewModelBinder.Bind(host, this, null);

            if (DialogDescription.Text == "Click the Finish button to exit the Setup Wizard.")
            {
                DialogDescription.Text = "Congratulations! The process is complete. " +
                    "\n Thanks for trusting in our heroine Veronica! " +
                    "\n \n Are you interested in taking part in our next \n " +
                    "projects?";
            }
            else
            {

            }

        }

            [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
            private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,      // x-coordinate of upper-left corner
            int nTopRect,       // y-coordinate of upper-left corner
            int nRightRect,     // x-coordinate of lower-right corner
            int nBottomRect,    // y-coordinate of lower-right corner
            int nWidthEllipse,  // height of ellipse
            int nHeightEllipse  // width of ellipse
            );

            /// <summary>
            /// Updates the titles of the dialog depending on the success of the installation action.
            /// </summary>
            /// <param name="session">The session.</param>
            public void UpdateTitles(ISession session)
        {
            if (Shell.UserInterrupted || Shell.Log.Contains("User canceled installation."))
            {
                DialogDescription.Text = "Hola1";
                //DialogDescription.Text = "[UserExitDescription1]";
            }
            else if (Shell.ErrorDetected)
            {
                DialogDescription.Text = "Hola";
                //DialogDescription.Text = Shell.CustomErrorDescription ?? "[FatalErrorDescription1]";
            }

            // `Localize` resolves [...] titles and descriptions into the localized strings stored in MSI resources tables
            this.Localize();
        }

        private void Title_Link(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://e-verse.com/");
        }

        private void Contact_Link(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://e-verse.com/contact/");
        }
    }

    /// <summary>
    /// ViewModel for standard ExitDialog.
    /// <para>Follows the design of the canonical Caliburn.Micro ViewModel (MVVM).</para>
    /// <para>See https://caliburnmicro.com/documentation/cheat-sheet</para>
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    internal class ExitDialogModel : Caliburn.Micro.Screen
    {
        public ManagedForm Host { get; set; }
        ISession session => Host?.Runtime.Session;
        IManagedUIShell shell => Host?.Shell;

        public BitmapImage Banner => session?.GetResourceBitmap("WixUI_Bmp_Dialog").ToImageSource();

        public void GoExit()
            => shell?.Exit();

        public void Cancel()
            => shell?.Exit();

        public void ViewLog()
        {
            if (shell != null)
                try
                {
                    string wixSharpDir = Path.GetTempPath().PathCombine("WixSharp");
                    if (!Directory.Exists(wixSharpDir))
                        Directory.CreateDirectory(wixSharpDir);

                    string logFile = wixSharpDir.PathCombine(Host.Runtime.ProductName + ".log");
                    IO.File.WriteAllText(logFile, shell.Log);

                    Process.Start(logFile);
                }
                catch
                {
                    // Catch all, we don't want the installer to crash in an
                    // attempt to view the log.
                }
        }
    }
}