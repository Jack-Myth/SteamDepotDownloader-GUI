﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DepotDownloader;
using SteamDepotDownloader_GUI.Properties;
using SteamKit2;
using SteamKit2.Unified.Internal;

namespace SteamDepotDownloader_GUI
{
    public partial class SteamDepotDownloaderForm : Form
    {

        public bool UseFileRegex;
        public List<string> AllowFileList=new List<string>();
        public string InstallDir="./Download";

        public delegate void UpdateBetaPasswordRecordDelegate(uint AppID, uint DepotID, string Branch, string BetaPassword);
        public UpdateBetaPasswordRecordDelegate UbpRDelegate;
        public ulong PendingManifestID=ContentDownloader.INVALID_MANIFEST_ID;

        public SteamDepotDownloaderForm()
        {
            InitializeComponent();
            UbpRDelegate = new UpdateBetaPasswordRecordDelegate(UpdateBetaPasswordRecord);
            //Load Download record.
            foreach (DownloadRecord Dr in ConfigStore.TheConfig.DownloadRecord)
            {
                DownloadConfig Dc = new DownloadConfig();
                Dc.AppID = Dr.AppID;
                Dc.ForceDepot = !Dr.NoForceDepot;
                if (Dc.ForceDepot)
                    Dc.DepotID = Dr.DepotID;
                else
                    Dc.DepotID = ContentDownloader.INVALID_DEPOT_ID;
                Dc.Branch = Dr.BranchName;
                Dc.DownloadManifestOnly = false;
                Dc.FilesToDownload = Dr.FileToDownload;
                Dc.UsingFileList = Dr.FileToDownload==null?false:true;
                Dc.InstallDirectory = Dr.InstallDir;
                Dc.FilesToDownloadRegex = new List<System.Text.RegularExpressions.Regex>();
                Dc.DownloadAllPlatforms = Dr.AllPlatforms;
                if(Dr.AdvancedConfig)
                {
                    Dc.MaxDownloads = Dr.MaxDownload;
                    Dc.MaxServers = Dr.MaxServer;
                    Dc.ManifestId = Dr.ManifestID;
                }
                else
                {
                    Dc.MaxDownloads = ConfigStore.TheConfig.MaxDownload;
                    Dc.MaxServers = ConfigStore.TheConfig.MaxServer;
                    Dc.ManifestId = ContentDownloader.INVALID_MANIFEST_ID;
                }
                if (Dr.FileRegex != null)
                {
                    foreach (string regex in Dr.FileRegex)
                    {
                        try
                        {
                            Dc.FilesToDownloadRegex.Add(new System.Text.RegularExpressions.Regex(
                                regex, System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase));
                        }
                        catch { };
                    }
                }
                CreateDownloadTask(Dr.DownloadName, Dc, Dr.AdvancedConfig,true);
            }
        }

        public void RefreshAppList()
        {
            //Update App List
            if (ContentDownloader.Steam3 == null)
                return;
            this.appList.Items.Clear();
            this.listDepots.Items.Clear();
            this.ClearDepotInfo();
            var WaitingForm = Waiting.ShowWaiting(Properties.Resources.UpdateAppListMsg);
            ContentDownloader.Steam3.RequestFreeAppLicense(730);
            IEnumerable<uint> licenseQuery;
            licenseQuery = ContentDownloader.Steam3.Licenses.Select(x => x.PackageID);
            ContentDownloader.Steam3.RequestPackageInfo(licenseQuery);
            ContentDownloader.Steam3.FillAppsList();
            foreach (uint AppID in ContentDownloader.Steam3.AppInfo.Keys)
            {
                int itemIndex = this.appList.Items.Add(ContentDownloader.Steam3.AppInfo[AppID].KeyValues["common"]["name"].AsString());
            }
            this.appList.Tag = ContentDownloader.Steam3.AppInfo.Keys;
            this.labelGameCount.Text = string.Format(Properties.Resources.Apps, ContentDownloader.Steam3.AppInfo.Count);
            WaitingForm.Close();
        }

