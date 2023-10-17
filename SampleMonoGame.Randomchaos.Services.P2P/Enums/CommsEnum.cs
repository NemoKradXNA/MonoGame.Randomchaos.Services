namespace SampleMonoGame.Randomchaos.Services.P2P.Enums
{
    public enum CommsEnum : int
    {
        /// <summary>   An enum constant representing the accepted option. </summary>
        Accepted = 0,
        /// <summary>   An enum constant representing the request UDP end point option. </summary>
        RequestUdpData = 1,
        /// <summary>   An enum constant representing the request UDP data response option. </summary>
        RequestUdpDataResponse = 2,
        /// <summary>   An enum constant representing the request client list option. </summary>
        RequestClientList = 3,
        /// <summary>   An enum constant representing the request client list response option. </summary>
        RequestClientListResponse = 4,
        /// <summary>   An enum constant representing the denied option. </summary>
        AccessDenied = 5,
        /// <summary>   Are you still there? </summary>
        Pulse = 6,
        /// <summary>   An enum constant representing the new client added option. </summary>
        NewClientAdded = 7,
        /// <summary>   An enum constant representing the client disconnected option. </summary>
        ClientDisconnected = 8,
        /// <summary>   An enum constant representing the send data option. </summary>
        SendData = 9,
        /// <summary>   An enum constant representing the closign option. </summary>
        Closing = 10,
        /// <summary>   An enum constant representing the closed option. </summary>
        Closed = 11
    }
}
