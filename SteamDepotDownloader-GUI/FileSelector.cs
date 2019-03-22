using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DepotDownloader;

namespace SteamDepotDownloader_GUI
{
    public partial class FileSelector : Form
    {
        public const string CachedDir = "./CachedDir";
        ulong ManifestID = ContentDownloader.INVALID_MANIFEST_ID;
        uint AppID;
        uint DepotID;
        string Branch;
        DepotDownloader.ProtoManifest depotManifest;
        ulong DepotMaxSize=0;
        float SizeDivisor = 1024f;
        string UnitStr = "KB";

        public FileSelector(uint appId,uint depotId,string branch)
        {
            InitializeComponent();
            this.ControlBox = false;
            AppID = appId;
            DepotID = depotId;
            Branch = branch;
        }

        private async void FileSelector_LoadAsync(object sender, EventArgs e)
        {
            string Password = "";
            ManifestID = ContentDownloader.GetSteam3DepotManifestStatic(DepotID, AppID, Branch,ref Password);
            if (ManifestID == ContentDownloader.INVALID_MANIFEST_ID)
            {
                MessageBox.Show(Properties.Resources.NoManifestID, "FileSelector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            System.IO.Directory.CreateDirectory(Program.CacheDir);
            var localProtoManifest = DepotDownloader.ProtoManifest.LoadFromFile(string.Format("{0}/{1}.bin",Program.CacheDir, ManifestID));
            if (localProtoManifest!=null)
            {
                depotManifest = localProtoManifest;
            }
            else
            {
                this.Text = "Downloading File List...";
                this.Refresh();
                ContentDownloader.steam3.RequestDepotKey(DepotID,AppID);
                if(!ContentDownloader.steam3.DepotKeys.ContainsKey(DepotID))
                {
                    MessageBox.Show(Properties.Resources.NoDepotKey, "FileSelector",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    //return;
                    //User may still need to use FileRegex
                    Close();
                }
                byte[] depotKey = ContentDownloader.steam3.DepotKeys[DepotID];
                //ContentDownloader.steam3.RequestAppTicket(AppID);
                ContentDownloader.steam3.RequestAppTicket(DepotID);
                var client = await DepotDownloader.ContentDownloader.GlobalCDNPool.GetConnectionForDepotAsync(AppID, DepotID, depotKey, System.Threading.CancellationToken.None).ConfigureAwait(true);
                var SteamKitdepotManifest = await client.DownloadManifestAsync(DepotID, ManifestID).ConfigureAwait(true);
                if (SteamKitdepotManifest != null)
                {
                    localProtoManifest = new ProtoManifest(SteamKitdepotManifest, ManifestID);
                    localProtoManifest.SaveToFile(string.Format("{0}/{1}.bin", Program.CacheDir, ManifestID));
                    depotManifest = localProtoManifest;
                }
            }
            if(depotManifest!=null)
            {
                foreach (var file in localProtoManifest.Files)
                {
                    //Process Dir
                    var FilePathSplited = file.FileName.Split('\\');
                    List<string> FilePathList = new List<string>(FilePathSplited);
                    TreeNode LastNode=null;
                    TreeNodeCollection FileNodes = this.treeViewFileList.Nodes;
                    while (FilePathList.Count != 0)
                    {
                        TreeNode TargetNode = null;
                        foreach (TreeNode Tn in FileNodes)
                        {
                            if (Tn.Text == FilePathList[0])
                            {
                                TargetNode = Tn;
                                break;
                            }
                        }
                        if (TargetNode == null)
                        {
                            LastNode = FileNodes.Add(FilePathList[0]);
                            FileNodes = LastNode.Nodes;
                        }
                        else
                        {
                            FileNodes = TargetNode.Nodes;
                        }
                        FilePathList.RemoveAt(0);
                    }
                    if(file.Flags!=SteamKit2.EDepotFileFlag.Directory)
                    {
                        DepotMaxSize += file.TotalSize;
                        //if(LastNode!=null)
                        LastNode.Tag = file.TotalSize;
                    }
                }
                float TargetSize = DepotMaxSize / 1024f;
                if(TargetSize<1024)
                {
                    SizeDivisor = 1024f;
                    UnitStr = "KB";
                }
                else if(TargetSize/1024f<1024)
                {
                    SizeDivisor = 1024 * 1024f;
                    UnitStr = "MB";
                }
                else
                {
                    SizeDivisor = 1024 * 1024 * 1024f;
                    UnitStr = "GB";
                }
                this.labelSize.Text = string.Format("{0}{1}/{2}{1}", 0.ToString("#0.00"), UnitStr, (DepotMaxSize / SizeDivisor).ToString("#0.00"));
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Program.MainWindowForm.checkBox1.Checked = false;
            Close();
        }

        private void ApplyFileList(TreeNode Tn)
        {
            if(Tn.Nodes.Count==0)
            {
                //This is a file.
                if(Tn.Checked)
                    Program.MainWindowForm.AllowFileList.Add(Tn.FullPath);
            }
            else
            {
                //This is a Dir
                foreach (TreeNode TnChild in Tn.Nodes)
                {
                    ApplyFileList(TnChild);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach(TreeNode Tn in this.treeViewFileList.Nodes)
            {
                ApplyFileList(Tn);
            }
            Close();
        }

        /// <summary>
        /// 设置所有父节点的选中状态
        /// </summary>
        /// <param name="currNode"></param>
        /// <param name="state"></param>
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;

            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }

        private void updateParentNodeCheckedState(TreeNode currNode)
        {
            TreeNode parentNode = currNode.Parent;
            if (parentNode == null)
                return;
            foreach(TreeNode Tn in parentNode.Nodes)
            {
                if (Tn.Checked)
                {
                    parentNode.Checked = true;
                    return;
                }
            }
            parentNode.Checked = false;
        }

        private ulong UpdateFileSize(TreeNode Tn)
        {
            if (Tn.Nodes!=null&&Tn.Nodes.Count == 0)
            {
                return Tn.Checked ? (ulong)Tn.Tag : 0;
            }
            else
            {
                ulong FolderSize=0;
                //This is a Dir
                foreach (TreeNode TnChild in Tn.Nodes)
                {
                    FolderSize+=UpdateFileSize(TnChild);
                }
                return FolderSize;
            }
        }

        /// <summary>
        /// 设置所有子节点的选中状态
        /// </summary>
        /// <param name="currNode"></param>
        /// <param name="state"></param>
        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes!=null&&nodes.Count > 0)
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
        }

        private void treeViewFileList_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    //节点选中后，选中所有子节点
                    setChildNodeCheckedState(e.Node, true);

                    //节点选中之后，选中所有父节点
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, true);
                    }
                }
                else
                {
                    //取消节点选中后，取消所有子节点的选中状态
                    setChildNodeCheckedState(e.Node, false);
                    updateParentNodeCheckedState(e.Node);
                }
            }
            {
                //Update FileSize
                ulong TotalSize = 0;
                foreach (TreeNode Tn in this.treeViewFileList.Nodes)
                    TotalSize += UpdateFileSize(Tn);
                this.labelSize.Text = string.Format("{0}{1}/{2}{1}", (TotalSize / SizeDivisor).ToString("#0.00"), UnitStr, (DepotMaxSize / SizeDivisor).ToString("#0.00"));
            }
        }

        private void checkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode Tn in this.treeViewFileList.Nodes)
            {
                if (Tn.Nodes != null && Tn.Nodes.Count > 0)
                    setChildNodeCheckedState(Tn, this.checkSelectAll.Checked);
                else
                    Tn.Checked = this.checkSelectAll.Checked;
            }
        }
    }
}
