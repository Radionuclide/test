using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using iba.Processing.IbaOpcUa;
using iba.Properties;
using Opc.Ua;
using Opc.Ua.Configuration;

namespace iba.Processing
{
    public class OpcUaWorker : IDisposable
    {
        private static string CfgXmlConfigFileName { get; } = "ibaDatCoordinatorOpcUaServerConfig.xml";
        private static string CfgConfigSectionName { get; } = "iba_ag.ibaDatCoordinatorOpcUaServer";
        public static string CfgUaTraceFilePath { get; } =
            "%CommonApplicationData%\\iba\\ibaDatCoordinator\\ibaDatCoordinatorLog_OpcUa.txt";
        public static string OpcUaServerString { get; } = "OPC UA Server";

        /// <summary>
        /// If set to true, this will allow clients with
        /// SHA-1-signed certificates and with 1024-bytes-long key certificates
        /// to be connected. (it is not recommended by OPC UA foundation)
        /// </summary>
        public static bool CfgAllowSha1Certificates{ get; } = true;


        #region Construction, Destruction, Init

        public OpcUaWorker()
        {
            Status = ExtMonWorkerStatus.Errored;
            StatusString = Resources.opcUaStatusNotInit;
            _opcUaData = OpcUaData.DefaultData;
        }

        public bool IsInitialized =>
            UaApplication != null && UaAppConfiguration != null && IbaOpcUaServer != null;

        public void Init()
        {
            if (IsInitialized) 
            {
                // disable double initialization
                return;
            }

            try
            {
                UaApplication = new ApplicationInstance();
                IbaOpcUaServer = new IbaOpcUaServer();

                UaApplication.ApplicationType = ApplicationType.Server;
                UaApplication.ConfigSectionName = CfgConfigSectionName;

                // load the application configuration;
                // do it now (not waiting for automatic loading or restart) for the following reasons:
                //  * to get the cfg file from the overridden path;
                //  * and to be able to configure application while server is yet offline;

                var cfgWithPath = Path.Combine(Application.StartupPath, CfgXmlConfigFileName);
                if (!File.Exists(cfgWithPath))
                {
                    // config file is critical; we cannot continue
                    UnInit();
                    throw new Exception($@"Cannot locate OPC UA configuration file: '{cfgWithPath}'");
                }
                try
                {
                    UaApplication.LoadApplicationConfiguration(cfgWithPath, true).Wait();
                    if (UaAppConfiguration == null)
                        throw new Exception();
                }
                catch
                {
                    // config file is critical; we cannot continue
                    UnInit();
                    throw new Exception($@"Cannot load OPC UA configuration file '{cfgWithPath}'");
                }

                // OPC UA trace file
                if (!string.IsNullOrEmpty(UaApplication.ApplicationConfiguration.TraceConfiguration.OutputFilePath))
                {
                    UaApplication.ApplicationConfiguration.TraceConfiguration.OutputFilePath = CfgUaTraceFilePath;
                    UaApplication.ApplicationConfiguration.TraceConfiguration.ApplySettings();
                }

                try
                {
                    // check the application certificate.
                    UaApplication.CheckApplicationInstanceCertificate(false, 0).Wait();
                }
                catch { /* not critical; can be set up later */ }

                // change status from initial (errored) to stopped
                Status = ExtMonWorkerStatus.Stopped;

                // start server (if it is enabled), update status, write to log
                RestartServer();

                RegisterEnums();

                // subscribe to tree structure changes 
                ExtMonInstance.ExtMonStructureChanged += (sender, args) => RebuildTree();

                // turn on monitoring timer
                InitializeMonitoringTimer();

                IbaOpcUaServer.SessionsStatusChanged += (sender, args) =>
                    /* reset monitored groups on any change of sessions status */
                    _monitoredGroups = null;

                IbaOpcUaServer.ClientCertificateRejected += (sender, args) =>
                    /* GUI will utilize this value to know whether it makes sense to refresh certificate list */
                    RejectedCertificatesCounter++;
            }
            catch (Exception ex)
            {
                try
                {
                    LogData.Data.Logger.Log(Level.Exception,
                        $"{nameof(OpcUaWorker)}.{nameof(Init)}. Cannot initialize instance: {ex.Message}.");
                }
                catch { /* logging is not critical */ }
            }
        }

        /// <summary> Doesn't throw exceptions. Uninitializes worker; stops server if applicable. </summary>
        public void UnInit()
        {
            try
            {
                IbaOpcUaServer?.Stop();
            }
            catch { /**/}
            UaApplication = null;
            IbaOpcUaServer = null;
        }

        public void Dispose()
        {
            UnInit();
        }

        // ReSharper disable once UnusedMember.Local - reserved for the future
        private void AssertInitialized()
        {
            // if one of these is null we cannot do anything
            // ReSharper disable once InvertIf
            if (!IsInitialized)
            {
                UnInit();
                throw new InvalidOperationException($@"{nameof(OpcUaWorker)} is not initialized.");
            }
        }

        #endregion


        #region Configuration of UA server

        private ApplicationInstance UaApplication { get; set; }

        private ApplicationConfiguration UaAppConfiguration => UaApplication?.ApplicationConfiguration;

        private IbaOpcUaServer IbaOpcUaServer { get; set; }

        /// <summary> A quick reference to <see cref="IbaOpcUaServer"/>.<see cref="IbaOpcUaNodeManager"/> </summary>
        private IbaOpcUaNodeManager NodeManager => IbaOpcUaServer?.IbaOpcUaNodeManager;

        private OpcUaData _opcUaData;
        public OpcUaData OpcUaData
        {
            get => _opcUaData;
            set
            {
                if (value == null)
                {
                    // do not allow to set null data here
                    return;
                }
                if (_opcUaData != null && _opcUaData.Equals(value))
                {
                    // Configuration has not changed
                    // do not restart agent
                    return;
                }
                _opcUaData = value;

                if (IbaOpcUaServer != null)
                {
                    RestartServer();
                }
            }
        }

