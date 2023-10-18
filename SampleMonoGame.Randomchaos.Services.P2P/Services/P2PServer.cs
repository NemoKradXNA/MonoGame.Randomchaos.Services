using Newtonsoft.Json;
using SampleMonoGame.Randomchaos.Services.P2P.Delegates;
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SampleMonoGame.Randomchaos.Services.P2P.Services
{
    public class P2PServer : IP2PServer
    {
        public event ServerStartEvent OnServerStart;
        public event ServerStopEvent OnServerStop;
        public event ConnectionAttemptEvent OnConnectionAttempt;
        public event ConnectionAttemptEvent OnNewClientAdded;
        public event ConnectionDroppedEvent OnConnectionDropped;
        public event DataReceivedEvent OnTcpDataReceived;
        public event DataReceivedEvent OnUdpDataReceived;
        public event ErrorEvent OnError;
        public event ClientCommsError OnClientCommsError;
        public event LogEvent OnLog;

        public string LocalIPv4Address { get; protected set; }
        public string ExternalIP4vAddress { get; protected set; }
        public string MachineName { get; protected set; }

        public int Port { get; protected set; }

        public bool IsRunning { get; protected set; } = false;

        public List<IClientData> Clients { get; protected set; } = new List<IClientData>();

        public TimeSpan ClearInactiveConnectionTimeout { get; set; } = new TimeSpan(0, 0, 10);
        protected readonly UdpClient _udpClient;
        protected readonly TcpListener _tcpListener;

        protected readonly IPEndPoint _udpEndPoint;
        protected readonly IPEndPoint _tcpEndPoint;

        public P2PServer(int port, string externalIPv4Address = null)
        {
            Port = port;
            ExternalIP4vAddress = externalIPv4Address;

            _udpClient = new UdpClient(Port);
            _udpClient.AllowNatTraversal(true);

            _tcpEndPoint = new IPEndPoint(IPAddress.Any, Port);
            _tcpListener = new TcpListener(_tcpEndPoint);
        }

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
            _tcpListener.Start();

            _tcpListener.BeginAcceptTcpClient(AcceptTcpClient, _tcpListener);

            _udpClient.BeginReceive(ReceiveUDPCallback, new {
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

                CommsPacket pkt = JsonConvert.DeserializeObject<CommsPacket>(receiveString);

                IClientData client = Clients.SingleOrDefault(w => $"{w.UdpEndPoint.Address}:{w.UdpEndPoint.Port}" == pkt.IPAddress);

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

        protected void AcceptTcpClient(IAsyncResult result)
        {
            if (IsRunning)
            {
                try
                {
                    // Client Exists?
                    TcpClient tcpClient = _tcpListener.EndAcceptTcpClient(result);
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

                    CommsPacket pkt = new CommsPacket(
                        CommsEnum.Accepted,
                        ExternalIP4vAddress,
                        "Your request has been received.")
                    {
                        Protocol = ProtocolTypesEnum.Tcp,
                        IPAddress = $"{ExternalIP4vAddress}:{Port}"
                    };

                    SendDataTo(client, pkt);

                    pkt = new CommsPacket(CommsEnum.RequestUdpData, ExternalIP4vAddress) { Protocol = ProtocolTypesEnum.Tcp, IPAddress = $"{ExternalIP4vAddress}:{Port}" };
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
                        //_udpListener.Send(pktData, pktData.Length);
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

        public Dictionary<IClientData, Exception> SendDataTo(List<IClientData> clients, ICommsPacket data)
        {
            Dictionary<IClientData, Exception> deadClients = new Dictionary<IClientData, Exception>();

            foreach (ClientData client in clients)
            {
                Exception ex = SendDataTo(client, data);

                if (ex != null)
                {
                    deadClients.Add(client, ex);
                }
            }

            return deadClients;
        }

        public virtual Dictionary<IClientData, Exception> SendDataToAll(ICommsPacket data)
        {
            return SendDataTo(Clients, data);
        }

       
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
                                OnTcpDataReceived(client.PacketData, pkt.Data);
                            }
                        }
                    }
                    else
                    {
                        CommsPacket pkt = JsonConvert.DeserializeObject<CommsPacket>(pktString);
                        ProcessPacket(client, pkt);

                        if (OnTcpDataReceived != null)
                        {
                            OnTcpDataReceived(client.PacketData, pkt.Data);
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

        public void DisconnectClient(IClientData client)
        {
            CommsPacket bcPkt = new CommsPacket()
            {
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

        protected void ProcessPacket(IClientData client, ICommsPacket pkt)
        {
            CommsPacket bcPkt = null;

            client.LastActivity = DateTime.UtcNow;

            switch (pkt.Comms)
            {
                case CommsEnum.Pulse:
                    break;
                case CommsEnum.RequestUdpDataResponse:
                    string[] pktData = pkt.IPAddress.Split(":");
                    client.UdpEndPoint = new IPEndPoint(IPAddress.Parse(pktData[0]), int.Parse(pktData[1]));
                    // This is a new client, add them to the list, and tell everyone they have a new friend :)

                    bcPkt = new CommsPacket()
                    {
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

                    bcPkt = new CommsPacket()
                    {
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
                        if (OnUdpDataReceived != null)
                        {
                            OnUdpDataReceived(client.PacketData, pkt.Data);
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

        public virtual void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;

                // Terminate all client sockets.
                CommsPacket pkt = new CommsPacket()
                {
                    Protocol = ProtocolTypesEnum.Tcp,
                    IPAddress = $"{ExternalIP4vAddress}:{Port}",
                    Comms = CommsEnum.Closing
                };

                SendDataToAll(pkt);

                pkt = new CommsPacket()
                {
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
