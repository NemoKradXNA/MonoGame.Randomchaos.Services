using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;

namespace Sample.MonoGame.Randomchaos.Services.Scene.Scenes
{
    public class OptionsScene : SceneFadeBase
    {

        private SpriteFont font;

        protected string NextScene;

        public OptionsScene(Game game, string name) : base(game, name) { }


        public override void Initialize()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (kbManager.KeyPress(Microsoft.Xna.Framework.Input.Keys.Escape))
                    sceneManager.LoadScene("mainMenu");
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap);

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/BG1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.LimeGreen);

            string str = "This is the options screen, press Esc to go back.";
            Vector2 pos = (new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) - font.MeasureString(str)) / 2;

            _spriteBatch.DrawString(font, str, pos, Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);

            DrawFader(gameTime);
        }
    }
}
