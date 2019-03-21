namespace SteamDepotDownloader_GUI
{
    partial class SteamDepotDownloaderForm
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
            this.appList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelGameCount = new System.Windows.Forms.Label();
            this.listDepots = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupDepotInfo = new System.Windows.Forms.GroupBox();
            this.labelExtraInfo = new System.Windows.Forms.Label();
            this.labelOS = new System.Windows.Forms.Label();
            this.labelDepotSize = new System.Windows.Forms.Label();
            this.labelAppName = new System.Windows.Forms.Label();
            this.labelDepotName = new System.Windows.Forms.Label();
            this.labelAppID = new System.Windows.Forms.Label();
            this.labelDepotID = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBranches = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBoxDownloading = new System.Windows.Forms.GroupBox();
            this.panelDownloading = new System.Windows.Forms.Panel();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonManuallyInput = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialogMain = new System.Windows.Forms.FolderBrowserDialog();
            this.textBoxAppSearch = new System.Windows.Forms.TextBox();
            this.pictureAvatar = new System.Windows.Forms.PictureBox();
            this.groupDepotInfo.SuspendLayout();
            this.groupBoxDownloading.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // appList
            // 
            this.appList.FormattingEnabled = true;
            this.appList.ItemHeight = 12;
            this.appList.Location = new System.Drawing.Point(12, 60);
            this.appList.Name = "appList";
            this.appList.Size = new System.Drawing.Size(217, 316);
            this.appList.TabIndex = 0;
            this.appList.SelectedIndexChanged += new System.EventHandler(this.appList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Apps:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "→";
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelName.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelName.Location = new System.Drawing.Point(361, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(303, 29);
            this.labelName.TabIndex = 4;
            this.labelName.Text = "Click Steam icon to login";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGameCount
            // 
            this.labelGameCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGameCount.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelGameCount.Location = new System.Drawing.Point(497, 38);
            this.labelGameCount.Name = "labelGameCount";
            this.labelGameCount.Size = new System.Drawing.Size(168, 26);
            this.labelGameCount.TabIndex = 5;
            this.labelGameCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listDepots
            // 
            this.listDepots.FormattingEnabled = true;
            this.listDepots.ItemHeight = 12;
            this.listDepots.Location = new System.Drawing.Point(259, 84);
            this.listDepots.Name = "listDepots";
            this.listDepots.Size = new System.Drawing.Size(217, 292);
            this.listDepots.TabIndex = 6;
            this.listDepots.SelectedIndexChanged += new System.EventHandler(this.listDepots_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(257, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Depots:";
            // 
            // groupDepotInfo
            // 
            this.groupDepotInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupDepotInfo.Controls.Add(this.labelExtraInfo);
            this.groupDepotInfo.Controls.Add(this.labelOS);
            this.groupDepotInfo.Controls.Add(this.labelDepotSize);
            this.groupDepotInfo.Controls.Add(this.labelAppName);
            this.groupDepotInfo.Controls.Add(this.labelDepotName);
            this.groupDepotInfo.Controls.Add(this.labelAppID);
            this.groupDepotInfo.Controls.Add(this.labelDepotID);
            this.groupDepotInfo.Location = new System.Drawing.Point(12, 392);
            this.groupDepotInfo.Name = "groupDepotInfo";
            this.groupDepotInfo.Size = new System.Drawing.Size(464, 270);
            this.groupDepotInfo.TabIndex = 8;
            this.groupDepotInfo.TabStop = false;
            this.groupDepotInfo.Text = "DepotInfo";
            // 
            // labelExtraInfo
            // 
            this.labelExtraInfo.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelExtraInfo.Location = new System.Drawing.Point(20, 202);
            this.labelExtraInfo.Name = "labelExtraInfo";
            this.labelExtraInfo.Size = new System.Drawing.Size(339, 65);
            this.labelExtraInfo.TabIndex = 15;
            this.labelExtraInfo.Text = "ExtraInfo:";
            // 
            // labelOS
            // 
            this.labelOS.AutoSize = true;
            this.labelOS.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOS.Location = new System.Drawing.Point(20, 173);
            this.labelOS.Name = "labelOS";
            this.labelOS.Size = new System.Drawing.Size(32, 16);
            this.labelOS.TabIndex = 14;
            this.labelOS.Text = "OS:";
            // 
            // labelDepotSize
            // 
            this.labelDepotSize.AutoSize = true;
            this.labelDepotSize.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDepotSize.Location = new System.Drawing.Point(20, 143);
            this.labelDepotSize.Name = "labelDepotSize";
            this.labelDepotSize.Size = new System.Drawing.Size(112, 16);
            this.labelDepotSize.TabIndex = 13;
            this.labelDepotSize.Text = "DepotMaxSize:";
            // 
            // labelAppName
            // 
            this.labelAppName.AutoSize = true;
            this.labelAppName.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelAppName.Location = new System.Drawing.Point(20, 112);
            this.labelAppName.Name = "labelAppName";
            this.labelAppName.Size = new System.Drawing.Size(72, 16);
            this.labelAppName.TabIndex = 12;
            this.labelAppName.Text = "AppName:";
            // 
            // labelDepotName
            // 
            this.labelDepotName.AutoSize = true;
            this.labelDepotName.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDepotName.Location = new System.Drawing.Point(20, 84);
            this.labelDepotName.Name = "labelDepotName";
            this.labelDepotName.Size = new System.Drawing.Size(88, 16);
            this.labelDepotName.TabIndex = 11;
            this.labelDepotName.Text = "DepotName:";
            // 
            // labelAppID
            // 
            this.labelAppID.AutoSize = true;
            this.labelAppID.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelAppID.Location = new System.Drawing.Point(20, 54);
            this.labelAppID.Name = "labelAppID";
            this.labelAppID.Size = new System.Drawing.Size(56, 16);
            this.labelAppID.TabIndex = 10;
            this.labelAppID.Text = "AppID:";
            // 
            // labelDepotID
            // 
            this.labelDepotID.AutoSize = true;
            this.labelDepotID.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDepotID.Location = new System.Drawing.Point(20, 27);
            this.labelDepotID.Name = "labelDepotID";
            this.labelDepotID.Size = new System.Drawing.Size(72, 16);
            this.labelDepotID.TabIndex = 9;
            this.labelDepotID.Text = "DepotID:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(488, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "→";
            // 
            // comboBranches
            // 
            this.comboBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBranches.FormattingEnabled = true;
            this.comboBranches.Location = new System.Drawing.Point(259, 38);
            this.comboBranches.Name = "comboBranches";
            this.comboBranches.Size = new System.Drawing.Size(217, 20);
            this.comboBranches.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(257, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "Branch:";
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(511, 283);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(209, 23);
            this.buttonDownload.TabIndex = 15;
            this.buttonDownload.Text = "Download Depot";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(606, 268);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "↓";
            // 
            // groupBoxDownloading
            // 
            this.groupBoxDownloading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDownloading.Controls.Add(this.panelDownloading);
            this.groupBoxDownloading.Location = new System.Drawing.Point(484, 312);
            this.groupBoxDownloading.Name = "groupBoxDownloading";
            this.groupBoxDownloading.Size = new System.Drawing.Size(236, 347);
            this.groupBoxDownloading.TabIndex = 17;
            this.groupBoxDownloading.TabStop = false;
            this.groupBoxDownloading.Text = "DownloadingDepot";
            // 
            // panelDownloading
            // 
            this.panelDownloading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDownloading.AutoScroll = true;
            this.panelDownloading.AutoScrollMinSize = new System.Drawing.Size(0, 350);
            this.panelDownloading.Location = new System.Drawing.Point(6, 21);
            this.panelDownloading.Name = "panelDownloading";
            this.panelDownloading.Size = new System.Drawing.Size(221, 320);
            this.panelDownloading.TabIndex = 0;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSettings.Location = new System.Drawing.Point(563, 69);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(157, 32);
            this.buttonSettings.TabIndex = 18;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            // 
            // buttonManuallyInput
            // 
            this.buttonManuallyInput.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonManuallyInput.Location = new System.Drawing.Point(563, 107);
            this.buttonManuallyInput.Name = "buttonManuallyInput";
            this.buttonManuallyInput.Size = new System.Drawing.Size(157, 32);
            this.buttonManuallyInput.TabIndex = 19;
            this.buttonManuallyInput.Text = "Manually Input";
            this.buttonManuallyInput.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(14, 20);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(186, 16);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Download Selected File Only";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new System.Drawing.Point(511, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 119);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Download Setting";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(14, 64);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(156, 16);
            this.checkBox3.TabIndex = 23;
            this.checkBox3.Text = "Download the whole App";
            this.toolTipMain.SetToolTip(this.checkBox3, "Ignore what you select in Depots");
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(14, 42);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(156, 16);
            this.checkBox2.TabIndex = 22;
            this.checkBox2.Text = "Download Manifest Only";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "Choose Install Directory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolTipMain
            // 
            this.toolTipMain.AutoPopDelay = 5000;
            this.toolTipMain.InitialDelay = 200;
            this.toolTipMain.ReshowDelay = 100;
            // 
            // textBoxAppSearch
            // 
            this.textBoxAppSearch.Location = new System.Drawing.Point(14, 9);
            this.textBoxAppSearch.Name = "textBoxAppSearch";
            this.textBoxAppSearch.Size = new System.Drawing.Size(215, 21);
            this.textBoxAppSearch.TabIndex = 22;
            this.textBoxAppSearch.TextChanged += new System.EventHandler(this.textBoxAppSearch_TextChanged);
            // 
            // pictureAvatar
            // 
            this.pictureAvatar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureAvatar.Image = global::SteamDepotDownloader_GUI.Properties.Resources.SteamGray;
            this.pictureAvatar.Location = new System.Drawing.Point(670, 9);
            this.pictureAvatar.Name = "pictureAvatar";
            this.pictureAvatar.Size = new System.Drawing.Size(50, 50);
            this.pictureAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureAvatar.TabIndex = 23;
            this.pictureAvatar.TabStop = false;
            this.pictureAvatar.Click += new System.EventHandler(this.pictureAvatar_Click);
            // 
            // SteamDepotDownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 674);
            this.Controls.Add(this.pictureAvatar);
            this.Controls.Add(this.textBoxAppSearch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonManuallyInput);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.groupBoxDownloading);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBranches);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupDepotInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listDepots);
            this.Controls.Add(this.labelGameCount);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.appList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "SteamDepotDownloaderForm";
            this.Text = "SteamDepotDownloader-GUI By JackMyth";
            this.Load += new System.EventHandler(this.SteamDepotDownloaderForm_Load);
            this.groupDepotInfo.ResumeLayout(false);
            this.groupDepotInfo.PerformLayout();
            this.groupBoxDownloading.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAvatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox appList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelGameCount;
        private System.Windows.Forms.ListBox listDepots;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupDepotInfo;
        private System.Windows.Forms.Label labelOS;
        private System.Windows.Forms.Label labelDepotSize;
        private System.Windows.Forms.Label labelAppName;
        private System.Windows.Forms.Label labelDepotName;
        private System.Windows.Forms.Label labelAppID;
        private System.Windows.Forms.Label labelDepotID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelExtraInfo;
        private System.Windows.Forms.ComboBox comboBranches;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBoxDownloading;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonManuallyInput;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelDownloading;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogMain;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TextBox textBoxAppSearch;
        private System.Windows.Forms.PictureBox pictureAvatar;
    }
}

