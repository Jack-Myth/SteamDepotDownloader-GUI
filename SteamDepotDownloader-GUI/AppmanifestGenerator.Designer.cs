namespace SteamDepotDownloader_GUI
{
    partial class AppmanifestGenerator
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxInstallDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelAppID = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxDLCs = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxDepots = new System.Windows.Forms.CheckedListBox();
            this.buttonGen = new System.Windows.Forms.Button();
            this.buttonSelectManifestID = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.textBoxInstallDir);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelAppID);
            this.panel1.Location = new System.Drawing.Point(6, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(233, 94);
            this.panel1.TabIndex = 0;
            // 
            // textBoxInstallDir
            // 
            this.textBoxInstallDir.Location = new System.Drawing.Point(81, 56);
            this.textBoxInstallDir.Name = "textBoxInstallDir";
            this.textBoxInstallDir.Size = new System.Drawing.Size(149, 21);
            this.textBoxInstallDir.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "installdir:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(45, 29);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(185, 21);
            this.textBoxName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // labelAppID
            // 
            this.labelAppID.AutoSize = true;
            this.labelAppID.Location = new System.Drawing.Point(4, 4);
            this.labelAppID.Name = "labelAppID";
            this.labelAppID.Size = new System.Drawing.Size(41, 12);
            this.labelAppID.TabIndex = 0;
            this.labelAppID.Text = "AppID:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 120);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "BaseInfomation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkedListBoxDLCs);
            this.groupBox2.Location = new System.Drawing.Point(13, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 145);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "InstalledDLCs";
            // 
            // checkedListBoxDLCs
            // 
            this.checkedListBoxDLCs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxDLCs.FormattingEnabled = true;
            this.checkedListBoxDLCs.Location = new System.Drawing.Point(6, 21);
            this.checkedListBoxDLCs.Name = "checkedListBoxDLCs";
            this.checkedListBoxDLCs.Size = new System.Drawing.Size(233, 116);
            this.checkedListBoxDLCs.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkedListBoxDepots);
            this.groupBox3.Location = new System.Drawing.Point(13, 293);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(245, 145);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "InstalledDepots";
            // 
            // checkedListBoxDepots
            // 
            this.checkedListBoxDepots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxDepots.FormattingEnabled = true;
            this.checkedListBoxDepots.Location = new System.Drawing.Point(6, 21);
            this.checkedListBoxDepots.Name = "checkedListBoxDepots";
            this.checkedListBoxDepots.Size = new System.Drawing.Size(233, 116);
            this.checkedListBoxDepots.TabIndex = 0;
            // 
            // buttonGen
            // 
            this.buttonGen.Location = new System.Drawing.Point(13, 473);
            this.buttonGen.Name = "buttonGen";
            this.buttonGen.Size = new System.Drawing.Size(245, 23);
            this.buttonGen.TabIndex = 4;
            this.buttonGen.Text = "Generate";
            this.buttonGen.UseVisualStyleBackColor = true;
            // 
            // buttonSelectManifestID
            // 
            this.buttonSelectManifestID.Location = new System.Drawing.Point(133, 444);
            this.buttonSelectManifestID.Name = "buttonSelectManifestID";
            this.buttonSelectManifestID.Size = new System.Drawing.Size(119, 23);
            this.buttonSelectManifestID.TabIndex = 5;
            this.buttonSelectManifestID.Text = "SelectManifestID";
            this.buttonSelectManifestID.UseVisualStyleBackColor = true;
            // 
            // AppmanifestGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 506);
            this.Controls.Add(this.buttonSelectManifestID);
            this.Controls.Add(this.buttonGen);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppmanifestGenerator";
            this.Text = "AppmanifestGenerator";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelAppID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxInstallDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox checkedListBoxDLCs;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox checkedListBoxDepots;
        private System.Windows.Forms.Button buttonGen;
        private System.Windows.Forms.Button buttonSelectManifestID;
    }
}