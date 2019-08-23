using DepotDownloader;
using SteamDepotDownloader_GUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
Refresh:
            this.Text = Resources.GettingManifestHistory;
            if (!(ConfigStore.TheConfig.StoredCookies.ContainsKey("__cfduid")&&
                ConfigStore.TheConfig.StoredCookies.ContainsKey("cf_clearance")))
            {
                do
                {
                    string CookieStr = "__cfduid=cookie1;cf_clearance=cookie2";
                    System.Diagnostics.Process.Start("https://steamdb.info");
                    if (Util.InputBox(Resources.InputSteamDBCookies, Resources.InputSteamDBCookies, ref CookieStr) == DialogResult.OK)
                    {
                        string[] Cookies = CookieStr.Split(';');
                        Dictionary<string, string> tmpCookieContainner = new Dictionary<string, string>();
                        for (int i = 0; i < Cookies.Length; i++)
                        {
                            string[] CookieItem = Cookies[i].Split('=');
                            if (CookieItem.Length < 2)
                                break;
                            tmpCookieContainner.Add(CookieItem[0], CookieItem[1]);
                        }
                        if (tmpCookieContainner.ContainsKey("__cfduid") && tmpCookieContainner.ContainsKey("cf_clearance"))
                        {
                            ConfigStore.TheConfig.StoredCookies["__cfduid"] = tmpCookieContainner["__cfduid"];
                            ConfigStore.TheConfig.StoredCookies["cf_clearance"] = tmpCookieContainner["cf_clearance"];
                            ConfigStore.Save();
                            break;
                        }
                    }
                    MessageBox.Show(Resources.InvalidCookies, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } while (false);
            }
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.Cookie,
                string.Format("__cfduid={0}; cf_clearance={1}", ConfigStore.TheConfig.StoredCookies["__cfduid"],
                ConfigStore.TheConfig.StoredCookies["cf_clearance"]));
            webClient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.19 Safari/537.36 Edg/77.0.235.9");
            //webClient.Headers.Add(HttpRequestHeader.re)
            string webPageStr;
            try
            {
                webPageStr = await webClient.DownloadStringTaskAsync("https://steamdb.info/depot/" + PendingDepotID.ToString() + "/manifests/");
            }
            catch
            {
                if (MessageBox.Show(Resources.FailedToGetManifestHistory + "\n" + Resources.refreshCookieRequest
                        , "SteamDepotDownloaderGUI", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    ConfigStore.TheConfig.StoredCookies.Remove("__cfduid");
                    ConfigStore.TheConfig.StoredCookies.Remove("cf_clearance");
                    goto Refresh;
                }
                PendingReturnManifestID = ContentDownloader.INVALID_MANIFEST_ID;
                Close();
                return;
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(webPageStr);
            //var doc = await webClient.LoadFromWebAsync("https://steamdb.info/depot/" + PendingDepotID.ToString() + "/manifests/");
            if (!Program.ManifestHistoryCache.ContainsKey(PendingDepotID))
            {
                ManifestHistoryRecord manifestHistoryRecord= new ManifestHistoryRecord();
                var DepotInfoNode = doc.DocumentNode.SelectNodes("//div[@class='wrapper-info scope-depot']/div/table/tbody/tr");
                //Check if xpath is succeed.
                if (DepotInfoNode.Count != 0)
                {
                    //The sequence for these data seems to be fixed,so use Index directly.
                    //Actually maybe use ChildNodes[0] will be better?
                    manifestHistoryRecord.DepotID = DepotInfoNode[0].ChildNodes[3].GetDirectInnerText();
                    manifestHistoryRecord.BuildID = DepotInfoNode[1].ChildNodes[3].GetDirectInnerText();
                    manifestHistoryRecord.ManifestID = DepotInfoNode[2].ChildNodes[3].GetDirectInnerText();
                    manifestHistoryRecord.DepotName = DepotInfoNode[3].ChildNodes[3].GetDirectInnerText();
                    manifestHistoryRecord.LastUpdate = DepotInfoNode[4].SelectSingleNode("//i").GetDirectInnerText();
                }
                DepotInfoNode = doc.DocumentNode.SelectNodes("//div[@id='manifests']/div[@class='table-responsive']/table/tbody/tr");
                if (DepotInfoNode.Count == 0)
                {
                    if(MessageBox.Show(Resources.FailedToGetManifestHistory+"\n"+Resources.refreshCookieRequest
                        , "SteamDepotDownloaderGUI", MessageBoxButtons.YesNo, MessageBoxIcon.Error)==DialogResult.Yes)
                    {
                        ConfigStore.TheConfig.StoredCookies.Remove("__cfduid");
                        ConfigStore.TheConfig.StoredCookies.Remove("cf_clearance");
                        goto Refresh;
                    }
                    PendingReturnManifestID = ContentDownloader.INVALID_MANIFEST_ID;
                    Close();
                    return;
                }
                manifestHistoryRecord.HistoryCollection = new List<ManifestHistoryRecord.ManifestHistoryRecordSingle>();
                for (int i = 0; i < DepotInfoNode.Count; i++)
                {
                    ManifestHistoryRecord.ManifestHistoryRecordSingle tmpRecord;
                    //Text,Tr,Text,Tr,Text,Tr,Text
                    //We only need,1,3,5
                    tmpRecord.Date = DepotInfoNode[i].ChildNodes[1].GetDirectInnerText();
                    var relativeDate = DateTime.Now - DateTime.Parse(DepotInfoNode[i].ChildNodes[3].Attributes["title"].Value);
                    string relativeDateStr;
                    if (relativeDate.TotalDays > 365)
                        relativeDateStr = ((int)(relativeDate.TotalDays / 365)).ToString() + " Year" + (relativeDate.TotalDays/365 >= 2 ? "s" : "") + " Ago";
                    else if (relativeDate.TotalDays>30)
                        relativeDateStr = ((int)(relativeDate.TotalDays / 30)).ToString() + " Month" + (relativeDate.TotalDays/30 >= 2 ? "s" : "") + " Ago";
                    else
                        relativeDateStr = ((int)relativeDate.TotalDays).ToString() + " Day" + (relativeDate.TotalDays >= 2 ? "s" : "") + " Ago";
                    tmpRecord.RelativeDate = relativeDateStr;
                    tmpRecord.ManifestID = ulong.Parse(DepotInfoNode[i].ChildNodes[5].GetDirectInnerText());
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
            labelLastUpdate.Text = string.Format(Resources.mids_LastUpdate, ManifestRecord.LastUpdate);
            this.listViewManifests.Items.Clear();
            for (int i=0;i<ManifestRecord.HistoryCollection.Count;i++)
            {
                ListViewItem curHistory = new ListViewItem();
                curHistory.Text = ManifestRecord.HistoryCollection[i].Date;
                curHistory.SubItems.Add(ManifestRecord.HistoryCollection[i].RelativeDate);
                curHistory.SubItems.Add(ManifestRecord.HistoryCollection[i].ManifestID.ToString());
                this.listViewManifests.Items.Add(curHistory);
            }
            this.Text = "ManifestID Selelctor";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (this.listViewManifests.SelectedItems.Count != 0)
                PendingReturnManifestID = ulong.Parse(this.listViewManifests.SelectedItems[0].SubItems[2].Text);
            else
                PendingReturnManifestID = ContentDownloader.INVALID_MANIFEST_ID;
            Close();
        }
    }
}
