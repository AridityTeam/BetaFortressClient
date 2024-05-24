using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using BetaFortressTeam.BetaFortressClient.Util;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System.Threading;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    public partial class MainForm : Form
    {
        static string progressOutput = null;

        public static async void InstallBetaFortress()
        {
            var gitProgress = new ProgressHandler((serverProgressOutput) =>
            {
                MainForm form = new MainForm();

                // Print output to console
                Console.Write(serverProgressOutput);

                progressOutput = $"{serverProgressOutput}";

                form.lblStatus.Text = progressOutput;
                return true;
            });

            if(!Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                CloneOptions cloneOptions = new CloneOptions();
                //cloneOptions.FetchOptions.OnTransferProgress = MainForm.TransferProgress;
                cloneOptions.FetchOptions.Depth = 1;
                cloneOptions.FetchOptions.OnProgress = gitProgress;
                var x = await Task.Run(() => Repository.Clone("https://github.com/Beta-Fortress-2-Team/bf.git", Steam.GetSourceModsPath + "/bf", cloneOptions));

                if(SetupManager.HasMissingModFiles())
                {
                    MessageBox.Show("We have detected that there are missing files during the installation!!\nYou can reinstall by clicking the RETRY button",
                        "Beta Fortress Client - Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                }
            }
        }
        
        public static void UpdateBetaFortress()
        {
            if(Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                if(SetupManager.HasMissingModFiles())
                {
                    DialogResult result = MessageBox.Show("We have detected that there are missing files!!\nYou can reinstall by clicking the RETRY button",
                        "Beta Fortress Client - Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if(result != DialogResult.Cancel)
                    {
                        new RecoveryToolForm().ShowDialog();
                    }
                }

                PullOptions pullOptions = new PullOptions();
                pullOptions.FetchOptions.Depth = 1;

                RepositoryOptions repoOptions = new RepositoryOptions();
                repoOptions.WorkingDirectoryPath = Steam.GetSourceModsPath + "/bf";

                Repository repo = new Repository(Steam.GetSourceModsPath + "/bf", repoOptions);
                var sig = new Signature("Beta Fortress Client Git Credential", "aridityteam@gmail.com", new DateTimeOffset(DateTime.Now));
                Commands.Pull(repo, sig, pullOptions);
            }
        }

        // only run this with admin privileges
        public static void UninstallBetaFortress()
        {
            if(Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                Directory.Delete(Steam.GetSourceModsPath + "/bf", true);
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                if(!Steam.IsAppInstalled(243750))
                {
                    DialogResult result = MessageBox.Show("Beta Fortress Client detects that you do not have Source SDK Base 2013 Multiplayer installed in your machine\n" +
                        "By clicking on OK, BF Client will try to install the base game/tool into your machine.", "Beta Fortress Client - Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if(result == DialogResult.OK)
                    {
                        Process.Start("steam://install/243750");
                    }
                }
                else if (!Steam.IsAppInstalled(243750))
                {
                    DialogResult result = MessageBox.Show("Beta Fortress Client detects that you do not have Team Fortress 2 installed in your machine\n" +
                        "By clicking on OK, BF Client will try to install the base game/tool into your machine.", "Beta Fortress Client - Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if(result == DialogResult.OK)
                    {
                        Process.Start("steam://install/440");
                    }
                }
                else
                {
                    gitCloneWorker.RunWorkerAsync();
                }
            }
            else
            {
                gitPullWorker.RunWorkerAsync();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Ready!";

            if(!Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                this.btnInstall.Text = "Install";
                this.btnUninstall.Enabled = false;
            }
            else
            {
                this.btnInstall.Text = "Update";
                this.btnUninstall.Enabled = true;
            }
        }

        public static bool TransferProgress(TransferProgress progress)
        {
            Console.WriteLine($"Objects: {progress.ReceivedObjects} of {progress.TotalObjects}, Bytes: {progress.ReceivedBytes}");
            if(new MainForm().IsHandleCreated)
            {
                new MainForm().BeginInvoke(new Action(() =>
                {
                    new MainForm().lblStatus.Text = $"Objects: {progress.ReceivedObjects} of {progress.TotalObjects}, Bytes: {progress.ReceivedBytes}";
                    new MainForm().pBar.Style = ProgressBarStyle.Continuous;
                    new MainForm().pBar.Maximum = ((int)progress.TotalObjects);
                    new MainForm().pBar.Value = ((int)progress.ReceivedObjects);
                }));
            }
            return true;
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Before uninstalling, Beta Fortress Client requires elevated/admin privileges.\nContinue?", 
                "Beta Fortress Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if(result == DialogResult.Yes) 
            {
                var path = string.Format("{0}/BFClientFileHandler.exe", Application.StartupPath);
                using (var process = Process.Start(new ProcessStartInfo(path)
                {
                    Verb = "runas",
                    Arguments = "/doNotAllocConsole /uninstall"
                }))
                {
                    process.WaitForExit();
                }
            }
        }

        private void btnRecovery_Click(object sender, EventArgs e)
        {
            RecoveryToolForm recovery = new RecoveryToolForm();
            recovery.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void gitCloneWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // set the style Continuous
            this.pBar.Style = ProgressBarStyle.Blocks;

            var gitProgress = new ProgressHandler((serverProgressOutput) =>
            {
                // Print output to console
                Console.Write(serverProgressOutput);

                progressOutput = $"{serverProgressOutput}";

                this.lblStatus.Text = progressOutput;
                return true;
            });

            var gitTransferProgress = new TransferProgressHandler((serverTransferProgressOutput) =>
            {
                this.pBar.Maximum = serverTransferProgressOutput.TotalObjects;

                try
                {
                    this.gitCloneWorker.ReportProgress(serverTransferProgressOutput.ReceivedObjects);
                    this.Invoke(new Action(() => {
                        this.pBar.Value = serverTransferProgressOutput.ReceivedObjects;
                    }));
                }
                catch(Exception)
                {

                }

                this.lblStatus.Text = "Objects: " + serverTransferProgressOutput.ReceivedObjects + " of " +
                                        serverTransferProgressOutput.TotalObjects + " Received bytes of: " +
                                        serverTransferProgressOutput.ReceivedBytes;

                return true;
            });

            if (!Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                CloneOptions cloneOptions = new CloneOptions();
                cloneOptions.FetchOptions.OnTransferProgress = gitTransferProgress;
                cloneOptions.FetchOptions.Depth = 1;
                cloneOptions.FetchOptions.OnProgress = gitProgress;
                Repository.Clone("https://github.com/Beta-Fortress-2-Team/bf.git", Steam.GetSourceModsPath + "/bf", cloneOptions);
            }
         }

        private void gitCloneWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //this.pBar.Value = e.ProgressPercentage;
        }

        private void gitCloneWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Installation was canceled.");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("There was an error installing Beta Fortress\n" + e.Error,
                    "Beta Fortress Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (Directory.Exists(Steam.GetSourceModsPath + "/bf"))
                {
                    if (SetupManager.HasMissingModFiles())
                    {
                        MessageBox.Show("We have detected that there are missing files during the installation!!\nYou can reinstall by clicking the RETRY button",
                            "Beta Fortress Client - Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    }
                }
                else
                { 
                    MessageBox.Show("Successfully installed Beta Fortress!\n" +
                                    "The mod will appear after you've restarted the Steam client");
                }
            }
        }

        private void gitPullWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                if (SetupManager.HasMissingModFiles())
                {
                    DialogResult result = MessageBox.Show("We have detected that there are missing files!!\nYou can reinstall by clicking the RETRY button",
                        "Beta Fortress Client - Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result != DialogResult.Cancel)
                    {
                        new RecoveryToolForm().ShowDialog();
                    }
                }

                PullOptions pullOptions = new PullOptions();
                pullOptions.FetchOptions.Depth = 1;

                RepositoryOptions repoOptions = new RepositoryOptions();
                repoOptions.WorkingDirectoryPath = Steam.GetSourceModsPath + "/bf";

                Repository repo = new Repository(Steam.GetSourceModsPath + "/bf", repoOptions);
                var sig = new Signature("Beta Fortress Client Git Credential", "aridityteam@gmail.com", new DateTimeOffset(DateTime.Now));
                Commands.Pull(repo, sig, pullOptions);
            }
        }

        private void gitPullWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void gitPullWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                if (SetupManager.HasMissingModFiles())
                {
                    DialogResult result = MessageBox.Show("We have detected that there are missing files!!\nYou can reinstall by clicking the RETRY button",
                        "Beta Fortress Client - Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result != DialogResult.Cancel)
                    {
                        new RecoveryToolForm().ShowDialog();
                    }
                }
            }
        }
    }
}
