
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.P2P.Delegates;
using MonoGame.Randomchaos.Services.P2P.Enums;
using MonoGame.Randomchaos.Services.P2P.Interfaces;
using MonoGame.Randomchaos.Services.P2P.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MonoGame.Randomchaos.Services.P2P.Services
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing peer 2 peer information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class P2PService : ServiceBase<IP2PService>, IP2PService
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
        /// <summary>   Gets or sets a value indicating whether the accepting connections. </summary>
        ///
        /// <value> True if accepting connections, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool AcceptingConnections
        {
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the clients. </summary>
        ///
        /// <value> The clients. </value>
        ///-------------------------------------------------------------------------------------------------

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
                else if (_p2pServer != null)
                {
                    return _p2pClient.Clients;
                }

                return new List<IClientPacketData>();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the session. </summary>
        ///
        /// <value> The session. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISessionData Session { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the player. </summary>
        ///
        /// <value> Information describing the player. </value>
        ///-------------------------------------------------------------------------------------------------

        public IPlayerData PlayerData
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the identifier of the client. </summary>
        ///
        /// <value> The identifier of the client. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the listening port. </summary>
        ///
        /// <value> The listening port. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ListeningPort { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is server. </summary>
        ///
        /// <value> True if this object is server, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsServer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the local IPv4 address. </summary>
        ///
        /// <value> The local IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string LocalIPv4Address { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the external IPv4 address. </summary>
        ///
        /// <value> The external IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ExternalIPv4Address { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name of the machine. </summary>
        ///
        /// <value> The name of the machine. </value>
        ///-------------------------------------------------------------------------------------------------

        public string MachineName { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the server IPv4 address. </summary>
        ///
        /// <value> The server IPv4 address. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ServerIPv4Address { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of players. </summary>
        ///
        /// <value> The number of players. </value>
        ///-------------------------------------------------------------------------------------------------

        public int PlayerCount
        {
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the peer 2 peer server. </summary>
        ///
        /// <value> The p 2p server. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IP2PServer _p2pServer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the peer 2 peer client. </summary>
        ///
        /// <value> The p 2p client. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IP2PClient _p2pClient { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="game">                 The game. </param>
        /// <param name="externalIPv4Address">  (Optional) The external IPv4 address. </param>
        ///-------------------------------------------------------------------------------------------------

        public P2PService(Game game, string externalIPv4Address = null) : base(game)
        {
            LoadHostDetails();

            if (string.IsNullOrEmpty(externalIPv4Address))
            {
                ExternalIPv4Address = LocalIPv4Address;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Boot client. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        /// <param name="msg">  (Optional) The message. </param>
        ///-------------------------------------------------------------------------------------------------

        public void BootClient(Guid id, string msg = null)
        {
            if (IsServer)
            {
                IClientData client = _p2pServer.Clients.SingleOrDefault(s => s.PacketData.Id == id);
                _p2pServer.DisconnectClient(client);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a server. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="port">                 The port. </param>
        /// <param name="externalIPv4Address">  (Optional) The external IPv4 address. </param>
        /// <param name="sessionName">          (Optional) Name of the session. </param>
        /// <param name="sessionToken">         (Optional) The session token. </param>
        /// <param name="name">                 (Optional) The name. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   2p on new client added. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="client">   The client. </param>
        ///-------------------------------------------------------------------------------------------------

        private void _p2p_OnNewClientAdded(IClientPacketData client)
        {
            if (OnNewClientAdded != null)
            {
                OnNewClientAdded(client);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   2p on TCP data received. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ///-------------------------------------------------------------------------------------------------

        private void _p2p_OnTcpDataReceived(ICommsPacket data)
        {
            if (OnTcpDataReceived != null)
            {
                OnTcpDataReceived(data);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   2p on log. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="lvl">      The level. </param>
        /// <param name="message">  The message. </param>
        /// <param name="ex">       (Optional) The exception. </param>
        /// <param name="args">     A variable-length parameters list containing arguments. </param>
        ///-------------------------------------------------------------------------------------------------

        private void _p2p_OnLog(LogLevelEnum lvl, string message, Exception ex = null, params object[] args)
        {
            if (OnLog != null)
            {
                OnLog(lvl, message, ex, args);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   2p on UDP data received. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ///-------------------------------------------------------------------------------------------------

        private void _p2p_OnUdpDataReceived(ICommsPacket data)
        {
            if (OnUdpDataReceived != null)
            {
                OnUdpDataReceived(data);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   2p on connection dropped. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="client">   The client. </param>
        ///-------------------------------------------------------------------------------------------------

        private void _p2p_OnConnectionDropped(IClientPacketData client)
        {
            if (OnConnectionDropped != null)
            {
                OnConnectionDropped(client);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   2p server on connection attempt. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="client">   The client. </param>
        ///-------------------------------------------------------------------------------------------------

        private void _p2pServer_OnConnectionAttempt(IClientPacketData client)
        {
            if (OnConnectionAttempt != null)
            {
                OnConnectionAttempt(client);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;Guid,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a data to. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="ids">  The identifiers. </param>
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;Guid,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Broadcasts the given data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>   A Dictionary&lt;Guid,Exception&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates data send packet. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="senderId">     Identifier for the sender. </param>
        /// <param name="senderIP">     The sender IP. </param>
        /// <param name="senderPort">   The sender port. </param>
        /// <param name="data">         The data. </param>
        ///
        /// <returns>   The new data send packet. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected ICommsPacket CreateDataSendPacket(Guid senderId, string senderIP, int senderPort, object? data)
        {
            return new CommsPacket()
            {
                Id = senderId,
                Comms = CommsEnum.SendData,
                Data = data,
                IPAddress = senderIP,
                Port = senderPort,
                Protocol = ProtocolTypesEnum.Udp,
                Sent = DateTime.UtcNow
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops a server. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Connects a client. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="serverIPv4Address">    The server IPv4 address. </param>
        /// <param name="port">                 The port. </param>
        /// <param name="clientIPv4Address">    The client IPv4 address. </param>
        /// <param name="clientPort">           The client port. </param>
        /// <param name="sessionName">          (Optional) Name of the session. </param>
        /// <param name="sessionToken">         (Optional) The session token. </param>
        /// <param name="name">                 (Optional) The name. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Disconnects the given informServer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="informServer"> (Optional) The inform server to disconnect. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets client by identifier. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   The client by identifier. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets random port number. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="exclude">  A variable-length parameters list containing exclude. </param>
        ///
        /// <returns>   The random port number. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets clients player game data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        ///
        /// <returns>   The clients player game data. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads host details. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets host entry. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <returns>   The host entry. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected IPHostEntry GetHostEntry()
        {
            string hostName = Dns.GetHostName();
            return Dns.GetHostEntry(hostName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Releases the unmanaged resources used by the P2PService and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/10/2023. </remarks>
        ///
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        ///-------------------------------------------------------------------------------------------------

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
