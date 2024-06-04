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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Principal;

using BetaFortressTeam.BetaFortressClient.Util;
using System.Reflection;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    public partial class SetupForm : Form
    {
        private static bool IsElevated()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public SetupForm()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            switch(this.tabControl1.SelectedIndex)
            {
                case 0:
                    this.tabControl1.SelectedIndex = 1;
                    break;
                case 1:
                    this.tabControl1.SelectedIndex = 2;
                    break;
                case 2:
                    SetupManager.HasCompletedSetup = true;
                    SetupManager.IsRunningSetup = false;
                    if(IsElevated())
                    {
                        try
                        {
                            RegistryKey bfClientKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PracticeMedicine\\BFClient");
                            if(bfClientKey != null)
                            {
                                try
                                {
                                    bfClientKey.SetValue("FirstRun", "1");
                                    SetupManager.FirstRun = false;
                                }
                                catch(Exception) // happens if the registry was null
                                {
                                }
                            }
                        }
                        catch (Exception)
                        {
                            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PracticeMedicine\\BFClient");
                            if(key == null)
                            {
                                key.CreateSubKey("Properties");
                            }
                        }
                        Application.Restart();
                        //Process.Start(Assembly.GetExecutingAssembly().Location);
                        Application.Exit(); // make sure it is closed
                    }
                    break;
            }

            if(this.tabControl1.SelectedIndex == 0)
            {
                this.tabControl1.SelectedIndex = 1;
            }
            else if (this.tabControl1.SelectedIndex == 1)
            {
                this.tabControl1.SelectedIndex = 2;
            }
            else if(this.tabControl1.SelectedIndex == 2)
            {
                SetupManager.HasCompletedSetup = true;
                SetupManager.IsRunningSetup = false;

                if(IsElevated())
                {
                    try
                    {
                        RegistryKey bfClientKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PracticeMedicine\\BFClient");
                        if(bfClientKey != null)
                        {
                            try
                            {
                                bfClientKey.SetValue("FirstRun", "1");
                                SetupManager.FirstRun = false;
                            }
                            catch(Exception) // happens if the registry was null
                            {
                            }
                        }
                    }
                    catch (Exception)
                    {
                        RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PracticeMedicine\\BFClient");
                        if(key == null)
                        {
                           key.CreateSubKey("Properties");
                        }
                    }

                    Application.Restart();
                    //Process.Start(Assembly.GetExecutingAssembly().Location);
                    Application.Exit(); // make sure it is closed
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if(this.tabControl1.SelectedIndex == 1)
            {
                this.tabControl1.SelectedIndex = 0;
            }
            else if (this.tabControl1.SelectedIndex == 2)
            {
                this.tabControl1.SelectedIndex = 1;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.tabControl1.SelectedIndex == 0)
            {
                this.btnBack.Enabled = false;
            }
            else if (this.tabControl1.SelectedIndex == 1)
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = false;
            }
            else if ( tabControl1.SelectedIndex == 2)
            {
                this.btnBack.Enabled = false;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "Select your Steam installation directory";
            ofd.ShowNewFolderButton = false;
            ofd.ShowDialog();
        }

        private void steamDirectoryText_TextChanged(object sender, EventArgs e)
        {
            if(Directory.Exists(this.steamDirectoryText.Text))
            {
                this.btnNext.Enabled = true;
            }
            else
            {
                this.btnNext.Enabled = false;
            }
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            if(Directory.Exists(Steam.GetSteamPath))
            {
                this.steamDirectoryText.Text = Steam.GetSteamPath;
            }
        }
    }
}
