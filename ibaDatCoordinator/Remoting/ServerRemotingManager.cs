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
    public class ServerRemotingManager
    {
        static ITransportContext serverChannel;
        static System.Collections.Concurrent.ConcurrentDictionary<string, string> clients = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();

        public static void SetupRemoting(CommunicationObject commObj, int portNr)
        {
            GenuineGlobalEventProvider.GenuineChannelsGlobalEvent += OnGenuineChannelsGlobalEvent;

            GenuineThreadPool.GenuineThreadPoolStrategy = GenuineThreadPoolStrategy.AlwaysNative;
            GenuineThreadPool.UnsafeQueuing = true;

            System.Collections.IDictionary props = new System.Collections.Hashtable();
            props["name"] = "RemoteChannel";
            props["port"] = portNr;
            props["InvocationTimeout"] = 60000;
            //props["MaxTimeSpanToReconnect"] = 1000;
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
            serverChannel = tcpChannel.ITransportContext;

            // Expose communication object on registered channel
            System.Runtime.Remoting.RemotingServices.Marshal(commObj, "IbaDatCoordinatorCommunicationObject", typeof(CommunicationObject));
            Log(Logging.Level.Info, "Remoting config OK");
        }

        public static string RegisterClient(string clientName)
        {
            try
            {
                HostInformation hi = GenuineUtility.CurrentSession as HostInformation;
                string remoteEp = hi.PhysicalAddress.ToString();
                if(!String.IsNullOrEmpty(remoteEp))
                    clients.TryAdd(remoteEp, clientName);
                return remoteEp;
            }
            catch (Exception)
            {
                return "";
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
                    if(e.HostInformation != null)
                        Log(Logging.Level.Debug, "{0} is connected", e.HostInformation.PhysicalAddress);

                    break;

                case GenuineEventType.GeneralConnectionReestablishing:
                    //Force close, don't allow automatic reconnection
                    //e->HostInformation->ITransportContext->KnownHosts->ReleaseHostResources(e->HostInformation, 
                    //    GenuineExceptions::Get_Receive_ConnectionClosed());
                    break;

                case GenuineEventType.GeneralConnectionClosed:
                    if (e.HostInformation != null)
                    {
                        string clientName = null;
                        string remoteEp = e.HostInformation.PhysicalAddress?.ToString();
                        if (!String.IsNullOrEmpty(remoteEp))
                            clients.TryRemove(remoteEp, out clientName);

                        if (!String.IsNullOrEmpty(clientName))
                            Log(Logging.Level.Info, String.Format(Properties.Resources.ClientDisconnected, clientName));
                        else
                            Log(Logging.Level.Debug, "{0} is disconnected", e.HostInformation.PhysicalAddress);

                        //try
                        //{
                        //    Object obj = e.HostInformation["Guid"];
                        //    if (obj != null)
                        //    {
                        //        Guid clientGuid = (Guid)obj;
                        //        //Disconnect(clientGuid);
                        //    }
                        //    else
                        //    {
                        //        Log(Logging.Level.Debug, "GeneralConnectionClosed received for GUID NULL");
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    Log(Logging.Level.Debug, "Error in GeneralConnectionClosed: {0}", ex.ToString());
                        //}
                    }
                    break;

                 case GenuineEventType.HostResourcesReleased:
                    break;
            }
        }
    

        public static void StopRemoting(CommunicationObject commObj)
        {
            try
            {
                System.Runtime.Remoting.RemotingServices.Disconnect(commObj);
            }
            catch(Exception ex)
            {
                Log(Logging.Level.Debug, "Error stopping remoting: {0}", ex);
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
