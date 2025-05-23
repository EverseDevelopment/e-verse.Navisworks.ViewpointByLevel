using Caliburn.Micro;
using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.UI.Forms;
using WixSharp.UI.WPF;

namespace EVerse.Navisworks.ViewpointByLevel.Installer
{
    /// <summary>
    /// The standard ProgressDialog.
    /// <para>Follows the design of the canonical Caliburn.Micro View (MVVM).</para>
    /// <para>See https://caliburnmicro.com/documentation/cheat-sheet</para>
    /// </summary>
    /// <seealso cref="WixSharp.UI.WPF.WpfDialog" />
    /// <seealso cref="WixSharp.IWpfDialog" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ProgressDialog : WpfDialog, IWpfDialog, IProgressDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog" /> class.
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
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

            var view = parent as IShellView;
            view?.SetSize(510, 424);

            container.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            parent.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

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


            model = new ProgressDialogModel { Host = ManagedFormHost };
            ViewModelBinder.Bind(model, this, null);

            model.StartExecute();
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
        /// Updates the titles of the dialog depending on what type of installation action MSI is performing.
        /// </summary>
        /// <param name="session">The session.</param>
        public void UpdateTitles(ISession session)
        {
            //if (session.IsUninstalling())
            //{
            //    DialogTitleLabel.Text = "[ProgressDlgTitleRemoving]";
            //    DialogDescription.Text = "[ProgressDlgTextRemoving]";
            //}
            //else if (session.IsRepairing())
            //{
            //    DialogTitleLabel.Text = "[ProgressDlgTextRepairing]";
            //    DialogDescription.Text = "[ProgressDlgTitleRepairing]";
            //}
            //else if (session.IsInstalling())
            //{
            //    DialogTitleLabel.Text = "[ProgressDlgTitleInstalling]";
            //    DialogDescription.Text = "[ProgressDlgTextInstalling]";
            //}

            // `Localize` resolves [...] titles and descriptions into the localized strings stored in MSI resources tables
            this.Localize();
        }

        ProgressDialogModel model;

        /// <summary>
        /// Processes information and progress messages sent to the user interface.
        /// <para> This method directly mapped to the
        /// <see cref="T:Microsoft.Deployment.WindowsInstaller.IEmbeddedUI.ProcessMessage" />.</para>
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageRecord">The message record.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="defaultButton">The default button.</param>
        /// <returns></returns>
        public override MessageResult ProcessMessage(InstallMessage messageType, Record messageRecord, MessageButtons buttons, MessageIcon icon, MessageDefaultButton defaultButton)
            => model?.ProcessMessage(messageType, messageRecord, "Finish!") ?? MessageResult.None;

        /// <summary>
        /// Called when MSI execution is complete.
        /// </summary>
        public override void OnExecuteComplete()
            => model?.OnExecuteComplete();

        /// <summary>
        /// Called when MSI execution progress is changed.
        /// </summary>
        /// <param name="progressPercentage">The progress percentage.</param>
        public override void OnProgress(int progressPercentage)
        {
            if (model != null)
                model.ProgressValue = progressPercentage;
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
    /// ViewModel for standard ProgressDialog.
    /// <para>Follows the design of the canonical Caliburn.Micro ViewModel (MVVM).</para>
    /// <para>See https://caliburnmicro.com/documentation/cheat-sheet</para>
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    internal class ProgressDialogModel : Caliburn.Micro.Screen
    {
        public ManagedForm Host;

        ISession session => Host?.Runtime.Session;
        IManagedUIShell shell => Host?.Shell;

        public BitmapImage Banner => session?.GetResourceBitmap("WixUI_Bmp_Banner").ToImageSource();

        public bool UacPromptIsVisible => (!WindowsIdentity.GetCurrent().IsAdmin() && Uac.IsEnabled() && !uacPromptActioned);

        public string CurrentAction { get => currentAction; set { currentAction = value; base.NotifyOfPropertyChange(() => CurrentAction); } }
        public int ProgressValue { get => progressValue; set { progressValue = value; base.NotifyOfPropertyChange(() => ProgressValue); } }

        bool uacPromptActioned = false;
        string currentAction;
        int progressValue;

        public string UacPrompt
        {
            get
            {
                if (Uac.IsEnabled())
                {
                    var prompt = session?.Property("UAC_WARNING");
                    if (prompt.IsNotEmpty())
                        return prompt;
                    else
                        return
                            "Please wait for UAC prompt to appear. " +
                            "If it appears minimized then activate it from the taskbar.";
                }
                else
                    return null;
            }
        }

        public void StartExecute()
            => shell?.StartExecute();

        public void Cancel()
        {
            if (shell.IsDemoMode)
                shell.GoNext();
            else
                shell.Cancel();
        }

        public MessageResult ProcessMessage(InstallMessage messageType, Record messageRecord, string currentStatus)
        {
            switch (messageType)
            {
                case InstallMessage.InstallStart:
                case InstallMessage.InstallEnd:
                    {
                        uacPromptActioned = true;
                        base.NotifyOfPropertyChange(() => UacPromptIsVisible);
                    }
                    break;

                case InstallMessage.ActionStart:
                    {
                        try
                        {
                            //messageRecord[0] - is reserved for FormatString value

                            /*
                            messageRecord[2] unconditionally contains the string to display

                            Examples:

                               messageRecord[0]    "Action 23:14:50: [1]. [2]"
                               messageRecord[1]    "InstallFiles"
                               messageRecord[2]    "Copying new files"
                               messageRecord[3]    "File: [1],  Directory: [9],  Size: [6]"

                               messageRecord[0]    "Action 23:15:21: [1]. [2]"
                               messageRecord[1]    "RegisterUser"
                               messageRecord[2]    "Registering user"
                               messageRecord[3]    "[1]"

                            */

                            if (messageRecord.FieldCount >= 3)
                                CurrentAction = messageRecord[2].ToString();
                            else
                                CurrentAction = null;
                        }
                        catch
                        {
                            //Catch all, we don't want the installer to crash in an attempt to process message.
                        }
                    }
                    break;
            }
            return MessageResult.OK;
        }

        public void OnExecuteComplete()
        {
            CurrentAction = null;
            shell?.GoNext();
        }
    }
}