using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels;

using Belikov.GenuineChannels.Utilities;
using Belikov.GenuineChannels.TransportContext;
using Belikov.GenuineChannels.GenuineTcp;
using Belikov.GenuineChannels.DotNetRemotingLayer;
using Belikov.GenuineChannels;

namespace iba.Remoting
{
    public class ClientRemotingManager
    {
        static ITransportContext remoteChannel;
        static ITransportContext secureRemoteChannel;

        public static void SetupRemoting()
        {
            GenuineGlobalEventProvider.GenuineChannelsGlobalEvent += OnGenuineChannelsGlobalEvent;

            GenuineThreadPool.GenuineThreadPoolStrategy = GenuineThreadPoolStrategy.AlwaysNative;
            GenuineThreadPool.UnsafeQueuing = true;

            // Creating common properties
            System.Collections.IDictionary props = new System.Collections.Hashtable();
            props["InvocationTimeout"] = 60000;
            //props["MaxTimeSpanToReconnect"] = 1000;
            props["ConnectTimeout"] = 10000;
            props["NoSizeChecking"] = true;
            props["MaxContentSize"] = 10000000;
            props["MaxTotalSize"] = 20000000;
            props["MaxQueuedItems"] = 250;
            props["CloseNamedConnectionAfterInactivity"] = 120000; //120s
            props["ReconnectionTries"] = 0; //This disables the reconnection mechanism
            props["TcpReadRequestBeforeProcessing"] = false; //Increases performance, remove this line when you want to use DXM
            props["TcpDisableNagling"] = true;	//Increase responsiveness
            props["TcpDualSocketMode"] = false; //Don't allow IP v6

            //Registering unsecure channel
            props["name"] = "RemoteChannel";
            GenuineTcpChannel tcpChannel = new GenuineTcpChannel(props, null, null);
            ChannelServices.RegisterChannel(tcpChannel, false);
            remoteChannel = tcpChannel.ITransportContext;

            //Registering secure channel
            System.Collections.Hashtable secureProps = new System.Collections.Hashtable(props);
            secureProps["name"] = "SecureRemoteChannel";
            secureProps["prefix"] = "gstcp";
            GenuineTcpChannel secureChannel = new GenuineTcpChannel(secureProps, null, null);
            ChannelServices.RegisterChannel(secureChannel, false);
            secureRemoteChannel = secureChannel.ITransportContext;
            secureRemoteChannel.IKeyStore.SetKey(ServerRemotingManager.SecureChannelName, new Belikov.GenuineChannels.Security.KeyProvider_SelfEstablishingSymmetric());
            secureRemoteChannel.SecuritySessionParameters = new Belikov.GenuineChannels.Security.SecuritySessionParameters(ServerRemotingManager.SecureChannelName);


            //Log(Logging.Level.Debug, "Registered channel");
        }

        public static void Disconnect(MarshalByRefObject mbr, Exception ex)
        {
            try
            {
                GetHostInformation(mbr, out ITransportContext tp, out HostInformation hi);
                if (tp != null && hi != null)
                    tp.KnownHosts.ReleaseHostResources(hi, ex);
            }
            catch(Exception)
            {
            }
        }

        //Replacement for GenuineUtility.FetchHostInformationFromMbr because it doesn't seem to work for secure channels
        static void GetHostInformation(MarshalByRefObject serverManager, out ITransportContext tp, out HostInformation hi)
        {
            //First try the normal way 
            hi = GenuineUtility.FetchHostInformationFromMbr(serverManager);
            if (hi != null)
            {
                tp = GenuineUtility.FetchTransportContextFromMbr(serverManager);
                return;
            }

            //Try again with the secure channel forced
            string uri;
            GenuineUtility.FetchChannelUriFromMbr(serverManager, out uri, out tp);
            if (uri != null)
            {
                tp = secureRemoteChannel;
                hi = tp.KnownHosts.Get(GenuineUtility.Parse(uri, out _));
            }
        }

        private static void OnGenuineChannelsGlobalEvent(object sender, GenuineEventArgs e)
        {
            Log(Logging.Level.Debug, "Genuine event : {0} caused by {1} for host {2}",
                e.EventType.ToString(), e.SourceException == null ? "null" : e.SourceException.Message,
                e.HostInformation != null ? e.HostInformation.Uri : "null");

            HostInformation hi = e.HostInformation;
            switch (e.EventType)
            {
                case GenuineEventType.GeneralConnectionEstablished:
                    {
                        if (e.HostInformation != null)
                        {
                            Log(Logging.Level.Debug, "Connected to {0}.", e.HostInformation.PhysicalAddress);
                        }
                        break;
                    }

                case GenuineEventType.GeneralConnectionReestablishing:
                    break;

                case GenuineEventType.GeneralConnectionClosed:
                    if (e.HostInformation != null)
                    {
                        try
                        {
                            Log(Logging.Level.Debug, "Connection to {0} is closed.", e.HostInformation.PhysicalAddress);

                            Object obj = e.HostInformation["Guid"];
                            if (obj != null)
                            {
                                Guid guid = (Guid)obj;
                                //Disconnect(guid);
                            }

                            e.HostInformation.ITransportContext.KnownHosts.ReleaseHostResources(e.HostInformation, e.SourceException);
                            
                        }
                        catch (Exception ex)
                        {
                            Log(Logging.Level.Debug, "Error in GeneralConnectionClosed: {0}", ex.ToString());
                        }
                    }
                    break;

                 case GenuineEventType.HostResourcesReleased:
                    break;
            }
        }
    
        static void Log(Logging.Level level, string msg)
        {
            try
            {
                Data.LogData.Data?.Log(level, msg);
            }
            catch(Exception)
            {
            }
        }

        static void Log(Logging.Level level, string msg, object arg)
        {
            Log(level, String.Format(msg, arg));
        }

        static void Log(Logging.Level level, string msg, object arg1, object arg2)
        {
            Log(level, String.Format(msg, arg1, arg2));
        }

        static void Log(Logging.Level level, string msg, params object[] args)
        {
            Log(level, String.Format(msg, args));
        }
    }
}
