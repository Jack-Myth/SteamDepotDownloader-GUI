using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DepotDownloader
{
    public class DownloadConfig
    {
        public bool   DownloadAllPlatforms { get; set; }
        public bool   DownloadManifestOnly { get; set; }
        public string InstallDirectory { get; set; }

        public bool         UsingFileList        { get; set; } = false;
        public List<string> FilesToDownload      { get; set; } = new List<string>();
        public List<Regex>  FilesToDownloadRegex { get; set; } = new List<Regex>();


        public string BetaPassword { get; set; }

        public ulong  ManifestId   { get; set; } = ContentDownloader.INVALID_MANIFEST_ID;

        public bool   VerifyAll    { get; set; }

        public int MaxServers   { get; set; } = 8;
        public int MaxDownloads { get; set; } = 4;

        public uint   AppID      { get; set; } = ContentDownloader.INVALID_APP_ID;
        public string Branch     { get; set; } = "public";
        public string OS         { get; set; } = null;
        
        public bool   ForceDepot { get; set; } = false;
        public uint   DepotID    { get; set; } = ContentDownloader.INVALID_DEPOT_ID;

        /// <summary>
        /// Call when downloading progress changed
        /// </summary>
        public event Action<float,string> OnReportProgressEvent;

        public event Action<bool> OnStateChangedEvent;

        public event Action<bool,string> OnDownloadFinishedEvent;

        /// <summary>
        /// 1 - type    (log, warning, error, exception)
        /// 2 - message (exception - Exception object, in other string)
        /// </summary>
        public event Action<string, object> OnMessageEvent;

        /// <summary>
        /// 1 - install directory
        /// 2 - full path
        /// </summary>
        public Func<string, string> SavePathProcessor;

        internal void FireReportProgressEvent(float Percent,string CurrentFileName)
        {
            OnReportProgressEvent?.Invoke(Percent,CurrentFileName);
        }

        internal void FireOnStateChangedEvent(bool IsDownloading)
        {
            OnStateChangedEvent?.Invoke(IsDownloading);
        }

        internal void FireOnDownloadFinishedEvent(bool IsSuccessful, string ErrorMsg)
        {
            OnDownloadFinishedEvent?.Invoke(IsSuccessful, ErrorMsg);
        }

        internal void FireOnMessageEvent(string type, object message)
        {
            OnMessageEvent?.Invoke(type, message);
        }
    }

    public class UserConfig
    {
        public int CellID { get; set; }
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
        public string SuppliedPassword { get; set; }
        public string TwoFactorAuthCode { get; set; } = null;
        public bool RememberPassword { get; set; } = false;
    }
}
