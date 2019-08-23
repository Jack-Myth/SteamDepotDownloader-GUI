using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamDepotDownloader_GUI
{

    public struct ManifestHistoryRecord
    {
        public struct ManifestHistoryRecordSingle
        {
            public string Date;
            public string RelativeDate;
            public ulong ManifestID;
        }
        public string DepotID;
        public string BuildID;
        public string ManifestID;
        public string DepotName;
        public string LastUpdate;
        public List<ManifestHistoryRecordSingle> HistoryCollection;
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static DepotDownloader.UserConfig UsrConfig;
        public static SteamDepotDownloaderForm MainWindowForm;
        public static Log LogForm;
        public const string CacheDir = "./Cache";

        public static Dictionary<uint, ManifestHistoryRecord> ManifestHistoryCache = new Dictionary<uint, ManifestHistoryRecord>();

        public static List<DepotDownloader.ContentDownloader> DownloaderInstances=new List<DepotDownloader.ContentDownloader>();
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DepotDownloader.ConfigStore.LoadFromFile(Path.Combine(Directory.GetCurrentDirectory(), "DepotDownloader.config"));
            LogForm = new Log();
            MainWindowForm = new SteamDepotDownloaderForm();
            Application.Run(MainWindowForm);
            Application.Exit();
        }
    }
}
