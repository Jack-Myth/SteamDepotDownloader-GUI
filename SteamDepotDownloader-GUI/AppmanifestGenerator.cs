using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DepotDownloader;

namespace SteamDepotDownloader_GUI
{
    public partial class AppmanifestGenerator : Form
    {
        private uint mAppID;
        private List<uint> DepotList;
        public AppmanifestGenerator(uint AppID)
        {
            InitializeComponent();
            mAppID = AppID;
            this.labelAppID.Text = "AppID:" + AppID.ToString();
            this.textBoxName.Text = Program.MainWindowForm.GetSelectedAppName();
            this.textBoxInstallDir.Text = Program.MainWindowForm.GetSelectedAppName();
            DepotList = Program.MainWindowForm.GetDepotsByAppID(AppID);
            for (int i = 0; i < DepotList.Count; i++)
            {
                string DepotName = Program.MainWindowForm.GetDepotValue(AppID,DepotList[i])["name"].AsString();
                this.checkedListBoxDepots.Items.Add(DepotName);
            }
        }
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }

        private void ButtonGen_Click(object sender, EventArgs e)
        {
            string PendingExportStr="";
            PendingExportStr = "\"AppState\"\n{\n";
            PendingExportStr += "\t\"appid\"\t\t\"" + this.mAppID.ToString() + "\"\n";
            PendingExportStr += "\t\"Universe\"\t\t\"1\"\n";
            PendingExportStr += "\t\"StateFlags\"\t\t\"4\"\n";
            PendingExportStr += "\t\"installdir\"\t\t\"" + this.textBoxInstallDir.Text + "\"\n";
            PendingExportStr += "\t\"LastUpdated\"\t\t\"" + ConvertDateTimeInt(DateTime.Now).ToString()+ "\"\n";
            PendingExportStr += "\t\"UpdateResult\"\t\t\"0\"\n";
            PendingExportStr += "\t\"SizeOnDisk\"\t\t\"1\"\n";
            PendingExportStr += "\t\"buildid\"\t\t\"1\"\n";
            PendingExportStr += "\t\"BytesToDownload\"\t\t\"1\"\n";
            PendingExportStr += "\t\"BytesDownloaded\"\t\t\"1\"\n";
            PendingExportStr += "\t\"AutoUpdateBehavior\"\t\t\"1\"\n";
            PendingExportStr += "\t\"AllowOtherDownloadsWhileRunning\"\t\t\"0\"\n";
            PendingExportStr += "\t\"ScheduledAutoUpdate\"\t\t\"0\"\n";
            PendingExportStr += "\t\"InstalledDepots\"\n\t{\n";
            for (int i = 0; i < this.checkedListBoxDepots.Items.Count; i++)
            {
                if (this.checkedListBoxDepots.GetItemChecked(i))
                {
                    string Password = "";
                    PendingExportStr += "\t\t\"" + DepotList[i].ToString() + "\"\n\t\t{\n";
                    PendingExportStr+="\t\t\t\"manifest\"\t\t\""+  
                                      ContentDownloader.GetSteam3DepotManifestStatic(DepotList[i], mAppID, "public", ref Password)+"\"\n\t\t}\n";
                }
            }
            PendingExportStr += "\t}\n\t\"MountedDepots\"\n\t{\n";
            for (int i = 0; i < this.checkedListBoxDepots.Items.Count; i++)
            {
                if (this.checkedListBoxDepots.GetItemChecked(i))
                {
                    string Password = "";
                    PendingExportStr += "\t\t\"" + DepotList[i].ToString() + "\"";
                    PendingExportStr += "\t\t\"" +
                                        ContentDownloader.GetSteam3DepotManifestStatic(DepotList[i], mAppID, "public", ref Password) + "\"\n";
                }
            }

            PendingExportStr += "\t}\n}";
            this.saveFileDialog1.FileName = "appmanifest_" + mAppID.ToString() + ".acf";
            if (this.saveFileDialog1.ShowDialog()==DialogResult.OK)
                System.IO.File.WriteAllText(this.saveFileDialog1.FileName, PendingExportStr);
        }
    }
}
