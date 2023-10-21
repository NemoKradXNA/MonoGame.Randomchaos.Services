
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
    /// <summary>   A Peer 2 Peer  server. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class P2PServer : IP2PServer
    {
        /// <summary>   Event queue for all listeners interested in OnServerStart events. </summary>
        public event ServerStartEvent OnServerStart;
        /// <summary>   Event queue for all listeners interested in OnServerStop events. </summary>
        public event ServerStopEvent OnServerStop;
        /// <summary>   Event queue for all listeners interested in OnConnectionAttempt events. </summary>
        public event ConnectionAttemptEvent OnConnectionAttempt;
        /// <summary>   Event queue for all listeners interested in OnNewClientAdded events. </summary>
        public event ConnectionAttemptEvent OnNewClientAdded;
        /// <summary>   Event queue for all listeners interested in OnConnectionDropped events. </summary>
        public event ConnectionDroppedEvent OnConnectionDropped;
        /// <summary>   Event queue for all listeners interested in OnTcpDataReceived events. </summary>
        public event DataReceivedEvent OnTcpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnUdpDataReceived events. </summary>
        public event DataReceivedEvent OnUdpDataReceived;
        /// <summary>   Event queue for all listeners interested in OnError events. </summary>
        public event ErrorEvent OnError;
        /// <summary>   Event queue for all listeners interested in OnClientCommsError events. </summary>
        public event ClientCommsError OnClientCommsError;
        /// <summary>   Event queue for all listeners interested in OnLog events. </summary>
        public event LogEvent OnLog;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the player. </summary>
        ///
        /// <value> Information describing the player. </value>
        ///-------------------------------------------------------------------------------------------------

        public IPlayerData PlayerData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the accepting connections. </summary>
        ///
        /// <value> True if accepting connections, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool AcceptingConnections { get; set; } = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the local IPv4 address. </summary>
        ///
        /// <value> The local IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string LocalIPv4Address { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the external IP 4v address. </summary>
        ///
        /// <value> The external IP 4v address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ExternalIP4vAddress { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name of the machine. </summary>
        ///
        /// <value> The name of the machine. </value>
        ///-------------------------------------------------------------------------------------------------

        public string MachineName { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the port. </summary>
        ///
        /// <value> The port. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Port { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is running. </summary>
        ///
        /// <value> True if this object is running, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsRunning { get; protected set; } = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clients. </summary>
        ///
        /// <value> The clients. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<IClientData> Clients { get; protected set; } = new List<IClientData>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clear inactive connection timeout. </summary>
        ///
        /// <value> The clear inactive connection timeout. </value>
        ///-------------------------------------------------------------------------------------------------

        public TimeSpan ClearInactiveConnectionTimeout { get; set; } = new TimeSpan(0, 0, 10);
        /// <summary>   (Immutable) the UDP client. </summary>
        protected readonly UdpClient _udpClient;
        /// <summary>   (Immutable) the TCP listener. </summary>
        protected readonly TcpListener _tcpListener;

        /// <summary>   (Immutable) the UDP end point. </summary>
        protected readonly IPEndPoint _udpEndPoint;
        /// <summary>   (Immutable) the TCP end point. </summary>
        protected readonly IPEndPoint _tcpEndPoint;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="port">                 The port. </param>
        /// <param name="externalIPv4Address">  (Optional) The external IPv4 address. </param>
        ///-------------------------------------------------------------------------------------------------

        public P2PServer(int port, string externalIPv4Address = null)
        {
            Port = port;
            ExternalIP4vAddress = externalIPv4Address;

            _udpClient = new UdpClient(Port);
            _udpClient.AllowNatTraversal(true);

            _tcpEndPoint = new IPEndPoint(IPAddress.Any, Port);
            _tcpListener = new TcpListener(_tcpEndPoint);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Start()
        {
            if (OnLog != null)
            {
                OnLog(LogLevelEnum.Information, $"Starting P2P Server...");
                OnLog(LogLevelEnum.Information, $"Local: [{LocalIPv4Address}:{Port}]");
                OnLog(LogLevelEnum.Information, $"External: [{ExternalIP4vAddress}:{Port}]");
                OnLog(LogLevelEnum.Information, $"Machine [{MachineName}]");
            }

            IsRunning = true;
            AcceptingConnections = true;

            _tcpListener.Start();

            _tcpListener.BeginAcceptTcpClient(AcceptTcpClient, _tcpListener);

            _udpClient.BeginReceive(ReceiveUDPCallback, new
            {
                listener = _udpClient,
                endPoint = _udpEndPoint,
            });


            _ = Task.Run(async () =>
            {
                while (IsRunning)
                {
                    try
                    {
                        await PollTcpAsync();
                        await Task.Delay(100);
                    }
                    catch (Exception ex)
                    {
                        if (OnLog != null)
                        {
                            OnLog(LogLevelEnum.Error, "Error during TCP Polling", ex);
                        }
                    }
                }
            });

            if (OnServerStart != null)
            {
                OnServerStart();
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Async callback, called on completion of receive UDP callback. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="result">   The result of the asynchronous operation. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void ReceiveUDPCallback(IAsyncResult result)
        {
            dynamic state = (dynamic)result.AsyncState;

            UdpClient listener = new UdpClient();
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Port);

            try
            {

                listener = state.listener;
                endpoint = state.endPoint;

                byte[] receiveBytes = listener.EndReceive(result, ref endpoint);

                if (OnLog != null)
                {
                    OnLog(LogLevelEnum.Information, "UDP message received...");
                }

                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                ICommsPacket pkt = JsonConvert.DeserializeObject<CommsPacket>(receiveString);

                IClientData client = Clients.SingleOrDefault(w => $"{w.UdpEndPoint}" == $"{pkt.IPAddress}:{pkt.Port}");

                if (client != null)
                {
                    ProcessPacket(client, pkt);
                }
                else // Dead or booted client.
                {
                    if (OnLog != null)
                    {
                        OnLog(LogLevelEnum.Warning, "UDP data from invalid client ignored.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex, $"Failed to receive UDP data.");
            }
            finally
            {
                if (_udpClient.Client != null)
                {
                    _udpClient.BeginReceive(ReceiveUDPCallback, new { listener = listener, endPoint = endpoint });
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Async callback, called on completion of accept TCP client. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="result">   The result of the asynchronous operation. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void AcceptTcpClient(IAsyncResult result)
        {
            if (IsRunning)
            {
                try
                {
                    // Client Exists?
                    TcpClient tcpClient = _tcpListener.EndAcceptTcpClient(result);


                    if (!AcceptingConnections)
                    {
                        tcpClient.Close();
                        return;
                    }

                    IClientData client = Clients.SingleOrDefault(s => s.Client == tcpClient);

                    if (client == null)
                    {
                        client = new ClientData()
                        {
                            Client = tcpClient,
                            LastActivity = DateTime.UtcNow
                        };
                    }

                    Clients.Add(client);

                    client.Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    if (OnLog != null)
                    {
                        OnLog(LogLevelEnum.Information, $"Connection accepted from [{client.Client.Client.RemoteEndPoint}] Id [{client.PacketData.Id}]");
                    }

                    if (OnConnectionAttempt != null)
                    {
                        OnConnectionAttempt(client.PacketData);
                    }

                    ICommsPacket pkt = new CommsPacket(Guid.Empty, CommsEnum.Accepted, ExternalIP4vAddress, "Your request has been received.")
                    {
                        Protocol = ProtocolTypesEnum.Tcp,
                        IPAddress = $"{ExternalIP4vAddress}:{Port}"
                    };

                    SendDataTo(client, pkt);

                    pkt = new CommsPacket(Guid.Empty, CommsEnum.RequestUdpData, ExternalIP4vAddress) { Protocol = ProtocolTypesEnum.Tcp, IPAddress = $"{ExternalIP4vAddress}:{Port}" };
                    SendDataTo(client, pkt);
                }
                catch (Exception ex)
                {
                    if (OnError != null)
                        OnError(ex, $"Failed to accept client.");
                }
                finally
                {
                    _tcpListener.BeginAcceptTcpClient(AcceptTcpClient, _tcpListener);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="client">   The client. </param>
        /// <param name="data">     The data. </param>
        ///
        /// <returns>   An Exception. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual Exception SendDataTo(IClientData client, ICommsPacket data)
        {
            try
            {
                byte[] pktData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

                switch (data.Protocol)
                {
                    case ProtocolTypesEnum.Tcp:
                        if (client.Client != null && client.Client.Connected)
                        {

                            NetworkStream NetStream = client.Client.GetStream();
                            NetStream.Write(pktData, 0, pktData.Length);
                        }
                        break;
                    case ProtocolTypesEnum.Udp:
                        _udpClient.Send(pktData, client.UdpEndPoint);
                        break;
                    default:
                        if (OnLog != null)
                        {
                            OnLog(LogLevelEnum.Warning, $"The {data.Protocol} format is no supported..");
                        }
                        break;
                }

                return null;
            }
            catch (Exception ex)
            {
                if (OnClientCommsError != null)
                {
                    OnClientCommsError(client, data);
                }

                if (OnError != null)
                {
                    OnError(ex, $"Failed to send server stopped message to client [{client}].");
                }

                return ex;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="clients">  The clients. </param>
        /// <param name="data">     The data. </param>
        ///
        /// <returns>   An Exception. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<IClientData, Exception> SendDataTo(List<IClientData> clients, ICommsPacket data)
        {
            Dictionary<IClientData, Exception> deadClients = new Dictionary<IClientData, Exception>();

            foreach (IClientData client in clients)
            {
                Exception ex = SendDataTo(client, data);

                if (ex != null)
                {
                    deadClients.Add(client, ex);
                }
            }

            return deadClients;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to all. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;IClientData,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual Dictionary<IClientData, Exception> SendDataToAll(ICommsPacket data)
        {
            return SendDataTo(Clients, data);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Poll TCP asynchronous. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <returns>   A Task. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected virtual async Task PollTcpAsync()
        {
            DateTime now = DateTime.UtcNow;

            List<IClientData> deadClients = Clients.Where(w => (now - w.LastActivity) > ClearInactiveConnectionTimeout).ToList();

            if (deadClients.Count > 0)
            {
                // pulse the dead clients, if they do not error, keep them.
                var exceptions = SendDataTo(deadClients, new CommsPacket()
                {
                    Comms = CommsEnum.Pulse,
                    IPAddress = "",
                    Protocol = ProtocolTypesEnum.Tcp,
                    Sent = now
                });

                if (exceptions != null)
                {
                    foreach (IClientData client in deadClients)
                    {
                        if (!exceptions.ContainsKey(client))
                        {
                            client.LastActivity = now;
                        }
                    }

                    deadClients = exceptions.Select(s => s.Key).ToList();
                }
            }

            List<IClientData> clients = Clients.Where(w => w.Client.Available > 0).ToList();

            foreach (IClientData client in clients)
            {
                if (!client.Client.Connected)
                {
                    // Remove the client from the clients list
                    deadClients.Add(client);
                    continue;
                }

                try
                {
                    byte[] data = new byte[client.Client.Available];
                    var stream = client.Client.GetStream();

                    await stream.ReadAsync(data);

                    string pktString = Encoding.UTF8.GetString(data);

                    // Could be more than one packet in here.
                    if (pktString.Contains("}{"))
                    {
                        pktString = $"[{pktString.Replace("}{", "},{")}]";
                        List<CommsPacket> packets = JsonConvert.DeserializeObject<List<CommsPacket>>(pktString);

                        foreach (ICommsPacket pkt in packets)
                        {
                            ProcessPacket(client, pkt);

                            if (OnTcpDataReceived != null)
                            {
                                OnTcpDataReceived(pkt);
                            }
                        }
                    }
                    else
                    {
                        ICommsPacket pkt = JsonConvert.DeserializeObject<CommsPacket>(pktString);
                        ProcessPacket(client, pkt);

                        if (OnTcpDataReceived != null)
                        {
                            OnTcpDataReceived(pkt);
                        }
                    }


                }
                catch (Exception ex)
                {
                    if (OnError != null)
                        OnError(ex, $"Failed to receive data from client [{client}].");

                    deadClients.Add(client);
                }
            }

            foreach (IClientData client in deadClients)
            {
                DisconnectClient(client);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Denies client. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="client">   The client. </param>
        ///-------------------------------------------------------------------------------------------------

        public void DenyClient(IClientData client)
        {
            SendDataTo(client, new CommsPacket()
            {
                Comms = CommsEnum.AccessDenied,
                IPAddress = $"{ExternalIP4vAddress}:{Port}",
                Data = "Access Denied",
            });

            DisconnectClient(client);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Disconnects the client described by client. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="client">   The client. </param>
        ///-------------------------------------------------------------------------------------------------

        public void DisconnectClient(IClientData client)
        {
            ICommsPacket bcPkt = new CommsPacket()
            {
                Id = Guid.Empty,
                Comms = CommsEnum.ClientDisconnected,
                IPAddress = $"{ExternalIP4vAddress}:{Port}",
                Data = client.PacketData,
                Protocol = ProtocolTypesEnum.Tcp,
                Sent = DateTime.UtcNow
            };

            SendDataTo(client, bcPkt);

            client.Close();
            Clients.Remove(client);


            SendDataToAll(bcPkt);

            if (OnConnectionDropped != null)
            {
                OnConnectionDropped(client.PacketData);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process the packet. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="client">   The client. </param>
        /// <param name="pkt">      The packet. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void ProcessPacket(IClientData client, ICommsPacket pkt)
        {
            ICommsPacket bcPkt = null;

            client.LastActivity = DateTime.UtcNow;

            switch (pkt.Comms)
            {
                case CommsEnum.Pulse:
                    break;
                case CommsEnum.RequestUdpDataResponse:
                    string[] pktData = pkt.IPAddress.Split(":");

                    client.UdpEndPoint = new IPEndPoint(IPAddress.Parse(pktData[0]), int.Parse(pktData[1]));
                    client.PacketData.PlayerGameData = pkt.Data;

                    // This is a new client, add them to the list, and tell everyone they have a new friend :)

                    bcPkt = new CommsPacket()
                    {
                        Id = Guid.Empty,
                        Comms = CommsEnum.NewClientAdded,
                        IPAddress = $"{ExternalIP4vAddress}:{Port}",
                        Data = client.PacketData,
                        Protocol = ProtocolTypesEnum.Tcp,
                        Sent = DateTime.UtcNow
                    };

                    SendDataToAll(bcPkt);

                    if (OnNewClientAdded != null)
                    {
                        OnNewClientAdded(client.PacketData);
                    }

                    break;
                case CommsEnum.Closed: // Client closed.

                    DisconnectClient(client);

                    break;
                case CommsEnum.RequestClientList:

                    List<IClientPacketData> dataList = Clients.Select(s => s.PacketData).ToList();

                    // add the server
                    dataList.Insert(0, new ClientPacketData()
                    {
                        Id = Guid.Empty,
                        PlayerGameData = PlayerData,
                        UdpAddress = ExternalIP4vAddress,
                        UdPPort = Port
                    });

                    bcPkt = new CommsPacket()
                    {
                        Id = Guid.Empty,
                        Comms = CommsEnum.RequestClientListResponse,
                        IPAddress = $"{ExternalIP4vAddress}:{Port}",
                        Data = dataList,
                        Protocol = ProtocolTypesEnum.Tcp,
                        Sent = DateTime.UtcNow
                    };

                    SendDataTo(client, bcPkt);

                    break;
                case CommsEnum.SendData:
                    if (client != null)
                    {
                        client.PacketData.PlayerGameData = pkt.Data;
                        if (OnUdpDataReceived != null)
                        {
                            OnUdpDataReceived(pkt);
                        }
                    }
                    else
                    {
                        if (OnLog != null)
                        {
                            OnLog(LogLevelEnum.Warning, "Data from unknown client, it was ignored");
                        }
                    }
                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;

                // Terminate all client sockets.
                ICommsPacket pkt = new CommsPacket()
                {
                    Id = Guid.Empty,
                    Protocol = ProtocolTypesEnum.Tcp,
                    IPAddress = $"{ExternalIP4vAddress}:{Port}",
                    Comms = CommsEnum.Closing
                };

                SendDataToAll(pkt);

                pkt = new CommsPacket()
                {
                    Id = Guid.Empty,
                    Protocol = ProtocolTypesEnum.Tcp,
                    IPAddress = $"{ExternalIP4vAddress}:{Port}",
                    Comms = CommsEnum.Closed
                };

                foreach (ClientData client in Clients)
                {
                    try
                    {
                        SendDataTo(client, pkt);
                    }
                    catch (Exception ex)
                    {
                        if (OnError != null)
                            OnError(ex, $"Failed to send server stopped message to client [{client}].");
                    }
                    finally
                    {
                        client.Close();
                    }
                }

                Clients.Clear();

                _tcpListener.Stop();
                _udpClient.Close();

                if (OnServerStop != null)
                {
                    OnServerStop();
                }
            }
        }
    }
}
