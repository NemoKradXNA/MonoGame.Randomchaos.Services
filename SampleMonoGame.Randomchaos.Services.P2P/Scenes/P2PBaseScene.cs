using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.P2P.Interfaces;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;

namespace SampleMonoGame.Randomchaos.Services.P2P.Scenes
{
    public abstract class P2PBaseScene : SceneFadeBase
    {
        protected IP2PService p2pService { get { return Game.Services.GetService<IP2PService>(); } }

        /// <summary>   The font. </summary>
        protected SpriteFont font;
        /// <summary>   The button font. </summary>
        protected SpriteFont buttonFont;

        public P2PBaseScene(Game game, string name):base(game, name) { }

        

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            DrawFader(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a button. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
        ///
        /// <param name="text">     The text. </param>
        /// <param name="bgTeture"> The background teture. </param>
        /// <param name="pos">      The position. </param>
        /// <param name="size">     The size. </param>
        ///
        /// <returns>   The new button. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected UIButton CreateButton(string text, Texture2D bgTeture, Point pos, Point size)
        {
            var btn = new UIButton(Game, pos, size)
            {
                BackgroundTexture = bgTeture,
                Font = buttonFont,
                Text = text,
                HighlightColor = Color.SkyBlue,
                Segments = new Rectangle(8, 8, 8, 8),
                TextColor = Color.DarkSlateGray,
            };

            btn.OnMouseClick += Btn_OnMouseClick;

            return btn;
        }

        protected abstract void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState);
    }
}