        /// <summary> Started / Stopped / Errored </summary>
        public ExtMonWorkerStatus Status { get; private set; }

        public string StatusString { get; private set; }

        public static string GetCurrentThreadString()
        {
            var thr = Thread.CurrentThread;
            string thrNameOrId = String.IsNullOrWhiteSpace(thr.Name) ? thr.ManagedThreadId.ToString() : thr.Name;
            return $"thr=[{thrNameOrId}]";
        }

        private void StartServer()
        {
            ApplyApplicationConfiguration();

            // start server
            UaApplication.Start(IbaOpcUaServer).Wait();
            
            RebuildTree();

            // handle resetting group list on every change of monitored items
            NodeManager.MonitoredItemsChanged += (sender, args) 
                => _monitoredGroups = null;

            // finally, set status
            Status = ExtMonWorkerStatus.Started;
            StatusString = Resources.opcUaStatusRunning;
        }

        private void StopServer()
        {
            // first, set status
            Status = ExtMonWorkerStatus.Stopped;
            StatusString = Resources.opcUaStatusDisabled;
            
            if (NodeManager == null)
            {
                // server was never started, so no need to stop it
                return;
            }

            // delete object tree because anyway on server stop all of them will become invalid
            NodeManager?.DeleteNodeRecursively(NodeManager.FolderIbaRoot, true);

            // stop application and server
            UaApplication.Stop();
        }

