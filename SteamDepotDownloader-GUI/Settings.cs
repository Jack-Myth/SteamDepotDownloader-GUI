using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamDepotDownloader_GUI
{
    public partial class Settings : Form
    {
        int MaxDownload;
        int MaxServer;
        bool MinimizeToTray;
        public int Clamp(int A,int B,int V)
        {
            return Math.Min(Math.Max(V, A), B);
        }
        public Settings()
        {
            InitializeComponent();
            this.LabelVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MaxDownload = DepotDownloader.ConfigStore.TheConfig.MaxDownload - 1;
            MaxServer = DepotDownloader.ConfigStore.TheConfig.MaxServer - 1;
            MinimizeToTray = DepotDownloader.ConfigStore.TheConfig.MinimizeToTray;
            this.comboBoxMaxDownload.SelectedIndex = Clamp(0, 19, MaxDownload);
            this.comboBoxMaxServer.SelectedIndex = Clamp(0, 19, MaxServer);
            this.checkBoxNotify.Checked = MinimizeToTray;
        }

        private void buttonClearCache_Click(object sender, EventArgs e)
        {
            var FileList = System.IO.Directory.GetFiles(Program.CacheDir);
            foreach (string FileName in FileList)
            {
                try
                {
                    System.IO.File.Delete(FileName);
                }
                catch { };
            }
            DepotDownloader.ConfigStore.TheConfig.LoginKeys.Clear();
            DepotDownloader.ConfigStore.TheConfig.LastManifests.Clear();
            DepotDownloader.ConfigStore.Save();
            MessageBox.Show(Properties.Resources.CacheCleared, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void comboBoxMaxServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int MaxServertmp = int.Parse(comboBoxMaxServer.SelectedText);
                if (MaxServer != MaxServertmp)
                {
                    MaxServer = MaxServertmp;
                    DepotDownloader.ConfigStore.TheConfig.MaxServer = MaxServer;
                    DepotDownloader.ConfigStore.Save();
                }
            }
            catch { };
        }

        private void comboBoxMaxDownload_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int MaxDownloadtmp = int.Parse(comboBoxMaxDownload.SelectedText);
                if (MaxDownload != MaxDownloadtmp)
                {
                    MaxDownload = MaxDownloadtmp;
                    DepotDownloader.ConfigStore.TheConfig.MaxServer = MaxServer;
                    DepotDownloader.ConfigStore.Save();
                }
            }
            catch { };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Jack-Myth/SteamDepotDownloader-GUI/releases");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Jack-Myth/SteamDepotDownloader-GUI");
        }

        private void checkBoxNotify_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBoxNotify.Checked!=MinimizeToTray)
            {
                MinimizeToTray = this.checkBoxNotify.Checked;
                DepotDownloader.ConfigStore.TheConfig.MinimizeToTray = this.checkBoxNotify.Checked;
                DepotDownloader.ConfigStore.Save();
            }
        }
    }
}
