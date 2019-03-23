namespace SteamDepotDownloader_GUI
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.buttonClearCache = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.comboBoxMaxServer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMaxDownload = new System.Windows.Forms.ComboBox();
            this.checkBoxNotify = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClearCache
            // 
            resources.ApplyResources(this.buttonClearCache, "buttonClearCache");
            this.buttonClearCache.Name = "buttonClearCache";
            this.buttonClearCache.UseVisualStyleBackColor = true;
            this.buttonClearCache.Click += new System.EventHandler(this.buttonClearCache_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // LabelVersion
            // 
            resources.ApplyResources(this.LabelVersion, "LabelVersion");
            this.LabelVersion.Name = "LabelVersion";
            // 
            // comboBoxMaxServer
            // 
            this.comboBoxMaxServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaxServer.FormattingEnabled = true;
            this.comboBoxMaxServer.Items.AddRange(new object[] {
            resources.GetString("comboBoxMaxServer.Items"),
            resources.GetString("comboBoxMaxServer.Items1"),
            resources.GetString("comboBoxMaxServer.Items2"),
            resources.GetString("comboBoxMaxServer.Items3"),
            resources.GetString("comboBoxMaxServer.Items4"),
            resources.GetString("comboBoxMaxServer.Items5"),
            resources.GetString("comboBoxMaxServer.Items6"),
            resources.GetString("comboBoxMaxServer.Items7"),
            resources.GetString("comboBoxMaxServer.Items8"),
            resources.GetString("comboBoxMaxServer.Items9"),
            resources.GetString("comboBoxMaxServer.Items10"),
            resources.GetString("comboBoxMaxServer.Items11"),
            resources.GetString("comboBoxMaxServer.Items12"),
            resources.GetString("comboBoxMaxServer.Items13"),
            resources.GetString("comboBoxMaxServer.Items14"),
            resources.GetString("comboBoxMaxServer.Items15"),
            resources.GetString("comboBoxMaxServer.Items16"),
            resources.GetString("comboBoxMaxServer.Items17"),
            resources.GetString("comboBoxMaxServer.Items18"),
            resources.GetString("comboBoxMaxServer.Items19")});
            resources.ApplyResources(this.comboBoxMaxServer, "comboBoxMaxServer");
            this.comboBoxMaxServer.Name = "comboBoxMaxServer";
            this.comboBoxMaxServer.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaxServer_SelectedIndexChanged);
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
            // comboBoxMaxDownload
            // 
            this.comboBoxMaxDownload.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaxDownload.FormattingEnabled = true;
            this.comboBoxMaxDownload.Items.AddRange(new object[] {
            resources.GetString("comboBoxMaxDownload.Items"),
            resources.GetString("comboBoxMaxDownload.Items1"),
            resources.GetString("comboBoxMaxDownload.Items2"),
            resources.GetString("comboBoxMaxDownload.Items3"),
            resources.GetString("comboBoxMaxDownload.Items4"),
            resources.GetString("comboBoxMaxDownload.Items5"),
            resources.GetString("comboBoxMaxDownload.Items6"),
            resources.GetString("comboBoxMaxDownload.Items7"),
            resources.GetString("comboBoxMaxDownload.Items8"),
            resources.GetString("comboBoxMaxDownload.Items9"),
            resources.GetString("comboBoxMaxDownload.Items10"),
            resources.GetString("comboBoxMaxDownload.Items11"),
            resources.GetString("comboBoxMaxDownload.Items12"),
            resources.GetString("comboBoxMaxDownload.Items13"),
            resources.GetString("comboBoxMaxDownload.Items14"),
            resources.GetString("comboBoxMaxDownload.Items15"),
            resources.GetString("comboBoxMaxDownload.Items16"),
            resources.GetString("comboBoxMaxDownload.Items17"),
            resources.GetString("comboBoxMaxDownload.Items18"),
            resources.GetString("comboBoxMaxDownload.Items19")});
            resources.ApplyResources(this.comboBoxMaxDownload, "comboBoxMaxDownload");
            this.comboBoxMaxDownload.Name = "comboBoxMaxDownload";
            this.comboBoxMaxDownload.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaxDownload_SelectedIndexChanged);
            // 
            // checkBoxNotify
            // 
            resources.ApplyResources(this.checkBoxNotify, "checkBoxNotify");
            this.checkBoxNotify.Name = "checkBoxNotify";
            this.checkBoxNotify.UseVisualStyleBackColor = true;
            this.checkBoxNotify.CheckedChanged += new System.EventHandler(this.checkBoxNotify_CheckedChanged);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Image = global::SteamDepotDownloader_GUI.Properties.Resources.Icon;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // Settings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.checkBoxNotify);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxMaxDownload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxMaxServer);
            this.Controls.Add(this.LabelVersion);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonClearCache);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClearCache;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label LabelVersion;
        private System.Windows.Forms.ComboBox comboBoxMaxServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMaxDownload;
        private System.Windows.Forms.CheckBox checkBoxNotify;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}