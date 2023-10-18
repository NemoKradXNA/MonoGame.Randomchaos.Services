using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Audio;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Coroutine;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Scene.Services;
using SampleMonoGame.Randomchaos.Services.P2P.Scenes;
using SampleMonoGame.Randomchaos.Services.P2P.Services;
using System.Globalization;

namespace SampleMonoGame.Randomchaos.Services.P2P
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>   The input service. </summary>
        IInputStateService inputService;
        /// <summary>   State of the kB. </summary>
        IKeyboardStateManager kbState;
        /// <summary>   The state. </summary>
        IMouseStateManager mState;

        ICoroutineService coroutineService { get { return Services.GetService<ICoroutineService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scene service. </summary>
        ///
        /// <value> The scene service. </value>
        ///-------------------------------------------------------------------------------------------------

        ISceneService sceneService { get { return Services.GetService<ISceneService>(); } }

        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            kbState = new KeyboardStateManager(this);
            mState = new MouseStateManager(this);
            inputService = new InputHandlerService(this, kbState, mState);

            // Set up P2P service
            new P2PService(this, null); // If you are playing over the WAN, you need to put your external IP here if you are a client.

            // Set up coroutine service
            new CoroutineService(this);

            // Setup audio
            new AudioService(this);

            // Set up our scene service
            new SceneService(this);

            new CameraService(this, .1f, 20000);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");

            sceneService.AddScene(new MainMenuScene(this, "mainMenu"));
            sceneService.AddScene(new ServerStartScene(this, "serverStartScene"));
            sceneService.AddScene(new ClientStartScene(this, "clientStartScene"));
            sceneService.AddScene(new GameLobyScene(this, "lobyScene"));

            //_graphics.IsFullScreen = true;
            //_graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            sceneService.LoadScene("mainMenu");
        }

        protected override void Update(GameTime gameTime)
        {
            inputService.PreUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            base.EndDraw();

            coroutineService.UpdateEndFrame(null);
        }
    }
}