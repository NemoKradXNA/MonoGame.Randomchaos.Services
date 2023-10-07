using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Audio;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Coroutine;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Scene.Services;
using System.Globalization;
using Test.Scenes;

namespace Test
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        IInputStateService inputService { get { return Services.GetService<IInputStateService>(); } }
        ICoroutineService coroutineService { get { return Services.GetService<ICoroutineService>(); } }
        ISceneService sceneService { get { return Services.GetService<ISceneService>(); } }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set up input servide
            new InputHandlerService(this, new KeyboardStateManager(this), new MouseStateManager(this));

            // Set up coroutine service
            new CoroutineService(this);

            // Setup audio
            new AudioService(this);

            // Set up our scene service
            new SceneService(this);

            // Set up camera service
            new CameraService(this, .1f, 20000);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");


            // add our scenes..
            sceneService.AddScene(new SplashScene(this, "splashScene", "titeSplash", "Audio/Music/CompanyJingle"));
            sceneService.AddScene(new TitleSplashScreen(this, "titeSplash", "mainMenu", "Audio/Music/GameJingle"));
            sceneService.AddScene(new MainMenuScene(this, "mainMenu", "Audio/Music/DemoMenu"));
            sceneService.AddScene(new OptionsScene(this, "optionsScene"));
            sceneService.AddScene(new GameScene(this, "gameScene", "Audio/Music/GameMusic"));

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