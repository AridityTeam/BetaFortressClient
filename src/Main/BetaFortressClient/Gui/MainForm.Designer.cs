namespace BetaFortressTeam.BetaFortressClient.Gui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRecovery = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.btnInstall = new System.Windows.Forms.Button();
            this.gitCloneWorker = new System.ComponentModel.BackgroundWorker();
            this.gitPullWorker = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnRecovery);
            this.panel1.Controls.Add(this.btnUninstall);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnSettings);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.pBar);
            this.panel1.Controls.Add(this.btnInstall);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 451);
            this.panel1.TabIndex = 0;
            // 
            // btnRecovery
            // 
            this.btnRecovery.Location = new System.Drawing.Point(12, 160);
            this.btnRecovery.Name = "btnRecovery";
            this.btnRecovery.Size = new System.Drawing.Size(122, 23);
            this.btnRecovery.TabIndex = 7;
            this.btnRecovery.Text = "Recovery Tool";
            this.btnRecovery.UseVisualStyleBackColor = true;
            this.btnRecovery.Click += new System.EventHandler(this.btnRecovery_Click);
            // 
            // btnUninstall
            // 
            this.btnUninstall.Location = new System.Drawing.Point(12, 106);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(122, 23);
            this.btnUninstall.TabIndex = 6;
            this.btnUninstall.Text = "Uninstall Beta Fortress";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "About BF Client";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(12, 386);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(115, 23);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 40);
            this.label1.TabIndex = 3;
            this.label1.Text = "Beta Fortress Client";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Location = new System.Drawing.Point(161, 61);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status";
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(163, 77);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(270, 23);
            this.pBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pBar.TabIndex = 1;
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(12, 77);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(122, 23);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "Install Beta Fortress";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // gitCloneWorker
            // 
            this.gitCloneWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.gitCloneWorker_DoWork);
            this.gitCloneWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.gitCloneWorker_ProgressChanged);
            this.gitCloneWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.gitCloneWorker_RunWorkerCompleted);
            // 
            // gitPullWorker
            // 
            this.gitPullWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.gitPullWorker_DoWork);
            this.gitPullWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.gitPullWorker_ProgressChanged);
            this.gitPullWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.gitPullWorker_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beta Fortress Client";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Button btnRecovery;
        private System.ComponentModel.BackgroundWorker gitCloneWorker;
        private System.ComponentModel.BackgroundWorker gitPullWorker;
    }
}