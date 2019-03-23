namespace SteamDepotDownloader_GUI
{
    partial class AdvancedInput
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
            this.components = new System.ComponentModel.Container();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.textBoxAppID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxMaxDownloads = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxMaxServers = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxManifest = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxBetaPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxBranch = new System.Windows.Forms.TextBox();
            this.checkBoxManifestOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxAllPlatforms = new System.Windows.Forms.CheckBox();
            this.comboBoxOS = new System.Windows.Forms.ComboBox();
            this.labelOS = new System.Windows.Forms.Label();
            this.buttonInstallDir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCellID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDepotID = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxDownloadName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDownload
            // 
            this.buttonDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownload.Location = new System.Drawing.Point(12, 258);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(396, 23);
            this.buttonDownload.TabIndex = 14;
            this.buttonDownload.Text = "创建下载任务";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // textBoxAppID
            // 
            this.textBoxAppID.Location = new System.Drawing.Point(291, 24);
            this.textBoxAppID.Name = "textBoxAppID";
            this.textBoxAppID.Size = new System.Drawing.Size(117, 21);
            this.textBoxAppID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "AppID:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxMaxDownloads);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxMaxServers);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxManifest);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxBetaPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxBranch);
            this.groupBox1.Controls.Add(this.checkBoxManifestOnly);
            this.groupBox1.Controls.Add(this.checkBoxAllPlatforms);
            this.groupBox1.Controls.Add(this.comboBoxOS);
            this.groupBox1.Controls.Add(this.labelOS);
            this.groupBox1.Controls.Add(this.buttonInstallDir);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxCellID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxDepotID);
            this.groupBox1.Location = new System.Drawing.Point(14, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 201);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "可选参数";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(254, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "文件列表文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(127, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 21;
            this.label8.Text = "同时文件下载数:";
            // 
            // textBoxMaxDownloads
            // 
            this.textBoxMaxDownloads.Location = new System.Drawing.Point(129, 117);
            this.textBoxMaxDownloads.Name = "textBoxMaxDownloads";
            this.textBoxMaxDownloads.Size = new System.Drawing.Size(117, 21);
            this.textBoxMaxDownloads.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "最大服务器数:";
            // 
            // textBoxMaxServers
            // 
            this.textBoxMaxServers.Location = new System.Drawing.Point(8, 117);
            this.textBoxMaxServers.Name = "textBoxMaxServers";
            this.textBoxMaxServers.Size = new System.Drawing.Size(117, 21);
            this.textBoxMaxServers.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(252, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "ManifestID:";
            // 
            // textBoxManifest
            // 
            this.textBoxManifest.Location = new System.Drawing.Point(254, 72);
            this.textBoxManifest.Name = "textBoxManifest";
            this.textBoxManifest.Size = new System.Drawing.Size(117, 21);
            this.textBoxManifest.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "Beta Password:";
            // 
            // textBoxBetaPassword
            // 
            this.textBoxBetaPassword.Location = new System.Drawing.Point(131, 72);
            this.textBoxBetaPassword.Name = "textBoxBetaPassword";
            this.textBoxBetaPassword.Size = new System.Drawing.Size(117, 21);
            this.textBoxBetaPassword.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Branch:";
            // 
            // textBoxBranch
            // 
            this.textBoxBranch.Location = new System.Drawing.Point(8, 72);
            this.textBoxBranch.Name = "textBoxBranch";
            this.textBoxBranch.Size = new System.Drawing.Size(117, 21);
            this.textBoxBranch.TabIndex = 5;
            // 
            // checkBoxManifestOnly
            // 
            this.checkBoxManifestOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxManifestOnly.AutoSize = true;
            this.checkBoxManifestOnly.Location = new System.Drawing.Point(280, 144);
            this.checkBoxManifestOnly.Name = "checkBoxManifestOnly";
            this.checkBoxManifestOnly.Size = new System.Drawing.Size(108, 16);
            this.checkBoxManifestOnly.TabIndex = 12;
            this.checkBoxManifestOnly.Text = "只下载Manifest";
            this.checkBoxManifestOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllPlatforms
            // 
            this.checkBoxAllPlatforms.AutoSize = true;
            this.checkBoxAllPlatforms.Location = new System.Drawing.Point(8, 144);
            this.checkBoxAllPlatforms.Name = "checkBoxAllPlatforms";
            this.checkBoxAllPlatforms.Size = new System.Drawing.Size(120, 16);
            this.checkBoxAllPlatforms.TabIndex = 11;
            this.checkBoxAllPlatforms.Text = "下载所有平台内容";
            this.checkBoxAllPlatforms.UseVisualStyleBackColor = true;
            // 
            // comboBoxOS
            // 
            this.comboBoxOS.Cursor = System.Windows.Forms.Cursors.Default;
            this.comboBoxOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOS.FormattingEnabled = true;
            this.comboBoxOS.Items.AddRange(new object[] {
            "Windows",
            "Linux",
            "MacOS"});
            this.comboBoxOS.Location = new System.Drawing.Point(254, 34);
            this.comboBoxOS.Name = "comboBoxOS";
            this.comboBoxOS.Size = new System.Drawing.Size(121, 20);
            this.comboBoxOS.TabIndex = 4;
            // 
            // labelOS
            // 
            this.labelOS.AutoSize = true;
            this.labelOS.Location = new System.Drawing.Point(252, 18);
            this.labelOS.Name = "labelOS";
            this.labelOS.Size = new System.Drawing.Size(65, 12);
            this.labelOS.TabIndex = 8;
            this.labelOS.Text = "操作系统：";
            // 
            // buttonInstallDir
            // 
            this.buttonInstallDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInstallDir.Location = new System.Drawing.Point(6, 172);
            this.buttonInstallDir.Name = "buttonInstallDir";
            this.buttonInstallDir.Size = new System.Drawing.Size(382, 23);
            this.buttonInstallDir.TabIndex = 13;
            this.buttonInstallDir.Text = "选择安装目录";
            this.toolTip1.SetToolTip(this.buttonInstallDir, "./Download");
            this.buttonInstallDir.UseVisualStyleBackColor = true;
            this.buttonInstallDir.Click += new System.EventHandler(this.buttonInstallDir_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(127, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "CellID:";
            // 
            // textBoxCellID
            // 
            this.textBoxCellID.Location = new System.Drawing.Point(129, 33);
            this.textBoxCellID.Name = "textBoxCellID";
            this.textBoxCellID.Size = new System.Drawing.Size(117, 21);
            this.textBoxCellID.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "DepotID:";
            // 
            // textBoxDepotID
            // 
            this.textBoxDepotID.Location = new System.Drawing.Point(6, 33);
            this.textBoxDepotID.Name = "textBoxDepotID";
            this.textBoxDepotID.Size = new System.Drawing.Size(117, 21);
            this.textBoxDepotID.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "TXT Files|*.txt|All Files|*.*";
            // 
            // textBoxDownloadName
            // 
            this.textBoxDownloadName.Location = new System.Drawing.Point(14, 24);
            this.textBoxDownloadName.Name = "textBoxDownloadName";
            this.textBoxDownloadName.Size = new System.Drawing.Size(271, 21);
            this.textBoxDownloadName.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "下载名称:";
            // 
            // AdvancedInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 293);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxDownloadName);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAppID);
            this.Controls.Add(this.buttonDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedInput";
            this.Text = "高级输入";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.TextBox textBoxAppID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDepotID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCellID;
        private System.Windows.Forms.Button buttonInstallDir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxBetaPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxBranch;
        private System.Windows.Forms.CheckBox checkBoxManifestOnly;
        private System.Windows.Forms.CheckBox checkBoxAllPlatforms;
        private System.Windows.Forms.ComboBox comboBoxOS;
        private System.Windows.Forms.Label labelOS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxManifest;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxMaxDownloads;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxMaxServers;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxDownloadName;
        private System.Windows.Forms.Label label9;
    }
}