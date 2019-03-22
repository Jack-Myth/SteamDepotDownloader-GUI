using System;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using System.IO.Compression;

namespace DepotDownloader
{
    [ProtoContract]
    struct DownloadRecord
    {
        [ProtoMember(1)]
        public string DownloadName;
        [ProtoMember(2)]
        public uint AppID;
        [ProtoMember(3)]
        public uint DepotID;
        [ProtoMember(4)]
        public string BranchName;
        [ProtoMember(5)]
        public string InstallDir;
        [ProtoMember(6, IsRequired = false)]
        public string BetaPassword;
        [ProtoMember(7, IsRequired = false)]
        public List<string> FileRegex;
        [ProtoMember(8, IsRequired = false)]
        public List<string> FileToDownload;
        [ProtoMember(9, IsRequired = false)]
        public bool NoForceDepot;
        [ProtoMember(10, IsRequired = false)]
        public int MaxServer;
        [ProtoMember(11, IsRequired = false)]
        public int MaxDownload;
        [ProtoMember(12, IsRequired = false)]
        public bool AllPlatforms;
        [ProtoMember(13, IsRequired = false)]
        public bool AdvancedConfig;
    }

    [ProtoContract]
    internal class ConfigStore
    {
        [ProtoMember(1)]
        public List<DownloadRecord> DownloadRecord { get; set; }

        [ProtoMember(3, IsRequired=false)]
        public Dictionary<string, byte[]> SentryData { get; private set; }

        [ProtoMember(4, IsRequired = false)]
        public System.Collections.Concurrent.ConcurrentDictionary<string, int> ContentServerPenalty { get; private set; }

        [ProtoMember(5, IsRequired = false)]
        public Dictionary<string, string> LoginKeys { get; private set; }

        [ProtoMember(6)]
        public Dictionary<uint, ulong> LastManifests { get; set; }

        [ProtoMember(7, IsRequired = false)]
        public int MaxServer = 8;
        [ProtoMember(8, IsRequired = false)]
        public int MaxDownload = 4;

        [ProtoMember(9, IsRequired = false)]
        public bool MinimizeToTray = false;

        string FileName = null;

        ConfigStore()
        {
            DownloadRecord = new List<DownloadRecord>();
            SentryData = new Dictionary<string, byte[]>();
            ContentServerPenalty = new System.Collections.Concurrent.ConcurrentDictionary<string, int>();
            LoginKeys = new Dictionary<string, string>();
            LastManifests = new Dictionary<uint, ulong>();
        }

        static bool Loaded
        {
            get { return TheConfig != null; }
        }

        public static ConfigStore TheConfig;

        public static void LoadFromFile(string filename)
        {
            if (Loaded)
                throw new Exception("Config already loaded");

            if (File.Exists(filename))
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                using (DeflateStream ds = new DeflateStream(fs, CompressionMode.Decompress))
                    TheConfig = ProtoBuf.Serializer.Deserialize<ConfigStore>(ds);
            }
            else
            {
                TheConfig = new ConfigStore();
            }

            TheConfig.FileName = filename;
        }

        public static void Save()
        {
            if (!Loaded)
                throw new Exception("Saved config before loading");

            using (FileStream fs = File.Open(TheConfig.FileName, FileMode.Create))
            using (DeflateStream ds = new DeflateStream(fs, CompressionMode.Compress))
                ProtoBuf.Serializer.Serialize<ConfigStore>(ds, TheConfig);
        }
    }
}
