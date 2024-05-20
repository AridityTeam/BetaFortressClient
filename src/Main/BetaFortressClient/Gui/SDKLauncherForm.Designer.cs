namespace BetaFortressTeam.BetaFortressClient.Gui
{
    partial class SDKLauncherForm
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
            this.sdkToolsList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // sdkToolsList
            // 
            this.sdkToolsList.FormattingEnabled = true;
            this.sdkToolsList.Items.AddRange(new object[] {
            "Hammer World Editor",
            "Half-Life Model Viewer"});
            this.sdkToolsList.Location = new System.Drawing.Point(2, 2);
            this.sdkToolsList.Name = "sdkToolsList";
            this.sdkToolsList.Size = new System.Drawing.Size(271, 121);
            this.sdkToolsList.TabIndex = 0;
            this.sdkToolsList.SelectedIndexChanged += new System.EventHandler(this.sdkToolsList_SelectedIndexChanged);
            // 
            // SDKLauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 283);
            this.Controls.Add(this.sdkToolsList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SDKLauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beta Fortress SDK - Beta Fortress Cient";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox sdkToolsList;
    }
}