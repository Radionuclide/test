using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
using Opc.Ua.Server;

namespace iba.Processing.IbaOpcUa
{
    [Serializable]
    public class IbaOpcUaDiagClient
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public DateTime LastMessageTime { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary> Is used in grid data binding </summary>
        public string LastMessageTimeString
        {
            get
            {
                DateTime localTime = LastMessageTime.ToLocalTime();
                string localTimeStr = localTime.ToLongTimeString();
                return (DateTime.Now - LastMessageTime) < TimeSpan.FromHours(24)
                    ? localTimeStr
                    : $@"{localTime.ToShortDateString()} {localTimeStr}";
            }
        }

        public List<IbaOpcUaDiagSubscription> Subscriptions;

        [Serializable]
        public class IbaOpcUaDiagSubscription
        {
            public uint Id { get; set; }
            public int MonitoredItemCount { get; set; }
            public double PublishingInterval { get; set; }
            public uint NextSequenceNumber { get; set; }
        }
    }

    public class IbaOpcUaServer : StandardServer
    {
        public IbaOpcUaNodeManager IbaOpcUaNodeManager;

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
            IbaOpcUaNodeManager = new IbaOpcUaNodeManager(server, configuration);
            nodeManagers.Add(IbaOpcUaNodeManager);
            
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
            ServerProperties properties = new ServerProperties
            {
                ManufacturerName = "iba-ag.com",
                ProductName = "ibaDatCoordinatorUaServer",
                ProductUri = "http://iba-ag.com",
                SoftwareVersion = Utils.GetAssemblySoftwareVersion(),
                BuildNumber = Utils.GetAssemblyBuildNumber(),
                BuildDate = Utils.GetAssemblyTimestamp()
            };


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
                if (field.GetValue(typeof(StatusCodes)) is uint id)
                {
                    resourceManager.Add(id, "en-US", field.Name);
                }
            }