        public void RestartServer()
        {
            try
            {
                string logMessage;
                var oldStatus = Status;

                StopServer();

                if (_opcUaData.Enabled)
                {
                    StartServer();

                    logMessage = Status == oldStatus
                        ?
                       // log 'was restarted' if status has not changed (now is 'Started' as before) 
                       String.Format(Resources.opcUaStatusRunningRestarted, StatusString)
                        :
                        // log 'was started' if status has changed from 'Errored' or 'Stopped' to 'Started' 
                        String.Format(Resources.opcUaStatusRunningStarted, StatusString);
                }
                else
                {

                    logMessage = Status == oldStatus
                        ?
                        // do not log anything if status has not changed (now is 'Stopped' as before) 
                        null
                        :
                        // log 'was stopped' if status has changed from 'Errored' or 'Started' to 'Stopped'
                        String.Format(Resources.opcUaStatusStopped, StatusString);
                }

                // log the message if it necessary
                if (logMessage != null)
                {
                    LogData.Data.Logger.Log(Level.Info, logMessage);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex is AggregateException && ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                Status = ExtMonWorkerStatus.Errored;
                StatusString = String.Format(Resources.opcUaStatusError, message);
                if (LogData.Data.Logger.IsOpen) LogData.Data.Logger.Log(Level.Exception, StatusString);
            }
        }
        
        private void ApplyApplicationConfiguration()
        {
            Debug.Assert(UaApplication != null);
            Debug.Assert(IbaOpcUaServer != null);
            Debug.Assert(OpcUaData != null);
            
            ApplyEndpoints();
            ApplySecurityPolicies();
            ApplyUserPolicies();

            // apply additional settings - allow old SHA-1 singed certificates with short keys
            if (CfgAllowSha1Certificates)
            {
                UaAppConfiguration.SecurityConfiguration.RejectSHA1SignedCertificates = false; // default is true
                UaAppConfiguration.SecurityConfiguration.MinimumCertificateKeySize = 1024; // default is 2048
            }
            UaAppConfiguration.Validate(ApplicationType.Server).Wait();
            
            // synchronize between files and _opcUaData.Certificates
            SynchronizeCertificates();
            
            // check own server certificate
            var serverCert = _opcUaData.GetServerCertificate();
            if (serverCert == null)  
                throw new InvalidOperationException("OPC UA Server has no configured certificate"); // todo. kls. localize
            if (!serverCert.IsTrusted)
                throw new InvalidOperationException("The configured OPC UA Server certificate is not trusted"); // todo. kls. localize

            // create list of allowed user certificates
            IbaOpcUaServer.CertifiedUsers.Clear();
            foreach (var certTag in _opcUaData.Certificates)
            {
                if (!certTag.IsUsedForAuthentication || !certTag.IsTrusted)
                    continue;
                Debug.Assert(certTag.Certificate != null);
                IbaOpcUaServer.CertifiedUsers.Add(certTag.Certificate);
            }
        }

        private void ApplyEndpoints()
        {
            UaAppConfiguration.ServerConfiguration.BaseAddresses.Clear();
            UaAppConfiguration.ServerConfiguration.AlternateBaseAddresses.Clear();

            foreach (var ep in _opcUaData.Endpoints)
                UaAppConfiguration.ServerConfiguration.BaseAddresses.Add(ep.Uri);

            if (_opcUaData.Endpoints.Count < 1)
                throw new InvalidOperationException("At least one endpoint should be configured");
        }

        private void ApplySecurityPolicies()
        {
            ServerSecurityPolicyCollection policies = UaAppConfiguration.ServerConfiguration.SecurityPolicies;

            policies.Clear();

            if (_opcUaData.HasSecurityNone)
            {
                policies.AddRange(OpcUaData.CreateSecurityPolicies(
                    OpcUaData.OpcUaSecurityAlgorithm.None, OpcUaData.OpcUaSecurityMode.Unknown));
            }

            if (_opcUaData.HasSecurityBasic128)
            {
                policies.AddRange(OpcUaData.CreateSecurityPolicies(
                    OpcUaData.OpcUaSecurityAlgorithm.Basic128Rsa15, _opcUaData.SecurityBasic128Mode));
            }

            if (_opcUaData.HasSecurityBasic256)
            {
                policies.AddRange(OpcUaData.CreateSecurityPolicies(
                    OpcUaData.OpcUaSecurityAlgorithm.Basic256, _opcUaData.SecurityBasic256Mode));
            }

            if (_opcUaData.HasSecurityBasic256Sha256)
            {
                policies.AddRange(OpcUaData.CreateSecurityPolicies(
                    OpcUaData.OpcUaSecurityAlgorithm.Basic256Sha256, _opcUaData.SecurityBasic256Sha256Mode));
            }

            if (policies.Count < 1)
                throw new InvalidOperationException("At least one security policy should be enabled");
        }

        private void ApplyUserPolicies()
        {
            UserTokenPolicyCollection policies = UaAppConfiguration.ServerConfiguration.UserTokenPolicies;

            policies.Clear();

            if (_opcUaData.IsAnonymousUserAllowed)
            {
                var policy = new UserTokenPolicy(UserTokenType.Anonymous);
                policies.Add(policy);
            }

            if (_opcUaData.IsNamedUserAllowed)
            {
                var policy = new UserTokenPolicy(UserTokenType.UserName);
                policies.Add(policy);
                // set user name ans password
                IbaOpcUaServer.SetUserAccountConfiguration(_opcUaData.UserName, _opcUaData.Password);
            }
            if (_opcUaData.IsCertifiedUserAllowed)
            {
                var policy = new UserTokenPolicy(UserTokenType.Certificate);
                policies.Add(policy);
            }

            if (policies.Count < 1)
            {
                throw new InvalidOperationException("At least one logon policy should be enabled");
            }
        }

        public virtual OpcUaData.NetworkConfiguration GetNetworkConfiguration()
        {
            OpcUaData.NetworkConfiguration cfg = new OpcUaData.NetworkConfiguration();
            cfg.Initialize();
            cfg.UaTraceFilePath = Utils.ReplaceSpecialFolderNames(CfgUaTraceFilePath);
            return cfg;
        }

        #region Certificates

        /// <summary> Indicates how many incoming connections were cancelled because of untrusted certificates.
        /// Change of this value may indicate that some new certificate has appeared in Rejected store.
        /// This may be helpful for optimizing certificate list refreshing. </summary>
        public int RejectedCertificatesCounter { get; private set; }
        private int _lastSyncRejectedCertificatesCounter = -1;

        private CertificateTrustList TrustedCertStore => UaAppConfiguration?.SecurityConfiguration?.TrustedPeerCertificates;
        private CertificateStoreIdentifier RejectedCertStore => UaAppConfiguration?.SecurityConfiguration?.RejectedCertificateStore;
        private CertificateIdentifier OwnCertStore => UaAppConfiguration?.SecurityConfiguration?.ApplicationCertificate;
        
        public List<OpcUaData.CertificateTag> HandleCertificate(string command, object args = null)
        {
            if (!IsInitialized)
                return null;

            // indicate that certificates were changed somehow
            // (it is applicable to all cases except "sync" and "forceSync")
            _opcUaData.CertificateChangesCounter++;

            string thumbprintArg = args as string; // is used in several cases

            switch (command)
            {
                case "forceSync": /*unconditionally sync list with stores*/
                    // for sync command the arg is ignored
                    Debug.Assert(args == null);

                    // undo increment; sync is not a change actually
                    _opcUaData.CertificateChangesCounter--;

                    // no additional action is needed;
                    // sync will be performed below
                    break;

                case "sync": /* sync if there's a change in counter of Rejected certificates */
                    // for sync command the arg is ignored
                    Debug.Assert(args == null);

                    // undo increment; sync is not a change actually
                    _opcUaData.CertificateChangesCounter--;

                    if (_lastSyncRejectedCertificatesCounter == RejectedCertificatesCounter)
                    {
                        // no changes; do nothing
                        return null;
                    }

                    // keep recent value
                    _lastSyncRejectedCertificatesCounter = RejectedCertificatesCounter;
                    
                    // sync will be performed below
                    break;

                case "generate":
                    if (!(args is OpcUaData.CGenerateCertificateArgs genArgs))
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    if (GenerateNewCertificate(genArgs) == null)
                        return null;
                    break;

                case "add":
                    if (!(args is X509Certificate2 certArg))
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    AddExistingCertificate(certArg);
                    break;

                case "remove":
                    if (thumbprintArg == null)
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    RemoveCertificateFromAllStores(thumbprintArg);
                    break;

                case "trust":
                    if (thumbprintArg == null)
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    SetCertificateTrust(thumbprintArg, true);
                    break;

                case "reject":
                    if (thumbprintArg == null)
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    SetCertificateTrust(thumbprintArg, false);
                    break;

                case "asUser":
                    if (thumbprintArg == null)
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    var localCertTag = _opcUaData.GetCertificate(thumbprintArg);
                    if (localCertTag != null)
                        localCertTag.IsUsedForAuthentication ^= true; // toggle
                    break;

                case "asServer":
                    if (thumbprintArg == null)
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    SetServerCertificate(thumbprintArg);
                    break;

                default:
                    throw new NotSupportedException(
                        $@"{nameof(HandleCertificate)}. Command {command} is not supported");
            }

            SynchronizeCertificates();
            return new List<OpcUaData.CertificateTag>(_opcUaData.Certificates);
        }

        public void SynchronizeCertificates()
        {
            // check id uniqueness
            var ids = new HashSet<string>();
            for (var i = _opcUaData.Certificates.Count - 1; i >= 0; i--)
            {
                var thumbprint = _opcUaData.Certificates[i].Thumbprint;
                if (string.IsNullOrWhiteSpace(thumbprint) || ids.Contains(thumbprint))
                {
                    // should not happen
                    _opcUaData.Certificates.RemoveAt(i); // remove duplicate
                }
                ids.Add(thumbprint);
            }

            var trustedCerts = GetTrustedCertificates();
            var rejectedCerts = GetRejectedCertificates();
            var ownCerts = GetOwnCertificates();
            var allCerts = new List<X509Certificate2>();
            allCerts.AddRange(trustedCerts);
            allCerts.AddRange(rejectedCerts);
            allCerts.AddRange(ownCerts);

            // remove non-existent (probably they were deleted by hand)
            for (var i = _opcUaData.Certificates.Count - 1; i >= 0; i--)
            {
                var certTag = _opcUaData.Certificates[i];
                Debug.Assert(certTag?.Thumbprint != null);
                var cert = GetCertificate(certTag.Thumbprint, allCerts); // find in any store by thumbprint

                if (cert == null)
                {
                    // not found in stores;
                    // remove it from our list
                    _opcUaData.Certificates.RemoveAt(i);
                }
            }

            // Add possible missing certificates from Trusted store
            foreach (var cert in trustedCerts)
            {
                // check if already present in collection or add it
                var certTag = _opcUaData.GetCertificate(cert.Thumbprint);

                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                if (certTag == null)
                    certTag = _opcUaData.AddCertificate(cert);

                // bind cert
                certTag.Certificate = cert;
                Debug.Assert(cert.Thumbprint == certTag.Thumbprint);

                // mark as trusted
                certTag.IsTrusted = true;
            }

            // Add possible missing certificates from Rejected store
            foreach (var cert in GetRejectedCertificates())
            {
                // check if already present in collection or add it
                var certTag = _opcUaData.GetCertificate(cert.Thumbprint);

                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                if (certTag == null)
                    certTag = _opcUaData.AddCertificate(cert);

                // bind cert
                certTag.Certificate = cert;
                Debug.Assert(cert.Thumbprint == certTag.Thumbprint);

                // mark as rejected
                certTag.IsTrusted = false;
            }

            // Add possible missing certificates from Own store
            foreach (var cert in ownCerts)
            {
                if (!cert.HasPrivateKey)
                {
                    // should not happen
                    // skip certs that do not have private keys
                    // because they cannot serve as our own certificate
                    continue;
                }

                // check if already present in collection or add it
                var certTag = _opcUaData.GetCertificate(cert.Thumbprint);

                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                if (certTag == null)
                    certTag = _opcUaData.AddCertificate(cert);

                // bind cert
                certTag.Certificate = cert;
                certTag.HasPrivateKey = cert.HasPrivateKey;
                Debug.Assert(cert.Thumbprint == certTag.Thumbprint);
            }

            // set server cert
            var serverCertTag = _opcUaData.GetServerCertificate(); // check if one is set in _opcUaData
            if (serverCertTag != null)
            {
                // reset the flag temporarily (for reliability; it will be set later in SetServerCertificate function) 
                serverCertTag.IsUsedForServer = false; 
                // set server certificate
                SetServerCertificate(serverCertTag.Thumbprint);
            }
            else
            {
                // try to select the one that was generated by default (may happen on the first launch)
                // this is needed to ensure server will run out of the box
                // when nothing special was yet configured by the user

                var currentConfigCertThumbprint = UaAppConfiguration.SecurityConfiguration?.ApplicationCertificate?.Thumbprint;
                // check if it exists and is present our list
                // (this is not always true, because SecurityConfiguration.ApplicationCertificate can contain cached data)
                if (currentConfigCertThumbprint != null && GetCertificate(currentConfigCertThumbprint, ownCerts) != null)
                {
                    _opcUaData.SetServerCertificateFlag(currentConfigCertThumbprint);
                }
            }
        }
        
        /// <summary> Removes the certificate from all stores (own, rejected, trusted) </summary>
        private async void RemoveCertificateFromAllStores(string thumbprint)
        {
            Debug.Assert(UaAppConfiguration != null);

            ICertificateStore store = OwnCertStore.OpenStore();
            try
            {
                await store.Delete(thumbprint);
            }
            finally
            {
                store.Close();
            }

            await RemoveCertificateFromTrustedAndRejected(thumbprint);           
        }

        private async Task RemoveCertificateFromTrustedAndRejected(string thumbprint)
        {
            Debug.Assert(UaAppConfiguration != null);

            ICertificateStore store = TrustedCertStore.OpenStore();
            try
            {
                await store.Delete(thumbprint);
            }
            finally
            {
                store.Close();
            }

            store = RejectedCertStore.OpenStore();
            try
            {
                await store.Delete(thumbprint);
            }
            finally
            {
                store.Close();
            }

            // Update Certificate validator; otherwise it still can keep deleted certs in his cache
            await UaApplication.ApplicationConfiguration.CertificateValidator.Update(UaAppConfiguration);
        }

        private void SetCertificateTrust(string thumbprint, bool isTrusted)
        {
            var cert = GetCertificate(thumbprint);
            if (cert != null)
            {
                SetCertificateTrust(cert, isTrusted);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary> Saves the certificate to the trusted or rejected certificate
        /// directory, depending on <see cref="isTrusted"/> argument. </summary>
        private void SetCertificateTrust(X509Certificate2 certificate, bool isTrusted)
        {
            Debug.Assert(UaAppConfiguration != null);

            // remove certificate from both stores
            // todo. kls. low priority - better async support?
            RemoveCertificateFromTrustedAndRejected(certificate.Thumbprint).Wait();

            // add certificate to the needed store
            ICertificateStore store =
                isTrusted ? TrustedCertStore.OpenStore() : RejectedCertStore.OpenStore();

            try
            {
                certificate.PrivateKey = null; // add to the trusted/rejected store without a private part
                // todo. kls. low priority - better async support?
                store.Add(certificate).Wait();
            }
            catch
            {
                // sometimes happen for some imported certificates,
                // though certificate is actually added normally
            }
            finally
            {
                store.Close();
            }
        }

        /// <summary> Saves the certificate to the trusted certificate store </summary>
        private void AddExistingCertificate(X509Certificate2 certificate)
        {
            if (certificate.HasPrivateKey)
            {
                ICertificateStore store = OwnCertStore.OpenStore();
                try
                {
                    // todo. kls. low priority - better async support?
                    store.Add(certificate).Wait();
                }
                catch
                {
                     /**/
                }
                finally
                {
                    store.Close();
                }  
            }
            else
            {
                // this will automatically remove existing duplicate (if exists)
                // add then will add the certificates to trusted store
                SetCertificateTrust(certificate, true);
            }
        }

        private X509Certificate2 GenerateNewCertificate(OpcUaData.CGenerateCertificateArgs genArgs)
        {
            try
            {
                IList<string> serverDomainNames = UaAppConfiguration.GetServerDomainNames();
                if (serverDomainNames.Count == 0)
                    serverDomainNames.Add(Dns.GetHostName());

                CertificateIdentifier certId = UaAppConfiguration.SecurityConfiguration.ApplicationCertificate;

                // SHA-2 has 224+ bits (SHA-224, SHA-256, SHA-384, SHA-512, SHA-512/256 и SHA-512/224)
                // we want to have 256 bits for SHA-2 in our case;
                // SHA-1 has 160 bits;
                // see CertificateFactory.GetRSAHashAlgorithm() for details
                ushort hashSizeInBits = (ushort) (genArgs.UseSha256 ? 256 /*SHA-2-256*/ : 160 /*SHA-1-160*/);

                var cert = CertificateFactory.CreateCertificate(
                    certId.StoreType, certId.StorePath, null, 
                    genArgs.ApplicationUri, genArgs.ApplicationName,
                    genArgs.ApplicationName,
                    serverDomainNames, (ushort)genArgs.KeySize, 
                    DateTime.Now.ToUniversalTime(), (ushort)genArgs.Lifetime,
                    hashSizeInBits);

                // certificate is automatically added to the own store;
                // copy it to the trusted store also
                SetCertificateTrust(cert, true);

                return cert;
            }
            catch
            {
                return null;
            }
        }

        private void SetServerCertificate(string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
                return;

            foreach (var cert in GetOwnCertificates())
            {
                // ReSharper disable once InvertIf
                if (cert.Thumbprint == thumbprint && cert.HasPrivateKey)
                {
                    UaAppConfiguration.SecurityConfiguration.ApplicationCertificate.Certificate = cert;
                    _opcUaData.SetServerCertificateFlag(cert.Thumbprint);
                    return;
                }
            }
        }
        

        private static List<X509Certificate2> GetCertificates(ICertificateStore store)
        {
            try
            {
                var list = new List<X509Certificate2>();
                var enumTask = store.Enumerate();
                // todo. kls. low priority - better async support?
                enumTask.Wait();
                foreach (var cert in enumTask.Result)
                {
                    list.Add(cert);
                }
                return list;
            }
            finally
            {
                store.Close();
            }
        }

        private List<X509Certificate2> GetOwnCertificates() => GetCertificates(OwnCertStore.OpenStore());

        private List<X509Certificate2> GetTrustedCertificates() => GetCertificates(TrustedCertStore.OpenStore());

        private List<X509Certificate2> GetRejectedCertificates() => GetCertificates(RejectedCertStore.OpenStore());

        // todo. kls. low priority - optimize lookup using hash 
        public X509Certificate2 GetCertificate(string thumbprint)
        {
            // look in own store
            X509Certificate2 cert = GetCertificate(thumbprint, GetOwnCertificates());
            if (cert != null)
                return cert;
            // look in trusted store
            cert = GetCertificate(thumbprint, GetTrustedCertificates());
            if (cert != null)
                return cert;
            // look in rejected store
            cert = GetCertificate(thumbprint, GetRejectedCertificates());
            return cert;
        }

        public X509Certificate2 GetCertificate(string thumbprint, List<X509Certificate2> certs)
        {
            foreach (var cert in certs)
            {
                if (cert.Thumbprint == thumbprint)
                    return cert;
            }
            return null;
        }

        #endregion

        #endregion


        #region Handling object tree - building, refreshing

        /// <summary> A quick reference to singleton <see cref="ExtMonData"/> </summary>
        private static ExtMonData ExtMonInstance => ExtMonData.Instance;

        /// <summary> A quick reference to <see cref="ExtMonData"/> instance lock </summary>
        private static object LockObject => ExtMonInstance.LockObject;

        /// <summary> A quick reference to <see cref="ExtMonData"/> recommended lock timeout </summary>
        private static int LockTimeout => ExtMonData.LockTimeout;


        #region Register enums

        private void RegisterEnums()
        {
            // In OPC UA there is a special support for enums.
            // (search for EnumValueType in "OPC UA Part 8 - DataAccess 1.03 Specification.pdf"
            // and "OPC UA Part 3 - Address Space Model 1.03 Specification.pdf")
            // Every user-defined enum should be declared by creating special nodes.
            // OPC UA Client then can read enum description nodes to interpret enum variables.
            // It can be implemented here if requested by customers.

            //_enumJobStatus = IbaOpcUaServer.RegisterEnumDataType(
            //    "JobStatus", "Current status of the job (started, stopped or disabled)",
            //    new Dictionary<int, string>
            //    {
            //        {(int) SnmpObjectsData.JobStatus.Disabled, "disabled"},
            //        {(int) SnmpObjectsData.JobStatus.Started, "started"},
            //        {(int) SnmpObjectsData.JobStatus.Stopped, "stopped"}
            //    }
            //);

            //_enumCleanupType = IbaOpcUaServer.RegisterEnumDataType(
            //    "LocalCleanupType", "Type of limitation of disk space usage",
            //    new Dictionary<int, string>
            //    {
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.None, "none"},
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDirectories, "limitDirectories"},
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDiskspace, "limitDiskSpace"},
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.SaveFreeSpace, "saveFreeSpace"}
            //    }
            //);
        }