        private void appList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get Branches and Depots(if app have any).
            this.comboBranches.Items.Clear();
            this.listDepots.Items.Clear();
            if (appList.SelectedIndex < 0)
                return;
            var Keys = (Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo>.KeyCollection)this.appList.Tag;
            try
            {
                var DepotList =new List<uint>();
                var depotsValue = ContentDownloader.Steam3.AppInfo[Keys.ElementAt(appList.SelectedIndex)].KeyValues["depots"];
                int PublicBranchIndex = -1;
                foreach (KeyValue branches in depotsValue["branches"].Children)
                {
                    int BranchIndex=this.comboBranches.Items.Add(branches.Name);
                    if (branches.Name.Equals("public", StringComparison.CurrentCultureIgnoreCase))
                        PublicBranchIndex = BranchIndex;
                }
                if (PublicBranchIndex >= 0)
                    this.comboBranches.SelectedIndex = PublicBranchIndex;
                else if (comboBranches.Items.Count > 0)
                    this.comboBranches.SelectedIndex = 0;
                foreach (KeyValue depot in depotsValue.Children)
                {
                    uint DepotID;
                    if (uint.TryParse(depot.Name, out DepotID))
                    {
                        DepotList.Add(DepotID);
                        this.listDepots.Items.Add(depot["name"].AsString());
                    }
                }
                this.listDepots.Tag = DepotList;
            } catch (Exception) { };
            ClearDepotInfo();
            ClearPendingManifest();
        }

