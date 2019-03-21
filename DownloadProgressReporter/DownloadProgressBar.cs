using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamDepotDownloader_GUI
{
    public partial class DownloadProgressBar : UserControl
    {
        public Action<string,uint,uint,string, bool, string> OnDownloadFinishedReport;
        public Action StopDownload;
        public Action RestartDownload;
        public Action<uint,uint,string, DownloadProgressBar> CancelDownload; //AppID,DepotID,Branch
        uint AppId, DepotId;
        string Branch;
        public bool Downloading = true;
        public bool IsDownloadFinished { get; private set; }
        public delegate void OnDownloadProgressDelegate(float Percent);
        OnDownloadProgressDelegate OdPDelegate;
        public delegate void OnDownloadFinishedDelegate(bool IsSuccessful, string ErrorMsg);
        OnDownloadFinishedDelegate OdFDelegate;
        public DownloadProgressBar()
        {
            InitializeComponent();
            OdPDelegate = new OnDownloadProgressDelegate(OnDownloadProgressView);
            OdFDelegate = new OnDownloadFinishedDelegate(OnDownloadFinishedView);
        }

        public void InitDownloading(string DownloadName,uint AppID,uint DepotID,string mBranch)
        {
            this.labelDepotName.Text = DownloadName;
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Value = 0;
            AppId = AppID;
            DepotId = DepotID;
            Branch = mBranch;
            if (Downloading)
                this.toolStripMenuItem1.Text = "Stop";
            else
                this.toolStripMenuItem1.Text = "Restart";
        }

        public void OnDownloadProgressView(float Percent)
        {
            this.progressBar1.Value = (int)(Percent * 100);
            this.progressBar1.Refresh();
        }
        public void OnDownloadProgress(float Percent,string Filename)
        {
            this.Invoke(OdPDelegate, Percent);
        }

        public void OnDownloadFinished(bool IsSuccessful,string ErrorMsg)
        {
            this.Invoke(OdFDelegate, IsSuccessful, ErrorMsg);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Downloading)
            {
                Downloading = false;
                toolStripMenuItem1.Text = "Restart";
                StopDownload.Invoke();
            }
            else
            {
                Downloading = true;
                toolStripMenuItem1.Text = "Stop";
                RestartDownload.Invoke();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CancelDownload.Invoke(AppId, DepotId, Branch,this);
        }

        public void OnDownloadFinishedView(bool IsSuccessful,string ErrorMsg)
        {
            IsDownloadFinished = true;
            OnDownloadFinishedReport.Invoke(this.labelDepotName.Text, AppId, DepotId, Branch, IsSuccessful, ErrorMsg);
        }

    }
}