            return resourceManager;
        }

        /// <summary> Called after the server has been started. </summary>
        protected override void OnServerStarted(IServerInternal server)
        {
            base.OnServerStarted(server);
            // request notifications when the user identity is changed. all valid users are accepted by default.
            server.SessionManager.ImpersonateUser += SessionManager_ImpersonateUser;

            server.SessionManager.SessionActivated += SessionManager_SessionChanged; 
            server.SessionManager.SessionClosing += SessionManager_SessionChanged;
            server.SessionManager.SessionCreated += SessionManager_SessionChanged;

            CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;
        }

        /// <summary> Occurs if there was a change of any kind (activated, closed, etc) in server sessions.
        /// This event can be helpful to double-check current monitored items. </summary>
        public event EventHandler<EventArgs> SessionsStatusChanged;

        /// <summary> Occurs if some incoming client's certificate was rejected.
        /// Newly rejected certificate can automatically appear in Rejected store.
        /// So, this event can be helpful if one needs to track certificate changes. </summary>
        public event EventHandler<EventArgs> ClientCertificateRejected;

        private void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                try
                {
                    // user with a unknown certificate tries to connect to our server
                    // we should add his sertificate as untrusted
                    TaskManager.Manager.CertificateManager.AddCertificate(e.Certificate, false);
                    ClientCertificateRejected?.Invoke(this, EventArgs.Empty);
                }
                catch { /* not critical */ }
            }
        }

        private void SessionManager_SessionChanged(Session session, SessionEventReason reason)
        {
            try
            {
                SessionsStatusChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { /* not critical */ }
        }

        /// <summary>  Called when a client tries to change its user identity. </summary>
        private void SessionManager_ImpersonateUser(Session session, ImpersonateEventArgs args)
        {
            switch (args.NewIdentity)
            {
                case AnonymousIdentityToken _:
                    // no additional checks, just grant access
                    return;

                case UserNameIdentityToken userNameToken:
                    VerifyNamedUser(userNameToken.UserName, userNameToken.DecryptedPassword);
                    break;

                case X509IdentityToken certToken:
                    try
                    {
                        // certToken.Certificate is always null, so let's compare certificates by raw data
                        foreach (var user in CertifiedUsers)
                        {
                            if (user.RawData.SequenceEqual(certToken.CertificateData))
                                // grant access
                                return;
                        }
                    }
                    catch
                    {
                        // can happen (improbably) when collection is modified by another thread 
                    }
                    TaskManager.Manager.CertificateManager.AddCertificate(certToken.Certificate, false);
                    throw ServiceResultException.Create(StatusCodes.BadUserAccessDenied,
                        $@"User certificate {certToken.Certificate.Thumbprint} is not accepted");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary> Verifies name and password for a username token.
        /// Throws if name or password is invalid. Just returns if everything is ok.
        /// </summary>
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void VerifyNamedUser(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                // an empty username is not accepted.
                throw ServiceResultException.Create(StatusCodes.BadIdentityTokenInvalid,
                    "Security token is not a valid username token. An empty username is not accepted.");
            }

            if (string.IsNullOrEmpty(password))
            {
                // an empty password is not accepted.
                throw ServiceResultException.Create(StatusCodes.BadIdentityTokenRejected,
                    "Security token is not a valid username token. An empty password is not accepted.");
            }

            if (userName != NamedUserAccountName)
            {
                throw ServiceResultException.Create(StatusCodes.BadUserAccessDenied,
                    $@"Invalid user name '{userName}'");
            }
            if (password != NamedUserAccountPassword)
            {
                throw ServiceResultException.Create(StatusCodes.BadUserAccessDenied,
                    $@"Invalid password for user '{userName}'");
            }

            // password is correct, user validated successfully - just return
        }

        #endregion


        #region UserAcccounts

        public string NamedUserAccountName { get; private set; }
        public string NamedUserAccountPassword { get; private set; }

        public readonly HashSet<X509Certificate2> CertifiedUsers = new HashSet<X509Certificate2>();

        public void SetUserAccountConfiguration(string name = null, string password = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("User Name cannot be null or whitespace", nameof(name));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("User Password cannot be null", nameof(password));

            NamedUserAccountName = name;
            NamedUserAccountPassword = password;
        }

        #endregion //UserAcccounts


        #region Diagnostics
        
        // todo. kls. low priority - remove this or show current user (and identity type) in diagnostics 
        //public IbaOpcUaUserAccount KlsGetUserForSession(NodeId sessionid)
        //{
        //    IList<Session> sessions = ServerInternal.SessionManager.GetSessions();

        //    if (sessions == null || sessions.Count == 0) return UserAccountAnonymous;

        //    for (int i = 0; i < sessions.Count; i++)
        //    {
        //        if (sessions[i].Id == sessionid)
        //        {
        //            if (sessions[i].Identity.TokenType == UserTokenType.Anonymous) return UserAccountAnonymous;

        //            string un = sessions[i].Identity.DisplayName;
        //            return KlsUserAccountFindByName(un) ?? UserAccountAnonymous;
        //        }
        //    }
        //    return UserAccountAnonymous;
        //}

        public List<IbaOpcUaDiagClient> GetClients()
        {
            try
            {
                var list = new List<IbaOpcUaDiagClient>();

                IList<Session> sessions = ServerInternal.SessionManager.GetSessions();

                if (sessions == null || sessions.Count == 0)
                {
                    return list;
                }

                // description details
                foreach (var s in sessions)
                {
                    var client = new IbaOpcUaDiagClient
                    {
                        Name = s.SessionDiagnostics.SessionName,
                        Id = s.SessionDiagnostics.SessionId.ToString(),
                        LastMessageTime = s.SessionDiagnostics.ClientLastContactTime,
                        Subscriptions = GetSubscriptions(s)
                    };
                    list.Add(client);
                }

                return list;
            }
            catch
            {
                // happens when server is stopped 
                return null;
            }
        }

        private List<IbaOpcUaDiagClient.IbaOpcUaDiagSubscription> GetSubscriptions(Session session)
        {
            try
            {
                var list = new List<IbaOpcUaDiagClient.IbaOpcUaDiagSubscription>();

                IList<Subscription> subs = ServerInternal.SubscriptionManager.GetSubscriptions();
                if (subs == null || subs.Count == 0)
                    return list;

                list.AddRange(from sub in subs
                    where sub != null && sub.Session == session
                    select new IbaOpcUaDiagClient.IbaOpcUaDiagSubscription
                    {
                        Id = sub.Id,
                        MonitoredItemCount = sub.MonitoredItemCount,
                        PublishingInterval = sub.PublishingInterval,
                        NextSequenceNumber = sub.Diagnostics.NextSequenceNumber
                    });

                return list;
            }
            catch
            {
                Debug.Assert(false);
            }
            return null;
        }


        #endregion

    }
}
