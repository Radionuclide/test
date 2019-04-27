using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
using Opc.Ua.Server;
using ibaOpcServer.IbaOpcUa;
using Opc.Ua.Configuration;

// ReSharper disable once CheckNamespace
namespace iba.ibaOPCServer
{
    public enum IbaOpcUaServerCertificateTrustMode
    {
        DontTrust,
        TrustNextTemporarily,
        TrustAllTemporarily,
        TrustNextPermanently,
        TrustAllPermanently
    }

    public class IbaOpcUaServer : StandardServer
    {
        public IbaOpcUaServer()
        {
            KlsStrEndpoints = "";
            KlsStrEndpointTcp = "";
            KlsStrSessions = "";
            KlsStrSubscriptions = "";
            KlsStrVarTree = "";
            KlsStrMonitoredItems = "";
            KlsStrWriteActions = "0";
        }

        #region Overridden Methods
        /// <summary>
        /// Creates the node managers for the server.
        /// </summary>
        /// <remarks>
        /// This method allows the sub-class create any additional node managers which it uses. The SDK
        /// always creates a CoreNodeManager which handles the built-in nodes defined by the specification.
        /// Any additional NodeManagers are expected to handle application specific nodes.
        /// </remarks>
        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            Utils.Trace("Creating the Node Managers.");

            List<INodeManager> nodeManagers = new List<INodeManager>();

            // create the custom node managers.
            IbaUaNodeManager = new IbaUaNodeManager(this, server, configuration);
            nodeManagers.Add(IbaUaNodeManager);
            
            // create master node manager.
            return new MasterNodeManager(server, configuration, null, nodeManagers.ToArray());
        }

        /// <summary>
        /// Loads the non-configurable properties for the application.
        /// </summary>
        /// <remarks>
        /// These properties are exposed by the server but cannot be changed by administrators.
        /// </remarks>
        protected override ServerProperties LoadServerProperties()
        {
            ServerProperties properties = new ServerProperties();

            properties.ManufacturerName = "iba-ag.com";
            properties.ProductName      = "ibaLogicUaServer";
            properties.ProductUri       = "http://iba-ag.com";
            properties.SoftwareVersion  = Utils.GetAssemblySoftwareVersion();
            properties.BuildNumber      = Utils.GetAssemblyBuildNumber();
            properties.BuildDate        = Utils.GetAssemblyTimestamp();

            // TBD - All applications have software certificates that need to added to the properties.

            return properties;
        }

        /// <summary>
        /// Creates the resource manager for the server.
        /// </summary>
        protected override ResourceManager CreateResourceManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            ResourceManager resourceManager = new ResourceManager(server, configuration);

            System.Reflection.FieldInfo[] fields = typeof(StatusCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (System.Reflection.FieldInfo field in fields)
            {
                uint? id = field.GetValue(typeof(StatusCodes)) as uint?;

                if (id != null)
                {
                    resourceManager.Add(id.Value, "en-US", field.Name);
                }
            }

            return resourceManager;
        }

        /// <summary>
        /// Called after the server has been started.
        /// </summary>
        protected override void OnServerStarted(IServerInternal server)
        {
            base.OnServerStarted(server);
            // request notifications when the user identity is changed. all valid users are accepted by default.
            server.SessionManager.ImpersonateUser += SessionManager_ImpersonateUser;
        }

        /// <summary>
        /// Called when a client tries to change its user identity.
        /// </summary>
        private void SessionManager_ImpersonateUser(Session session, ImpersonateEventArgs args)
        {
            // check for a user name token.
            UserNameIdentityToken userNameToken = args.NewIdentity as UserNameIdentityToken;

            if (userNameToken != null)
            {
                VerifyPassword(userNameToken.UserName, userNameToken.DecryptedPassword);
                //todo
                //args.Identity = new UserIdentity(userNameToken);
            }
        }