        private void listDepots_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listDepots.SelectedIndex < 0)
                return;
            var listDepotIDs = (List<uint>)this.listDepots.Tag;
            uint DepotID = listDepotIDs[this.listDepots.SelectedIndex];
            var Keys = (Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo>.KeyCollection)this.appList.Tag;
            var depotsValue = ContentDownloader.Steam3.AppInfo[Keys.ElementAt(appList.SelectedIndex)].KeyValues["depots"][DepotID.ToString()];
            this.labelAppID.Text = "AppID:" + Keys.ElementAt(appList.SelectedIndex).ToString();
            this.labelDepotID.Text = "DepotID:" + DepotID.ToString();
            this.labelAppName.Text = "AppName:"+ ContentDownloader.Steam3.AppInfo[Keys.ElementAt(appList.SelectedIndex)].KeyValues["common"]["name"].AsString();
            this.labelDepotName.Text = "DepotName:" + depotsValue["name"].AsString();
            this.labelOS.Text = "OS:" + depotsValue[DepotID.ToString()]["config"]["oslist"].AsString();
            this.labelDepotSize.Text = "DepotMaxSize:";
            try
            {
                float SizeInKB = long.Parse(depotsValue["maxsize"].AsString()) / 1024f;
                if (SizeInKB < 1024)
                    this.labelDepotSize.Text += SizeInKB.ToString("#0.00") + "KB";
                else if (SizeInKB / 1024 < 1024)
                    this.labelDepotSize.Text += (SizeInKB / 1024f).ToString("#0.00") + "MB";
                else
                    this.labelDepotSize.Text += (SizeInKB / 1024f / 1024f).ToString("#0.00") + "GB";
            }catch { };
            ClearPendingManifest();
        }

        private void ClearDepotInfo()
        {
            this.labelAppID.Text = "AppID:";
            this.labelDepotID.Text = "DepotID:";
            this.labelAppName.Text = "AppName:";
            this.labelDepotName.Text = "DepotName:";
            this.labelDepotSize.Text = "DepotMaxSize:";
            this.labelOS.Text = "OS:";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listDepotIDs = (List<uint>)this.listDepots.Tag;
            uint DepotID = listDepotIDs[this.listDepots.SelectedIndex];
            var Keys = (Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo>.KeyCollection)this.appList.Tag;
            uint AppID = Keys.ElementAt(appList.SelectedIndex);
            if (this.checkBox1.Checked)
            {
                (new FileSelector(AppID,DepotID,this.comboBranches.Text,PendingManifestID)).ShowDialog();
            }
        }

        public void CreateDownloadTask(string DownloadName,DownloadConfig Dc,bool AdvancedConfig,bool IsRestore)
        {
            DownloadProgressBar Dpb = new DownloadProgressBar();
            Dc.OnReportProgressEvent += Dpb.OnDownloadProgress;
            Dc.OnDownloadFinishedEvent += Dpb.OnDownloadFinished;
            Dc.OnStateChangedEvent += Dpb.OnStateChanged;
            var TargetDownloader = new ContentDownloader();
            Program.DownloaderInstances.Add(TargetDownloader);
            Dpb.RestartDownload += TargetDownloader.RestartDownload;
            Dpb.StopDownload += TargetDownloader.StopDownload;
            Dpb.CancelDownload += this.CancelDownload;
            Dpb.OnDownloadFinishedReport += this.OnDownloadFinished;
            Dpb.InitDownloading(DownloadName, Dc.AppID, Dc.DepotID, Dc.Branch);
            this.panelDownloading.Controls.Add(Dpb);
            RegroupDownloadProgressControl();
            if (!IsRestore)
            {
                DownloadRecord Dr = new DownloadRecord();
                Dr.AppID = Dc.AppID;
                Dr.DepotID = Dc.DepotID;
                Dr.BranchName = Dc.Branch;
                Dr.FileToDownload = AllowFileList;
                Dr.InstallDir = InstallDir;
                Dr.NoForceDepot = !Dc.ForceDepot;
                Dr.DownloadName = DownloadName;
                Dr.MaxDownload = Dc.MaxDownloads;
                Dr.MaxServer = Dc.MaxServers;
                Dr.AllPlatforms = Dc.DownloadAllPlatforms;
                Dr.AdvancedConfig = AdvancedConfig;
                Dr.ManifestID = Dc.ManifestId;
                if (Dc.FilesToDownloadRegex != null)
                {
                    Dr.FileRegex = new List<string>();
                    foreach (System.Text.RegularExpressions.Regex Fileregex in Dc.FilesToDownloadRegex)
                        Dr.FileRegex.Add(Fileregex.ToString());
                }
                ConfigStore.TheConfig.DownloadRecord.Add(Dr);
                ConfigStore.Save();
            }
            TargetDownloader.Config = Dc;
            if (!IsRestore)
                TargetDownloader.RestartDownload();
            else
                Dpb.Downloading = false;
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            var listDepotIDs = (List<uint>)this.listDepots.Tag;
            uint DepotID = 0;
            if (this.listDepots.SelectedIndex>=0)
                DepotID = listDepotIDs[this.listDepots.SelectedIndex];
            var Keys = (Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo>.KeyCollection)this.appList.Tag;
            uint AppID = Keys.ElementAt(appList.SelectedIndex);
            var depotsValue = ContentDownloader.Steam3.AppInfo[Keys.ElementAt(appList.SelectedIndex)].KeyValues["depots"][DepotID.ToString()];
            if (AppID == 0)
                return;
            //Check if there have any same depot are downloading
            //bool bHaveAnySame=false;
            foreach(DownloadRecord ConfigDr in ConfigStore.TheConfig.DownloadRecord)
            {
                if (ConfigDr.AppID == AppID && ConfigDr.DepotID == DepotID && ConfigDr.BranchName == this.comboBranches.Text)
                {
                    MessageBox.Show(Properties.Resources.SameTask);
                    return;
                }
            }
            DownloadConfig Dc=new DownloadConfig();
            Dc.AppID = AppID;
            Dc.Branch = this.comboBranches.Text;
            Dc.DownloadManifestOnly = this.checkBox2.Checked;
            Dc.FilesToDownload = AllowFileList;
            Dc.UsingFileList = this.checkBox1.Checked;
            Dc.InstallDirectory = InstallDir;
            Dc.ForceDepot = !this.checkBox3.Checked&&this.listDepots.SelectedIndex>=0;
            Dc.MaxDownloads = Math.Max(0, ConfigStore.TheConfig.MaxDownload);
            Dc.MaxServers = Math.Max(0, ConfigStore.TheConfig.MaxServer);
            Dc.DownloadAllPlatforms = this.checkBox4.Checked;
            Dc.ManifestId = PendingManifestID;
            if (Dc.ForceDepot)
                Dc.DepotID = DepotID;
            else
                Dc.DepotID = ContentDownloader.INVALID_DEPOT_ID;
            string DownloadName;
            if (Dc.ForceDepot)
                DownloadName = depotsValue["name"].AsString();
            else
                DownloadName = ContentDownloader.Steam3.AppInfo[Keys.ElementAt(appList.SelectedIndex)].KeyValues["common"]["name"].AsString();
            CreateDownloadTask(DownloadName,Dc,false,false);
        }

        public void UpdateBetaPasswordRecord(uint AppID,uint DepotID,string Branch,string BetaPassword)
        {
            for (int i=0;i<ConfigStore.TheConfig.DownloadRecord.Count;i++)
            {
                if (ConfigStore.TheConfig.DownloadRecord[i].AppID == AppID && 
                    ConfigStore.TheConfig.DownloadRecord[i].DepotID == DepotID && 
                    ConfigStore.TheConfig.DownloadRecord[i].BranchName == Branch)
                {
                    DownloadRecord Dr = ConfigStore.TheConfig.DownloadRecord[i];
                    Dr.BetaPassword = BetaPassword;
                    ConfigStore.TheConfig.DownloadRecord[i] = Dr;
                    ConfigStore.Save();
                    break;
                }
            }
        }

        private void OnDownloadFinished(string DownloadName,uint AppID,uint DepotID,string Branch,bool IsSuccessful,string ErrorMsg)
        {
            if(IsSuccessful)
            {
                MessageBox.Show(string.Format(Properties.Resources.DownloadSuccessful, DownloadName),"SteamDepotDownlaoder-GUI",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(ErrorMsg, string.Format(Properties.Resources.DownloadFailed, DownloadName), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach(DownloadRecord Dr in ConfigStore.TheConfig.DownloadRecord)
            {
                if(Dr.AppID==AppID&&Dr.DepotID==DepotID&&Dr.BranchName==Branch)
                {
                    ConfigStore.TheConfig.DownloadRecord.Remove(Dr);
                    ConfigStore.Save();
                    break;
                }
            }
            RegroupDownloadProgressControl();
        }

        private void CancelDownload(uint AppID,uint DepotID,string Branch,DownloadProgressBar progressBar)
        {
            foreach(ContentDownloader downloader in Program.DownloaderInstances)
            {
                if(downloader.Config.AppID==AppID&&
                    downloader.Config.DepotID==DepotID&&
                    downloader.Config.Branch==Branch)
                {
                    downloader.StopDownload();
                    Program.DownloaderInstances.Remove(downloader);
                    this.panelDownloading.Controls.Remove(progressBar);
                    foreach (DownloadRecord Dr in ConfigStore.TheConfig.DownloadRecord)
                    {
                        if (Dr.AppID == AppID && Dr.DepotID == DepotID && Dr.BranchName == Branch)
                        {
                            ConfigStore.TheConfig.DownloadRecord.Remove(Dr);
                            ConfigStore.Save();
                            break;
                        }
                    }
                    RegroupDownloadProgressControl();
                    return;
                }
            }
        }

        private void RegroupDownloadProgressControl()
        {
            List<Control> PendingRemoveControl=new List<Control>();
            foreach(DownloadProgressBar Dpb in panelDownloading.Controls)
            {
                if (Dpb.IsDownloadFinished)
                    PendingRemoveControl.Add(Dpb);
            }
            foreach (Control prc in PendingRemoveControl)
                panelDownloading.Controls.Remove(prc);
            for(int i=0;i<this.panelDownloading.Controls.Count;i++)
            {
                var Dpb = ((DownloadProgressBar)panelDownloading.Controls[i]);
                Dpb.Location = new Point(3, 3 + i * (Dpb.Height + 5));
                //Dpb.Size = new Size(198, 25);
                Dpb.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialogMain.Description = Properties.Resources.ChooseInstallDirectory;
            if(folderBrowserDialogMain.ShowDialog()==DialogResult.OK)
            {
                InstallDir = folderBrowserDialogMain.SelectedPath;
                toolTipMain.SetToolTip(this.button1, InstallDir);
            }
        }

        private void SteamDepotDownloaderForm_Load(object sender, EventArgs e)
        {
            if (ConfigStore.TheConfig.LoginKeys.Count != 0)
            {
                new AutoLogin(ConfigStore.TheConfig.LoginKeys.Keys.ToList()).ShowDialog();
            }
            if (!(ContentDownloader.Steam3 != null && ContentDownloader.Steam3.IsConnected))
                new Login().ShowDialog();
        }

        private void textBoxAppSearch_TextChanged(object sender, EventArgs e)
        {
            int Index = this.appList.FindString(this.textBoxAppSearch.Text);
            if (Index >= 0)
            {
                this.appList.SelectedIndex = Index;
                this.appList.TopIndex = Index;
            }
        }

        public string HttpGet(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                //request.CookieContainer = Cookie;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception)
            {
                //LoggerManagerSingle.Instance.Error("http get 网站出错", ex);
            }

            return "";
        }

        public void OnLoginSuccessfulAsync()
        {
            this.pictureAvatar.Image = Properties.Resources.Steam;
        }

        public void OnDisconnectAsync()
        {
            this.pictureAvatar.Image = Properties.Resources.SteamGray;
        }

        public void UpdateUserName(string UserName)
        {
            this.labelName.Text = UserName;
        }

        public void UpdateUserAvatar(string Hash)
        {
            if (Hash == "")
                Hash = "fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb";
            this.pictureAvatar.ImageLocation = string.Format("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/{0}/{1}_full.jpg", Hash.Substring(0, 2), Hash);
        }

        private void pictureAvatar_Click(object sender, EventArgs e)
        {
            if (ContentDownloader.Steam3 != null)
            {
                if(ContentDownloader.Steam3.IsConnected==false)
                {
                    ContentDownloader.ShutdownSteam3();
                    (new Login()).ShowDialog();
                    return;
                }
                if (MessageBox.Show(Properties.Resources.LogoutQuestion, "SteamDepotDownloader-GUI", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ContentDownloader.ShutdownSteam3();
                }
            }
            else
            {
                (new Login()).ShowDialog();
            }
        }

        private void buttonManuallyInput_Click(object sender, EventArgs e)
        {
            new AdvancedInput().ShowDialog();
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }

        private void SteamDepotDownloaderForm_Resize(object sender, EventArgs e)
        {
            if(WindowState==FormWindowState.Minimized)
            {
                if(ConfigStore.TheConfig.MinimizeToTray)
                {
                    this.notifyIcon1.Visible = true;
                    Hide();
                }
            }
            else
            {
                this.notifyIcon1.Visible = false;
            }
        }

        private void SteamDepotDownloaderForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (ConfigStore.TheConfig.MinimizeToTray)
                {
                    this.notifyIcon1.Visible = true;
                    Hide();
                }
            }
        }

        private void SteamDepotDownloaderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ContentDownloader.ShutdownSteam3();
            Application.Exit();
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            Program.LogForm.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        void ClearPendingManifest()
        {
            PendingManifestID = ContentDownloader.INVALID_MANIFEST_ID;
            this.buttonSelectManifest.Text = string.Format(Resources.CurrentManifestID, Resources.Default);
            if (this.comboBranches.Text.Equals(ContentDownloader.DEFAULT_BRANCH, StringComparison.OrdinalIgnoreCase))
            {
                this.buttonSelectManifest.Enabled = true;
                this.toolTipMain.SetToolTip(this.buttonSelectManifest, "");
            }
            else
            {
                this.buttonSelectManifest.Enabled = false;
                this.toolTipMain.SetToolTip(this.buttonSelectManifest, Resources.bnp_nomanifest);
            }
        }

        private void ComboBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearPendingManifest();
        }

        private void ButtonSelectManifest_Click(object sender, EventArgs e)
        {
            var listDepotIDs = (List<uint>)this.listDepots.Tag;
            uint DepotID = 0;
            if (this.listDepots.SelectedIndex >= 0)
                DepotID = listDepotIDs[this.listDepots.SelectedIndex];
            string PendingManifestIDStr = "";
            System.Diagnostics.Process.Start("https://steamdb.info/depot/" + DepotID.ToString() + "/manifests/");
            Util.InputBox("SteamDepotDownloader-GUI", Resources.InputManifesID, ref PendingManifestIDStr);
            if (PendingManifestIDStr == "")
            {
                PendingManifestID = ContentDownloader.INVALID_MANIFEST_ID;
            }
            else
            {
                if (!ulong.TryParse(PendingManifestIDStr, out PendingManifestID))
                    PendingManifestID = ContentDownloader.INVALID_MANIFEST_ID;
            }
            if (PendingManifestID == ContentDownloader.INVALID_MANIFEST_ID)
                this.buttonSelectManifest.Text = string.Format(Resources.CurrentManifestID, Resources.Default);
            else
                this.buttonSelectManifest.Text = string.Format(Resources.CurrentManifestID, PendingManifestID.ToString());
        }

        private void ButtonToolsMenu_Click(object sender, EventArgs e)
        {
            this.contextMenuStripTools.Show(PointToScreen(new Point(
                this.buttonToolsMenu.Location.X+this.buttonToolsMenu.Size.Width,
                this.buttonToolsMenu.Location.Y)));
        }

        public uint GetSelectedAppID()
        {
            if (appList.SelectedIndex < 0)
                return 0;
            var Keys = (Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo>.KeyCollection)this.appList.Tag;
            uint AppID = Keys.ElementAt(appList.SelectedIndex);
            return AppID;
        }

        public string GetSelectedAppName()
        {
            return ContentDownloader.Steam3.AppInfo[GetSelectedAppID()].KeyValues["common"]["name"].AsString();
        }

        public uint GetSelectedDepotID()
        {
            var listDepotIDs = (List<uint>)this.listDepots.Tag;
            uint DepotID = listDepotIDs[this.listDepots.SelectedIndex];
            return DepotID;
        }

        public List<uint> GetDepotsByAppID(uint AppID)
        {
            var DepotList = new List<uint>();
            var depotsValue = ContentDownloader.Steam3.AppInfo[AppID].KeyValues["depots"];
            int PublicBranchIndex = -1;
            foreach (KeyValue branches in depotsValue["branches"].Children)
            {
                int BranchIndex = this.comboBranches.Items.Add(branches.Name);
                if (branches.Name.Equals("public", StringComparison.CurrentCultureIgnoreCase))
                    PublicBranchIndex = BranchIndex;
            }
            if (PublicBranchIndex >= 0)
                this.comboBranches.SelectedIndex = PublicBranchIndex;
            else if (comboBranches.Items.Count > 0)
                this.comboBranches.SelectedIndex = 0;
            foreach (KeyValue depot in depotsValue.Children)
            {
                uint DepotID;
                if (uint.TryParse(depot.Name, out DepotID))
                {
                    DepotList.Add(DepotID);
                }
            }
            return DepotList;
        }

        public KeyValue GetDepotValue(uint AppID, uint DepotID)
        {
            return ContentDownloader.Steam3.AppInfo[AppID].KeyValues["depots"][DepotID.ToString()];
        }

        private void AppmanifestGeneratorToolStripMenuAG_Click(object sender, EventArgs e)
        {
            if (ContentDownloader.Steam3==null||!ContentDownloader.Steam3.IsConnected)
            {
                MessageBox.Show(Resources.NeedLogin,Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            uint AppID = GetSelectedAppID();
            if (AppID!=0)
                new AppmanifestGenerator(AppID).ShowDialog();
        }
    }
}
