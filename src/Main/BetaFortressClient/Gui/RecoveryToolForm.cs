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
                    var path = string.Format("{0}/BFClientFileHandler.exe", Application.StartupPath);
                    using (var process = Process.Start(new ProcessStartInfo(path)
                    {
                        Verb = "runas",
                        Arguments = "/doNotAllocConsole /uninstall",
                        CreateNoWindow = true
                    }))
                    {
                        //process.WaitForExit();
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
