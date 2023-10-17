
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;

namespace SampleMonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A client data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ClientData : IClientData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Date/Time of the last activity. </summary>
        ///
        /// <value> The last activity. </value>
        ///-------------------------------------------------------------------------------------------------

        public DateTime LastActivity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the packet. </summary>
        ///
        /// <value> Information describing the packet. </value>
        ///-------------------------------------------------------------------------------------------------

        public IClientPacketData PacketData { get; set; } = new ClientPacketData();
        /// <summary>   The UDP end point. </summary>
        protected IPEndPoint _UdpEndPoint;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the UDP end point. </summary>
        ///
        /// <value> The UDP end point. </value>
        ///-------------------------------------------------------------------------------------------------

        public IPEndPoint UdpEndPoint
        {
            get
            {
                if (_UdpEndPoint == null && !string.IsNullOrEmpty(PacketData.UdpAddress))
                {
                    _UdpEndPoint = PacketData.GetIPEndPoint();
                }

                return _UdpEndPoint;
            }
            set
            {
                _UdpEndPoint = value;

                PacketData.UdpAddress = _UdpEndPoint.Address.ToString();
                PacketData.UdPPort = _UdpEndPoint.Port;
            }
        }
        /// <summary>   The client. </summary>
        public TcpClient Client { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            if (UdpEndPoint != null)
                return $"{PacketData.Id} ({PacketData})";
            else
                return $"{PacketData.Id} (UDP Endpoint Unknown)";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Closes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Close()
        {
            Client.Close();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Finalizer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        ~ClientData()
        {

        }
    }
}
