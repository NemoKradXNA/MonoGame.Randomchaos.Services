using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using System.Collections;

namespace MonoGame.Randomchaos.Windows_Desktop_Template.Scenes
{
    public class SplashScene : SceneFadeBase
    {

        /// <summary>   The font. </summary>
        private SpriteFont font;
        /// <summary>   The wait in seconds. </summary>
        float waitSeconds = 3;
        /// <summary>   True to waiting. </summary>
        bool waiting = false;

        /// <summary>   The next scene. </summary>
        protected string NextScene;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">         The game. </param>
        /// <param name="name">         The name. </param>
        /// <param name="nextScene">    The next scene. </param>
        ///-------------------------------------------------------------------------------------------------

        public SplashScene(Game game, string name, string nextScene, string audioAsset) : base(game, name, audioAsset) { NextScene = nextScene; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadScene()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");

            base.LoadScene();
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
            if (State == SceneStateEnum.Loaded && !waiting)
            {
                coroutineService.StartCoroutine(WaitSecondsAndExit(waitSeconds));
            }

            if (State == SceneStateEnum.Loaded && (kbManager.KeysPressed().Length > 0 || msManager.LeftButtonDown || msManager.RightButtonDown))
            {
                sceneManager.LoadScene(NextScene);
            }

            base.Update(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Silver);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap);

            Point imgSize = new Point(256, 256);

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Logo/CompanyLogo"),
                new Rectangle((GraphicsDevice.Viewport.Width / 2) - (imgSize.X / 2), (GraphicsDevice.Viewport.Height / 2) - (imgSize.Y / 2), 
                imgSize.X, imgSize.Y), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);

            DrawFader(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wait seconds and exit. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="seconds">  The seconds. </param>
        ///
        /// <returns>   An IEnumerator. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected IEnumerator WaitSecondsAndExit(float seconds)
        {
            waiting = true;
            yield return new WaitForSeconds(Game, seconds);

            if (State == SceneStateEnum.Loaded)
                sceneManager.LoadScene(NextScene);

        }
    }
}
