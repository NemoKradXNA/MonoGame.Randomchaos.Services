using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        IKeyboardStateManager kbState;
        IMouseStateManager mState;
        IInputStateService inputService;
        ICoroutineService coroutineService { get { return Services.GetService<ICoroutineService>(); } }

        ISceneService sceneService { get { return Services.GetService<ISceneService>(); } }

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

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            sceneService.LoadScene("splashScene");
        }

        protected override void Update(GameTime gameTime)
        {
            
            // TODO: Add your update logic here
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
