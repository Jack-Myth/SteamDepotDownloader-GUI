namespace SteamDepotDownloader_GUI
{
    partial class Waiting
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
            this.WaitingMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WaitingMsg
            // 
            this.WaitingMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WaitingMsg.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WaitingMsg.Location = new System.Drawing.Point(13, 13);
            this.WaitingMsg.Name = "WaitingMsg";
            this.WaitingMsg.Size = new System.Drawing.Size(357, 23);
            this.WaitingMsg.TabIndex = 0;
            this.WaitingMsg.Text = "Updating App List...";
            this.WaitingMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Waiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 46);
            this.Controls.Add(this.WaitingMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Waiting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "请稍候...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label WaitingMsg;
    }
}