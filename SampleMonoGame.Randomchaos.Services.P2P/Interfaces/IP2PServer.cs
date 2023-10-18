using SampleMonoGame.Randomchaos.Services.P2P.Delegates;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using System;
using System.Collections.Generic;

namespace SampleMonoGame.Randomchaos.Services.P2P.Interfaces
{
    public interface IP2PServer
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

        PlayerData PlayerData { get; set; }

        string LocalIPv4Address { get; }
        string ExternalIP4vAddress { get; }
        string MachineName { get; }
        int Port { get; }
         bool IsRunning { get; }
        List<IClientData> Clients { get; }
        TimeSpan ClearInactiveConnectionTimeout { get; set; }

        void Start();
        Dictionary<IClientData, Exception> SendDataTo(List<IClientData> clients, ICommsPacket data);
        Dictionary<IClientData, Exception> SendDataToAll(ICommsPacket data);
        void DenyClient(IClientData client);
        void DisconnectClient(IClientData client);
        void Stop();
        Exception SendDataTo(IClientData client, ICommsPacket data);
    }
}
