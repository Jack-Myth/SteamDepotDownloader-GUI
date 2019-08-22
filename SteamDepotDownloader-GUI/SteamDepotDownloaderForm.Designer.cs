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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SteamDepotDownloaderForm));
            this.appList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelGameCount = new System.Windows.Forms.Label();
            this.listDepots = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupDepotInfo = new System.Windows.Forms.GroupBox();
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
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxAppSearch = new System.Windows.Forms.TextBox();
            this.pictureAvatar = new System.Windows.Forms.PictureBox();
            this.buttonLog = new System.Windows.Forms.Button();
            this.buttonSelectManifest = new System.Windows.Forms.Button();
            this.folderBrowserDialogMain = new System.Windows.Forms.FolderBrowserDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupDepotInfo.SuspendLayout();
            this.groupBoxDownloading.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // appList
            // 
            this.appList.FormattingEnabled = true;
            resources.ApplyResources(this.appList, "appList");
            this.appList.Name = "appList";
            this.appList.SelectedIndexChanged += new System.EventHandler(this.appList_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labelName
            // 
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Name = "labelName";
            // 
            // labelGameCount
            // 
            resources.ApplyResources(this.labelGameCount, "labelGameCount");
            this.labelGameCount.Name = "labelGameCount";
            // 
            // listDepots
            // 
            this.listDepots.FormattingEnabled = true;
            resources.ApplyResources(this.listDepots, "listDepots");
            this.listDepots.Name = "listDepots";
            this.listDepots.SelectedIndexChanged += new System.EventHandler(this.listDepots_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupDepotInfo
            // 
            resources.ApplyResources(this.groupDepotInfo, "groupDepotInfo");
            this.groupDepotInfo.Controls.Add(this.labelOS);
            this.groupDepotInfo.Controls.Add(this.labelDepotSize);
            this.groupDepotInfo.Controls.Add(this.labelAppName);
            this.groupDepotInfo.Controls.Add(this.labelDepotName);
            this.groupDepotInfo.Controls.Add(this.labelAppID);
            this.groupDepotInfo.Controls.Add(this.labelDepotID);
            this.groupDepotInfo.Name = "groupDepotInfo";
            this.groupDepotInfo.TabStop = false;
            // 
            // labelOS
            // 
            resources.ApplyResources(this.labelOS, "labelOS");
            this.labelOS.Name = "labelOS";
            // 
            // labelDepotSize
            // 
            resources.ApplyResources(this.labelDepotSize, "labelDepotSize");
            this.labelDepotSize.Name = "labelDepotSize";
            // 
            // labelAppName
            // 
            resources.ApplyResources(this.labelAppName, "labelAppName");
            this.labelAppName.Name = "labelAppName";
            // 
            // labelDepotName
            // 
            resources.ApplyResources(this.labelDepotName, "labelDepotName");
            this.labelDepotName.Name = "labelDepotName";
            // 
            // labelAppID
            // 
            resources.ApplyResources(this.labelAppID, "labelAppID");
            this.labelAppID.Name = "labelAppID";
            // 
            // labelDepotID
            // 
            resources.ApplyResources(this.labelDepotID, "labelDepotID");
            this.labelDepotID.Name = "labelDepotID";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // comboBranches
            // 
            this.comboBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBranches.FormattingEnabled = true;
            resources.ApplyResources(this.comboBranches, "comboBranches");
            this.comboBranches.Name = "comboBranches";
            this.comboBranches.SelectedIndexChanged += new System.EventHandler(this.ComboBranches_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // buttonDownload
            // 
            resources.ApplyResources(this.buttonDownload, "buttonDownload");
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBoxDownloading
            // 
            resources.ApplyResources(this.groupBoxDownloading, "groupBoxDownloading");
            this.groupBoxDownloading.Controls.Add(this.panelDownloading);
            this.groupBoxDownloading.Name = "groupBoxDownloading";
            this.groupBoxDownloading.TabStop = false;
            // 
            // panelDownloading
            // 
            resources.ApplyResources(this.panelDownloading, "panelDownloading");
            this.panelDownloading.Name = "panelDownloading";
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonManuallyInput
            // 
            resources.ApplyResources(this.buttonManuallyInput, "buttonManuallyInput");
            this.buttonManuallyInput.Name = "buttonManuallyInput";
            this.buttonManuallyInput.UseVisualStyleBackColor = true;
            this.buttonManuallyInput.Click += new System.EventHandler(this.buttonManuallyInput_Click);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.checkBox1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Name = "checkBox4";
            this.toolTipMain.SetToolTip(this.checkBox4, resources.GetString("checkBox4.ToolTip"));
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.toolTipMain.SetToolTip(this.checkBox3, resources.GetString("checkBox3.ToolTip"));
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTipMain.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
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
            resources.ApplyResources(this.textBoxAppSearch, "textBoxAppSearch");
            this.textBoxAppSearch.Name = "textBoxAppSearch";
            this.textBoxAppSearch.TextChanged += new System.EventHandler(this.textBoxAppSearch_TextChanged);
            // 
            // pictureAvatar
            // 
            resources.ApplyResources(this.pictureAvatar, "pictureAvatar");
            this.pictureAvatar.Image = global::SteamDepotDownloader_GUI.Properties.Resources.SteamGray;
            this.pictureAvatar.Name = "pictureAvatar";
            this.pictureAvatar.TabStop = false;
            this.pictureAvatar.Click += new System.EventHandler(this.pictureAvatar_Click);
            // 
            // buttonLog
            // 
            resources.ApplyResources(this.buttonLog, "buttonLog");
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // buttonSelectManifest
            // 
            resources.ApplyResources(this.buttonSelectManifest, "buttonSelectManifest");
            this.buttonSelectManifest.Name = "buttonSelectManifest";
            this.buttonSelectManifest.UseVisualStyleBackColor = true;
            this.buttonSelectManifest.Click += new System.EventHandler(this.ButtonSelectManifest_Click);
            // 
            // notifyIcon1
            // 
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // SteamDepotDownloaderForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSelectManifest);
            this.Controls.Add(this.buttonLog);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SteamDepotDownloaderForm_FormClosed);
            this.Load += new System.EventHandler(this.SteamDepotDownloaderForm_Load);
            this.SizeChanged += new System.EventHandler(this.SteamDepotDownloaderForm_SizeChanged);
            this.Resize += new System.EventHandler(this.SteamDepotDownloaderForm_Resize);
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
        private System.Windows.Forms.ComboBox comboBranches;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBoxDownloading;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonManuallyInput;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelDownloading;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogMain;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TextBox textBoxAppSearch;
        private System.Windows.Forms.PictureBox pictureAvatar;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        internal System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.Button buttonSelectManifest;
    }
}

