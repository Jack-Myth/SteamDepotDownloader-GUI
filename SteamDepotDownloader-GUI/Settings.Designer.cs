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
            this.buttonClearCache = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.comboBoxMaxServer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMaxDownload = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonClearCache
            // 
            this.buttonClearCache.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearCache.Location = new System.Drawing.Point(12, 107);
            this.buttonClearCache.Name = "buttonClearCache";
            this.buttonClearCache.Size = new System.Drawing.Size(155, 23);
            this.buttonClearCache.TabIndex = 0;
            this.buttonClearCache.Text = "Clear Cache";
            this.buttonClearCache.UseVisualStyleBackColor = true;
            this.buttonClearCache.Click += new System.EventHandler(this.buttonClearCache_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 207);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Designed By JackMyth";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(12, 178);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Check New Version";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // LabelVersion
            // 
            this.LabelVersion.Location = new System.Drawing.Point(10, 370);
            this.LabelVersion.Name = "LabelVersion";
            this.LabelVersion.Size = new System.Drawing.Size(155, 13);
            this.LabelVersion.TabIndex = 3;
            this.LabelVersion.Text = "version";
            this.LabelVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxMaxServer
            // 
            this.comboBoxMaxServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaxServer.FormattingEnabled = true;
            this.comboBoxMaxServer.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboBoxMaxServer.Location = new System.Drawing.Point(14, 24);
            this.comboBoxMaxServer.Name = "comboBoxMaxServer";
            this.comboBoxMaxServer.Size = new System.Drawing.Size(151, 20);
            this.comboBoxMaxServer.TabIndex = 4;
            this.comboBoxMaxServer.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaxServer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Max Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Max Download:";
            // 
            // comboBoxMaxDownload
            // 
            this.comboBoxMaxDownload.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaxDownload.FormattingEnabled = true;
            this.comboBoxMaxDownload.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboBoxMaxDownload.Location = new System.Drawing.Point(14, 71);
            this.comboBoxMaxDownload.Name = "comboBoxMaxDownload";
            this.comboBoxMaxDownload.Size = new System.Drawing.Size(151, 20);
            this.comboBoxMaxDownload.TabIndex = 6;
            this.comboBoxMaxDownload.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaxDownload_SelectedIndexChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 242);
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
            this.Text = "Settings";
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
    }
}