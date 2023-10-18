
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using System;

namespace SampleMonoGame.Randomchaos.Services.P2P.Delegates
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Server start event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public delegate void ServerStartEvent();

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Server stop event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public delegate void ServerStopEvent();

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Connection attempt event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///
    /// <param name="client">   The client. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void ConnectionAttemptEvent(IClientPacketData client);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Connection dropped event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///
    /// <param name="client">   The client. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void ConnectionDroppedEvent(IClientPacketData client);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Data received event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///
    /// <param name="client">   The client. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void DataReceivedEvent(ICommsPacket pkt);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Client communications error. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///
    /// <param name="client">   The client. </param>
    /// <param name="data">     The data. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void ClientCommsError(IClientData client, ICommsPacket data);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Error event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///
    /// <param name="e">            An Exception to process. </param>
    /// <param name="ExtraInfo">    Information describing the extra. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void ErrorEvent(Exception e, string ExtraInfo);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Logs an event. </summary>
    ///
    /// <remarks>   Charles Humphrey, 17/10/2023. </remarks>
    ///
    /// <param name="lvl">      The level. </param>
    /// <param name="message">  The message. </param>
    /// <param name="ex">       (Optional) The exception. </param>
    /// <param name="args">     A variable-length parameters list containing arguments. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void LogEvent(LogLevelEnum lvl, string message, Exception ex = null, params object?[] args);
}
