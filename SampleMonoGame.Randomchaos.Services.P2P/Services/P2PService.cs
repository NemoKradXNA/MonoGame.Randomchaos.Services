using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using System.Net;

namespace SampleMonoGame.Randomchaos.Services.P2P.Services
{
    public class P2PService : ServiceBase<IP2PService>, IP2PService
    {
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
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public P2PService(Game game) : base(game)
        {
            LoadHostDetails();
        }

        public void StartServer(int port)
        {
            ListeningPort = port;
        }

        public void StopServer()
        {

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
    }
}
