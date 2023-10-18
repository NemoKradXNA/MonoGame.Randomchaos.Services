using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SampleMonoGame.Randomchaos.Services.P2P.Delegates;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace SampleMonoGame.Randomchaos.Services.P2P.Services
{
    public class P2PClient : IP2PClient
    {
        protected readonly IPEndPoint _serverEndpoint;
        protected IPEndPoint _clientEndpoint;

        protected TcpClient _tcpClient;
        protected UdpClient _udpClient;

        public List<IClientPacketData> Clients { get; protected set; } = new List<IClientPacketData>();

        public event DataReceivedEvent OnTcpDataReceived;
        public event DataReceivedEvent OnUdpDataReceived;
        public event ConnectionDroppedEvent OnConnectionDropped;
        public event ConnectionDroppedEvent OnClientConnectionDropped;
        public event ConnectionAttemptEvent OnNewClientAdded;
        public event LogEvent OnLog;

        public PlayerData PlayerData { get; set; }
        public string IPv4Address { get { return $"{_clientEndpoint.Address}:{_clientEndpoint.Port}"; } }
        public string ServerIPv4Address { get { return $"{_serverEndpoint.Address}:{_serverEndpoint.Port}"; } }

        public Guid? Id { get; protected set; }
        public int Port { get; protected set; }

        public bool IsConnected { get { return _tcpClient.Connected; } }

        public DateTime ServerLastActive { get; protected set; }
        public TimeSpan ServerInactiveTimeout { get; set; } = new TimeSpan(0, 0, 10);

        public P2PClient(string serverIPv4Address, int serverPort, string clientIPv4Address, int clientPort)
        {
            _serverEndpoint = new IPEndPoint(IPAddress.Parse(serverIPv4Address), serverPort);
            _clientEndpoint = new IPEndPoint(IPAddress.Parse(clientIPv4Address), clientPort);

            _tcpClient = new TcpClient();

            Port = clientPort;

            _udpClient = new UdpClient(Port);
            _udpClient.AllowNatTraversal(true);
        }

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
                    _udpClient.Send(pktData, pktData.Length, endpoint != null ? endpoint : _serverEndpoint);
                    break;
                default:

                    break;
            }
        }

        public void BroadcastTo(ICommsPacket data, List<IClientPacketData> clients)
        {
            foreach (IClientPacketData client in clients)
            {
                SendDataTo(data, client.GetIPEndPoint());
            }
        }


        protected void ConnectToServerAsync(IAsyncResult result)
        {
            _ = Task.Run(async () =>
            {
                try
                {

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
                    _udpClient.BeginReceive(UdpReceiveData, new { listener = listener, endPoint = endpoint });
                }
            }

        }

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

                    AddClient(new ClientPacketData()
                    {
                        Id = Guid.Empty,
                        UdpAddress = _serverEndpoint.Address.ToString(),
                        UdPPort = _serverEndpoint.Port
                    });

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

        protected virtual IClientPacketData GetClientByIdAndAddress(IClientPacketData pktData)
        {
            return Clients.SingleOrDefault(s => s.Id == pktData.Id || (s.UdpAddress == pktData.UdpAddress && s.UdPPort == pktData.UdPPort));
        }
    }
}
