
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Delegates;

namespace MonoGame.Randomchaos.UI
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A button. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UIButton : UIBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the background texture. </summary>
        ///
        /// <value> The background texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D BackgroundTexture { get; set; }

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the text. </summary>
        ///
        /// <value> The color of the text. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color TextColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the highlight. </summary>
        ///
        /// <value> The color of the highlight. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color HighlightColor { get; set; }

        /// <summary>   The background color. </summary>
        protected Color bgColor;
        /// <summary>   The text color. </summary>
        protected Color txtColor;        

        /// <summary>   Event queue for all listeners interested in OnMouseOver events. </summary>
        public event UIMouseEvent OnMouseOver;
        /// <summary>   Event queue for all listeners interested in OnMouseClick events. </summary>
        public event UIMouseEvent OnMouseClick;
        /// <summary>   Event queue for all listeners interested in OnMouseDown events. </summary>
        public event UIMouseEvent OnMouseDown;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the text position. </summary>
        ///
        /// <value> The text position. </value>
        ///-------------------------------------------------------------------------------------------------

        protected virtual Vector2 TextPosition
        {
            get
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    Vector2 tp = Position.ToVector2();
                    Vector2 m = Font.MeasureString(Text);

                    tp.Y += (Size.Y / 2) - (m.Y * .4f);
                    tp.X += (Size.X / 2) - (m.X * .5f);

                    return tp - TextPositionOffset;
                }
                return Vector2.Zero;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="position"> The position. </param>
        /// <param name="size">     The size. </param>
        ///-------------------------------------------------------------------------------------------------

        public UIButton(Game game, Point position, Point size) : base(game, position, size)
        {
            TextColor = Color.White;
            HighlightColor = Color.White;
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
            if (IsMouseOver && IsTopMost)
            {
                // Mouse over, highlight
                bgColor = HighlightColor;
                txtColor = HighlightColor;

                if (inputManager.MouseManager.LeftClicked)
                {
                    if (OnMouseClick != null)
                        OnMouseClick(this, inputManager.MouseManager);
                }

                if (OnMouseOver != null)
                    OnMouseOver(this, inputManager.MouseManager);

                if (inputManager.MouseManager.LeftButtonDown)
                {
                    if (OnMouseDown != null)
                        OnMouseDown(this, inputManager.MouseManager);
                }
            }
            else
            {
                bgColor = Tint;
                txtColor = TextColor;
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
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            Color colorBG = bgColor;
            Color colorTx = txtColor;

            if (!Enabled)
            {
                colorBG = GreyScaleColor(colorBG);                
                colorTx = GreyScaleColor(colorTx);
            }

            // Draw BG
            _spriteBatch.Draw(BackgroundTexture, Rectangle, colorBG);
            if (!string.IsNullOrEmpty(Text))
                _spriteBatch.DrawString(Font, Text, TextPosition, colorTx);
            _spriteBatch.End();
        }
    }
}
