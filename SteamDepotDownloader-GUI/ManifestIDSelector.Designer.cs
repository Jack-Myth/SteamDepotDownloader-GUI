namespace SteamDepotDownloader_GUI
{
    partial class ManifestIDSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManifestIDSelector));
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelDepotID = new System.Windows.Forms.Label();
            this.labelBuildID = new System.Windows.Forms.Label();
            this.labelManifestID = new System.Windows.Forms.Label();
            this.labelDepotName = new System.Windows.Forms.Label();
            this.labelLastUpdate = new System.Windows.Forms.Label();
            this.listViewManifests = new System.Windows.Forms.ListView();
            this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderManifestID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelDepotID
            // 
            resources.ApplyResources(this.labelDepotID, "labelDepotID");
            this.labelDepotID.Name = "labelDepotID";
            // 
            // labelBuildID
            // 
            resources.ApplyResources(this.labelBuildID, "labelBuildID");
            this.labelBuildID.Name = "labelBuildID";
            // 
            // labelManifestID
            // 
            resources.ApplyResources(this.labelManifestID, "labelManifestID");
            this.labelManifestID.Name = "labelManifestID";
            // 
            // labelDepotName
            // 
            resources.ApplyResources(this.labelDepotName, "labelDepotName");
            this.labelDepotName.Name = "labelDepotName";
            // 
            // labelLastUpdate
            // 
            resources.ApplyResources(this.labelLastUpdate, "labelLastUpdate");
            this.labelLastUpdate.Name = "labelLastUpdate";
            // 
            // listViewManifests
            // 
            this.listViewManifests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDate,
            this.columnHeaderRDate,
            this.columnHeaderManifestID});
            this.listViewManifests.FullRowSelect = true;
            this.listViewManifests.HideSelection = false;
            resources.ApplyResources(this.listViewManifests, "listViewManifests");
            this.listViewManifests.MultiSelect = false;
            this.listViewManifests.Name = "listViewManifests";
            this.listViewManifests.UseCompatibleStateImageBehavior = false;
            this.listViewManifests.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDate
            // 
            resources.ApplyResources(this.columnHeaderDate, "columnHeaderDate");
            // 
            // columnHeaderRDate
            // 
            resources.ApplyResources(this.columnHeaderRDate, "columnHeaderRDate");
            // 
            // columnHeaderManifestID
            // 
            resources.ApplyResources(this.columnHeaderManifestID, "columnHeaderManifestID");
            // 
            // ManifestIDSelector
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewManifests);
            this.Controls.Add(this.labelLastUpdate);
            this.Controls.Add(this.labelDepotName);
            this.Controls.Add(this.labelManifestID);
            this.Controls.Add(this.labelBuildID);
            this.Controls.Add(this.labelDepotID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManifestIDSelector";
            this.Load += new System.EventHandler(this.ManifestIDSelector_LoadAsync);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelDepotID;
        private System.Windows.Forms.Label labelBuildID;
        private System.Windows.Forms.Label labelManifestID;
        private System.Windows.Forms.Label labelDepotName;
        private System.Windows.Forms.Label labelLastUpdate;
        private System.Windows.Forms.ListView listViewManifests;
        private System.Windows.Forms.ColumnHeader columnHeaderDate;
        private System.Windows.Forms.ColumnHeader columnHeaderRDate;
        private System.Windows.Forms.ColumnHeader columnHeaderManifestID;
    }
}