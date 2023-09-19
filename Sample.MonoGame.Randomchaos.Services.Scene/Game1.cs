
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Audio;
using MonoGame.Randomchaos.Services.Coroutine;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Scene.Services;
using Sample.MonoGame.Randomchaos.Services.Scene.Scenes;
using System.Globalization;

// Images for this sample are public domain and sourced here: https://www.publicdomainpictures.net/en/view-image.php?image=349541&picture=bokeh-background

namespace Sample.MonoGame.Randomchaos.Services.Scene
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A game 1. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Game1 : Game
    {
        /// <summary>   The graphics. </summary>
        private GraphicsDeviceManager _graphics;
        /// <summary>   The sprite batch. </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>   State of the kB. </summary>
        IKeyboardStateManager kbState;
        /// <summary>   The state. </summary>
        IMouseStateManager mState;
        /// <summary>   The input service. </summary>
        IInputStateService inputService;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the coroutine service. </summary>
        ///
        /// <value> The coroutine service. </value>
        ///-------------------------------------------------------------------------------------------------

        ICoroutineService coroutineService { get { return Services.GetService<ICoroutineService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scene service. </summary>
        ///
        /// <value> The scene service. </value>
        ///-------------------------------------------------------------------------------------------------

        ISceneService sceneService { get { return Services.GetService<ISceneService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set up coroutine service
            new CoroutineService(this);

            // Setup audio
            new AudioService(this);

            // Set up our scene service
            new SceneService(this);

            // Set up user input hadlers
            kbState = new KeyboardStateManager(this);
            mState = new MouseStateManager(this);

            inputService = new InputHandlerService(this, kbState, mState);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");


            // add our scenes..
            sceneService.AddScene(new SplashScene(this, "splashScene", "titeSplash"));
            sceneService.AddScene(new TitleSplashScreen(this, "titeSplash", "mainMenu"));
            sceneService.AddScene(new MainMenuScene(this, "mainMenu"));
            sceneService.AddScene(new OptionsScene(this, "optionsScene"));
            sceneService.AddScene(new GameScene(this, "gameScene"));

            base.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            sceneService.LoadScene("splashScene");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Update(GameTime gameTime)
        {
            
            // TODO: Add your update logic here
            inputService.PreUpdate(gameTime);
            base.Update(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ends a draw. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void EndDraw()
        {
            base.EndDraw();

            coroutineService.UpdateEndFrame(null);
        }
    }
}
