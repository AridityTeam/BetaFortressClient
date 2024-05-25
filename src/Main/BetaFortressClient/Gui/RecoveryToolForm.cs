/* 
    Copyright (C) 2024 The Beta Fortress Team, All rights reserved

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LibGit2Sharp;
using BetaFortressTeam.BetaFortressClient.Util;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    public partial class RecoveryToolForm : Form
    {
        public RecoveryToolForm()
        {
            InitializeComponent();
        }

        private void RecoveryToolForm_Load(object sender, EventArgs e)
        {
            this.btnNext.Enabled = false;
            this.btnBack.Enabled = false;

            if(!Directory.Exists(ModManager.GetModPath))
            {
                MessageBox.Show("You cannot use the Recovery Tool if you don't have the mod installed.",
                    "Beta Fortress Client", MessageBoxButtons.OK);
                this.Close();
                //return;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
            {
                tabControl1.SelectedIndex = 1;
            }
            else if(tabControl1.SelectedIndex == 1)
            {
                tabControl1.SelectedIndex = 2;
            }
            else if(tabControl1.SelectedIndex == 2)
            {
                tabControl1.SelectedIndex = 3;

                DialogResult result = MessageBox.Show("Before resetting, Beta Fortress Client requires elevated admin privileges.\nContinue?", 
                "Beta Fortress Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(result == DialogResult.Yes) 
                {
                    Console.WriteLine("[ BFCLIENT ] Uninstalling Beta Fortress...");
                    this.label9.Text = "Uninstalling Beta Fortress...";
                    var path = string.Format("{0}/BFClientFileHandler.exe", Application.StartupPath);
                    using (var process = Process.Start(new ProcessStartInfo(path)
                    {
                        Verb = "runas",
                        Arguments = "/doNotAllocConsole /uninstall",
                        CreateNoWindow = true
                    }))
                    {
                        process.WaitForExit();
                    }

                    if (!Directory.Exists(Steam.GetSourceModsPath + "/bf"))
                    {
                        this.label9.Text = "Installing Beta Fortress...";
                        CloneOptions cloneOptions = new CloneOptions();
                        //cloneOptions.FetchOptions.OnTransferProgress = gitTransferProgress;
                        cloneOptions.FetchOptions.Depth = 1;
                        //cloneOptions.FetchOptions.OnProgress = gitProgress;
                        Repository.Clone("https://github.com/Beta-Fortress-2-Team/bf.git", Steam.GetSourceModsPath + "/bf", cloneOptions);
                    }
                    else
                    {
                        this.label9.Text = "Cancelling operation due to an error occured";

                        MessageBox.Show("The mod directory still exists.\n" +
                            "Git cannot clone into non-empty directories\n" +
                            "Cancelling operation...", "Beta Fortress Client", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        this.tabControl1.SelectedIndex = 5;
                    }
                }
            }
            else if(tabControl1.SelectedIndex == 5)
            {
                Application.Restart();

                // make sure the app exits
                Application.Exit();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 2)
            {
                tabControl1.SelectedIndex = 1;
            }
            else if(tabControl1.SelectedIndex == 1)
            {
                tabControl1.SelectedIndex = 0;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
            {
                this.btnBack.Enabled = false;
                this.btnNext.Enabled = true;
            }
            else if(tabControl1.SelectedIndex == 1)
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = false;
            }
            else if(tabControl1.SelectedIndex == 2)
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = true;
            }
            else if(tabControl1.SelectedIndex == 3)
            {
                this.btnBack.Enabled = true;
                this.btnNext.Enabled = true;
            }
            else if(tabControl1.SelectedIndex == 4)
            {
                this.btnBack.Enabled = false;
                this.btnNext.Enabled = false;
            }
            else if(tabControl1.SelectedIndex == 5)
            {
                this.btnNext.Text = "Restart";

                this.btnBack.Enabled = false;
                this.btnNext.Enabled = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBox2.Checked)
            {
                this.btnNext.Enabled = true;
            }
            else
            {
                this.btnNext.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBox1.Checked)
            {
                this.btnNext.Enabled = true;
            }
            else
            {
                this.btnNext.Enabled = false;
            }
        }
    }
}
