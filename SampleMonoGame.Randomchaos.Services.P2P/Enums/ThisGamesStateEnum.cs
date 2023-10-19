
namespace SampleMonoGame.Randomchaos.Services.P2P.Enums
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent this games state enums. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum ThisGamesStateEnum : long
    {
        /// <summary>   An enum constant representing the none option. </summary>
        None = 0,
        /// <summary>   An enum constant representing the client not ready option. </summary>
        ClientNotReady = 1,
        /// <summary>   An enum constant representing the client ready option. </summary>
        ClientReady = 2,
        /// <summary>   An enum constant representing the paused option. </summary>
        Paused = 4,
        /// <summary>   An enum constant representing the in game option. </summary>
        InGame = 8,

        /// <summary>   An enum constant representing the set up option. All players and their psoitinos are sent. </summary>
        SetUp = 24, 

        /// <summary>   An enum constant representing the update option. </summary>
        Update = 40,

    }
}
