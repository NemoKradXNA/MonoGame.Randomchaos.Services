using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;

namespace MonoGame.Randomchaos.Cross_Platform_Desktop_Template.Scenes
{
    public class GameScene : SceneFadeBase
    {

        /// <summary>   The font. </summary>
        private SpriteFont font;

        /// <summary>   The next scene. </summary>
        protected string NextScene;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public GameScene(Game game, string name, string audioAsset) : base(game, name, audioAsset) { }

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
            if (State == SceneStateEnum.Loaded)
            {
                if (kbManager.KeyPress(Microsoft.Xna.Framework.Input.Keys.Escape))
                    sceneManager.LoadScene("mainMenu");
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

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/UI/Background1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Orange);

            string str = "This is the game play screen, press Esc to go back.";
            Vector2 pos = (new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) - font.MeasureString(str)) / 2;

            _spriteBatch.DrawString(font, str, pos, Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);

            DrawFader(gameTime);
        }
    }
}
