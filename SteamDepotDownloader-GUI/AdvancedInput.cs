using DepotDownloader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamDepotDownloader_GUI
{
    public partial class AdvancedInput : Form
    {
        DepotDownloader.DownloadConfig Dc=new DownloadConfig();
        public AdvancedInput()
        {
            InitializeComponent();
            Dc.InstallDirectory = "./Download";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string fileListData = File.ReadAllText(this.openFileDialog1.FileName);
            var files = fileListData.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            Dc.UsingFileList = true;
            Dc.FilesToDownload = new List<string>();
            Dc.FilesToDownloadRegex = new List<Regex>();

            foreach (var fileEntry in files)
            {
                try
                {
                    Regex rgx = new Regex(fileEntry, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Dc.FilesToDownloadRegex.Add(rgx);
                }
                catch
                {
                    Dc.FilesToDownload.Add(fileEntry);
                    continue;
                }
            }

            this.button1.Text = Properties.Resources.FileListLoaded;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right)
            {
                if(MessageBox.Show(Properties.Resources.ClearFileListQuestion, "Advanced Input", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    Dc.UsingFileList = false;
                    Dc.FilesToDownload = null;
                    Dc.FilesToDownloadRegex = null;
                }
            }
        }

        private void buttonInstallDir_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;
            Dc.InstallDirectory = this.folderBrowserDialog1.SelectedPath;
            this.toolTip1.SetToolTip(this.buttonInstallDir, Dc.InstallDirectory);
        }

        private uint LoadParamter(string Str,uint Default)
        {
            if (Str != "")
                return uint.Parse(Str);
            else
                return Default;
        }

        private int LoadParamter(string Str, int Default)
        {
            if (Str != "")
                return int.Parse(Str);
            else
                return Default;
        }

        private string LoadParamter(string Str, string Default)
        {
            return Str == "" ? Default : Str;
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Dc.AppID = LoadParamter(this.textBoxAppID.Text,ContentDownloader.INVALID_APP_ID);
                Dc.DepotID = LoadParamter(this.textBoxAppID.Text, ContentDownloader.INVALID_DEPOT_ID);
                if (Dc.DepotID != ContentDownloader.INVALID_DEPOT_ID)
                    Dc.ForceDepot = true;
                else
                    Dc.ForceDepot = false;
                Program.UsrConfig.CellID = LoadParamter(this.textBoxCellID.Text, Program.UsrConfig.CellID);
                Dc.OS = this.comboBoxOS.Text.ToLowerInvariant();
                Dc.Branch = LoadParamter(this.textBoxBranch.Text, "public");
                Dc.BetaPassword = LoadParamter(this.textBoxBranch.Text, "");
                Dc.MaxDownloads = LoadParamter(this.textBoxMaxDownloads.Text, Dc.MaxDownloads);
                Dc.MaxServers = LoadParamter(this.textBoxMaxDownloads.Text, Dc.MaxServers);
                Dc.DownloadAllPlatforms = this.checkBoxAllPlatforms.Checked;
                Dc.DownloadManifestOnly = this.checkBoxManifestOnly.Checked;
                Program.MainWindowForm.CreateDownloadTask(this.textBoxDownloadName.Text,Dc);
                Close();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.ParamterError, "Advanced Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
