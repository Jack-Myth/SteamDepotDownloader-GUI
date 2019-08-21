using SteamKit2;
using SteamKit2.Unified.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace DepotDownloader
{

    class Steam3Session
    {
        public class Credentials
        {
            public bool LoggedOn { get; set; }
            public ulong SessionToken { get; set; }

            public bool IsValid
            {
                get { return LoggedOn; }
            }
        }

        public ReadOnlyCollection<SteamApps.LicenseListCallback.License> Licenses
        {
            get;
            private set;
        }

        public Dictionary<uint, byte[]> AppTickets { get; private set; }
        public Dictionary<uint, ulong> AppTokens { get; private set; }
        public Dictionary<uint, byte[]> DepotKeys { get; private set; }
        public ConcurrentDictionary<string, SteamApps.CDNAuthTokenCallback> CDNAuthTokens { get; private set; }
        public Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo> AppInfo { get; private set; }
        public Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo> PackageInfo { get; private set; }
        public Dictionary<string, byte[]> AppBetaPasswords { get; private set; }
        public bool IsConnected { get => bConnected; }
        public bool IsAborted { get => bAborted;  }

        public SteamClient steamClient;
        public SteamUser steamUser;
        SteamApps steamApps;
        SteamUnifiedMessages.UnifiedService<IPublishedFile> steamPublishedFile;

        CallbackManager callbacks;

        bool authenticatedUser;
        bool bConnected;
        bool bConnecting;
        bool bAborted;
        bool bExpectingDisconnectRemote;
        bool bDidDisconnect;
        int connectionBackoff;
        int seq; // more hack fixes
        DateTime connectTime;

        // input
        SteamUser.LogOnDetails logonDetails;

        // output
        Credentials credentials;

        static readonly TimeSpan STEAM3_TIMEOUT = TimeSpan.FromSeconds( 10 );


        public Steam3Session( SteamUser.LogOnDetails details )
        {
            this.logonDetails = details;

            this.authenticatedUser = details.Username != null;
            this.credentials = new Credentials();
            this.bConnected = false;
            this.bConnecting = false;
            this.bAborted = false;
            this.seq = 0;

            this.AppTickets = new Dictionary<uint, byte[]>();
            this.AppTokens = new Dictionary<uint, ulong>();
            this.DepotKeys = new Dictionary<uint, byte[]>();
            this.CDNAuthTokens = new ConcurrentDictionary<string, SteamApps.CDNAuthTokenCallback>();
            this.AppInfo = new Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo>();
            this.PackageInfo = new Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo>();
            this.AppBetaPasswords = new Dictionary<string, byte[]>();

            this.steamClient = new SteamClient();

            this.steamUser = this.steamClient.GetHandler<SteamUser>();
            this.steamApps = this.steamClient.GetHandler<SteamApps>();
            var steamUnifiedMessages = this.steamClient.GetHandler<SteamUnifiedMessages>();
            this.steamPublishedFile = steamUnifiedMessages.CreateService<IPublishedFile>();

            this.callbacks = new CallbackManager( this.steamClient );

            this.callbacks.Subscribe<SteamClient.ConnectedCallback>( ConnectedCallback );
            this.callbacks.Subscribe<SteamClient.DisconnectedCallback>( DisconnectedCallback );
            this.callbacks.Subscribe<SteamUser.LoggedOnCallback>( LogOnCallback );
            this.callbacks.Subscribe<SteamUser.SessionTokenCallback>( SessionTokenCallback );
            this.callbacks.Subscribe<SteamApps.LicenseListCallback>( LicenseListCallback );
            this.callbacks.Subscribe<SteamUser.UpdateMachineAuthCallback>( UpdateMachineAuthCallback );
            this.callbacks.Subscribe<SteamUser.LoginKeyCallback>( LoginKeyCallback );
            this.callbacks.Subscribe<SteamUser.AccountInfoCallback>(AccountInfoCallback);
            //this.callbacks.Subscribe<SteamKit2>

            Logger.Info( "Connecting to Steam3..." );

            if ( authenticatedUser )
            {
                FileInfo fi = new FileInfo( String.Format( "{0}.sentryFile", logonDetails.Username ) );
                if ( ConfigStore.TheConfig.SentryData != null && ConfigStore.TheConfig.SentryData.ContainsKey( logonDetails.Username ) )
                {
                    logonDetails.SentryFileHash = Util.SHAHash( ConfigStore.TheConfig.SentryData[ logonDetails.Username ] );
                }
                else if ( fi.Exists && fi.Length > 0 )
                {
                    var sentryData = File.ReadAllBytes( fi.FullName );
                    logonDetails.SentryFileHash = Util.SHAHash( sentryData );
                    ConfigStore.TheConfig.SentryData[ logonDetails.Username ] = sentryData;
                    ConfigStore.Save();
                }
            }

            Connect();
        }

        public delegate bool WaitCondition();
        public bool WaitUntilCallback( Action submitter, WaitCondition waiter )
        {
            while ( !bAborted && !waiter() )
            {
                submitter();

                int seq = this.seq;
                do
                {
                    WaitForCallbacks();
                }
                while ( !bAborted && this.seq == seq && !waiter() );
            }

            return bAborted;
        }

        public Credentials WaitForCredentials()
        {
            if ( credentials.IsValid || bAborted )
                return credentials;

            WaitUntilCallback( () => { }, () => { return credentials.IsValid; } );

            return credentials;
        }

        public void RequestAppInfo( uint appId )
        {
            if ( AppInfo.ContainsKey( appId ) || bAborted )
                return;

            bool completed = false;
            Action<SteamApps.PICSTokensCallback> cbMethodTokens = ( appTokens ) =>
            {
                completed = true;
                if ( appTokens.AppTokensDenied.Contains( appId ) )
                {
                    Logger.Error( "Insufficient privileges to get access token for app {0}", appId );
                }

                foreach ( var token_dict in appTokens.AppTokens )
                {
                    this.AppTokens.Add( token_dict.Key, token_dict.Value );
                }
            };

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.PICSGetAccessTokens( new List<uint>() { appId }, new List<uint>() { } ), cbMethodTokens );
            }, () => { return completed; } );

            completed = false;
            Action<SteamApps.PICSProductInfoCallback> cbMethod = ( appInfo ) =>
            {
                completed = !appInfo.ResponsePending;

                foreach ( var app_value in appInfo.Apps )
                {
                    var app = app_value.Value;

                    Logger.Info( "Got AppInfo for {0}", app.ID );
                    AppInfo.Add( app.ID, app );
                }

                foreach ( var app in appInfo.UnknownApps )
                {
                    AppInfo.Add( app, null );
                }
            };

            SteamApps.PICSRequest request = new SteamApps.PICSRequest( appId );
            if ( AppTokens.ContainsKey( appId ) )
            {
                request.AccessToken = AppTokens[ appId ];
                request.Public = false;
            }

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.PICSGetProductInfo( new List<SteamApps.PICSRequest>() { request }, new List<SteamApps.PICSRequest>() { } ), cbMethod );
            }, () => { return completed; } );
        }

        public void RequestAppsInfo(List<uint> appIds)
        {
            foreach (var existedId in AppInfo.Keys)
            {
                appIds.Remove(existedId);
            }
            if (appIds.Count==0 || bAborted)
                return;

            bool completed = false;
            Action<SteamApps.PICSTokensCallback> cbMethodTokens = (appTokens) =>
            {
                completed = true;
                foreach (uint appId in appIds)
                {
                    if (appTokens.AppTokensDenied.Contains(appId))
                    {
                        Logger.Error("Insufficient privileges to get access token for app {0}", appId);
                    }
                }

                foreach (var token_dict in appTokens.AppTokens)
                {
                    this.AppTokens.Add(token_dict.Key, token_dict.Value);
                }
            };

            WaitUntilCallback(() =>
            {
                callbacks.Subscribe(steamApps.PICSGetAccessTokens(appIds, new List<uint>() { }), cbMethodTokens);
            }, () => { return completed; });

            completed = false;
            Action<SteamApps.PICSProductInfoCallback> cbMethod = (appInfo) =>
            {
                completed = !appInfo.ResponsePending;

                foreach (var app_value in appInfo.Apps)
                {
                    var app = app_value.Value;

                    Logger.Info("Got AppInfo for {0}", app.ID);
                    AppInfo[app.ID]= app;
                }

                foreach (var app in appInfo.UnknownApps)
                {
                    AppInfo[app] = null;
                }
            };

            var requests = new List<SteamApps.PICSRequest>();
            foreach (var appId in appIds)
            {
                if (AppTokens.ContainsKey(appId))
                {
                    var request =new SteamApps.PICSRequest(appId);
                    request.AccessToken = AppTokens[appId];
                    request.Public = false;
                    requests.Add(request);
                }
            }

            WaitUntilCallback(() =>
            {
                callbacks.Subscribe(steamApps.PICSGetProductInfo(requests, new List<SteamApps.PICSRequest>() { }), cbMethod);
            }, () => { return completed; });
        }

        public void RequestPackageInfo( IEnumerable<uint> packageIds )
        {
            List<uint> packages = packageIds.ToList();
            packages.RemoveAll( pid => PackageInfo.ContainsKey( pid ) );

            if ( packages.Count == 0 || bAborted )
                return;

            bool completed = false;
            Action<SteamApps.PICSProductInfoCallback> cbMethod = ( packageInfo ) =>
            {
                completed = !packageInfo.ResponsePending;

                foreach ( var package_value in packageInfo.Packages )
                {
                    var package = package_value.Value;
                    PackageInfo.Add( package.ID, package );
                }

                foreach ( var package in packageInfo.UnknownPackages )
                {
                    PackageInfo.Add( package, null );
                }
            };

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.PICSGetProductInfo( new List<uint>(), packages ), cbMethod );
            }, () => { return completed; } );
        }

        public bool RequestFreeAppLicense( uint appId )
        {
            bool success = false;
            bool completed = false;
            Action<SteamApps.FreeLicenseCallback> cbMethod = ( resultInfo ) =>
            {
                completed = true;
                success = resultInfo.GrantedApps.Contains( appId );
            };

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.RequestFreeLicense( appId ), cbMethod );
            }, () => { return completed; } );

            return success;
        }

        public void RequestAppTicket( uint appId )
        {
            if ( AppTickets.ContainsKey( appId ) || bAborted )
                return;


            if ( !authenticatedUser )
            {
                AppTickets[ appId ] = null;
                return;
            }

            bool completed = false;
            Action<SteamApps.AppOwnershipTicketCallback> cbMethod = ( appTicket ) =>
            {
                completed = true;

                if ( appTicket.Result != EResult.OK )
                {
                    Logger.Error( "Unable to get appticket for {0}: {1}", appTicket.AppID, appTicket.Result );
                    //Abort();
                }
                else
                {
                    Logger.Info( "Got appticket for {0}!", appTicket.AppID );
                    AppTickets[ appTicket.AppID ] = appTicket.Ticket;
                }
            };

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.GetAppOwnershipTicket( appId ), cbMethod );
            }, () => { return completed; } );
        }

        public void RequestDepotKey( uint depotId, uint appid = 0 )
        {
            if ( DepotKeys.ContainsKey( depotId ) || bAborted )
                return;

            bool completed = false;

            Action<SteamApps.DepotKeyCallback> cbMethod = ( depotKey ) =>
            {
                completed = true;
                Logger.Info( "Got depot key for {0} result: {1}", depotKey.DepotID, depotKey.Result );

                if ( depotKey.Result != EResult.OK )
                {
                    //Abort();
                    return;
                }

                DepotKeys[ depotKey.DepotID ] = depotKey.DepotKey;
            };

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.GetDepotDecryptionKey( depotId, appid ), cbMethod );
            }, () => { return completed; } );
        }

        public string ResolveCDNTopLevelHost(string host)
        {
            // SteamPipe CDN shares tokens with all hosts
            if (host.EndsWith( ".steampipe.steamcontent.com" ) )
            {
                return "steampipe.steamcontent.com";
            }

            return host;
        }

        public void RequestCDNAuthToken( uint appid, uint depotid, string host )
        {
            host = ResolveCDNTopLevelHost( host );
            var cdnKey = string.Format( "{0:D}:{1}", depotid, host );

            if ( CDNAuthTokens.ContainsKey( cdnKey ) || bAborted )
                return;

            bool completed = false;
            Action<SteamApps.CDNAuthTokenCallback> cbMethod = ( cdnAuth ) =>
            {
                completed = true;
                Logger.Info( "Got CDN auth token for {0} result: {1} (expires {2})", host, cdnAuth.Result, cdnAuth.Expiration );

                if ( cdnAuth.Result != EResult.OK )
                {
                    //Abort();
                    return;
                }

                CDNAuthTokens.TryAdd( cdnKey, cdnAuth );
            };

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.GetCDNAuthToken( appid, depotid, host ), cbMethod );
            }, () => { return completed; } );
        }

        public void CheckAppBetaPassword( uint appid, string password )
        {
            bool completed = false;
            Action<SteamApps.CheckAppBetaPasswordCallback> cbMethod = ( appPassword ) =>
            {
                completed = true;

                Logger.Info( "Retrieved {0} beta keys with result: {1}", appPassword.BetaPasswords.Count, appPassword.Result );

                foreach ( var entry in appPassword.BetaPasswords )
                {
                    AppBetaPasswords[ entry.Key ] = entry.Value;
                }
            };

            WaitUntilCallback( () =>
            {
                callbacks.Subscribe( steamApps.CheckAppBetaPassword( appid, password ), cbMethod );
            }, () => { return completed; } );
        }

        public PublishedFileDetails GetPubfileDetails( PublishedFileID pubFile )
        {
            var pubFileRequest = new CPublishedFile_GetDetails_Request();
            pubFileRequest.publishedfileids.Add( pubFile );

            bool completed = false;
            PublishedFileDetails details = null;

            Action<SteamUnifiedMessages.ServiceMethodResponse> cbMethod = callback =>
            {
                completed = true;
                if ( callback.Result == EResult.OK )
                {
                    var response = callback.GetDeserializedResponse<CPublishedFile_GetDetails_Response>();
                    details = response.publishedfiledetails[0];
                }
                else
                {
                    throw new Exception( $"EResult {(int)callback.Result} ({callback.Result}) while retrieving UGC id for pubfile {pubFile}.");
                }
            };

            WaitUntilCallback(() =>
            {
                callbacks.Subscribe( steamPublishedFile.SendMessage( api => api.GetDetails( pubFileRequest ) ), cbMethod );
            }, () => { return completed; });

            return details;
        }

        void Connect()
        {
            bAborted = false;
            bConnected = false;
            bConnecting = true;
            connectionBackoff = 0;
            bExpectingDisconnectRemote = false;
            bDidDisconnect = false;
            this.connectTime = DateTime.Now;
            this.steamClient.Connect();
        }

        private void Abort( bool sendLogOff = true )
        {
            Disconnect( sendLogOff );
        }
        public void Disconnect( bool sendLogOff = true )
        {
            if ( sendLogOff )
            {
                steamUser.LogOff();
            }

            steamClient.Disconnect();
            bConnected = false;
            bConnecting = false;
            bAborted = true;

            // flush callbacks until our disconnected event
            while ( !bDidDisconnect )
            {
                callbacks.RunWaitAllCallbacks( TimeSpan.FromMilliseconds( 100 ) );
            }
        }

        public void TryWaitForLoginKey()
        {
            if ( logonDetails.Username == null || !SteamDepotDownloader_GUI.Program.UsrConfig.RememberPassword ) return;

            DateTime waitUntil = DateTime.Now.AddSeconds( 5 );



            while ( true )
            {
                DateTime now = DateTime.Now;
                if ( now >= waitUntil ) break;

                if ( ConfigStore.TheConfig.LoginKeys.ContainsKey( logonDetails.Username ) ) break;

                callbacks.RunWaitAllCallbacks( TimeSpan.FromMilliseconds( 100 ) );
            }
        }

        private void WaitForCallbacks()
        {
            callbacks.RunWaitCallbacks( TimeSpan.FromSeconds( 1 ) );

            TimeSpan diff = DateTime.Now - connectTime;

            if ( diff > STEAM3_TIMEOUT && !bConnected )
            {
                Logger.Error( "Timeout connecting to Steam3." );
                Abort();

                return;
            }
        }

        private void ConnectedCallback( SteamClient.ConnectedCallback connected )
        {
            Logger.Info( " Done!" );
            bConnecting = false;
            bConnected = true;
            if ( !authenticatedUser )
            {
                Logger.Info( "Logging anonymously into Steam3..." );
                steamUser.LogOnAnonymous();
            }
            else
            {
                Logger.Info( "Logging '{0}' into Steam3...", logonDetails.Username );
                steamUser.LogOn( logonDetails );
            }
        }

        private void DisconnectedCallback( SteamClient.DisconnectedCallback disconnected )
        {
            bDidDisconnect = true;

            if ( disconnected.UserInitiated || bExpectingDisconnectRemote )
            {
                Logger.Info( "Disconnected from Steam" );
            }
            else if ( connectionBackoff >= 10 )
            {
                Logger.Error( "Could not connect to Steam after 10 tries" );
                Abort( false );
            }
            else if ( !bAborted )
            {
                if ( bConnecting )
                {
                    Logger.Warning( "Connection to Steam failed. Trying again" );
                }
                else
                {
                    Logger.Warning( "Lost connection to Steam. Reconnecting" );
                }

                Thread.Sleep( 1000 * ++connectionBackoff );
                steamClient.Connect();
            }
        }

        private void LogOnCallback( SteamUser.LoggedOnCallback loggedOn )
        {
            bool isSteamGuard = loggedOn.Result == EResult.AccountLogonDenied;
            bool is2FA = loggedOn.Result == EResult.AccountLoginDeniedNeedTwoFactor;
            bool isLoginKey = SteamDepotDownloader_GUI.Program.UsrConfig.RememberPassword && logonDetails.LoginKey != null && loggedOn.Result == EResult.InvalidPassword;

            if ( isSteamGuard || is2FA || isLoginKey )
            {
                bExpectingDisconnectRemote = true;
                Abort( false );

                if ( !isLoginKey )
                {
                    Logger.Error( "This account is protected by Steam Guard." );
                }

                if ( is2FA )
                {
                    Logger.Error( "Need your 2 factor auth code from your authenticator app!!!" );
                    return;
                }
                else if ( isLoginKey )
                {
                    ConfigStore.TheConfig.LoginKeys.Remove( logonDetails.Username );
                    ConfigStore.Save();

                    logonDetails.LoginKey = null;

                    if (SteamDepotDownloader_GUI.Program.UsrConfig.SuppliedPassword != null )
                    {
                        Logger.Warning( "Login key was expired. Connecting with supplied password." );
                        logonDetails.Password = SteamDepotDownloader_GUI.Program.UsrConfig.SuppliedPassword;
                    }
                    else
                    {
                        Logger.Error( "Login key was expired!!!" );
                        return;
                    }
                }
                else
                {
                    Logger.Error( "Need authentication code from your email address!!!" );
                    return;
                }

                Logger.Info( "Retrying Steam3 connection..." );
                Connect();

                return;
            }
            else if ( loggedOn.Result == EResult.ServiceUnavailable )
            {
                Logger.Error( "Unable to login to Steam3: {0}", loggedOn.Result );
                Abort( false );

                return;
            }
            else if ( loggedOn.Result != EResult.OK )
            {
                Logger.Error( "Unable to login to Steam3: {0}", loggedOn.Result );
                Abort();

                return;
            }

            Logger.Info( " Done!" );

            this.seq++;
            credentials.LoggedOn = true;

            SteamDepotDownloader_GUI.Program.MainWindowForm.OnLoginSuccessfulAsync();

            if (SteamDepotDownloader_GUI.Program.UsrConfig.CellID == 0 )
            {
                Logger.Info( "Using Steam3 suggested CellID: " + loggedOn.CellID );
                SteamDepotDownloader_GUI.Program.UsrConfig.CellID = ( int )loggedOn.CellID;
            }
        }

        private void SessionTokenCallback( SteamUser.SessionTokenCallback sessionToken )
        {
            Logger.Info( "Got session token!" );
            credentials.SessionToken = sessionToken.SessionToken;
        }

        private void LicenseListCallback( SteamApps.LicenseListCallback licenseList )
        {
            if ( licenseList.Result != EResult.OK )
            {
                Logger.Error( "Unable to get license list: {0} ", licenseList.Result );
                Abort();

                return;
            }

            Logger.Info( "Got {0} licenses for account!", licenseList.LicenseList.Count );
            Licenses = licenseList.LicenseList;


            
            IEnumerable<uint> licenseQuery = Licenses.Select( lic => lic.PackageID);

            Logger.Info( "Licenses: {0}", string.Join( ", ", licenseQuery ) );
        }

        private void UpdateMachineAuthCallback( SteamUser.UpdateMachineAuthCallback machineAuth )
        {
            byte[] hash = Util.SHAHash( machineAuth.Data );
            Logger.Info( "Got Machine Auth: {0} {1} {2} {3}", machineAuth.FileName, machineAuth.Offset, machineAuth.BytesToWrite, machineAuth.Data.Length, hash );

            ConfigStore.TheConfig.SentryData[ logonDetails.Username ] = machineAuth.Data;
            ConfigStore.Save();

            var authResponse = new SteamUser.MachineAuthDetails
            {
                BytesWritten = machineAuth.BytesToWrite,
                FileName = machineAuth.FileName,
                FileSize = machineAuth.BytesToWrite,
                Offset = machineAuth.Offset,

                SentryFileHash = hash, // should be the sha1 hash of the sentry file we just wrote

                OneTimePassword = machineAuth.OneTimePassword, // not sure on this one yet, since we've had no examples of steam using OTPs

                LastError = 0, // result from win32 GetLastError
                Result = EResult.OK, // if everything went okay, otherwise ~who knows~

                JobID = machineAuth.JobID, // so we respond to the correct server job
            };

            // send off our response
            steamUser.SendMachineAuthResponse( authResponse );
        }

        private void LoginKeyCallback( SteamUser.LoginKeyCallback loginKey )
        {
            Logger.Info( "Accepted new login key for account {0}", logonDetails.Username );

            if (SteamDepotDownloader_GUI.Program.UsrConfig.RememberPassword)
            {
                ConfigStore.TheConfig.LoginKeys[logonDetails.Username] = loginKey.LoginKey;
                ConfigStore.Save();
            }
            steamUser.AcceptNewLoginKey( loginKey );
        }

        public void FillAppsList()
        {
            var setList = new HashSet<uint>();
            foreach(var packageInfo in PackageInfo)
            {
                foreach (var appInfo in packageInfo.Value.KeyValues["appids"].Children)
                {
                    setList.Add((uint)appInfo.AsInteger());
                }
            }
            RequestAppsInfo(new List<uint>(setList));
        }

        private void AccountInfoCallback(SteamUser.AccountInfoCallback accountInfo)
        {
            SteamDepotDownloader_GUI.Program.MainWindowForm.UpdateUserName(accountInfo.PersonaName);
        }
    }
}
