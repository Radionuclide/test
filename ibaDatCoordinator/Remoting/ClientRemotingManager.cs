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
        static ITransportContext channel;

        public static void SetupRemoting()
        {
            GenuineGlobalEventProvider.GenuineChannelsGlobalEvent += OnGenuineChannelsGlobalEvent;

            GenuineThreadPool.GenuineThreadPoolStrategy = GenuineThreadPoolStrategy.AlwaysNative;
            GenuineThreadPool.UnsafeQueuing = true;

            System.Collections.IDictionary props = new System.Collections.Hashtable();
            props["name"] = "RemoteChannel";
            props["InvocationTimeout"] = 60000;
            //props["MaxTimeSpanToReconnect"] = 1000;
            props["ConnectTimeout"] = 10000;
            props["NoSizeChecking"] = true;
            props["MaxContentSize"] = 10000000;
            props["MaxTotalSize"] = 20000000;
            props["MaxQueuedItems"] = 250;
            props["CloseNamedConnectionAfterInactivity"] = 120000; //120s
            props["ReconnectionTries"] = 0;//(int ^) 0; //This disables the reconnection mechanism
            props["TcpReadRequestBeforeProcessing"] = false;//(bool ^) false;   //Increases performance, remove this line when you want to use DXM
            props["TcpDisableNagling"] = true;		//Increase responsiveness

            GenuineTcpChannel tcpChannel = new GenuineTcpChannel(props, null, null);
            ChannelServices.RegisterChannel(tcpChannel, false);
            channel = tcpChannel.ITransportContext;

            //Log(Logging.Level.Debug, "Registered channel");
        }

        public static void Disconnect(MarshalByRefObject mbr, Exception ex)
        {
            try
            {
                HostInformation hi = GenuineUtility.FetchHostInformationFromMbr(mbr);
                if (hi != null)
                    hi.ITransportContext.KnownHosts.ReleaseHostResources(hi, ex);
            }
            catch(Exception)
            {
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
