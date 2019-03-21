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
    public partial class Log : Form
    {
        StringBuilder LogBuilder=new StringBuilder();
        public Log()
        {
            InitializeComponent();
        }

        private void Log_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static void Logx(string LogStr)
        {
            Program.LogForm.LogBuilder.AppendLine(LogStr);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (LogBuilder.Length > 0)
            {
                textBoxLog.AppendText(LogBuilder.ToString());
                LogBuilder.Clear();
                textBoxLog.ScrollToCaret();//滚动到光标处
                if (textBoxLog.Text.Length > 10240)
                {
                    textBoxLog.Text = Program.LogForm.textBoxLog.Text.Substring(9000);
                    textBoxLog.Select(Program.LogForm.textBoxLog.TextLength, 0);
                    textBoxLog.ScrollToCaret();
                }
            }
        }

        private void Log_VisibleChanged(object sender, EventArgs e)
        {
            textBoxLog.Select(Program.LogForm.textBoxLog.TextLength, 0);
            textBoxLog.ScrollToCaret();
        }
    }
}
