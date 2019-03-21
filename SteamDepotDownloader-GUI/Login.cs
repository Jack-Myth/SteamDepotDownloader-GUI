using DepotDownloader;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Program.UsrConfig = new UserConfig();
            Program.UsrConfig.Username = this.textBoxAccountName.Text;
            Program.UsrConfig.Password = this.textBoxPassword.Text;
            Program.UsrConfig.RememberPassword = this.checkBoxRememberPass.Checked;
            Program.UsrConfig.TwoFactorAuthCode = this.textBoxTwoFractorAuthCode.Text;
            if (DepotDownloader.ContentDownloader.InitializeSteam3(Program.UsrConfig)&&DepotDownloader.ContentDownloader.steam3.bConnected)
            {
                Close();
                SteamDepotDownloader_GUI.Program.MainWindowForm.RefreshAppList();
            }
            else
                MessageBox.Show(Properties.Resources.LoginFailed, "SteamDepotDownloader-GUI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
