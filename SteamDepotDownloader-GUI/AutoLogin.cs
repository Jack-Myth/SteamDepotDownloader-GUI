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
    public partial class AutoLogin : Form
    {
        public AutoLogin(List<string> mAccountList)
        {
            InitializeComponent();
            foreach(string Account in mAccountList)
            {
                this.comboBoxAccount.Items.Add(Account);
            }
            this.comboBoxAccount.SelectedIndex = 0;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Program.UsrConfig = new DepotDownloader.UserConfig();
            Program.UsrConfig.Username = this.comboBoxAccount.Text;
            Program.UsrConfig.RememberPassword = true;
            if(DepotDownloader.ContentDownloader.InitializeSteam3(Program.UsrConfig)&&DepotDownloader.ContentDownloader.steam3.bConnected)
                SteamDepotDownloader_GUI.Program.MainWindowForm.RefreshAppList();
            else
                MessageBox.Show(Properties.Resources.AutoLoginFailed, "SteamDepotDownloader-GUI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Close();
        }
    }
}