        /// <summary>
        /// Validates the password for a username token.
        /// </summary>
        private void VerifyPassword(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName))
            {
                // an empty username is not accepted.
                throw ServiceResultException.Create(StatusCodes.BadIdentityTokenInvalid,
                    "Security token is not a valid username token. An empty username is not accepted.");
            }

            if (String.IsNullOrWhiteSpace(password))
            {
                // an empty password is not accepted.
                throw ServiceResultException.Create(StatusCodes.BadIdentityTokenRejected,
                    "Security token is not a valid username token. An empty password is not accepted.");
            }

            TranslationInfo info;

            // look for the username
            IbaOpcUaUserAccount? acc = KlsUserAccountFindByName(userName);
            
            if (acc.HasValue && !string.IsNullOrWhiteSpace(acc.Value.UserName))
            {
                // we've found the user
                // check his psw
                if (string.Compare(password, acc.Value.Password, StringComparison.Ordinal) == 0)
                {
                    // password is correct
                    // user validated successfully
                    return;
                }

                // if we are here, then user's password is incorrect

                // construct translation object with default text.
                info = new TranslationInfo(
                    "InvalidPassword",
                    "en-US",
                    "Invalid password.",
                    userName);

                // create an exception with a vendor defined sub-code.
                throw new ServiceResultException(new ServiceResult(
                    StatusCodes.BadUserAccessDenied,
                    "InvalidPassword",
                    Configuration.ApplicationUri,
                    new LocalizedText(info)));

            }

            // if we are here, then no user was found with such a name

            // construct translation object with default text.
            info = new TranslationInfo(
                "InvalidUsername",
                "en-US",
                "Invalid username.",
                userName);

            // create an exception with a vendor defined sub-code.
            throw new ServiceResultException(new ServiceResult(
                StatusCodes.BadUserAccessDenied,
                "InvalidUsername",
                Configuration.ApplicationUri,
                new LocalizedText(info)));
        }
        #endregion

        
        public IbaUaNodeManager IbaUaNodeManager; // todo. kls. make private??

        // todo protect by lock for safe thread access? or not so important?
        public string KlsStrEndpoints { get; private set; }
        public string KlsStrEndpointTcp { get; private set; }
        public string KlsStrSessions { get; private set; }
        public string KlsStrSubscriptions { get; private set; }
        public string KlsStrVarTree { get; private set; }
        public string KlsStrMonitoredItems { get; private set; }
        public string KlsStrMonitoredItems2 { get; private set; }
        public string KlsStrWriteActions { get; private set; }

        private int _klsWriteActionsCount;

        #region trust functionality
        private IbaOpcUaServerCertificateTrustMode _trustMode = IbaOpcUaServerCertificateTrustMode.DontTrust;
        public delegate void TrustModeChangedHandler(IbaOpcUaServerCertificateTrustMode mode);

        public event TrustModeChangedHandler OnTrustModeChanged;
        public IbaOpcUaServerCertificateTrustMode TrustMode
        {
            set
            {
                _trustMode = value;
                _trustModeResetHandlers();
                CertificateValidator cv = Configuration.CertificateValidator;

                switch (_trustMode)
                {
                    case IbaOpcUaServerCertificateTrustMode.DontTrust:
                        // do not add any handler
                        break;
                    case IbaOpcUaServerCertificateTrustMode.TrustNextTemporarily:
                        cv.CertificateValidation += KlsCertificateValidator_CertificateValidation_TrustNextTmp;
                        break;
                    case IbaOpcUaServerCertificateTrustMode.TrustAllTemporarily:
                        cv.CertificateValidation += KlsCertificateValidator_CertificateValidation_TrustAllTmp;
                        break;
                    case IbaOpcUaServerCertificateTrustMode.TrustNextPermanently:
                        cv.CertificateValidation += KlsCertificateValidator_CertificateValidation_TrustNextPrm;
                        break;
                    case IbaOpcUaServerCertificateTrustMode.TrustAllPermanently:
                        cv.CertificateValidation += KlsCertificateValidator_CertificateValidation_TrustAllPrm;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                OnTrustModeChanged(_trustMode);
            }
            get { return _trustMode; }
        }

        private void _trustModeResetHandlers()
        {
            CertificateValidator cv = Configuration.CertificateValidator;

            cv.CertificateValidation -= KlsCertificateValidator_CertificateValidation_TrustAllTmp;
            cv.CertificateValidation -= KlsCertificateValidator_CertificateValidation_TrustAllPrm;
            cv.CertificateValidation -= KlsCertificateValidator_CertificateValidation_TrustNextPrm;
            cv.CertificateValidation -= KlsCertificateValidator_CertificateValidation_TrustNextTmp;
        }


        private void KlsCertificateValidator_CertificateValidation_TrustNextTmp(CertificateValidator validator, CertificateValidationEventArgs e)
        {
            // trust temporarily
            KlsCertificateValidator_CertificateValidation_TrustAllTmp(validator, e);

            // reset mode to don't trust
            TrustMode = IbaOpcUaServerCertificateTrustMode.DontTrust;
        }
        private void KlsCertificateValidator_CertificateValidation_TrustAllTmp(CertificateValidator validator, CertificateValidationEventArgs e)
        {
            try
            {
                if (e.Error != null && e.Error.Code == StatusCodes.BadCertificateUntrusted)
                {
                    e.Accept = true;
                    Utils.Trace((int)Utils.TraceMasks.Security, "Automatically temporarily accepted certificate: {0}", e.Certificate.Subject);
                }
            }
            catch (Exception exception)
            {
                Utils.Trace(exception, "Error accepting certificate.");
            }
        }
        private void KlsCertificateValidator_CertificateValidation_TrustNextPrm(CertificateValidator validator, CertificateValidationEventArgs e)
        {
            // trust permanently
            KlsCertificateValidator_CertificateValidation_TrustAllPrm(validator, e);

            // reset mode to don't trust
            TrustMode = IbaOpcUaServerCertificateTrustMode.DontTrust;
        }
        private void KlsCertificateValidator_CertificateValidation_TrustAllPrm(CertificateValidator validator, CertificateValidationEventArgs e)
        {
            try
            {
                if (e.Error != null && e.Error.Code == StatusCodes.BadCertificateUntrusted)
                {
                    e.Accept = true;
                    // add cert to permanent store
                    KlsTrustCertificatePermanently(e.Certificate);
                    Utils.Trace((int)Utils.TraceMasks.Security, "Automatically permanently accepted certificate: {0}", e.Certificate.Subject);
                }
            }
            catch (Exception exception)
            {
                Utils.Trace(exception, "Error accepting certificate.");
            }
        }
        /// <summary>
        /// Saves the certificate in the trusted certificate directory.
        /// </summary>
        public void KlsTrustCertificatePermanently(X509Certificate2 certificate)
        {
            try
            {
                // delete certificate from rejected
                ICertificateStore store = Configuration.SecurityConfiguration.RejectedCertificateStore.OpenStore();
                try
                {
                    store.Delete(certificate.Thumbprint);
                }
                finally
                {
                    store.Close();
                }

                // add certificate to trusted
                store = Configuration.SecurityConfiguration.TrustedPeerCertificates.OpenStore();
                try
                {
                    store.Delete(certificate.Thumbprint);
                    store.Add(certificate);
                }
                finally
                {
                    store.Close();
                }
            }
            catch (Exception e)
            {
                Utils.Trace(e, "Could not write certificate to directory: {0}", Configuration.SecurityConfiguration.TrustedPeerCertificates);
            }
        }
        #endregion //trust functionality


        #region UserAcccounts

        /// <summary>
        /// Whether password encryption is enabled. This applies only to the token type "UserName" of tcp endpoint with Security=None
        /// </summary>
        private bool _passwordEncryptionForTcpNoneEndpoint = true;

        /// <summary>
        /// Whether password encryption is enabled. This applies only to the token type "UserName" of tcp endpoint with Security=None
        /// </summary>
        public bool PasswordEncryptionForTcpNoneEndpoint
        {
            get { return _passwordEncryptionForTcpNoneEndpoint; }
            set
            {
                _passwordEncryptionForTcpNoneEndpoint = value;

                EndpointDescription description = null;
                // find endpointdescription without security
                foreach (var endpointDescription in Endpoints)
                {
                    if (endpointDescription.SecurityMode == MessageSecurityMode.None &&
                        endpointDescription.EndpointUrl.ToLower().Contains("opc.tcp://"))
                    {
                        description = endpointDescription;
                        break;
                    }
                }
                if (description == null) throw new Exception("Endpoint with Security=None not found");
              
                // find token policy for UserName token type
                foreach (UserTokenPolicy policy in description.UserIdentityTokens)
                {
                    if (!policy.PolicyId.ToUpper().Contains("1")) continue;

                    policy.SecurityPolicyUri = _passwordEncryptionForTcpNoneEndpoint
                        ? SecurityPolicies.Basic256
                        : SecurityPolicies.None;
                }
            }
        }

        // list of specific user account (can be empty)
        // needed to be accessed by the form, so is public
        public List<IbaOpcUaUserAccount> UserAccounts = new List<IbaOpcUaUserAccount>();
        
        // represents anonymous user, always exists
        // needed to be accessed by the form, so is public
        public IbaOpcUaUserAccount UserAccountAnonymous;

        public IbaOpcUaUserAccount? KlsUserAccountFindByName(string username)
        {
            foreach (var acc in UserAccounts)
            {
                if (string.Compare(username, acc.UserName, StringComparison.OrdinalIgnoreCase) == 0)
                    return acc;
            }

            // not found
            return null; 
        }
        public void KlsUserAccountsReadFromFile()
        {
            // todo implement read from file or registry
            UserAccounts.Clear();
        }
        public void KlsUserAccountsCreateDummyUsers()
        {
            UserAccounts.Clear();
            
            // simple user - can read and browse
            var user = new IbaOpcUaUserAccount
            {
                TokenType = UserTokenType.UserName,
                UserName = "user1",
                Password = "123"
            };
            user.PermissionsMakeDefault();
            UserAccounts.Add(user);

            // user that has all rights 
            var admin = new IbaOpcUaUserAccount
            {
                TokenType = UserTokenType.UserName,
                UserName = "admin",
                Password = "admin"
            };
            admin.PermissionsEnableAll();
            UserAccounts.Add(admin);
        }
        public void KlsUserAccountsSaveToFile()
        {
            // todo implement
        }

        #endregion //UserAcccounts
        public void KlsInitialize(IbaOpcUaUserAccount? anonymousUser, List<IbaOpcUaUserAccount> preconfiguredUsers,
            bool passwordEncryptionForTcpNoneEndpoint)
        {
            // build var tree
            KlsUpdateConfiguration();

            // prepare endpoint strings for quick use
            KlsStrEndpoints = String.Join(", ", ServerInternal.EndpointAddresses);
            KlsStrEndpointTcp = GetEndpointAddress("opc.tcp");
            // if there is something inside, let's delete last comma

            // set info that we do not have sessions yet
            KlsUpdateSessionsList();

            
            // user management
            {

                // create anonymous user
                UserAccountAnonymous.TokenType = UserTokenType.Anonymous;
                UserAccountAnonymous.PermissionsMakeDefault();

                // set his permissions from file if exist
                if (anonymousUser.HasValue)
                {
                    UserAccountAnonymous.PermissionsMask = anonymousUser.Value.PermissionsMask;
                }

                // set password encryption override
                PasswordEncryptionForTcpNoneEndpoint = passwordEncryptionForTcpNoneEndpoint;

                // copy preconfigured users
                if (preconfiguredUsers != null && preconfiguredUsers.Count != 0)
                {
                    UserAccounts.Clear();
                    foreach (var acc in preconfiguredUsers)
                    {
                        // skip account if it is incomplete
                        if (!acc.CheckIntegrity()) continue;
                        // copy to server's accounts list
                        UserAccounts.Add(acc);
                    }
                }
                else
                    KlsUserAccountsCreateDummyUsers();
            }
        }

        private string GetEndpointAddress(string value)
        {
            var uri = ServerInternal.EndpointAddresses.FirstOrDefault(
                    a => a.Scheme.Equals(value, StringComparison.OrdinalIgnoreCase));
            // get user accounts from file
            if (uri != null)
                return uri.AbsoluteUri;

            return String.Empty;
        }
        public void KlsUninitialize()
        {
            // delete var tree
            KlsUpdateConfiguration();
            // ensure we get an exception on attempt of usage of _onlineServer
            //_onlineServer = null;
        }

        public void KlsUpdateConfiguration()
        {

            //// get PMAC status from online server
            //PmacStatusForUaServer pmacStatus = new PmacStatusForUaServer
            //{
            //    IsOnline = _onlineServer.IsOnline,
            //};

            //if (_onlineServer.IsOnline)
            //{
            //    pmacStatus.IsRunning = _onlineServer.bTargetRunning;
            //    pmacStatus.ProjectName = _onlineServer.KlsCurrentProjectName;
            //    pmacStatus.MaxDongleItems = _onlineServer.KlsMaxOpcDongleItems;
            //    pmacStatus.AvailableVariablesCount = varInfo == null ? 0 : varInfo.VarList.Count;
            //}

            //// rebuild node tree according to varInfo from PMAC
            //_ibaUaNodeManager.KlsUpdateVarTree(varInfo, pmacStatus);

            //// get vartree status string
            //KlsStrVarTree = _ibaUaNodeManager.KlsGetDescriptionStringVarTree();
        }

        //public void KlsUpdateWatchVariableValues(List<VariableInformation.tWatchlistElement> watchList)
        //{
        //    // todo remove statistics
        //    //const double k = 0.1;
        //    //const int Multiplier = 1;

        //    //DateTime start = DateTime.Now;
        //    //for (int i = 0; i < Multiplier; i++)
        //    _ibaUaNodeManager.KlsUpdateWatchVariableValues(watchList);

        //    //double durMs = (DateTime.Now - start).TotalMilliseconds * 1000 / Multiplier ;
        //    //IbaUaNodeManager.TmpKls_DelaySlow = IbaUaNodeManager.TmpKls_DelaySlow * (1 - k) + durMs * k;

        //}

        /// <summary>
        /// todo delete or optimize in release version
        /// </summary>
        /// <returns></returns>
        public string KlsGetDescriptionStringSubscriptions()
        {
            string s = "";
            try
            {
              IList<Subscription> subs = ServerInternal.SubscriptionManager.GetSubscriptions();
#if DEBUG

                if (subs == null || subs.Count == 0)
                    return s;

                s += string.Format("Count = {0}: ", subs.Count);

                const int max = 20;
                int current = 0;
                foreach (var item in subs)
                {
                    if (item == null) continue;
                    
                        s += string.Format("[id={0}, mic={1}, pi={2}], ", item.Id, item.MonitoredItemCount, item.PublishingInterval);

                    // stop processing if list is too big
                    current++;
                    if (current > max) break;
                }
                // remove last ", "
                s = s.TrimEnd(' ', ',');
                return s;
#else
            return string.Format("Count = {0}", subs.Count);
#endif
            }
            catch
            {
                return "error";
            }


        }

        public bool KlsAddWatch(string sVarName)
        {
            KlsStrMonitoredItems = IbaUaNodeManager.KlsGetDescriptionStringMonitoredItems();
            KlsStrMonitoredItems2 = IbaUaNodeManager.KlsGetDescriptionStringMonitoredItems2();
            //return _onlineServer.AddWatchVariable(sVarName);
            return false;
        }
        public bool KlsRemoveWatch(string sVarName)
        {
            KlsStrMonitoredItems = IbaUaNodeManager.KlsGetDescriptionStringMonitoredItems();
            KlsStrMonitoredItems2 = IbaUaNodeManager.KlsGetDescriptionStringMonitoredItems2();
//            return _onlineServer.AddRemoveWatch(sVarName);
            return false;
        }

        public void KlsWrite(object ve, object value, ushort valueSize)
        {
            _klsWriteActionsCount++;
            KlsStrWriteActions = _klsWriteActionsCount.ToString();
//            _onlineServer.SetVariable(ve, value, valueSize);
        }

        public IbaOpcUaUserAccount KlsGetUserForSession(NodeId sessionid)
        {
            IList<Session> sessions = ServerInternal.SessionManager.GetSessions();

            if (sessions == null || sessions.Count == 0) return UserAccountAnonymous;
            
            for (int i = 0; i < sessions.Count; i++)
            {
                if (sessions[i].Id == sessionid)
                {
                    if (sessions[i].Identity.TokenType == UserTokenType.Anonymous) return UserAccountAnonymous;

                    string un = sessions[i].Identity.DisplayName;
                    return KlsUserAccountFindByName(un) ?? UserAccountAnonymous;
                }
            }
            return UserAccountAnonymous;
        }
        private void KlsUpdateSessionsList()
        {
            //List<Session> sessions = new List<Session>(ServerInternal.SessionManager.GetSessions());
            IList<Session> sessions = ServerInternal.SessionManager.GetSessions();

            if (sessions == null || sessions.Count == 0)
            {
                KlsStrSessions = "<no sessions>";
                return;
            }
            // count active sessions
            int activeSessionsCount = 0;
            for (int i = 0; i < sessions.Count; i++)
                if (sessions[i].Activated) activeSessionsCount++;

            // description overview
            KlsStrSessions = string.Format("Total = {0}; Active = {1}: ", sessions.Count, activeSessionsCount);

            // description details
            for (int i = 0; i < sessions.Count; i++)
            {
                Session s = sessions[i];
                if (!s.Activated) continue;
                KlsStrSessions += string.Format("[{0} {1} {2}], ",
                    sessions[i].SessionDiagnostics.SessionName,
                    sessions[i].SessionDiagnostics.SessionId,
                    sessions[i].Identity.DisplayName);
            }
            // remove trailing delimiter
            KlsStrSessions = KlsStrSessions.TrimEnd(',', ' ', ':');
        }


        #region override by kolesnik

        public override ResponseHeader CreateSession(RequestHeader requestHeader, ApplicationDescription clientDescription, string serverUri, string endpointUrl, string sessionName, byte[] clientNonce, byte[] clientCertificate, double requestedSessionTimeout, uint maxResponseMessageSize, out NodeId sessionId, out NodeId authenticationToken, out double revisedSessionTimeout, out byte[] serverNonce, out byte[] serverCertificate, out EndpointDescriptionCollection serverEndpoints, out SignedSoftwareCertificateCollection serverSoftwareCertificates, out SignatureData serverSignature, out uint maxRequestMessageSize)
        {
            ResponseHeader header =
                base.CreateSession(requestHeader, clientDescription, serverUri, endpointUrl, sessionName, clientNonce, clientCertificate, requestedSessionTimeout, maxResponseMessageSize, out sessionId, out authenticationToken, out revisedSessionTimeout, out serverNonce, out serverCertificate, out serverEndpoints, out serverSoftwareCertificates, out serverSignature, out maxRequestMessageSize);

            KlsUpdateSessionsList();
            return header;
        }

        public override ResponseHeader ActivateSession(RequestHeader requestHeader, SignatureData clientSignature,
            SignedSoftwareCertificateCollection clientSoftwareCertificates, StringCollection localeIds,
            ExtensionObject userIdentityToken, SignatureData userTokenSignature, out byte[] serverNonce,
            out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            ResponseHeader responseHeader = 
                base.ActivateSession(
                requestHeader, clientSignature, clientSoftwareCertificates, localeIds, userIdentityToken, userTokenSignature, out serverNonce, out results, out diagnosticInfos);

            KlsUpdateSessionsList();

            return responseHeader;
        }

        public override ResponseHeader CloseSession(RequestHeader requestHeader, bool deleteSubscriptions)
        {
            ResponseHeader header =
                base.CloseSession(requestHeader, deleteSubscriptions);

            KlsUpdateSessionsList();

            return header;
        }

        #endregion


        public static string GetFQDN()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();
            if (!string.IsNullOrEmpty(domainName))
            {
                domainName = "." + domainName;
                if (!hostName.EndsWith(domainName)) // if hostname does not already include domain name
                {
                    hostName += domainName; // add the domain name part
                }
            }
            return hostName; // return the fully qualified name
        }
    }
}
