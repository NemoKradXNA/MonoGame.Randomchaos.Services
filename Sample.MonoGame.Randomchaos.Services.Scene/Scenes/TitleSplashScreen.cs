using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using System.Collections;

namespace Sample.MonoGame.Randomchaos.Services.Scene.Scenes
{
    public class TitleSplashScreen : SceneFadeBase
    {

        private SpriteFont font;
        float waitSeconds = 5;
        bool waiting = false;

        protected string NextScene;

        public TitleSplashScreen(Game game, string name, string nextScene) : base(game, name) { NextScene = nextScene; }


        public override void Initialize()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");
            base.Initialize();
        }

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

        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap);

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/BG1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.LightCoral);

            string str = $"This is the title screen, press any key or click the mouse to exit, or wait {waitSeconds} seconds.";
            Vector2 pos = (new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) - font.MeasureString(str)) / 2;

            _spriteBatch.DrawString(font, str, pos + new Vector2(-1,1), Color.Black);
            _spriteBatch.DrawString(font, str, pos, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);

            DrawFader(gameTime);
        }

        protected IEnumerator WaitSecondsAndExit(float seconds)
        {
            waiting = true;
            yield return new WaitForSeconds(Game, seconds);

            if (State == SceneStateEnum.Loaded)
                sceneManager.LoadScene(NextScene);

        }
    }
}
