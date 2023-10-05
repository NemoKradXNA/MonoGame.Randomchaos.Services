using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Coroutine;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Scene.Services;
using Samples.MonoGame.Randomchaos.PostProcessing.Scenes;
using System.Globalization;

namespace Samples.MonoGame.Randomchaos.PostProcessing
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

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //_graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            new InputHandlerService(this, new KeyboardStateManager(this), new MouseStateManager(this));

            // Set up coroutine service
            new CoroutineService(this);

            // Set up our scene service
            new SceneService(this);

            new CameraService(this, .1f, 20000);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");

            sceneService.AddScene(new MainMenuScene(this, "mainMenu"));

            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
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