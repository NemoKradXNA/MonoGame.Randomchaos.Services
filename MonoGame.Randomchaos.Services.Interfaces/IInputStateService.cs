

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for input state service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IInputStateService : IInputStateManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the manager for keyboard. </summary>
        ///
        /// <value> The keyboard manager. </value>
        ///-------------------------------------------------------------------------------------------------

        IKeyboardStateManager KeyboardManager { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the manager for game pad. </summary>
        ///
        /// <value> The game pad manager. </value>
        ///-------------------------------------------------------------------------------------------------

        IGamePadManager GamePadManager { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the manager for mouse. </summary>
        ///
        /// <value> The mouse manager. </value>
        ///-------------------------------------------------------------------------------------------------

        IMouseStateManager MouseManager { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the accelerometer handler. </summary>
        ///
        /// <value> The accelerometer handler. </value>
        ///-------------------------------------------------------------------------------------------------

        IAccelerometerHandler AccelerometerHandler { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the manager for touch collection. </summary>
        ///
        /// <value> The touch collection manager. </value>
        ///-------------------------------------------------------------------------------------------------

        ITouchCollectionManager TouchCollectionManager { get; set; }
    }
}
