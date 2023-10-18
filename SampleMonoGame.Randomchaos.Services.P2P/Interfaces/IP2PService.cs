using SampleMonoGame.Randomchaos.Services.P2P.Delegates;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using System;
using System.Collections.Generic;

namespace SampleMonoGame.Randomchaos.Services.P2P.Interfaces
{
    public interface IP2PService
    {
        event ServerStartEvent OnServerStart;
        event ServerStopEvent OnServerStop;
        event ConnectionAttemptEvent OnConnectionAttempt;
        event ConnectionAttemptEvent OnNewClientAdded;
        event ConnectionDroppedEvent OnConnectionDropped;
        event DataReceivedEvent OnTcpDataReceived;
        event DataReceivedEvent OnUdpDataReceived;
        event ErrorEvent OnError;
        event ClientCommsError OnClientCommsError;
        event LogEvent OnLog;

        SessionData Session { get; set; }

        Guid ClientId { get; }
        bool IsServer { get; set; }
        int ListeningPort { get; }
        string ServerIPv4Address { get; }
        string LocalIPv4Address { get; }
        string ExternalIPv4Address { get; }
        string MachineName { get; }

        int PlayerCount { get; }
        void StartServer(int port, string externalIPv4Address = null, string sessionName = null, string sessionToken = null);
        void StopServer();

        void BootClient(Guid id, string msg = null);

        void ConnectClient(string serverIPv4Address, int port, string clientIPv4Address, int clientPort, string sessionName = null, string sessionToken = null);
        void Disconnect(bool informServer = true);

        Exception SendDataTo(Guid id, object? data);

        Dictionary<Guid, Exception> SendDataTo(List<Guid> ids, object? data);

        Dictionary<Guid, Exception> Broadcast(object? data);

    }
}
