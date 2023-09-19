
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using System.Collections;

namespace Sample.MonoGame.Randomchaos.Services.Scene.Scenes
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A splash scene. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

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

        public SplashScene(Game game, string name, string nextScene) : base(game, name) { NextScene = nextScene; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");
            base.Initialize();
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

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap);

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/BG1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            string str = $"This is a splash screen, press any key or click the mouse to exit, or wait {waitSeconds} seconds.";
            Vector2 pos = (new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) - font.MeasureString(str)) / 2;
            
            _spriteBatch.DrawString(font, str, pos, Color.Black);

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
