/*
Based on ServicePublisher and ServiceLocator classes of
    Shawn A. Van Ness (http://www.arithex.com/)

This pair of C# classes wraps UdpClient's multicast functionality, for use 
as a simple client-server rendezvous mechanism.

*/

using System;
using System.Net;
using System.Net.Sockets;

using System.Diagnostics;

namespace iba.Utility
{
    //
    // Public types

    /// <summary>
    /// Advertises a service on the network via UDP multicast groups.
    /// </summary>
    public class ServicePublisher
    {
        //
        // Construction

        /// <summary>
        /// Initializes a new instance of the ServicePublisher class, for the specified service id.
        /// </summary>
        /// <param name="serviceId">Unique identifer for the client/server protocol being advertised.</param>
        public ServicePublisher(Guid serviceId, IPAddress groupAddress, int groupPortNr)
        {
            this.serviceId = serviceId;
            this.listener = null;
            this.response = null;
            this.GroupAddress = groupAddress;
            this.GroupPortNr = groupPortNr;
            this.properties = null;
        }

        public int DefaultGroupTTL = 5;
        public IPAddress GroupAddress;
        public int GroupPortNr;

        public event Func<System.Collections.IDictionary, bool> ProvideProperties;

        //
        // Public API

        /// <summary>
        /// Begin advertising the presence of the service.
        /// </summary>
        /// <param name="endpointProps">Property-bag of protocol-specific connection parameters.</param>
        public void PublishServiceEndpoint(System.Collections.IDictionary endpointProps)
        {
            PublishServiceEndpoint(endpointProps, DefaultGroupTTL);
        }

        /// <summary>
        /// Begin advertising the presence of the service.
        /// </summary>
        /// <param name="endpointProps">Property-bag of protocol-specific connection parameters.</param>
        /// <param name="ttl">The number of router-hops to advertise across.</param>
        public void PublishServiceEndpoint(System.Collections.IDictionary endpointProps, int ttl)
        {
            this.properties = endpointProps;
            PublishServiceEndpoint(CreateMessageFromProperties(properties), ttl);
        }

