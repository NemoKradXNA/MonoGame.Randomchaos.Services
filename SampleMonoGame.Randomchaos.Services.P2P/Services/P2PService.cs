using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
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

        public int ListeningPort { get; protected set; }
        public bool IsServer { get; set; }
        public string LocalIPv4Address { get; protected set; }
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
                    return 0;
                }
            }
        }

        protected IP2PServer _p2pServer { get; set; }

        public P2PService(Game game) : base(game)
        {
            LoadHostDetails();
        }

        public void StartServer(int port, string externalIPv4Address = null)
        {
            ListeningPort = port;

            if (_p2pServer != null) // make sure the old one is stopped..
            {
                _p2pServer.Stop();
                // remove previously hooked events.
            }

            _p2pServer = new P2PServer(port, externalIPv4Address);

            ServerIPv4Address = $"{externalIPv4Address}:{port}";

            _p2pServer.OnConnectionAttempt += _p2pServer_OnConnectionAttempt;
            _p2pServer.OnConnectionDropped += _p2pServer_OnConnectionDropped;
            _p2pServer.OnUdpDataReceived += _p2pServer_OnUdpDataReceived;
            _p2pServer.OnLog += _p2pServer_OnLog;

            _p2pServer.Start();

            
        }

        public void BootClient(Guid id, string msg = null)
        {
            if (IsServer)
            {
                IClientData client = _p2pServer.Clients.SingleOrDefault(s => s.PacketData.Id == id);
                _p2pServer.DisconnectClient(client);
            }
        }

        private void _p2pServer_OnLog(LogLevelEnum lvl, string message, Exception ex = null, params object[] args)
        {
            if (OnLog != null)
            {
                OnLog(lvl, message, ex, args);
            }
        }

        private void _p2pServer_OnUdpDataReceived(IClientPacketData client, object? data)
        {
            if (OnUdpDataReceived != null)
            {
                OnUdpDataReceived(client, data);
            }
        }

        private void _p2pServer_OnConnectionDropped(IClientPacketData client)
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
                    return _p2pServer.SendDataTo(client, CreateDataSendPacket(ServerIPv4Address, ListeningPort, data));
                }

                return new Exception("A client with this Id does not exist.");
            }
            else
            {
                // Do client send.
                return null;
            }
        }

        public Dictionary<Guid, Exception> SendDataTo(List<Guid> ids, object? data)
        {
            Dictionary<Guid, Exception> retVal = new Dictionary<Guid, Exception>();

            if (IsServer)
            {
                List<IClientData> clients = _p2pServer.Clients.Where(s => ids.Contains(s.PacketData.Id)).ToList();
                var excpeitons = _p2pServer.SendDataTo(clients, CreateDataSendPacket(ServerIPv4Address, ListeningPort, data));

                if (excpeitons != null && excpeitons.Count > 0)
                {
                    foreach (IClientData client in excpeitons.Keys)
                    {
                        retVal.Add(client.PacketData.Id, excpeitons[client]);
                    }
                }
            }

            return retVal;
        }

        public Dictionary<Guid, Exception> Broadcast(object? data)
        {
            Dictionary<Guid, Exception> retVal = new Dictionary<Guid, Exception>();

            if (IsServer)
            {
                var excpeitons = _p2pServer.SendDataToAll(CreateDataSendPacket(ServerIPv4Address, ListeningPort, data));

                if (excpeitons != null && excpeitons.Count > 0)
                {
                    foreach (IClientData client in excpeitons.Keys)
                    {
                        retVal.Add(client.PacketData.Id, excpeitons[client]);
                    }
                }

                return null;
            }

            return retVal;
        }

        protected ICommsPacket CreateDataSendPacket(string senderIP, int senderPort, object? data)
        {
            return new CommsPacket()
            {
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
            _p2pServer.OnConnectionAttempt -= _p2pServer_OnConnectionAttempt;
            _p2pServer.OnConnectionDropped -= _p2pServer_OnConnectionDropped;
            _p2pServer.OnUdpDataReceived -= _p2pServer_OnUdpDataReceived;
            _p2pServer.OnLog -= _p2pServer_OnLog;

            _p2pServer.Stop();
        }

        public void ConnectClient(string serverIPv4Address, int port)
        {

        }
        public void Disconnect()
        {

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
                // Close client.
            }
            base.Dispose(disposing);
        }
    }
}
