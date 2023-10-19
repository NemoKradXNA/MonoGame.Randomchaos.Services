using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using Newtonsoft.Json;
using SampleMonoGame.Randomchaos.Services.P2P.Delegates;
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SampleMonoGame.Randomchaos.Services.P2P.Services
{
    public class P2PService : ServiceBase<IP2PService>, IP2PService
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

        public bool AcceptingConnections {
            get 
            {
                if (IsServer && _p2pServer != null)
                {
                    return _p2pServer.AcceptingConnections;
                }

                return false;
            }
            set
            {
                if (IsServer && _p2pServer != null)
                {
                    _p2pServer.AcceptingConnections = value;
                }
            }
        }

        public List<IClientPacketData> Clients
        {
            get
            {
                if (IsServer)
                {
                    if (_p2pServer != null)
                    {
                        return _p2pServer.Clients.Select(s => s.PacketData).ToList();
                    }
                    
                }
                else if(_p2pServer != null) 
                {
                    return _p2pClient.Clients;
                }

                return new List<IClientPacketData>();
            }
        }

        public SessionData Session { get; set; }
        public PlayerData PlayerData
        {
            get
            {
                if (IsServer && _p2pServer != null)
                {
                    return _p2pServer.PlayerData;
                }
                else if (_p2pClient != null)
                {
                    return _p2pClient.PlayerData;
                }

                return null;
            }
            set
            {
                if (IsServer && _p2pServer != null)
                {
                    _p2pServer.PlayerData = value;
                }
                else if (_p2pClient != null)
                {
                    _p2pClient.PlayerData = value;
                }

            }
        }

        public Guid ClientId
        {
            get
            {
                if (_p2pClient != null && _p2pClient.Id != null)
                {
                    return _p2pClient.Id.Value;
                }

                return Guid.Empty;
            }
        }

        public int ListeningPort { get; protected set; }
        public bool IsServer { get; set; }
        public string LocalIPv4Address { get; protected set; }
        public string ExternalIPv4Address{get; protected set; }
        public string MachineName { get; protected set; }

        public string ServerIPv4Address { get; protected set; }
        public int PlayerCount {
            get
            {
                if (IsServer)
                {
                    if (_p2pServer != null && _p2pServer.IsRunning)
                    {
                        return _p2pServer.Clients.Count;
                    }
                    
                    return 0;
                }
                else
                {
                    if (_p2pClient != null)
                    {
                        return _p2pClient.Clients.Count;
                    }
                    return 0;
                }
            }
        }

        protected IP2PServer _p2pServer { get; set; }

        protected IP2PClient _p2pClient { get; set; }

        public P2PService(Game game, string extrenalIPv4Address = null) : base(game)
        {
            LoadHostDetails();

            if (string.IsNullOrEmpty(extrenalIPv4Address))
            {
                ExternalIPv4Address = LocalIPv4Address;
            }
        }

        public void BootClient(Guid id, string msg = null)
        {
            if (IsServer)
            {
                IClientData client = _p2pServer.Clients.SingleOrDefault(s => s.PacketData.Id == id);
                _p2pServer.DisconnectClient(client);
            }
        }

        public void StartServer(int port, string externalIPv4Address = null, string sessionName = null, string sessionToken = null, string name = "Server")
        {
            Session = new SessionData()
            {
                Name = sessionName,
                Token = sessionToken,
            };

            ListeningPort = port;

            if (_p2pServer != null) // make sure the old one is stopped..
            {
                _p2pServer.Stop();
                // remove previously hooked events.
            }

            _p2pServer = new P2PServer(port, externalIPv4Address)
            {
                PlayerData = new PlayerData()
                {
                    Name = name,
                    Session = Session
                }
            };

            ServerIPv4Address = externalIPv4Address;

            _p2pServer.OnConnectionAttempt += _p2pServer_OnConnectionAttempt;
            _p2pServer.OnNewClientAdded += _p2p_OnNewClientAdded;
            _p2pServer.OnConnectionDropped += _p2p_OnConnectionDropped;
            _p2pServer.OnUdpDataReceived += _p2p_OnUdpDataReceived;
            _p2pServer.OnTcpDataReceived += _p2p_OnTcpDataReceived;
            _p2pServer.OnLog += _p2p_OnLog;

            _p2pServer.Start();
        }

        private void _p2p_OnNewClientAdded(IClientPacketData client)
        {
            if (OnNewClientAdded != null)
            {
                OnNewClientAdded(client);
            }
        }

        private void _p2p_OnTcpDataReceived(ICommsPacket data)
        {
            if (OnTcpDataReceived != null)
            {
                OnTcpDataReceived(data);
            }
        }


        private void _p2p_OnLog(LogLevelEnum lvl, string message, Exception ex = null, params object[] args)
        {
            if (OnLog != null)
            {
                OnLog(lvl, message, ex, args);
            }
        }

        private void _p2p_OnUdpDataReceived(ICommsPacket data)
        {
            if (OnUdpDataReceived != null)
            {
                OnUdpDataReceived(data);
            }
        }

        private void _p2p_OnConnectionDropped(IClientPacketData client)
        {
            if (OnConnectionDropped != null)
            {
                OnConnectionDropped(client);
            }
        }

        private void _p2pServer_OnConnectionAttempt(IClientPacketData client)
        {
            if (OnConnectionAttempt != null)
            {
                OnConnectionAttempt(client);
            }
        }


        public Exception SendDataTo(Guid id, object? data)
        {

            if (IsServer && _p2pServer != null && _p2pServer.IsRunning)
            {
                // get the client
                IClientData client = _p2pServer.Clients.SingleOrDefault(s => s.PacketData.Id == id);
                if (client != null)
                {
                    return _p2pServer.SendDataTo(client, CreateDataSendPacket(Guid.Empty, ServerIPv4Address, ListeningPort, data));
                }
            }
            else
            {
                // Do client send.
                IClientPacketData clientPkt = _p2pClient.Clients.SingleOrDefault(s => s.Id == id);

                if (clientPkt != null)
                {
                    _p2pClient.SendDataTo(CreateDataSendPacket(_p2pClient.Id.Value, ExternalIPv4Address, _p2pClient.Port, data), clientPkt.GetIPEndPoint());
                    return null;
                }

            }

            return new Exception("A client with this Id does not exist.");
        }

        public Dictionary<Guid, Exception> SendDataTo(List<Guid> ids, object? data)
        {
            Dictionary<Guid, Exception> retVal = new Dictionary<Guid, Exception>();

            if (IsServer)
            {
                List<IClientData> clients = _p2pServer.Clients.Where(s => ids.Contains(s.PacketData.Id)).ToList();
                var excpeitons = _p2pServer.SendDataTo(clients, CreateDataSendPacket(Guid.Empty, ServerIPv4Address, ListeningPort, data));

                if (excpeitons != null && excpeitons.Count > 0)
                {
                    foreach (IClientData client in excpeitons.Keys)
                    {
                        retVal.Add(client.PacketData.Id, excpeitons[client]);
                    }
                }
            }
            else
            {
                List<IClientPacketData> clients = _p2pClient.Clients.Where(s => ids.Contains(s.Id)).ToList();
                _p2pClient.BroadcastTo(CreateDataSendPacket(_p2pClient.Id.Value, ServerIPv4Address, ListeningPort, data), clients);
            }

            return retVal;
        }

        public Dictionary<Guid, Exception> Broadcast(object? data)
        {
            Dictionary<Guid, Exception> retVal = new Dictionary<Guid, Exception>();

            if (IsServer)
            {
                var excpeitons = _p2pServer.SendDataToAll(CreateDataSendPacket(Guid.Empty, ServerIPv4Address, ListeningPort, data));

                if (excpeitons != null && excpeitons.Count > 0)
                {
                    foreach (IClientData client in excpeitons.Keys)
                    {
                        retVal.Add(client.PacketData.Id, excpeitons[client]);
                    }
                }

                return null;
            }
            else
            {
                _p2pClient.BroadcastTo(CreateDataSendPacket(_p2pClient.Id.Value, ExternalIPv4Address, _p2pClient.Port, data), _p2pClient.Clients);
            }

            return retVal;
        }

        protected ICommsPacket CreateDataSendPacket(Guid senderId, string senderIP, int senderPort, object? data)
        {
            return new CommsPacket()
            {
                Id  = senderId,
                Comms = CommsEnum.SendData,
                Data = data,
                IPAddress = senderIP,
                Port = senderPort,
                Protocol = ProtocolTypesEnum.Udp,
                Sent = DateTime.UtcNow
            };
        }

        public void StopServer()
        {
            _p2pServer.Stop();

            _p2pServer.OnConnectionAttempt -= _p2pServer_OnConnectionAttempt;
            _p2pServer.OnNewClientAdded -= _p2p_OnNewClientAdded;
            _p2pServer.OnConnectionDropped -= _p2p_OnConnectionDropped;
            _p2pServer.OnUdpDataReceived -= _p2p_OnUdpDataReceived;
            _p2pServer.OnTcpDataReceived -= _p2p_OnTcpDataReceived;
            _p2pServer.OnLog -= _p2p_OnLog;
        }

        public void ConnectClient(string serverIPv4Address, int port, string clientIPv4Address, int clientPort, string sessionName = null, string sessionToken = null, string name = null)
        {
            Session = new SessionData()
            {
                Name = sessionName,
                Token = sessionToken,
            };

            if (_p2pClient != null) // make sure the old one is stopped..
            {
                _p2pClient.Disconnect();
            }

            ListeningPort = port;

            _p2pClient = new P2PClient(serverIPv4Address, port, clientIPv4Address, clientPort)
            {
                PlayerData = new PlayerData()
                {
                    Name = string.IsNullOrEmpty(name) ? $"Client {clientIPv4Address}:{clientPort}" : name,
                    Session = Session
                }
            };

            _p2pClient.OnConnectionDropped += _p2p_OnConnectionDropped;
            _p2pClient.OnClientConnectionDropped += _p2p_OnConnectionDropped;
            _p2pClient.OnLog += _p2p_OnLog;
            _p2pClient.OnUdpDataReceived += _p2p_OnUdpDataReceived;
            _p2pClient.OnTcpDataReceived += _p2p_OnTcpDataReceived;
            _p2pClient.OnNewClientAdded += _p2p_OnNewClientAdded;

            ServerIPv4Address = $"{serverIPv4Address}";

            _p2pClient.Connect();
        }

        public void Disconnect(bool informServer = true)
        {
            if (!IsServer && _p2pClient != null)
            {
                _p2pClient.Disconnect(informServer);

                _p2pClient.OnConnectionDropped -= _p2p_OnConnectionDropped;
                _p2pClient.OnClientConnectionDropped -= _p2p_OnConnectionDropped;
                _p2pClient.OnLog -= _p2p_OnLog;
                _p2pClient.OnUdpDataReceived -= _p2p_OnUdpDataReceived;
                _p2pClient.OnTcpDataReceived -= _p2p_OnTcpDataReceived;
            }
        }

        public IClientPacketData GetClientById(Guid id)
        {
            IClientPacketData retVal;

            if (IsServer)
            {
                retVal = _p2pServer.Clients.Select(s => s.PacketData).SingleOrDefault(s => s.Id == id);
            }
            else
            {
                retVal = _p2pClient.Clients.SingleOrDefault(s => s.Id == id);
            }

            return retVal;
        }

        public int GetRandomPortNumber(params int[] exclude)
        {
            if (exclude == null)
            {
                exclude = new int[0];
            }

            int portNumber = (int)MathHelper.Lerp(6000, 7000, DateTime.UtcNow.Millisecond / 1000f);

            while (exclude.Contains(portNumber))
            {
                portNumber = (int)MathHelper.Lerp(6000, 7000, DateTime.UtcNow.Millisecond / 1000f);
            }

            return portNumber;
        }

        public List<T> GetClientsPlayerGameData<T>()
        {
            List<T> retVal = new List<T>();

            foreach (IClientPacketData data in Clients)
            {
                if (data.PlayerGameData != null)
                {
                    T pkt = JsonConvert.DeserializeObject<T>(data.PlayerGameData.ToString());

                    if (pkt != null)
                    {
                        retVal.Add(pkt);
                    }
                }
            }

            return retVal;
        }

        protected void LoadHostDetails()
        {
            IPHostEntry hostEntry = GetHostEntry();

            if (LocalIPv4Address == null)
            {
                LocalIPv4Address = hostEntry.AddressList[1].ToString();
            }

            if (MachineName == null)
            {
                MachineName = hostEntry.HostName;
            }
        }

        protected IPHostEntry GetHostEntry()
        {
            string hostName = Dns.GetHostName();
            return Dns.GetHostEntry(hostName);
        }

        protected override void Dispose(bool disposing)
        {
            if (IsServer && _p2pServer != null && _p2pServer.IsRunning)
            {
                StopServer();
            }
            else
            {
                Disconnect();
                // Close client.
            }
            base.Dispose(disposing);
        }
    }
}
