using DepotDownloader;
using SteamDepotDownloader_GUI.Properties;
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
    public partial class ManifestIDSelector : Form
    {
        private ulong PendingReturnManifestID = ContentDownloader.INVALID_MANIFEST_ID;
        private uint PendingDepotID=ContentDownloader.INVALID_DEPOT_ID;
        string CallbackNotify = "";

        public ManifestIDSelector(uint DepotID)
        {
            InitializeComponent();
            PendingDepotID = DepotID;
        }
        public static ulong ChooseManifestID(uint DepotID)
        {
            ManifestIDSelector manifestIDSelector = new ManifestIDSelector(DepotID);
            manifestIDSelector.ShowDialog();
            return manifestIDSelector.PendingReturnManifestID;
        }

        private async void ManifestIDSelector_LoadAsync(object sender, EventArgs e)
        {
            this.Text = Resources.GettingManifestHistory;
            HtmlAgilityPack.HtmlWeb webClient = null;
            if (!Program.htmlWebWeakInstance.TryGetTarget(out webClient))
            {
                AZ.MiniBlinkNet.WebView wv = new AZ.MiniBlinkNet.WebView();
                int NavTimes = 0;
                wv.OnNavigate += OnNavigate;
                wv.LoadURL("https://steamdb.info/depot/" + PendingDepotID.ToString() + "/manifests/");
                var watingDate = DateTime.Now;
                while ((DateTime.Now - watingDate).TotalSeconds <= 20)
                {
                    if(CallbackNotify!="")
                    {
                        string CookieStr = wv.GetCookie();
                        string[] Cookies = CookieStr.Split(";".ToCharArray());
                        if (Cookies.Length < 2)  //Check if cookie has already existed.
                        {
                            if (CallbackNotify.Contains("chk_jschl"))
                            {
                                CallbackNotify = "";
                                continue;
                            }
                            if (NavTimes < 1)
                            {
                                NavTimes++;
                                continue;
                            }
                        }
                        webClient = new HtmlAgilityPack.HtmlWeb();
                        webClient.PreRequest = request =>
                        {
                            for (int i = 0; i < Cookies.Length; i++)
                            {
                                string[] CookieItem = Cookies[i].Split('=');
                                request.CookieContainer.Add(new System.Net.Cookie(CookieItem[0], CookieItem[1]));
                            }
                            return true;
                        };
                    }

                    if (webClient != null)
                    {
                        Program.htmlWebWeakInstance.SetTarget(webClient);
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                }
                if (webClient == null)
                {
                    MessageBox.Show(Resources.dbconnectfailed, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
            }
            var doc = await webClient.LoadFromWebAsync("https://steamdb.info/depot/" + PendingDepotID.ToString() + "/manifests/");
            if (!Program.ManifestHistoryCache.ContainsKey(PendingDepotID))
            {
                ManifestHistoryRecord manifestHistoryRecord= new ManifestHistoryRecord();
                var DepotInfoNode = doc.DocumentNode.SelectNodes("//div[@class='wrapper-info scope-depot']/div/table/tbody/tr");
                //Check if xpath is succeed.
                if (DepotInfoNode.Count != 0)
                {
                    //The sequence for these data seems to be fixed,so use Index directly.
                    //Actually maybe use ChildNodes[0] will be better?
                    manifestHistoryRecord.DepotID = DepotInfoNode[0].ChildNodes[1].GetDirectInnerText();
                    manifestHistoryRecord.BuildID = DepotInfoNode[1].ChildNodes[1].GetDirectInnerText();
                    manifestHistoryRecord.ManifestID = DepotInfoNode[2].ChildNodes[1].GetDirectInnerText();
                    manifestHistoryRecord.DepotName = DepotInfoNode[3].ChildNodes[1].GetDirectInnerText();
                    manifestHistoryRecord.LastUpdate = DepotInfoNode[4].ChildNodes[1].ChildNodes[0].GetDirectInnerText() +
                        DepotInfoNode[4].ChildNodes[1].ChildNodes[0].GetDirectInnerText();
                }
                DepotInfoNode = doc.DocumentNode.SelectNodes("//div[@id='manifests']/div[@class='table-responsive']/table/tbody/tr");
                if (DepotInfoNode.Count == 0)
                {
                    MessageBox.Show(Resources.FailedToGetManifestHistory, "SteamDepotDownloaderGUI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    PendingReturnManifestID = ContentDownloader.INVALID_MANIFEST_ID;
                    Close();
                    return;
                }
                manifestHistoryRecord.HistoryCollection = new List<ManifestHistoryRecord.ManifestHistoryRecordSingle>();
                for (int i = 0; i < DepotInfoNode.Count; i++)
                {
                    ManifestHistoryRecord.ManifestHistoryRecordSingle tmpRecord;
                    tmpRecord.Date = DepotInfoNode[i].ChildNodes[0].GetDirectInnerText();
                    tmpRecord.RelativeDate = DepotInfoNode[i].ChildNodes[1].GetDirectInnerText();
                    tmpRecord.ManifestID = ulong.Parse(DepotInfoNode[i].ChildNodes[2].GetDirectInnerText());
                    manifestHistoryRecord.HistoryCollection.Add(tmpRecord);
                }
                Program.ManifestHistoryCache.Add(PendingDepotID, manifestHistoryRecord);
            }
            var ManifestRecord = Program.ManifestHistoryCache[PendingDepotID];
            //The sequence for these data seems to be fixed,so use Index directly.
            //Actually maybe use ChildNodes[0] will be better?
            labelDepotID.Text = string.Format(Resources.mids_DepotID, ManifestRecord.DepotID);
            labelBuildID.Text = string.Format(Resources.mids_BuildID, ManifestRecord.BuildID);
            labelManifestID.Text = string.Format(Resources.mids_ManifestID, ManifestRecord.ManifestID);
            labelDepotName.Text = string.Format(Resources.mids_DepotName, ManifestRecord.DepotName);
            labelLastUpdate.Text = string.Format(Resources.mids_ManifestID, ManifestRecord.LastUpdate);
            for(int i=0;i<ManifestRecord.HistoryCollection.Count;i++)
            {
                ListViewItem curHistory = new ListViewItem();
                curHistory.Text = ManifestRecord.HistoryCollection[i].Date;
                curHistory.SubItems.Add(ManifestRecord.HistoryCollection[i].RelativeDate);
                curHistory.SubItems.Add(ManifestRecord.HistoryCollection[i].ManifestID.ToString());
                this.listViewManifests.Items.Add(curHistory);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (this.listViewManifests.SelectedItems.Count != 0)
                PendingReturnManifestID = ulong.Parse(this.listViewManifests.SelectedItems[0].SubItems[1].Text);
            else
                PendingReturnManifestID = ContentDownloader.INVALID_MANIFEST_ID;
            Close();
        }

        private void OnNavigate(object sender,AZ.MiniBlinkNet.NavigateEventArgs e)
        {
            CallbackNotify = e.URL;
        }
    }
}
