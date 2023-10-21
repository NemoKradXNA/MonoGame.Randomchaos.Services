
using MonoGame.Randomchaos.Services.P2P.Delegates;
using MonoGame.Randomchaos.Services.P2P.Enums;
using MonoGame.Randomchaos.Services.P2P.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A peer 2 peer client. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class P2PClient : IP2PClient
    {
        /// <summary>   (Immutable) the server endpoint. </summary>
        protected readonly IPEndPoint _serverEndpoint;
        /// <summary>   The client endpoint. </summary>
        protected IPEndPoint _clientEndpoint;

        /// <summary>   The TCP client. </summary>
        protected TcpClient _tcpClient;
        /// <summary>   The UDP client. </summary>
        protected UdpClient _udpClient;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clients. </summary>
        ///
        /// <value> The clients. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<IClientPacketData> Clients { get; protected set; } = new List<IClientPacketData>();

        /// <summary>   Event queue for all listeners interested in OnTcpDataReceived events. </summary>
        public event DataReceivedEvent OnTcpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnUdpDataReceived events. </summary>
        public event DataReceivedEvent OnUdpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnConnectionDropped events. </summary>
        public event ConnectionDroppedEvent OnConnectionDropped;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Event queue for all listeners interested in OnClientConnectionDropped events.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------

        public event ConnectionDroppedEvent OnClientConnectionDropped;
        /// <summary>   Event queue for all listeners interested in OnNewClientAdded events. </summary>
        public event ConnectionAttemptEvent OnNewClientAdded;
        /// <summary>   Event queue for all listeners interested in OnLog events. </summary>
        public event LogEvent OnLog;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the player. </summary>
        ///
        /// <value> Information describing the player. </value>
        ///-------------------------------------------------------------------------------------------------

        public IPlayerData PlayerData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the IPv4 address. </summary>
        ///
        /// <value> The IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string IPv4Address { get { return $"{_clientEndpoint.Address}:{_clientEndpoint.Port}"; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the server IPv4 address. </summary>
        ///
        /// <value> The server IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ServerIPv4Address { get { return $"{_serverEndpoint.Address}:{_serverEndpoint.Port}"; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ///-------------------------------------------------------------------------------------------------

        public Guid? Id { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the port. </summary>
        ///
        /// <value> The port. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Port { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is connected. </summary>
        ///
        /// <value> True if this object is connected, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsConnected { get { return _tcpClient.Connected; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Date/Time of the server last active. </summary>
        ///
        /// <value> The server last active. </value>
        ///-------------------------------------------------------------------------------------------------

        public DateTime ServerLastActive { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the server inactive timeout. </summary>
        ///
        /// <value> The server inactive timeout. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan ServerInactiveTimeout { get; set; } = new TimeSpan(0, 0, 10);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="serverIPv4Address">    The server IPv4 address. </param>
        /// <param name="serverPort">           The server port. </param>
        /// <param name="clientIPv4Address">    The client IPv4 address. </param>
        /// <param name="clientPort">           The client port. </param>
        ///-------------------------------------------------------------------------------------------------

        public P2PClient(string serverIPv4Address, int serverPort, string clientIPv4Address, int clientPort)
        {
            _serverEndpoint = new IPEndPoint(IPAddress.Parse(serverIPv4Address), serverPort);
            _clientEndpoint = new IPEndPoint(IPAddress.Parse(clientIPv4Address), clientPort);

            _tcpClient = new TcpClient();

            Port = clientPort;

            _udpClient = new UdpClient(Port);
            _udpClient.AllowNatTraversal(true);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Disconnects the given informServer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="informServer"> (Optional) The inform server to disconnect. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Disconnect(bool informServer = true)
        {
            if (_tcpClient.Connected)
            {
                if (informServer)
                {
                    SendDataTo(new CommsPacket()
                    {
                        Comms = CommsEnum.Closing,
                        IPAddress = IPv4Address,
                        Protocol = ProtocolTypesEnum.Tcp
                    });
                }

                _udpClient.Close();

                if (informServer)
                {
                    SendDataTo(new CommsPacket()
                    {
                        Comms = CommsEnum.Closed,
                        IPAddress = IPv4Address,
                        Protocol = ProtocolTypesEnum.Tcp
                    });
                }

                _tcpClient.Close();

                if (OnConnectionDropped != null)
                {
                    OnConnectionDropped(new ClientPacketData() { Id = Id.Value, UdpAddress = _clientEndpoint.Address.ToString(), UdPPort = _clientEndpoint.Port });
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Connects this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void Connect()
        {
            if (OnLog != null)
            {
                OnLog(LogLevelEnum.Information, "Connecting to Server...");
            }

            _tcpClient.Client.BeginConnect(_serverEndpoint, ConnectToServerAsync, null);

            int w = 0;
            while (!_tcpClient.Connected && w < 5)
            {
                if (OnLog != null)
                {
                    OnLog(LogLevelEnum.Information, ".");
                }
                Task.Delay(500).Wait();
                w++;
            }

            if (!_tcpClient.Connected)
            {
                if (OnLog != null)
                {
                    OnLog(LogLevelEnum.Information, "Did not connect to the server in time...");
                }
            }

            _udpClient.BeginReceive(UdpReceiveData, new { listener = _udpClient, endPoint = _serverEndpoint });

            ServerLastActive = DateTime.UtcNow;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="data">     The data. </param>
        /// <param name="endpoint"> (Optional) The endpoint. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SendDataTo(ICommsPacket data, IPEndPoint endpoint = null)
        {
            byte[] pktData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

            switch (data.Protocol)
            {
                case ProtocolTypesEnum.Tcp:
                    try
                    {
                        if (_tcpClient != null && _tcpClient.Connected)
                        {

                            NetworkStream NetStream = _tcpClient.GetStream();
                            NetStream.Write(pktData, 0, pktData.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (OnLog != null)
                        {
                            OnLog(LogLevelEnum.Error, "Server is down...");
                        }
                    }
                    break;
                case ProtocolTypesEnum.Udp:
                    try
                    {
                        _udpClient.Send(pktData, pktData.Length, endpoint != null ? endpoint : _serverEndpoint);
                    }
                    catch { } // Bad send.
                    break;
                default:

                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Broadcasts to. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="data">     The data. </param>
        /// <param name="clients">  The clients. </param>
        ///-------------------------------------------------------------------------------------------------

        public void BroadcastTo(ICommsPacket data, List<IClientPacketData> clients)
        {
            try
            {
                foreach (IClientPacketData client in clients)
                {
                    SendDataTo(data, client.GetIPEndPoint());
                }
            }
            catch { }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Async callback, called on completion of connect to server Asynchronous.
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="result">   The result of the asynchronous operation. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void ConnectToServerAsync(IAsyncResult result)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000);
                    while (_tcpClient.Connected)
                    {
                        await PollTcpAsync();
                        await Task.Delay(500);
                    }
                }
                catch (Exception ex)
                {
                    if (OnLog != null)
                    {
                        OnLog(LogLevelEnum.Error, "Poll Error", ex);
                    }
                }
            });

            if (OnLog != null)
            {
                OnLog(LogLevelEnum.Information, "Connected to Server...");
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Async callback, called on completion of UDP receive data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="result">   The result of the asynchronous operation. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void UdpReceiveData(IAsyncResult result)
        {
            dynamic state = (dynamic)result.AsyncState;

            UdpClient listener = new UdpClient();
            IPEndPoint endpoint = _serverEndpoint;

            try
            {

                listener = state.listener;
                endpoint = state.endPoint;

                byte[] receiveBytes = listener.EndReceive(result, ref endpoint);

                if (OnUdpDataReceived != null)
                {
                    string receiveString = Encoding.ASCII.GetString(receiveBytes);

                    ICommsPacket pkt = JsonConvert.DeserializeObject<CommsPacket>(receiveString);

                    OnUdpDataReceived(pkt);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (_tcpClient != null && _tcpClient.Connected)
                {
                    try
                    {
                        _udpClient.BeginReceive(UdpReceiveData, new { listener = listener, endPoint = endpoint });
                    }
                    catch { }
                }
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process the packet described by pkt. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="pkt">  The packet. </param>
        ///-------------------------------------------------------------------------------------------------

        protected async void ProcessPacket(ICommsPacket pkt)
        {
            if (pkt.Protocol == ProtocolTypesEnum.Tcp)
            {
                if (OnTcpDataReceived != null)
                {
                    OnTcpDataReceived(pkt);
                }
            }
            else
            {
                if (OnUdpDataReceived != null)
                {
                    OnUdpDataReceived(pkt);
                }
            }

            switch (pkt.Comms)
            {
                case CommsEnum.Pulse:
                    ServerLastActive = DateTime.UtcNow;
                    break;
                case CommsEnum.Accepted:
                    break;
                case CommsEnum.RequestUdpData:

                    ServerLastActive = DateTime.UtcNow;
                    ICommsPacket epPkt = new CommsPacket()
                    {
                        //Id = Id.Value, // We dont have an ide yet..
                        Comms = CommsEnum.RequestUdpDataResponse,
                        IPAddress = IPv4Address,
                        Protocol = pkt.Protocol,
                        Sent = DateTime.UtcNow,
                        Data = PlayerData
                    };
                    SendDataTo(epPkt);

                    //AddClient(new ClientPacketData()
                    //{
                    //    Id = Guid.Empty,
                    //    UdpAddress = _serverEndpoint.Address.ToString(),
                    //    UdPPort = _serverEndpoint.Port
                    //});

                    // At runtime, this is firing faster than the screen load...
                    await Task.Delay(1000);

                    break;
                case CommsEnum.NewClientAdded:
                    ServerLastActive = DateTime.UtcNow;

                    IClientPacketData clientPkt = JsonConvert.DeserializeObject<ClientPacketData>(pkt.Data.ToString());

                    AddClient(clientPkt);

                    // request client list.
                    pkt = new CommsPacket()
                    {
                        Id = Id.Value,
                        Comms = CommsEnum.RequestClientList,
                        IPAddress = IPv4Address,
                        Protocol = pkt.Protocol,
                        Sent = DateTime.UtcNow
                    };

                    SendDataTo(pkt);

                    break;
                case CommsEnum.RequestClientListResponse:

                    ServerLastActive = DateTime.UtcNow;
                    List<ClientPacketData> dataList = JsonConvert.DeserializeObject<List<ClientPacketData>>(pkt.Data.ToString());

                    foreach (ClientPacketData data in dataList)
                    {
                        AddClient(data);
                    }

                    break;
                case CommsEnum.ClientDisconnected: // You or someone else got booted
                    IClientPacketData clntPktData = null;

                    if (pkt.Data != null)
                    {
                        clntPktData = JsonConvert.DeserializeObject<ClientPacketData>(pkt.Data.ToString());

                        var existing = GetClientByIdAndAddress(clntPktData);

                        if (existing != null)
                        {
                            Clients.Remove(existing);
                        }
                        if (clntPktData.ToString() == IPv4Address) // Its me, I have been booted...
                        {
                            if (OnLog != null)
                            {
                                OnLog(LogLevelEnum.Warning, "The  server disconnected me...");
                            }
                            Disconnect(false);

                            if (OnClientConnectionDropped != null)
                            {
                                var svr = Clients.SingleOrDefault(s => s.Id == Guid.Empty);
                                if (svr != null)
                                {
                                    OnClientConnectionDropped(svr);
                                }
                            }
                        }
                        else
                        {
                            if (OnClientConnectionDropped != null)
                            {
                                OnClientConnectionDropped(clntPktData);
                            }
                        }
                    }

                    break;
                case CommsEnum.Closed:
                    // Either the server or another client has closed.
                    IClientPacketData pktData = null;

                    if (pkt.Data != null)
                    {
                        pktData = JsonConvert.DeserializeObject<ClientPacketData>(pkt.Data.ToString());

                        if (Clients.Contains(pktData))
                        {
                            Clients.Remove(pktData);
                        }
                    }
                    else // either unknown client, or the server..
                    {
                        Disconnect(false);

                        pktData = Clients.SingleOrDefault(s => s.Id == Guid.Empty);

                        if (pktData == null)
                        {
                            pktData = new ClientPacketData()
                            {
                                Id = Guid.Empty,
                                UdpAddress = _serverEndpoint.Address.ToString(),
                                UdPPort = _serverEndpoint.Port,
                            };
                        }
                        else
                        {
                            Clients.Remove(pktData);
                        }
                    }

                    if (OnClientConnectionDropped != null)
                    {
                        OnClientConnectionDropped(pktData);
                    }

                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a client. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="clientPacket"> Message describing the client. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void AddClient(IClientPacketData clientPacket)
        {
            if (clientPacket.ToString() != IPv4Address)
            {
                var exists = GetClientByIdAndAddress(clientPacket);
                if (exists == null)
                {
                    Clients.Add(clientPacket);
                }
            }
            else if (Id == null)
            {
                Id = clientPacket.Id;
                // Dont want to add me 
            }

            if (OnNewClientAdded != null)
            {
                OnNewClientAdded(clientPacket);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Poll TCP asynchronous. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <returns>   A Task. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected async Task PollTcpAsync()
        {
            try
            {
                if (DateTime.UtcNow - ServerLastActive > ServerInactiveTimeout)
                {
                    ServerLastActive = DateTime.UtcNow;
                    try
                    {
                        SendDataTo(new CommsPacket() { Id = Id != null ? Id.Value : Guid.Empty, Comms = CommsEnum.Pulse, IPAddress = IPv4Address, Protocol = ProtocolTypesEnum.Tcp });
                    }
                    catch (Exception ex)
                    {
                        // Pulse the server.
                        if (OnLog != null)
                        {
                            OnLog(LogLevelEnum.Error, "Server down!", ex);
                        }
                        Disconnect();
                    }
                }

                if (_tcpClient.Available > 0)
                {
                    byte[] data = new byte[_tcpClient.Available];
                    var stream = _tcpClient.GetStream();

                    await stream.ReadAsync(data);

                    string pktString = Encoding.UTF8.GetString(data);

                    // Could be more than one packet in here.
                    if (pktString.Contains("}{"))
                    {
                        pktString = $"[{pktString.Replace("}{", "},{")}]";
                        List<CommsPacket> packets = JsonConvert.DeserializeObject<List<CommsPacket>>(pktString);

                        foreach (CommsPacket pkt in packets)
                        {
                            ProcessPacket(pkt);
                        }
                    }
                    else
                    {
                        ProcessPacket(JsonConvert.DeserializeObject<CommsPacket>(pktString));
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnLog != null)
                {
                    OnLog(LogLevelEnum.Error, "TCP read error", ex);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets client by identifier and address. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="pktData">  Information describing the packet. </param>
        ///
        /// <returns>   The client by identifier and address. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected virtual IClientPacketData GetClientByIdAndAddress(IClientPacketData pktData)
        {
            return Clients.SingleOrDefault(s => s.Id == pktData.Id || (s.UdpAddress == pktData.UdpAddress && s.UdPPort == pktData.UdPPort));
        }
    }
}
