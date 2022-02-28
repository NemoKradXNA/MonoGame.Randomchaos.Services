
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;

namespace MonoGame.Randomchaos.Services.Input
{
    public class InputHandlerService : ServiceBase<IInputStateService>, IInputStateService
    {
        public IKeyboardStateManager KeyboardManager { get; set; }
        /// <summary>
        /// Manager for game pad input, available on all platforms
        /// </summary>
        public IGamePadManager GamePadManager { get; set; }
        /// <summary>
        /// Manager used for mouse input, available in Windows only
        /// </summary>
        public IMouseStateManager MouseManager { get; set; }

        public IAccelerometerHandler AccelerometerHandler { get; set; }

        public ITouchCollectionManager TouchCollectionManager { get; set; }

        public InputHandlerService(Game game, IKeyboardStateManager kbm = null, IMouseStateManager msm = null,
           IGamePadManager gpm = null, IAccelerometerHandler ach = null, ITouchCollectionManager tcm = null) : base(game)
        {
            KeyboardManager = kbm;
            GamePadManager = gpm;
            MouseManager = msm;
            AccelerometerHandler = ach;
            TouchCollectionManager = tcm;
        }

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
