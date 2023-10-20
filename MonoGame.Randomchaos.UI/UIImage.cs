

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;

namespace MonoGame.Randomchaos.UI
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An image. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UIImage : UIBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the texture. </summary>
        ///
        /// <value> The texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D Texture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the background. </summary>
        ///
        /// <value> The background. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D Background { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="position"> The position. </param>
        /// <param name="size">     The size. </param>
        ///-------------------------------------------------------------------------------------------------

        public UIImage(Game game, Point position, Point size) : base(game, position, size)
        {

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
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            Color tint = Tint;

            if (!Enabled)
            {
                tint = GreyScaleColor(Tint);
            }

            // Draw BG
            if (Background != null)
                _spriteBatch.Draw(Background, Rectangle, tint);

            if (Texture != null)
                _spriteBatch.Draw(Texture, Rectangle, tint);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