        #endregion


        #region Building the tree

        private HashSet<IbaOpcUaVariable> _deletionPendingNodes;

        public bool RebuildTree()
        {
            var man = TaskManager.Manager;
            if (man == null || UaAppConfiguration == null || IbaOpcUaServer?.IbaOpcUaNodeManager == null)
            {
                // don't throw here, just return
                return false; // rebuild failed
            }

            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    // reset currently monitored items to ensure they don't cache any outdated items
                    _monitoredGroups = null;

                    if (!ExtMonInstance.IsStructureValid)
                    {
                        // ExtMonData structure is invalid;
                        // it makes no sense to rebuild our tree
                        return false; // rebuild failed
                    }

                    _deletionPendingNodes = new HashSet<IbaOpcUaVariable>(NodeManager.GetFlatListOfAllIbaVariables());

                    foreach (var node in ExtMonInstance.FolderRoot.Children)
                    {
                        Debug.Assert(node is ExtMonData.ExtMonFolder);
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (node is ExtMonData.ExtMonFolder xmFolder)
                            BuildFolderRecursively(null, xmFolder);
                    }

                    // delete nodes that were marked for deletion and were not updated
                    foreach (var node in _deletionPendingNodes)
                    {
                        if (node is IbaOpcUaVariable iv )
                        {
                            // remove handler
                            // ReSharper disable once DelegateSubtraction
                            iv.OnReadValue -= OnReadProductSpecificValue;

                            // delete node, but don't delete empty folders yet (to preserve section folders)
                            NodeManager.DeleteNodeRecursively(iv, false);
                        }
                    }

                    // delete all empty folders. (section folders will be preserved even if empty)
                    NodeManager.DeleteEmptySubfolders();
                    
                    return true; // rebuilt successfully
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // failed to acquire a lock
                try
                {
                    LogData.Data.Logger.Log(Level.Debug,
                        $"{nameof(OpcUaWorker)}.{nameof(RebuildTree)}. Error acquiring lock when rebuilding the tree, {GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                return false; // rebuild failed
            }
        }

        private void BuildFolderRecursively(FolderState uaParentFolder, ExtMonData.ExtMonFolder startingFolder)
        {
            try
            {
                FolderState uaFolder = CreateOrUpdateOpcUaFolder(uaParentFolder, startingFolder);

                foreach (var node in startingFolder.Children)
                {
                    switch (node)
                    {
                        case ExtMonData.ExtMonFolder xmFolder:
                            BuildFolderRecursively(uaFolder, xmFolder);
                            break;
                        case ExtMonData.ExtMonVariableBase xmv:
                            CreateOrUpdateOpcUaValue(uaFolder, xmv);
                            break;
                        default:
                            continue;
                    }
                }
            }
            catch
            {
                // go on with other items 
                // even if current one has failed 
            }

        }


        #region Create/Update Value/Folder

        private FolderState CreateOrUpdateOpcUaFolder(FolderState uaParentFolder, ExtMonData.ExtMonFolder xmFolderToCreate)
        {
            if (uaParentFolder == null)
                uaParentFolder = NodeManager.FolderIbaRoot;

            Debug.Assert(IbaOpcUaNodeManager.IsValidBrowseName(xmFolderToCreate.UaBrowseName));

            NodeState node = NodeManager.Find(xmFolderToCreate.UaFullPath);

            Debug.Assert(node == null || node.NodeId.Identifier as string == xmFolderToCreate.UaFullPath);

            switch (node)
            {
                case null:
                    // node does not exist. Create it
                    return CreateOpcUaFolder(uaParentFolder, xmFolderToCreate);
                case FolderState folder:
                    // such folder already exists;
                    // update its Caption and Description
                    // because it could be renamed or reordered
                    folder.DisplayName = xmFolderToCreate.Caption;
                    folder.Description = xmFolderToCreate.Description;
                    return folder;
                default:
                    // node exists but it's not a folder
                    // it's strange, and it should not happen...
                    Debug.Assert(false);
                    return null;
            }
        }

        private FolderState CreateOpcUaFolder(FolderState uaParentFolder, ExtMonData.ExtMonFolder xmFolderToCreate)
        {
            // create
            FolderState folder = NodeManager.CreateFolderAndItsNode(
                uaParentFolder ?? NodeManager.FolderIbaRoot,
                xmFolderToCreate.UaBrowseName, xmFolderToCreate.Caption, xmFolderToCreate.Description);

            // ensure created NodeId looks as expected
            Debug.Assert(folder.NodeId.Identifier as string == xmFolderToCreate.UaFullPath);

            return folder;
        }


        // ReSharper disable once UnusedMethodReturnValue.Local - reserved for the future
        private IbaOpcUaVariable CreateOrUpdateOpcUaValue(FolderState uaParentFolder, ExtMonData.ExtMonVariableBase xmv)
        {
            Debug.Assert(IbaOpcUaNodeManager.IsValidBrowseName(xmv.UaBrowseName));

            NodeState node = NodeManager.Find(xmv.UaFullPath);

            switch (node)
            {
                case null:
                    // node does not exist. Create it
                    return CreateOpcUaValue(uaParentFolder, xmv);
                case IbaOpcUaVariable iv:
                    // such variable already exists
                    // let's update cross reference
                    iv.SetCrossReference(xmv);
                    // this node is present in ExtMonData and should NOT be deleted
                    _deletionPendingNodes.Remove(iv);
                    // Caption and Description theoretically never change for variables
                    Debug.Assert(iv.DisplayName == xmv.Caption);
                    Debug.Assert(iv.Description == xmv.Description);
                    return iv;
                default:
                    // node exists but it's not IbaOpcUaVariable
                    // it's strange, and it should not happen...
                    Debug.Assert(false);
                    return null;
            }
        }

        private IbaOpcUaVariable CreateOpcUaValue(FolderState uaParentFolder, ExtMonData.ExtMonVariableBase xmv)
        {
            IbaOpcUaVariable iv = NodeManager.CreateIbaVariable(uaParentFolder, xmv);

            // ensure created NodeId looks as expected
            Debug.Assert(iv.NodeId.Identifier as string == xmv.UaFullPath);

            // add handler
            iv.OnReadValue += OnReadProductSpecificValue;
            return iv;
        }

        /// <summary> Transfers value from <see cref="ExtMonData.ExtMonVariableBase"/>
        /// to a corresponding OPC UA node </summary>
        private void SetOpcUaValue(ExtMonData.ExtMonVariableBase xmv)
        {
            if (xmv?.UaVar == null)
                return;

            if (xmv.UaVar.IsDeleted)
            {
                NodeManager.SetNullValueAndMarkAsDeleted(xmv.UaVar);
            }
            else
            {
                NodeManager.SetValueScalar(xmv.UaVar, xmv.ObjValue);
            }
        }

        #endregion

        #endregion


        #region Value refresh and OnRead event handlers


        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool RefreshGroup(ExtMonData.ExtMonGroup xmGroup)
        {
            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    if (ExtMonInstance.RebuildTreeIfItIsInvalid())
                    {
                        // tree was rebuilt completely
                        // no need to update some parts of it
                        // just return right now
                        return true; // data was updated
                    }

                    if (xmGroup.IsUpToDate())
                    {
                        // data is fresh, no need to change something
                        return false; // was not updated
                    }

                    var man = TaskManager.Manager;

                    bool bSuccess;
                    switch (xmGroup)
                    {
                        case ExtMonData.LicenseInfo licenseInfo:
                            bSuccess = man.ExtMonRefreshLicenseInfo(licenseInfo);
                            break;
                        case ExtMonData.GlobalCleanupDriveInfo driveInfo:
                            bSuccess = man.ExtMonRefreshGlobalCleanupDriveInfo(driveInfo);
                            break;
                        case ExtMonData.JobInfoBase jobInfo:
                            bSuccess = man.ExtMonRefreshJobInfo(jobInfo);
                            break;
                        default:
                            // should not happen
                            bSuccess = false;
                            Debug.Assert(false);
                            break;
                    }

                    if (!bSuccess)
                    {
                        // failed to update the data;
                        // mark tree invalid to rebuild it later
                        LogData.Data.Logger.Log(Level.Debug,
                            $"{nameof(OpcUaWorker)}.{nameof(RefreshGroup)}. Failed to refresh group {xmGroup.Caption}; tree is marked invalid.");
                        ExtMonInstance.IsStructureValid = false;
                        return false; // data was NOT updated
                    }

                    // TaskManager has updated the group successfully;
                    // copy it to UA tree
                    foreach (var xmv in xmGroup.GetFlatListOfAllVariables())
                    {
                        SetOpcUaValue(xmv);
                    }

                    return true; // data was updated
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // failed to acquire a lock
                try
                {
                    LogData.Data.Logger.Log(Level.Debug,
                        $"{nameof(OpcUaWorker)}.{nameof(RefreshGroup)}. Error acquiring lock when updating {xmGroup.Caption}, {GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                return false; // data was NOT updated
            }
        }


        private ServiceResult OnReadProductSpecificValue(ISystemContext context,
            NodeState node, NumericRange indexRange, QualifiedName dataEncoding,
            // ReSharper disable RedundantAssignment
            ref object value, 
            ref StatusCode statusCode, ref DateTime timestamp)
            // ReSharper restore RedundantAssignment
        {
            if (!(node is IbaOpcUaVariable iv)) //we handle only iba variables here 
            {
                value = null;
                statusCode = StatusCodes.Bad;
                Debug.Assert(false); // should not happen
                return ServiceResult.Good; // statusCode is bad; serviceResult is good
            }

            if (iv.IsDeleted)
            {
                // don't try to refresh deleted variables
                iv.Value = null;
                Debug.Assert(iv.StatusCode == StatusCodes.BadObjectDeleted);
                iv.StatusCode = StatusCodes.BadObjectDeleted;
            }
            else
            {
                Debug.Assert(iv.Value == value); // should be the same at this point
                Debug.Assert(iv.ExtMonVar.Group != null);

                // refresh data if it is too old (or rebuild the whole tree if necessary)
                RefreshGroup(iv.ExtMonVar.Group);
            }

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            value = iv.Value;
            statusCode = iv.StatusCode;
            Debug.Assert(iv.StatusCode == StatusCodes.Good || iv.StatusCode == StatusCodes.BadObjectDeleted);

            return ServiceResult.Good;
        }

        #endregion


        #region Monitored itmes handling

        private readonly System.Timers.Timer _monitoringTimer = new System.Timers.Timer { Enabled = false };

        private void InitializeMonitoringTimer()
        {
            // set interval to value slightly bigger than AgeThreshold,
            // to ensure we don't request update twice within one interval
            _monitoringTimer.Interval = (int)(ExtMonData.AgeThreshold.TotalMilliseconds * 1.04);
            // let it be always enabled as long as worker lives;
            // if OPC UA is disabled then timer handler does nothing and returns immediately
            _monitoringTimer.Enabled = true;
            _monitoringTimer.Elapsed += OnMonitoringTimerTick;
        }

        private HashSet<ExtMonData.ExtMonGroup> _monitoredGroups;

        private void OnMonitoringTimerTick(object sender, EventArgs args)
        {
            if (!IsInitialized || !OpcUaData.Enabled || NodeManager == null)
                return;

            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    if (_monitoredGroups == null)
                        PrepareMonitoredGroupsList();

                    if (_monitoredGroups == null)
                        return;

                    // refresh all groups that have items being monitored
                    foreach (var group in _monitoredGroups)
                    {
                        RefreshGroup(group);
                    }
                }
                catch
                {
                    /*should not happen*/
                    Debug.Assert(false);
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // failed to acquire a lock
                try
                {
                    LogData.Data.Logger.Log(Level.Debug,
                        $"{nameof(OpcUaWorker)}.{nameof(OnMonitoringTimerTick)}. Error acquiring lock, {GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }
            }
        }


        private void PrepareMonitoredGroupsList()
        {
            try
            {
                _monitoredGroups = null;
                var monitoredNodes = NodeManager.GetMonitoredNodes();

                // prepare a set of groups to be refreshed
                var newMonitoredGroupsList = new HashSet<ExtMonData.ExtMonGroup>();
                foreach (var kvp in monitoredNodes)
                {
                    NodeState node = kvp.Value.Node;
                    if (node is IbaOpcUaVariable iv && !iv.IsDeleted && iv.ExtMonVar?.Group != null)
                    {
                        newMonitoredGroupsList.Add(iv.ExtMonVar.Group);
                    }
                }

                _monitoredGroups = newMonitoredGroupsList;
            }
            catch
            {
                /*should not happen*/
                Debug.Assert(false);
            }
        }

        #endregion

        #endregion


        #region Tree Snapshot for GUI

        public List<ExtMonData.GuiTreeNodeTag> GetObjectTreeSnapShot()
        {
            if (!IsInitialized)
                return null;

            try
            {
                // check tree structure before taking a snapshot
                ExtMonInstance.RebuildTreeIfItIsInvalid();

                // we need List rather than HashSet to keep the original order
                var result = new List<ExtMonData.GuiTreeNodeTag>();
                var objList = ExtMonInstance.GetFlatListOfAllChildren();
                if (objList == null)
                {
                    return null;
                }

                //retrieve information about each node
                foreach (var node in objList)
                {
                    var tag = GetTreeNodeTag(node);
                    if (tag != null)
                    {
                        result.Add(tag);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(OpcUaWorker)}.{nameof(GetObjectTreeSnapShot)}. {ex.Message}");
                return null;
            }
        }

        /// <summary> Gets all information about a node in the format convenient for GUI tree. </summary>
        public ExtMonData.GuiTreeNodeTag GetTreeNodeTag(ExtMonData.ExtMonNode node)
        {
            try
            {
                if (node == null)
                {
                    Debug.Assert(false);
                    return null;
                }

                ExtMonData.GuiTreeNodeTag tag = new ExtMonData.GuiTreeNodeTag
                {
                    Type = "",
                    Value = "",
                    Caption = node.Caption,
                    Description = node.Description,
                    IsFolder = true,
                    OpcUaNodeId = node.UaFullPath
                };

                Debug.Assert(tag.OpcUaNodeId != null);

                // ReSharper disable once InvertIf
                if (node is ExtMonData.ExtMonVariableBase xmv)
                {
                    tag.IsFolder = false;

                    var type = xmv.ObjValue.GetType();
                    tag.Type = type.IsEnum ? "Enum" : type.Name;

                    // try to get an updated value
                    try
                    {
                        RefreshGroup(xmv.Group);
                        object val = xmv.ObjValue;
                        tag.Value = type.IsEnum ? $"{(int)val} ({val})" : $"{val}";
                    }
                    catch
                    {
                        tag.Value = "(error)";
                        Debug.Assert(false);
                    }
                }
                return tag;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(OpcUaWorker)}.{nameof(GetTreeNodeTag)}({node}). {ex.Message}");
                return null;
            }
        }

        /// <summary> Gets all information about a node in the format convenient for GUI tree. </summary>
        public ExtMonData.GuiTreeNodeTag GetTreeNodeTag(string nodeId)
        {
            if (!IsInitialized)
                return null;

            try
            {
                if (string.IsNullOrWhiteSpace(nodeId))
                {
                    Debug.Assert(false);
                    // illegal nodeId
                    return null;
                }

                List<ExtMonData.ExtMonNode> children = ExtMonInstance.GetFlatListOfAllChildren();
                foreach (var node in children)
                {
                    if (node.UaFullPath == nodeId)
                        return GetTreeNodeTag(node); 
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(OpcUaWorker)}.{nameof(GetTreeNodeTag)}({nodeId}). {ex.Message}");
            }
            return null;
        }

        #endregion


        #region Diagnostics

        public List<IbaOpcUaDiagClient> GetClients() => 
            Status == ExtMonWorkerStatus.Started ? IbaOpcUaServer.GetClients() : null;
        
        public string GetDiagnosticString()
        {
            try
            {
                if (!OpcUaData.Enabled || NodeManager == null)
                    return null;

                string strEndpoints = "";
                foreach (string adr in UaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses)
                {
                    strEndpoints += adr + ", ";
                }
                strEndpoints = strEndpoints.Trim(' ', ',');


                var monitoredNodes = NodeManager.GetMonitoredNodes();

                string strMonNodes = "";
                string strMonGroups = "";

                foreach (var kvp in monitoredNodes)
                {
                    NodeState node = kvp.Value.Node;
                    if (node is IbaOpcUaVariable iv && !iv.IsDeleted && iv.ExtMonVar?.Group != null)
                    {
                        strMonNodes += $@"{iv.ExtMonVar.Caption}, ";
                    }
                }

                if (_monitoredGroups != null)
                {
                    foreach (var group in _monitoredGroups)
                    {
                        strMonGroups += $@"{group.Caption}, ";
                    }
                }

                strMonNodes = strMonNodes.TrimEnd(' ', ',');
                strMonGroups = strMonGroups.TrimEnd(' ', ',');

                return
                    $"Endpoints: {strEndpoints}\r\nMonitoredGroups: {strMonGroups}\r\nMonitoredNodes: {strMonNodes}";
            }
            catch
            {
                /*getting diagnostic string is not critical*/
                Debug.Assert(false);
            }
            return null;
        }

        #endregion
    }
}
