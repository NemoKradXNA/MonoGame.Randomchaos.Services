

using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;

namespace MonoGame.Randomchaos.Services.Input
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing input handlers information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class InputHandlerService : ServiceBase<IInputStateService>, IInputStateService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the manager for keyboard. </summary>
        ///
        /// <value> The keyboard manager. </value>
        ///-------------------------------------------------------------------------------------------------

        public IKeyboardStateManager KeyboardManager { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Manager for game pad input, available on all platforms. </summary>
        ///
        /// <value> The game pad manager. </value>
        ///-------------------------------------------------------------------------------------------------

        public IGamePadManager GamePadManager { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Manager used for mouse input, available in Windows only. </summary>
        ///
        /// <value> The mouse manager. </value>
        ///-------------------------------------------------------------------------------------------------

        public IMouseStateManager MouseManager { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the accelerometer handler. </summary>
        ///
        /// <value> The accelerometer handler. </value>
        ///-------------------------------------------------------------------------------------------------

        public IAccelerometerHandler AccelerometerHandler { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the manager for touch collection. </summary>
        ///
        /// <value> The touch collection manager. </value>
        ///-------------------------------------------------------------------------------------------------

        public ITouchCollectionManager TouchCollectionManager { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="kbm">  (Optional) The kbm. </param>
        /// <param name="msm">  (Optional) The msm. </param>
        /// <param name="gpm">  (Optional) The gpm. </param>
        /// <param name="ach">  (Optional) The ach. </param>
        /// <param name="tcm">  (Optional) The tcm. </param>
        ///-------------------------------------------------------------------------------------------------

        public InputHandlerService(Game game, IKeyboardStateManager kbm = null, IMouseStateManager msm = null,
           IGamePadManager gpm = null, IAccelerometerHandler ach = null, ITouchCollectionManager tcm = null) : base(game)
        {
            KeyboardManager = kbm;
            GamePadManager = gpm;
            MouseManager = msm;
            AccelerometerHandler = ach;
            TouchCollectionManager = tcm;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();

            if (KeyboardManager != null)
                KeyboardManager.Initialize();

            if (GamePadManager != null)
                GamePadManager.Initialize();

            if (MouseManager != null)
                MouseManager.Initialize();

            if (AccelerometerHandler != null)
                AccelerometerHandler.Initialize();

            if (TouchCollectionManager != null)
                TouchCollectionManager.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            if (Game.IsActive)
            {
                if (KeyboardManager != null)
                    KeyboardManager.Update(gameTime);

                if (GamePadManager != null)
                    GamePadManager.Update(gameTime);

                if (MouseManager != null)
                    MouseManager.Update(gameTime);

                if (AccelerometerHandler != null)
                    AccelerometerHandler.Update(gameTime);

                if (TouchCollectionManager != null)
                    TouchCollectionManager.Update(gameTime);

                base.Update(gameTime);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pre update. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void PreUpdate(GameTime gameTime)
        {
            if (Game.IsActive)
            {
                if (KeyboardManager != null)
                    KeyboardManager.PreUpdate(gameTime);

                if (GamePadManager != null)
                    GamePadManager.PreUpdate(gameTime);

                if (MouseManager != null)
                    MouseManager.PreUpdate(gameTime);

                if (AccelerometerHandler != null)
                    AccelerometerHandler.PreUpdate(gameTime);

                if (TouchCollectionManager != null)
                    TouchCollectionManager.PreUpdate(gameTime);
            }
        }
    }
}
