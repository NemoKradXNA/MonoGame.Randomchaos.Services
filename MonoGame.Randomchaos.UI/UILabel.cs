


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;

namespace MonoGame.Randomchaos.UI
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A label. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UILabel : UIBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the font. </summary>
        ///
        /// <value> The font. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteFont Font { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the text. </summary>
        ///
        /// <value> The text. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Text { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the text position offset. </summary>
        ///
        /// <value> The text position offset. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 TextPositionOffset { get; set; }

        /// <summary>   The background. </summary>
        public Texture2D Background;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the text alingment. </summary>
        ///
        /// <value> The text alingment. </value>
        ///-------------------------------------------------------------------------------------------------

        public TextAlingmentEnum TextAlingment { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the shadow offset. </summary>
        ///
        /// <value> The shadow offset. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 ShadowOffset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the shadow. </summary>
        ///
        /// <value> The color of the shadow. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color ShadowColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the measure. </summary>
        ///
        /// <value> The measure. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 Measure
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                    return Vector2.Zero;
                return Font.MeasureString(Text);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the text position. </summary>
        ///
        /// <value> The text position. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Vector2 TextPosition
        {
            get
            {
                Vector2 tp = Position.ToVector2();
                Vector2 m = Font.MeasureString(Text);

                switch (TextAlingment)
                {
                    case TextAlingmentEnum.LeftBottom:
                        tp.Y = Size.Y - m.Y;
                        break;
                    case TextAlingmentEnum.LeftMiddle:
                        tp.Y += (Size.Y / 2) - m.Y * .5f;
                        break;
                    case TextAlingmentEnum.LeftTop:
                        break;
                    case TextAlingmentEnum.Middle:
                        tp.Y += (Size.Y / 2) - m.Y * .4f;
                        tp.X += (Size.X / 2) - m.X * .5f;
                        break;
                    case TextAlingmentEnum.MiddleBottom:
                        tp.Y = Size.Y - m.Y;
                        tp.X += (Size.X / 2) - m.X * .5f;
                        break;
                    case TextAlingmentEnum.MiddleTop:
                        tp.X += (Size.X / 2) - m.X * .5f;
                        break;
                    case TextAlingmentEnum.RightBottom:
                        tp.Y = Size.Y - m.Y;
                        tp.X = Size.X - m.X;
                        break;
                    case TextAlingmentEnum.RightMidle:
                        tp.Y += (Size.Y / 2) - m.Y * 4;
                        tp.X = Size.X - m.X;
                        break;
                    case TextAlingmentEnum.RightTop:
                        tp.X = Size.X - m.X;
                        break;
                }

                return tp + TextPositionOffset;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public UILabel(Game game) : base(game, Point.Zero, Point.Zero) { TextAlingment = TextAlingmentEnum.Middle; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
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
            _spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState);

            Color tint = Tint;

            if (!Enabled)
            {
                tint = GreyScaleColor(Tint);
            }

            // Draw BG
            if (Background != null)
            {
                tint = Color.White;

                if (!Enabled)
                {
                    tint = GreyScaleColor(Color.White);
                }

                _spriteBatch.Draw(Background, new Rectangle(Position.X, Position.Y, Size.X, Size.Y), tint);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                if (ShadowOffset != Vector2.Zero)
                    _spriteBatch.DrawString(Font, Text, TextPosition + ShadowOffset, ShadowColor);
                _spriteBatch.DrawString(Font, Text, TextPosition, tint);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
