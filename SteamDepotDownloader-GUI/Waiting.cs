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
    public partial class Waiting : Form
    {
        public static Waiting ShowWaiting(string Message)
        {
            Waiting WaitingForm = new Waiting();
            WaitingForm.WaitingMsg.Text = Message;
            WaitingForm.Show();
            return WaitingForm;
        }
        public Waiting()
        {
            InitializeComponent();
            this.ControlBox = false;
        }
    }
}
