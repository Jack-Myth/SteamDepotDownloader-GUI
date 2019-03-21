using SteamKit2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DepotDownloader
{
    internal class ContentDownloader
    {
        public const uint INVALID_APP_ID = uint.MaxValue;
        public const uint INVALID_DEPOT_ID = uint.MaxValue;
        public const ulong INVALID_MANIFEST_ID = ulong.MaxValue;

        public DownloadConfig Config=null;

        public static Steam3Session steam3;
        public static Steam3Session.Credentials steam3Credentials;

        public CancellationTokenSource cts;


        public static CDNClientPool GlobalCDNPool;

        private CDNClientPool cdnPool;

        private const string DEFAULT_DOWNLOAD_DIR = "depots";
        private const string CONFIG_DIR = ".DepotDownloader";
        private static readonly string STAGING_DIR = Path.Combine( CONFIG_DIR, "staging" );

        public Task DownloadTask=null;

        public ContentDownloader()
        {
            cdnPool = new CDNClientPool(steam3);
        }

        ~ContentDownloader()
        {
            if (cdnPool != null)
            {
                cdnPool.Shutdown();
                cdnPool = null;
            }
        }
        public sealed class DepotDownloadInfo
        {
            public uint id { get; private set; }
            public string installDir { get; private set; }
            public string contentName { get; private set; }

            public ulong manifestId { get; private set; }
            public byte[] depotKey;

            public DepotDownloadInfo( uint depotid, ulong manifestId, string installDir, string contentName )
            {
                this.id = depotid;
                this.manifestId = manifestId;
                this.installDir = installDir;
                this.contentName = contentName;
            }
        }

        public void ClearCache()
        {
            string configPath = Path.Combine(Config.InstallDirectory, CONFIG_DIR);
            if (Directory.Exists(configPath))
                Directory.Delete(configPath, true);
        }

        bool CreateDirectories( uint depotId, uint depotVersion, out string installDir )
        {
            installDir = null;
            try
            {
                if ( string.IsNullOrWhiteSpace( Config.InstallDirectory ) )
                {
                    Directory.CreateDirectory( DEFAULT_DOWNLOAD_DIR );

                    string depotPath = Path.Combine( DEFAULT_DOWNLOAD_DIR, depotId.ToString() );
                    Directory.CreateDirectory( depotPath );

                    installDir = Path.Combine( depotPath, depotVersion.ToString() );
                    Directory.CreateDirectory( installDir );

                    Directory.CreateDirectory( Path.Combine( installDir, CONFIG_DIR ) );
                    Directory.CreateDirectory( Path.Combine( installDir, STAGING_DIR ) );
                }
                else
                {
                    Directory.CreateDirectory( Config.InstallDirectory );

                    installDir = Config.InstallDirectory;

                    Directory.CreateDirectory( Path.Combine( installDir, CONFIG_DIR ) );
                    Directory.CreateDirectory( Path.Combine( installDir, STAGING_DIR ) );
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                return false;
            }

            return true;
        }

        bool TestIsFileIncluded( string filename )
        {
            if ( !Config.UsingFileList )
                return true;

            foreach ( string fileListEntry in Config.FilesToDownload )
            {
                if ( fileListEntry.Equals( filename, StringComparison.OrdinalIgnoreCase ) )
                    return true;
            }

            foreach ( Regex rgx in Config.FilesToDownloadRegex )
            {
                Match m = rgx.Match( filename );

                if ( m.Success )
                    return true;
            }

            return false;
        }

        static bool AccountHasAccess( uint depotId )
        {
            if ( steam3 == null || steam3.steamUser.SteamID == null || ( steam3.Licenses == null && steam3.steamUser.SteamID.AccountType != EAccountType.AnonUser ) )
                return false;

            IEnumerable<uint> licenseQuery;
            if ( steam3.steamUser.SteamID.AccountType == EAccountType.AnonUser )
            {
                licenseQuery = new List<uint>() { 17906 };
            }
            else
            {
                licenseQuery = steam3.Licenses.Select( x => x.PackageID );
            }

            steam3.RequestPackageInfo( licenseQuery );

            foreach ( var license in licenseQuery )
            {
                SteamApps.PICSProductInfoCallback.PICSProductInfo package;
                if ( steam3.PackageInfo.TryGetValue( license, out package ) && package != null )
                {
                    if ( package.KeyValues[ "appids" ].Children.Any( child => child.AsInteger() == depotId ) )
                        return true;

                    if ( package.KeyValues[ "depotids" ].Children.Any( child => child.AsInteger() == depotId ) )
                        return true;
                }
            }

            return false;
        }

        internal static KeyValue GetSteam3AppSection( uint appId, EAppInfoSection section )
        {
            if ( steam3 == null || steam3.AppInfo == null )
            {
                return null;
            }

            SteamApps.PICSProductInfoCallback.PICSProductInfo app;
            if ( !steam3.AppInfo.TryGetValue( appId, out app ) || app == null )
            {
                return null;
            }

            KeyValue appinfo = app.KeyValues;
            string section_key;

            switch ( section )
            {
                case EAppInfoSection.Common:
                    section_key = "common";
                    break;
                case EAppInfoSection.Extended:
                    section_key = "extended";
                    break;
                case EAppInfoSection.Config:
                    section_key = "config";
                    break;
                case EAppInfoSection.Depots:
                    section_key = "depots";
                    break;
                default:
                    throw new NotImplementedException();
            }

            KeyValue section_kv = appinfo.Children.Where( c => c.Name == section_key ).FirstOrDefault();
            return section_kv;
        }

        static uint GetSteam3AppBuildNumber( uint appId, string branch )
        {
            if ( appId == INVALID_APP_ID )
                return 0;


            KeyValue depots = ContentDownloader.GetSteam3AppSection( appId, EAppInfoSection.Depots );
            KeyValue branches = depots[ "branches" ];
            KeyValue node = branches[ branch ];

            if ( node == KeyValue.Invalid )
                return 0;

            KeyValue buildid = node[ "buildid" ];

            if ( buildid == KeyValue.Invalid )
                return 0;

            return uint.Parse( buildid.Value );
        }

        public static ulong GetSteam3DepotManifestStatic(uint depotId, uint appId, string branch,ref string BetaPassword)
        {
            KeyValue depots = GetSteam3AppSection(appId, EAppInfoSection.Depots);
            KeyValue depotChild = depots[depotId.ToString()];

            if (depotChild == KeyValue.Invalid)
                return INVALID_MANIFEST_ID;

            // Shared depots can either provide manifests, or leave you relying on their parent app.
            // It seems that with the latter, "sharedinstall" will exist (and equals 2 in the one existance I know of).
            // Rather than relay on the unknown sharedinstall key, just look for manifests. Test cases: 111710, 346680.
            if (depotChild["manifests"] == KeyValue.Invalid && depotChild["depotfromapp"] != KeyValue.Invalid)
            {
                uint otherAppId = (uint)depotChild["depotfromapp"].AsInteger();
                if (otherAppId == appId)
                {
                    // This shouldn't ever happen, but ya never know with Valve. Don't infinite loop.
                    Logger.Error("App {0}, Depot {1} has depotfromapp of {2}!",
                        appId, depotId, otherAppId);
                    return INVALID_MANIFEST_ID;
                }

                steam3.RequestAppInfo(otherAppId);

                return GetSteam3DepotManifestStatic(depotId, otherAppId, branch,ref BetaPassword);
            }

            var manifests = depotChild["manifests"];
            var manifests_encrypted = depotChild["encryptedmanifests"];

            if (manifests.Children.Count == 0 && manifests_encrypted.Children.Count == 0)
                return INVALID_MANIFEST_ID;

            var node = manifests[branch];

            if (branch != "Public" && node == KeyValue.Invalid)
            {
                var node_encrypted = manifests_encrypted[branch];
                if (node_encrypted != KeyValue.Invalid)
                {
                    string password= BetaPassword;
                    if (password==""&&Util.InputBox("SteamDepotDownloader_GUI", SteamDepotDownloader_GUI.Properties.Resources.BetaPasswordRequest, ref password) != System.Windows.Forms.DialogResult.OK)
                    {
                        Logger.Error("Need credentials");
                        return INVALID_MANIFEST_ID;
                    }

                    if (password == "")
                    {
                        Logger.Error("Need credentials");
                        return INVALID_MANIFEST_ID;
                    }

                    BetaPassword = password;

                    var encrypted_v1 = node_encrypted["encrypted_gid"];
                    var encrypted_v2 = node_encrypted["encrypted_gid_2"];

                    if (encrypted_v1 != KeyValue.Invalid)
                    {
                        byte[] input = Util.DecodeHexString(encrypted_v1.Value);
                        byte[] manifest_bytes = CryptoHelper.VerifyAndDecryptPassword(input, password);

                        if (manifest_bytes == null)
                        {
                            Logger.Error("Password was invalid for branch {0}", branch);
                            return INVALID_MANIFEST_ID;
                        }

                        return BitConverter.ToUInt64(manifest_bytes, 0);
                    }
                    else if (encrypted_v2 != KeyValue.Invalid)
                    {

                        // Submit the password to Steam now to get encryption keys
                        steam3.CheckAppBetaPassword(appId, password);

                        if (!steam3.AppBetaPasswords.ContainsKey(branch))
                        {
                            Logger.Error("Password was invalid for branch {0}", branch);
                            return INVALID_MANIFEST_ID;
                        }

                        byte[] input = Util.DecodeHexString(encrypted_v2.Value);
                        byte[] manifest_bytes;
                        try
                        {
                            manifest_bytes = CryptoHelper.SymmetricDecryptECB(input, steam3.AppBetaPasswords[branch]);
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Failed to decrypt branch {0}: {1}", branch, e.Message);
                            return INVALID_MANIFEST_ID;
                        }

                        return BitConverter.ToUInt64(manifest_bytes, 0);
                    }
                    else
                    {
                        Logger.Error("Unhandled depot encryption for depotId {0}", depotId);
                        return INVALID_MANIFEST_ID;
                    }

                }

                return INVALID_MANIFEST_ID;
            }

            if (node.Value == null)
                return INVALID_MANIFEST_ID;

            return UInt64.Parse(node.Value);
        }

        ulong GetSteam3DepotManifest( uint depotId, uint appId, string branch )
        {
            if ( Config.ManifestId != INVALID_MANIFEST_ID )
                return Config.ManifestId;

            string BetaPassword = Config.BetaPassword;
            ulong retVal = GetSteam3DepotManifestStatic(depotId, appId, branch,ref BetaPassword);
            Config.BetaPassword = BetaPassword;
            return retVal;
        }

        static string GetAppOrDepotName( uint depotId, uint appId )
        {
            if ( depotId == INVALID_DEPOT_ID )
            {
                KeyValue info = GetSteam3AppSection( appId, EAppInfoSection.Common );

                if ( info == null )
                    return String.Empty;

                return info[ "name" ].AsString();
            }
            else
            {
                KeyValue depots = GetSteam3AppSection( appId, EAppInfoSection.Depots );

                if ( depots == null )
                    return String.Empty;

                KeyValue depotChild = depots[ depotId.ToString() ];

                if ( depotChild == null )
                    return String.Empty;

                return depotChild[ "name" ].AsString();
            }
        }

        public static bool InitializeSteam3( UserConfig UsrConfig )
        {
            string loginKey = null;

            if (UsrConfig.Username != null && UsrConfig.RememberPassword )
            {
                _ = ConfigStore.TheConfig.LoginKeys.TryGetValue(UsrConfig.Username, out loginKey );
            }

            steam3 = new Steam3Session(
                new SteamUser.LogOnDetails()
                {
                    Username = UsrConfig.Username,
                    Password = loginKey == null ? UsrConfig.Password : null,
                    ShouldRememberPassword = UsrConfig.RememberPassword,
                    LoginKey = loginKey,
                    TwoFactorCode = UsrConfig.TwoFactorAuthCode
                }
            );

            steam3Credentials = steam3.WaitForCredentials();

            if ( !steam3Credentials.IsValid )
            {
                Logger.Error( "Unable to get steam3 credentials." );
                ShutdownSteam3();
                return false;
            }

            GlobalCDNPool = new CDNClientPool(steam3);

            return true;
        }

        public static void ShutdownSteam3()
        {
            if (GlobalCDNPool != null)
            {
                GlobalCDNPool.Shutdown();
                GlobalCDNPool = null;
            }

            if ( steam3 == null )
                return;

            steam3.TryWaitForLoginKey();
            steam3.Disconnect();
            steam3 = null;
        }

        

        public async Task DownloadAppAsync( )
        {
            if ( steam3 != null )
                steam3.RequestAppInfo( Config.AppID );

            if ( !AccountHasAccess( Config.AppID ) )
            {
                if ( steam3.RequestFreeAppLicense( Config.AppID ) )
                {
                    Logger.Error( "Obtained FreeOnDemand license for app {0}", Config.AppID );
                }
                else
                {
                    string contentName = GetAppOrDepotName( INVALID_DEPOT_ID, Config.AppID );
                    Logger.Error( "App {0} ({1}) is not available from this account.", Config.AppID, contentName );
                    return;
                }
            }

            Logger.Info( "Using app branch: '{0}'.", Config.Branch );

            var depotIDs = new List<uint>();
            KeyValue depots = GetSteam3AppSection( Config.AppID, EAppInfoSection.Depots );


            if ( Config.ForceDepot )
            {
                depotIDs.Add( Config.DepotID );
            }
            else
            {
                if ( depots != null )
                {
                    foreach ( var depotSection in depots.Children )
                    {
                        uint id = INVALID_DEPOT_ID;
                        if ( depotSection.Children.Count == 0 )
                            continue;

                        if ( !uint.TryParse( depotSection.Name, out id ) )
                            continue;

                        if ( Config.DepotID != INVALID_DEPOT_ID && id != Config.DepotID )
                            continue;

                        if ( !Config.DownloadAllPlatforms )
                        {
                            var depotConfig = depotSection[ "config" ];
                            if ( depotConfig != KeyValue.Invalid && depotConfig[ "oslist" ] != KeyValue.Invalid && !string.IsNullOrWhiteSpace( depotConfig[ "oslist" ].Value ) )
                            {
                                var oslist = depotConfig[ "oslist" ].Value.Split( ',' );
                                if ( Array.IndexOf( oslist, Config.OS ?? Util.GetSteamOS() ) == -1 )
                                    continue;
                            }
                        }

                        depotIDs.Add( id );
                    }
                }
                if (depotIDs.Count == 0 && Config.DepotID == INVALID_DEPOT_ID)
                {
                    Logger.Error( "Couldn't find any depots to download for app {0}", Config.AppID );
                    return;
                }
                else if ( depotIDs.Count == 0 )
                {
                    var msg = $"Depot {Config.DepotID} not listed for app {Config.AppID}";
                    if ( !Config.DownloadAllPlatforms )
                    {
                        msg += " or not available on this platform";
                    }

                    Logger.Error(msg);
                    return;
                }
            }

            var infos = new List<DepotDownloadInfo>();

            foreach ( var depot in depotIDs )
            {
                var info = GetDepotInfo( depot, Config.AppID, Config.Branch );
                if ( info != null )
                {
                    infos.Add( info );
                }
            }

            if(infos.Count==0)
            {
                Config.FireOnDownloadFinishedEvent(false, "No Depot available from this account.");
                return;
            }

            try
            {
                await DownloadSteam3Async( Config.AppID, infos ).ConfigureAwait( false );
            }
            catch ( OperationCanceledException )
            {
                Logger.Error( "App {0} was not completely downloaded.", Config.AppID );
            }
        }

        public DepotDownloadInfo GetDepotInfo( uint depotId, uint appId, string branch )
        {
            if ( steam3 != null && appId != INVALID_APP_ID )
                steam3.RequestAppInfo( ( uint )appId );

            string contentName = GetAppOrDepotName( depotId, appId );

            if ( !AccountHasAccess( depotId ) )
            {
                Logger.Error( "Depot {0} ({1}) is not available from this account.", depotId, contentName );

                return null;
            }

            // Skip requesting an app ticket
            steam3.AppTickets[ depotId ] = null;

            ulong manifestID = GetSteam3DepotManifest( depotId, appId, branch );
            if ( manifestID == INVALID_MANIFEST_ID && branch != "public" )
            {
                Logger.Warning( "Warning: Depot {0} does not have branch named \"{1}\". Trying public branch.", depotId, branch );
                branch = "public";
                manifestID = GetSteam3DepotManifest( depotId, appId, branch );
            }

            if ( manifestID == INVALID_MANIFEST_ID )
            {
                Logger.Error( "Depot {0} ({1}) missing public subsection or manifest section.", depotId, contentName );
                return null;
            }

            uint uVersion = GetSteam3AppBuildNumber( appId, branch );

            string installDir;
            if ( !CreateDirectories( depotId, uVersion, out installDir ) )
            {
                Logger.Error( "Error: Unable to create install directories!" );
                return null;
            }

            steam3.RequestDepotKey( depotId, appId );
            if ( !steam3.DepotKeys.ContainsKey( depotId ) )
            {
                Logger.Error( "No valid depot key for {0}, unable to download.", depotId );
                return null;
            }

            byte[] depotKey = steam3.DepotKeys[ depotId ];

            var info = new DepotDownloadInfo( depotId, manifestID, installDir, contentName );
            info.depotKey = depotKey;
            return info;
        }

        private class ChunkMatch
        {
            public ChunkMatch( ProtoManifest.ChunkData oldChunk, ProtoManifest.ChunkData newChunk )
            {
                OldChunk = oldChunk;
                NewChunk = newChunk;
            }
            public ProtoManifest.ChunkData OldChunk { get; private set; }
            public ProtoManifest.ChunkData NewChunk { get; private set; }
        }

        private async Task DownloadSteam3Async( uint appId, List<DepotDownloadInfo> depots )
        {
            ulong TotalBytesCompressed = 0;
            ulong TotalBytesUncompressed = 0;

            foreach ( var depot in depots )
            {
                ulong DepotBytesCompressed = 0;
                ulong DepotBytesUncompressed = 0;

                Logger.Info( "Downloading depot {0} - {1}", depot.id, depot.contentName );

                cts = new CancellationTokenSource();
                
                
                cdnPool.ExhaustedToken = cts;

                ProtoManifest oldProtoManifest = null;
                ProtoManifest newProtoManifest = null;
                string configDir = Path.Combine( depot.installDir, CONFIG_DIR );

                ulong lastManifestId = INVALID_MANIFEST_ID;
                ConfigStore.TheConfig.LastManifests.TryGetValue( depot.id, out lastManifestId );

                // In case we have an early exit, this will force equiv of verifyall next run.
                ConfigStore.TheConfig.LastManifests[ depot.id ] = INVALID_MANIFEST_ID;
                ConfigStore.Save();

                if ( lastManifestId != INVALID_MANIFEST_ID )
                {
                    var oldManifestFileName = Path.Combine( configDir, string.Format( "{0}.bin", lastManifestId ) );
                    if ( File.Exists( oldManifestFileName ) )
                        oldProtoManifest = ProtoManifest.LoadFromFile( oldManifestFileName );
                }

                if ( lastManifestId == depot.manifestId && oldProtoManifest != null )
                {
                    newProtoManifest = oldProtoManifest;
                    Logger.Info( "Already have manifest {0} for depot {1}.", depot.manifestId, depot.id );
                }
                else
                {
                    var newManifestFileName = Path.Combine( configDir, $"{depot.manifestId}.bin");
                    newProtoManifest = ProtoManifest.LoadFromFile( newManifestFileName );

                    if ( newProtoManifest != null )
                    {
                        Logger.Info( "Already have manifest {0} for depot {1}.", depot.manifestId, depot.id );
                    }
                    else
                    {
                        Logger.Info( "Downloading depot manifest..." );

                        DepotManifest depotManifest = null;

                        while ( depotManifest == null )
                        {
                            CDNClient client = null;
                            try
                            {
                                client = await cdnPool.GetConnectionForDepotAsync( appId, depot.id, depot.depotKey, CancellationToken.None ).ConfigureAwait( false );

                                depotManifest = await client.DownloadManifestAsync( depot.id, depot.manifestId ).ConfigureAwait( false );

                                cdnPool.ReturnConnection( client );
                            }
                            catch ( WebException e )
                            {
                                cdnPool.ReturnBrokenConnection( client );

                                if ( e.Status == WebExceptionStatus.ProtocolError )
                                {
                                    var response = e.Response as HttpWebResponse;
                                    if ( response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden )
                                    {
                                        Logger.Error( "Encountered 401 for depot manifest {0} {1}. Aborting.", depot.id, depot.manifestId );
                                        break;
                                    }
                                    else
                                    {
                                        Logger.Error( "Encountered error downloading depot manifest {0} {1}: {2}", depot.id, depot.manifestId, response.StatusCode );
                                    }
                                }
                                else
                                {
                                    Logger.Error( "Encountered error downloading manifest for depot {0} {1}: {2}", depot.id, depot.manifestId, e.Status );
                                }
                            }
                            catch ( Exception e )
                            {
                                cdnPool.ReturnBrokenConnection( client );
                                Logger.Error( "Encountered error downloading manifest for depot {0} {1}: {2}", depot.id, depot.manifestId, e.Message );
                            }
                        }

                        if ( depotManifest == null )
                        {
                            Logger.Error( "\nUnable to download manifest {0} for depot {1}", depot.manifestId, depot.id );
                            return;
                        }

                        newProtoManifest = new ProtoManifest(depotManifest, depot.manifestId);
                        newProtoManifest.SaveToFile(newManifestFileName);

                        Logger.Info( " Done!" );
                    }
                }

                newProtoManifest.Files.Sort( ( x, y ) => x.FileName.CompareTo( y.FileName ));

                if ( Config.DownloadManifestOnly )
                {
                    StringBuilder manifestBuilder = new StringBuilder();
                    string txtManifest = Path.Combine( depot.installDir, $"manifest_{depot.id}.txt");

                    foreach ( var file in newProtoManifest.Files )
                    {
                        if ( file.Flags.HasFlag( EDepotFileFlag.Directory ) )
                            continue;

                        manifestBuilder.Append($"{file.FileName}\n");
                    }

                    File.WriteAllText( txtManifest, manifestBuilder.ToString() );
                    continue;
                }

                ulong complete_download_size = 0;
                ulong size_downloaded = 0;
                string stagingDir = Path.Combine( depot.installDir, STAGING_DIR );

                var filesAfterExclusions = newProtoManifest.Files.AsParallel().Where( f => TestIsFileIncluded( f.FileName ) ).ToList();

                // Pre-process
                filesAfterExclusions.ForEach( file =>
                {
                    var fileFinalPath = Path.Combine( depot.installDir, file.FileName );
                    fileFinalPath = Config.SavePathProcessor?.Invoke(fileFinalPath) ?? fileFinalPath;
                    var fileStagingPath = Path.Combine( stagingDir, file.FileName );

                    if ( file.Flags.HasFlag( EDepotFileFlag.Directory ) )
                    {
                        Directory.CreateDirectory( fileFinalPath );
                        Directory.CreateDirectory( fileStagingPath );
                    }
                    else
                    {
                        // Some manifests don't explicitly include all necessary directories
                        Directory.CreateDirectory( Path.GetDirectoryName( fileFinalPath ) );
                        Directory.CreateDirectory( Path.GetDirectoryName( fileStagingPath ) );

                        complete_download_size += file.TotalSize;
                    }
                } );

                var semaphore = new SemaphoreSlim( Config.MaxDownloads );
                var files = filesAfterExclusions.Where( f => !f.Flags.HasFlag( EDepotFileFlag.Directory ) ).ToArray();
                var tasks = new Task[ files.Length ];
                for ( var i = 0; i < files.Length; i++ )
                {
                    var file = files[ i ];
                    var task = Task.Run( async () =>
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        try
                        {
                            await semaphore.WaitAsync(cts.Token).ConfigureAwait( false );
                            cts.Token.ThrowIfCancellationRequested();

                            string fileFinalPath = Path.Combine( depot.installDir, file.FileName );
                            fileFinalPath = Config.SavePathProcessor?.Invoke(fileFinalPath) ?? fileFinalPath;
                            string fileStagingPath = Path.Combine( stagingDir, file.FileName );

                            // This may still exist if the previous run exited before cleanup
                            if ( File.Exists( fileStagingPath ) )
                            {
                                File.Delete( fileStagingPath );
                            }

                            FileStream fs = null;
                            List<ProtoManifest.ChunkData> neededChunks;
                            FileInfo fi = new FileInfo( fileFinalPath );
                            if ( !fi.Exists )
                            {
                                // create new file. need all chunks
                                fs = File.Create( fileFinalPath );
                                fs.SetLength( ( long )file.TotalSize );
                                neededChunks = new List<ProtoManifest.ChunkData>( file.Chunks );
                            }
                            else
                            {
                                // open existing
                                ProtoManifest.FileData oldManifestFile = null;
                                if ( oldProtoManifest != null )
                                {
                                    oldManifestFile = oldProtoManifest.Files.SingleOrDefault( f => f.FileName == file.FileName );
                                }

                                if ( oldManifestFile != null )
                                {
                                    neededChunks = new List<ProtoManifest.ChunkData>();

                                    if ( Config.VerifyAll || !oldManifestFile.FileHash.SequenceEqual( file.FileHash ) )
                                    {
                                        // we have a version of this file, but it doesn't fully match what we want

                                        var matchingChunks = new List<ChunkMatch>();

                                        foreach ( var chunk in file.Chunks )
                                        {
                                            var oldChunk = oldManifestFile.Chunks.FirstOrDefault( c => c.ChunkID.SequenceEqual( chunk.ChunkID ) );
                                            if ( oldChunk != null )
                                            {
                                                matchingChunks.Add( new ChunkMatch( oldChunk, chunk ) );
                                            }
                                            else
                                            {
                                                neededChunks.Add( chunk );
                                            }
                                        }

                                        File.Move( fileFinalPath, fileStagingPath );

                                        fs = File.Open( fileFinalPath, FileMode.Create );
                                        fs.SetLength( ( long )file.TotalSize );

                                        using ( var fsOld = File.Open( fileStagingPath, FileMode.Open ) )
                                        {
                                            foreach ( var match in matchingChunks )
                                            {
                                                fsOld.Seek( ( long )match.OldChunk.Offset, SeekOrigin.Begin );

                                                byte[] tmp = new byte[ match.OldChunk.UncompressedLength ];
                                                fsOld.Read( tmp, 0, tmp.Length );

                                                byte[] adler = Util.AdlerHash( tmp );
                                                if ( !adler.SequenceEqual( match.OldChunk.Checksum ) )
                                                {
                                                    neededChunks.Add( match.NewChunk );
                                                }
                                                else
                                                {
                                                    fs.Seek( ( long )match.NewChunk.Offset, SeekOrigin.Begin );
                                                    fs.Write( tmp, 0, tmp.Length );
                                                }
                                            }
                                        }

                                        File.Delete( fileStagingPath );
                                    }
                                }
                                else
                                {
                                    // No old manifest or file not in old manifest. We must validate.

                                    fs = File.Open( fileFinalPath, FileMode.Open );
                                    if ( ( ulong )fi.Length != file.TotalSize )
                                    {
                                        fs.SetLength( ( long )file.TotalSize );
                                    }

                                    neededChunks = Util.ValidateSteam3FileChecksums( fs, file.Chunks.OrderBy( x => x.Offset ).ToArray() );
                                }

                                if ( !neededChunks.Any() )
                                {
                                    size_downloaded += file.TotalSize;

                                    string fileName = Path.GetFileName(fileFinalPath);
                                    float pcnts = ((float) size_downloaded / (float) complete_download_size);
                                    Config.FireReportProgressEvent(pcnts, fileName);
                                    //Config.FireReportProgressEvent($"Downloading -{pcnts,6:#00.00}% {fileName}");
                                    Logger.Info( $"{pcnts,6:#00.00}% {fileName}");
                                    fs?.Dispose();
                                    return;
                                }
                                else
                                {
                                    size_downloaded += ( file.TotalSize - ( ulong )neededChunks.Select( x => ( long )x.UncompressedLength ).Sum() );
                                }
                            }

                            foreach ( var chunk in neededChunks )
                            {
                                if ( cts.IsCancellationRequested ) break;

                                string chunkID = Util.EncodeHexString( chunk.ChunkID );
                                CDNClient.DepotChunk chunkData = null;

                                while ( !cts.IsCancellationRequested )
                                {
                                    CDNClient client;
                                    try
                                    {
                                        client = await cdnPool.GetConnectionForDepotAsync( appId, depot.id, depot.depotKey, cts.Token ).ConfigureAwait( false );
                                    }
                                    catch ( OperationCanceledException )
                                    {
                                        break;
                                    }

                                    DepotManifest.ChunkData data = new DepotManifest.ChunkData();
                                    data.ChunkID = chunk.ChunkID;
                                    data.Checksum = chunk.Checksum;
                                    data.Offset = chunk.Offset;
                                    data.CompressedLength = chunk.CompressedLength;
                                    data.UncompressedLength = chunk.UncompressedLength;

                                    try
                                    {
                                        chunkData = await client.DownloadDepotChunkAsync( depot.id, data ).ConfigureAwait( false );
                                        cdnPool.ReturnConnection( client );
                                        break;
                                    }
                                    catch ( WebException e )
                                    {
                                        cdnPool.ReturnBrokenConnection( client );

                                        if ( e.Status == WebExceptionStatus.ProtocolError )
                                        {
                                            var response = (HttpWebResponse)e.Response;
                                            if ( response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden )
                                            {
                                                Logger.Error( "Encountered 401 for chunk {0}. Aborting.", chunkID );
                                                cts.Cancel();
                                                break;
                                            }
                                            else
                                            {
                                                Logger.Error( "Encountered error downloading chunk {0}: {1}", chunkID, response.StatusCode );
                                            }
                                        }
                                        else
                                        {
                                            Logger.Error( "Encountered error downloading chunk {0}: {1}", chunkID, e.Status );
                                        }
                                    }
                                    catch ( Exception e )
                                    {
                                        cdnPool.ReturnBrokenConnection( client );
                                        Logger.Error( "Encountered unexpected error downloading chunk {0}: {1}", chunkID, e.Message );
                                    }
                                }

                                if ( chunkData == null )
                                {
                                    Logger.Error( "Failed to find any server with chunk {0} for depot {1}. Aborting.", chunkID, depot.id );
                                    cts.Cancel();
                                    return;
                                }

                                TotalBytesCompressed += chunk.CompressedLength;
                                DepotBytesCompressed += chunk.CompressedLength;
                                TotalBytesUncompressed += chunk.UncompressedLength;
                                DepotBytesUncompressed += chunk.UncompressedLength;

                                fs.Seek( ( long )chunk.Offset, SeekOrigin.Begin );
                                fs.Write( chunkData.Data, 0, chunkData.Data.Length );

                                size_downloaded += chunk.UncompressedLength;
                            }

                            fs.Dispose();
                            
                            string filename = Path.GetFileName(fileFinalPath);
                            float percents = ((float) size_downloaded / (float) complete_download_size);
                            Config.FireReportProgressEvent(percents, filename);
                            //Config.FireReportProgressEvent($"Downloading -{percents,6:#00.00}% {filename}");

                            Logger.Info( $"{percents,6:#00.00}% {filename}" );
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }, cts.Token);

                    tasks[ i ] = task;
                }

                await Task.WhenAll( tasks ).ConfigureAwait( false );

                ConfigStore.TheConfig.LastManifests[ depot.id ] = depot.manifestId;
                ConfigStore.Save();

                Logger.Info( "Depot {0} - Downloaded {1} bytes ({2} bytes uncompressed)", depot.id, DepotBytesCompressed, DepotBytesUncompressed );
            }

            Logger.Info( "Total downloaded: {0} bytes ({1} bytes uncompressed) from {2} depots", TotalBytesCompressed, TotalBytesUncompressed, depots.Count );
            Config.FireOnDownloadFinishedEvent(true, "");
        }
        public void StopDownload()
        {
            if (cts == null)
                return;
            if (!cts.IsCancellationRequested)
                cts.Cancel();
        }

        public void RestartDownload()
        {
            if (cts==null||cts.IsCancellationRequested)
            {
                new Thread(() => DownloadAppAsync().Wait()).Start();
            }
        }

        
    }
}