        public void PublishServiceEndpoint(Byte[] responseMsg, int ttl)
        {
            this.response = responseMsg;
            this.ttl = ttl;

            // Initalize a new UDP socket for the multicast group
            this.listener = new UdpClient(GroupPortNr);

            // Punt remainder of implementation to background thread (UdpClient.Receive blocks!)
            System.Threading.Thread listenerThread =
                new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadProc));
            listenerThread.IsBackground = true;
            listenerThread.Name = "Service publisher thread";
            listenerThread.Priority = System.Threading.ThreadPriority.Lowest;
            listenerThread.Start();
        }

        /// <summary>
        /// Stops advertising the service.
        /// </summary>
        public void StopPublishing()
        {
            if (this.listener != null)
            {
                UdpClient local = listener;
                listener = null;

                try
                {
                    // Cleanly withdraw our membership from the multicast group
                    local.DropMulticastGroup(GroupAddress);
                }
                catch (Exception)
                {
                }

                // Closing the underlying socket will cause UdpClient.Receive to throw
                local.Close();
            }
        }

        //
        // Implementation

        private void ThreadProc()
        {
            // Loop forever (until underlying socket is closed, anyway)
            try 
            {
				bool bJoined = false;			
                while (true)
                {
					if(!bJoined)
					{
						try
						{
                            //Try a join on every adapter
                            string localHost = Dns.GetHostName();
                            IPHostEntry hostEntry = Dns.GetHostEntry(localHost);
                            for(int i=0; i<hostEntry.AddressList.Length; i++)
                            {
                                if(IPAddress.IsLoopback(hostEntry.AddressList[i]))
                                    continue;

                                try
                                {
                                    // Join the multicast group (sends an IGMP group-membership report to routers)
                                    this.listener.Ttl = (short)ttl;
                                    this.listener.JoinMulticastGroup(GroupAddress, hostEntry.AddressList[i]);
                                    bJoined = true;
                                }
                                catch (Exception)
                                {
                                }
                            }
						}
						catch(Exception)
						{
							
						}
                    }

                    if (bJoined || (listener.Available > 0))
                    {
                        try
                        {
                            // Wait for broadcast... will block until data recv'd, or underlying socket is closed
                            IPEndPoint callerEndpoint = null;
                            if (listener == null)
                                break;

                            byte[] request = this.listener.Receive(ref callerEndpoint);

                            // Verify first 128 bits are indeed our guid
                            if ((request != null) && (request.Length >= 16))
                            {
                                byte[] temp = new byte[16];
                                request.CopyTo(temp, 0);
                                Guid requestGuid = new Guid(temp);

                                if (requestGuid == this.serviceId)
                                {
                                    // Let publisher define any dynamic properties
                                    if (OnProvideProperties(properties))
                                        this.response = CreateMessageFromProperties(properties);

                                    // Send response (our guid, followed by serialized endpoint info)
                                    this.listener.Send(this.response, this.response.Length, callerEndpoint);
                                }
                            }
                        }
                        catch (System.Net.Sockets.SocketException)
                        { } // expected (client got too impatient?)
                    }
                    else
                    {
                        //Even when not joined we can still receive a message via the loopback interface
                        //So let's wait for 5s and then we can try the join again.
                        listener.Client.Poll(5000000, SelectMode.SelectRead);
                    }
                }
            }
            catch (System.ObjectDisposedException)
            { } // expected
            catch (System.NullReferenceException)
            { } // also expected?
            catch (Exception)
            { }
        }

        private readonly Guid serviceId;
        private UdpClient listener;
        private byte[] response;
		private int ttl;
        private System.Collections.IDictionary properties;

        protected bool OnProvideProperties(System.Collections.IDictionary properties)
        {
            if((ProvideProperties != null) && (properties != null))
                return ProvideProperties(properties);
            else
                return false;
        }

        private byte[] CreateMessageFromProperties(System.Collections.IDictionary properties)
        {
            // Prepare canned response: serialize serviceId and endpointProps into byte[]
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(this.serviceId.ToByteArray(), 0, 16);
            bf.Serialize(ms, properties);
            return ms.ToArray();
        }
    }

    public interface IServiceDiscovered
    {
        void ServiceDiscovered(ServiceLocator.HostResponse host);
    }

    /// <summary>
    /// Searches hosts on the network for a specific service, via UDP multicast groups.
    /// </summary>
    public sealed class ServiceLocator
    {
        //
        // Construction

        private ServiceLocator(IPAddress groupAddress) 
        { } // this class is noncreatable (static members only)

        //
        // Public API
        public static int DefaultClientTimeout
        {
            get
            {
                return 3000;
            }
        }

        static Tuple<int,int> localPortRange = new Tuple<int,int>(12900, 12910);
        public static Tuple<int,int> LocalPortRange
        {
            get { return localPortRange; }
            set 
            {
                if (value.Item1 <= value.Item2)
                    localPortRange = new Tuple<int, int>(value.Item1, value.Item2);
                else
                    localPortRange = new Tuple<int, int>(value.Item2, value.Item1);
            }
        }

        /// <summary>
        /// Locates hosts on the network which expose the requested service.
        /// </summary>
        /// <param name="serviceId">Unique identifier for the requested service.</param>
        /// <returns>An array of HostResponse structures.</returns>
        public static HostResponse[] LocateService(Guid serviceId, IPAddress groupAddress, int portNr)
        {
            return LocateService(serviceId, groupAddress, portNr, DefaultClientTimeout, false, false);
        }

        /// <summary>
        /// Locates hosts on the network which expose the requested service.
        /// </summary>
        /// <param name="serviceId">Unique identifier for the requested service.</param>
        /// <param name="timeout">Time to wait for responses from remote hosts.</param>
        /// <returns>An array of HostResponse structures.</returns>
        public static HostResponse[] LocateService(Guid serviceId, IPAddress groupAddress, int portNr, System.TimeSpan timeout) 
        {
            return LocateService(serviceId, groupAddress, portNr, timeout.Milliseconds, false, false);
        }

        /// <summary>
        /// Locates hosts on the network which expose the requested service.
        /// </summary>
        /// <param name="serviceId">Unique identifier for the requested service.</param>
        /// <param name="millisecondTimeout">Time (in milliseconds) to wait for responses from remote hosts.</param>
        /// <returns>An array of HostResponse structures.</returns>
        public static HostResponse[] LocateService(Guid serviceId, IPAddress groupAddress, int portNr, int millisecondTimeout)
        {
            return LocateService(serviceId, groupAddress, portNr, millisecondTimeout, false, false);
        }

        public static HostResponse[] LocateService(Guid serviceId, IPAddress groupAddress, int portNr, 
            int millisecondTimeout, bool bRawMessage, bool bAllowSameIpAddress)
        {
            // Impose reasonable range on timeout
            if (millisecondTimeout < 100)
                millisecondTimeout = 100;
            else if (millisecondTimeout > 7000)
                millisecondTimeout = 7000;

            // Dynamically allocate client port
            MulticastUdpClient sender = CreateUdpClient();

            // Construct simple datagram w/ serviceId
            byte[] request = serviceId.ToByteArray();
            IPEndPoint groupEP = new IPEndPoint(groupAddress, portNr);

			// Send the query on all interfaces
			string localHost = Dns.GetHostName();
			IPHostEntry hostEntry = Dns.GetHostEntry(localHost);
			foreach(IPAddress addr in hostEntry.AddressList)
			{
                try
                {
                    if (IPAddress.IsLoopback(addr))
                    {
                        //Also send to local address to find local server even when no network cable is connected
                        sender.Send(request, request.Length, new IPEndPoint(addr, portNr));
                    }
                    else
                    {
                        sender.SetMulticastInterface(addr);
                        sender.Send(request, request.Length, groupEP);
                    }
                }
                catch (Exception)
                {
                }
			}

            // Accumulate responses on a threadpool thread
            ResponseAccumProc rap = new ResponseAccumProc(ResponseAccumProcImpl);
            IAsyncResult ar = rap.BeginInvoke(sender,serviceId,null,bRawMessage, bAllowSameIpAddress, null,null);

            // Wait the requisite amount of time, then shut the door
            System.Threading.Thread.Sleep(millisecondTimeout);
            sender.Close(); // will kick the bkgrnd thread out of the blocked recv method

            // Return the results
            HostResponse[] hrs = rap.EndInvoke(ar); // waits for async delegate to complete
            return hrs;
        }

        private static MulticastUdpClient CreateUdpClient()
        {
            if (localPortRange != null)
            {
                int begin = localPortRange.Item1;
                int end = localPortRange.Item2;
                for (int port = begin; port <= end; port++)
                {
                    try
                    {
                        return new MulticastUdpClient(port);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return new MulticastUdpClient();
        }

        internal static MulticastUdpClient asyncClient = null;
        public static IAsyncResult StartLocateService(Guid serviceId, IPAddress groupAddress, int portNr, IServiceDiscovered serviceDisc, bool bRawMessage, bool bAllowSameIpAddress)
        {
            if(asyncClient != null)
                throw new InvalidOperationException("Previous search is still busy, stop this first by calling StopLocateService");

            // Dynamically allocate client port
            asyncClient = CreateUdpClient();

            // Construct simple datagram w/ serviceId
            byte[] request = serviceId.ToByteArray();
            IPEndPoint groupEP = new IPEndPoint(groupAddress, portNr);

            // Send the query on every network interface
			try
			{
				string localHost = Dns.GetHostName();
				IPHostEntry hostEntry = Dns.GetHostEntry(localHost);
				foreach(IPAddress addr in hostEntry.AddressList)
				{
                    try
                    {
                        if (IPAddress.IsLoopback(addr))
                        {
                            //Also send to local address to find local server even when no network cable is connected
                            asyncClient.Send(request, request.Length, new IPEndPoint(addr, portNr));
                        }
                        else
                        {
                            asyncClient.SetMulticastInterface(addr);
                            asyncClient.Send(request, request.Length, groupEP);
                        }
                    }
                    catch (Exception)
                    {
                    }
				}
			}
			catch(Exception ex)
			{
				asyncClient = null;
				throw ex;
			}

            // Accumulate responses on a threadpool thread
            ResponseAccumProc rap = new ResponseAccumProc(ResponseAccumProcImpl);
            IAsyncResult ar = rap.BeginInvoke(asyncClient, serviceId, serviceDisc, bRawMessage, bAllowSameIpAddress, null, null);

            return ar;
        }

        public static HostResponse[] StopLocateService(IAsyncResult ar)
        {
            if(asyncClient != null)
                asyncClient.Close(); // will kick the bkgrnd thread out of the blocked recv method

            // Return the results
            System.Runtime.Remoting.Messaging.AsyncResult asyncRes = (System.Runtime.Remoting.Messaging.AsyncResult) ar;
            ResponseAccumProc rap = (ResponseAccumProc) asyncRes.AsyncDelegate;
            HostResponse[] hrs = rap.EndInvoke(ar); // waits for async delegate to complete
            asyncClient = null;
            return hrs;
        }

        

        /// <summary>
        /// This nested-type encapsulates the IPAddress and other response info, from a remote service endpoint.
        /// </summary>
        [Serializable]
        public class HostResponse
        {
            private readonly IPAddress address;
            private readonly System.Collections.IDictionary endpointProps;
			
            internal HostResponse(IPAddress address, System.Collections.IDictionary endpointProps)
            {
                this.address = address;
                this.endpointProps = endpointProps;
            }

            /// <summary>
            /// Gets the IPAddress of the responding host.
            /// </summary>
            public IPAddress IPAddress
            {
                get
                { return this.address; }
            }

            /// <summary>
            /// Gets a collection of protocol-specific endpoint info from the responding host.
            /// </summary>
            public System.Collections.IDictionary EndpointProperties
            {
                get
                { return this.endpointProps; }
            }

            // Canonical value-type comparison goo (uniqueness based on address)
            /// <summary>
            /// Overridden. Returns a value indicating whether this instance is equal to a specified object.
            /// </summary>
            /// <param name="obj">An object to compare with this instance.</param>
            /// <returns><b>true</b> if <i>obj</i> is an instance of <see cref="HostResponse"/> and equals the value of this instance; otherwise, <b>false</b>.</returns>
            public override bool Equals(object obj)
            {
                return ((obj is HostResponse) && 
                    this.address.Equals(((HostResponse)obj).address));
            }
            /// <summary>
            /// Overridden. Returns the hash code for this instance.
            /// </summary>
            /// <returns>A 32-bit signed integer hash code.</returns>
            public override int GetHashCode() 
            {
                return this.address.GetHashCode();
            }
            /// <summary>
            /// Compares two HostResponse structures, based on the originating IPAddress.
            /// </summary>
            /// <param name="a">A HostResponse structure.</param>
            /// <param name="b">A HostResponse structure.</param>
            /// <returns><b>true</b> if the two responses are from the same IP address, <b>false</b> otherwise.</returns>
            public static bool operator==( HostResponse a, HostResponse b) 
            {
				if(Object.ReferenceEquals(a, null) || Object.ReferenceEquals(b, null))
					return Object.ReferenceEquals(a, null) == Object.ReferenceEquals(b, null);

                return a.address.Equals(b.address);
            }
            /// <summary>
            /// Compares two HostResponse structures, based on the originating IPAddress.
            /// </summary>
            /// <param name="a">A HostResponse structure.</param>
            /// <param name="b">A HostResponse structure.</param>
            /// <returns><b>false</b> if the two responses are from the same IP address, <b>true</b> otherwise.</returns>
            public static bool operator !=( HostResponse a, HostResponse b) 
            {
                return !(a==b);
            }
        }

        //
        // Implementation

        internal delegate HostResponse[] ResponseAccumProc(UdpClient udpClient, Guid serviceId, IServiceDiscovered callback, bool bRawMessage, bool bAllowSameIpAddress);

        internal static HostResponse[] ResponseAccumProcImpl(UdpClient udpClient, Guid serviceId, IServiceDiscovered callback, bool bRawMessage, bool bAllowSameIpAddress)
        {
            // Accumulate responses
            System.Collections.ArrayList responses = new System.Collections.ArrayList();
            
            // Loop forever (until underlying socket is closed, anyway)
            try
            {
                System.Net.IPAddress[] localAddresses = System.Net.Dns.GetHostAddresses("");
 
                while (true)
                {
                    // Grab a response datagram
                    IPEndPoint remoteEndpoint = null;
                    byte[] response = udpClient.Receive(ref remoteEndpoint); //blocks until socket closed

                    if ((response == null) || (response.Length == 0))
                        continue;

                    //Check if this is a local address
                    IPAddress remoteAddress = remoteEndpoint.Address;
                    for(int i = 0; i < localAddresses.Length; i++)
                    {
                        if(remoteAddress.Equals(localAddresses[i]))
                        {
                            //Always use 127.0.0.1 for local
                            remoteAddress = IPAddress.Parse("127.0.0.1");
                            break;
                        }
                    }
                    
                    if (bRawMessage)
                    {
                        System.Collections.Hashtable dictionary = new System.Collections.Hashtable();
                        dictionary.Add("RawMessage", response);
                        HostResponse host = new HostResponse(remoteAddress, dictionary);
                        if(AddUnique(host, responses, bAllowSameIpAddress))
                        {
                            if(callback != null)
                                callback.ServiceDiscovered(host);
                        }
                    }
                    else
                    {
                        // Format is a 16-byte guid, followed by serialized propertybag
                        if (response.Length >= 16)
                        {
                            // Unmarshal the response
                            System.IO.MemoryStream responseStream = new System.IO.MemoryStream(response, false);

                            byte[] temp = new byte[16];
                            responseStream.Read(temp, 0, 16);
                            Guid guid = new Guid(temp);

                            if (guid == serviceId)
                            {                               
                                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf =
                                    new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                                object endpointProps = bf.Deserialize(responseStream);
                                HostResponse host = new HostResponse(remoteAddress, (System.Collections.IDictionary)endpointProps);
                                if(AddUnique(host, responses, bAllowSameIpAddress))
                                {
                                    if(callback != null)
                                        callback.ServiceDiscovered(host);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.ObjectDisposedException)
            {} // expected
            catch (System.Net.Sockets.SocketException)
            {} // expected
            catch (Exception)
            {}

            ServiceLocator.asyncClient = null;
            return responses.ToArray(typeof(HostResponse)) as HostResponse[];
        }

        private static bool AddUnique(HostResponse host, System.Collections.ArrayList list, bool bAllowSameIpAddress)
        {
            foreach(HostResponse entry in list)
            {
                if(entry.IPAddress.Equals(host.IPAddress))
                {
                    if(!bAllowSameIpAddress)
                        return false;
                    else if(ArePropertiesEqual(host.EndpointProperties, entry.EndpointProperties))
                        return false;
                }
            }

            list.Add(host);
            return true;
        }

        private static bool ArePropertiesEqual(System.Collections.IDictionary dict1, System.Collections.IDictionary dict2)
        {
            if(dict1.Count != dict2.Count)
                return false;

            foreach(System.Collections.DictionaryEntry entry in dict1)
            {
                if((entry.Value == null) && (dict2[entry.Key] != null))
                    return false;

                if(!entry.Value.Equals(dict2[entry.Key]))
                    return false;
            }
            return true;
        }
    }

    //
    // Helper classes

    public sealed class PdaMulticastGroupSettings
    {
        public static readonly Guid ServerGuid = new Guid("6730B7EF-1760-4652-9460-B09B45024B53");
        public static readonly IPAddress GroupAddress = IPAddress.Parse("226.254.92.220");
        public static readonly int ServerPort = 12800;
    }

	internal class MulticastUdpClient : UdpClient
	{
		public MulticastUdpClient() : base()
		{
		}

        public MulticastUdpClient(int port) : base(port)
        {
        }

		public void SetMulticastInterface(IPAddress addr)
		{
			byte[] addrBytes = addr.GetAddressBytes();
			Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, addrBytes);
		}
	}
}
