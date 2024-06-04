/* 
    Copyright (C) 2024 The Aridity Team, All rights reserved

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using BetaFortressTeam.BetaFortressClient.Util;
using System.Runtime.CompilerServices;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static async void InstallBetaFortress()
        {
            string progressOutput = null;
            MainWindow form = new MainWindow();

            form.pBar.Visibility = Visibility.Visible;
            form.lblStatus.Visibility = Visibility.Visible;
            
            form.lblStatus.Content = "Preparing for installation...";
            form.pBar.IsIndeterminate = true;

            var gitProgress = new ProgressHandler((serverProgressOutput) =>
            {
                // Print output to console
                Console.Write(serverProgressOutput);

                progressOutput = $"{serverProgressOutput}";

                form.lblStatus.Content = progressOutput;
                return true;
            });

            if(!Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                CloneOptions cloneOptions = new CloneOptions();
                cloneOptions.FetchOptions.OnTransferProgress = MainWindow.TransferProgress;
                cloneOptions.FetchOptions.Depth = 1;
                cloneOptions.FetchOptions.OnProgress = gitProgress;
                form.pBar.IsIndeterminate = false;

                var x = await Task.Run(() => Repository.Clone("https://github.com/Beta-Fortress-2-Team/bf.git", Steam.GetSourceModsPath + "/bf", cloneOptions));

                if(SetupManager.HasMissingModFiles())
                {
                    MessageBox.Show("We have detected that there are missing files during the installation!!\nDo you want to reinstall now?",
                        "Beta Fortress Client - Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                }

                form.pBar.Visibility = Visibility.Hidden;
                form.lblStatus.Visibility = Visibility.Hidden;
            
                form.lblStatus.Content = "Done!";
                form.pBar.IsIndeterminate = false;
            }
        }
        
        public static void UpdateBetaFortress()
        {
            if(Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                if(SetupManager.HasMissingModFiles())
                {
                    MessageBoxResult result = MessageBox.Show("We have detected that there are missing files!!\nDo you want to reinstall now?",
                        "Beta Fortress Client - Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if(result != MessageBoxResult.Cancel)
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

        public static bool TransferProgress(TransferProgress progress)
        {
            MainWindow window = new MainWindow();
            Console.WriteLine($"Objects: {progress.ReceivedObjects} of {progress.TotalObjects}, Bytes: {progress.ReceivedBytes}");
            window.lblStatus.Content = $"Objects: {progress.ReceivedObjects} of {progress.TotalObjects}, Bytes: {progress.ReceivedBytes}";
            window.pBar.Maximum = ((int)progress.TotalObjects);
            window.pBar.Value = ((int)progress.ReceivedObjects);
            return true;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if(!ModManager.IsModInstalled)
            {
                this.btnInstallnUpdate.Content = "Install";
            }
            else
            {
                this.btnInstallnUpdate.Content = "Update";
            }

            this.pBar.Visibility = Visibility.Hidden;
            this.lblStatus.Visibility = Visibility.Hidden;
        }

        private void Close_Click(object sender, RoutedEventArgs e) 
        {
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) 
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            this.btnInstallnUpdate.IsEnabled = false;
            this.btnSettings.IsEnabled = false;

            Storyboard s = (Storyboard)this.FindResource("openSettings");
            s.Begin();

            this.btnInstallnUpdate.IsEnabled = true;
            this.btnSettings.IsEnabled = true;
        }

        private void btnInstallnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(!ModManager.IsModInstalled)
            {
                InstallBetaFortress();
            }
            else
            {
                UpdateBetaFortress();
            }
        }

        private void btnCloseSettings_Click(object sender, RoutedEventArgs e)
        {
            this.btnInstallnUpdate.IsEnabled = false;
            this.btnSettings.IsEnabled = false;

            Storyboard s = (Storyboard)this.FindResource("closeSettings");
            s.Begin();

            this.btnInstallnUpdate.IsEnabled = true;
            this.btnSettings.IsEnabled = true;
        }

        private void btnEditModCfg_Click(object sender, RoutedEventArgs e)
        {
            TextEditorWindow editor = new TextEditorWindow();
            editor.ShowDialog();
        }

        private void btnUninstall_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Before uninstalling, Beta Fortress Client requires elevated/admin privileges.\nContinue?", 
                "Beta Fortress Client", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if(result == MessageBoxResult.Yes) 
            {
                var path = string.Format("{0}/BetaFortressClient.exe", System.Windows.Forms.Application.StartupPath);
                using (var process = Process.Start(new ProcessStartInfo(path)
                {
                    Verb = "runas",
                    UseShellExecute = true,
                    Arguments = "/console /doNotAllocConsole /uninstall",
                    WindowStyle = ProcessWindowStyle.Hidden
                }))
                {
                    process.WaitForExit();
                }
            }
        }

        private void btnRecovery_Click(object sender, RoutedEventArgs e)
        {
            RecoveryToolForm tool = new RecoveryToolForm();
            tool.ShowDialog();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(Steam.IsAppInstalled(243750) && Steam.IsAppInstalled(440) && !SetupManager.HasMissingModFiles())
            {
                Steam.RunApp(243750, "-game " + ModManager.GetModPath + " " + this.launchOptions.Text);
            }
            else
            {
                MessageBox.Show("Cannot launch Beta Fortress!\n" +
                    "You can't launch Beta Fortress because of the following reasons:\n" +
                    "1. Team Fortress 2 and Source SDK Base 2013 Multiplayer isn't installed.\n" +
                    "2. Your current installation has missing files.\n", "Beta Fortress Client",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
