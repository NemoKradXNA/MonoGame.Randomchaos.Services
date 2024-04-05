
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Delegates;
using System;

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
        /// <summary>   Gets or sets the text shadow. </summary>
        ///
        /// <value> The text shadow. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 TextShadow { get; set; } = Vector2.Zero;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the text shadow. </summary>
        ///
        /// <value> The color of the text shadow. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color TextShadowColor { get; set; } = Color.Black;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the button shadow. </summary>
        ///
        /// <value> The button shadow. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector2 ButtonShadow { get; set; } = Vector2.Zero;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the button shadow. </summary>
        ///
        /// <value> The color of the button shadow. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color ButtonShadowColor { get; set; } = Color.Black;

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
        /// <summary>   Gets or sets the segments. </summary>
        ///
        /// <value> The segments. </value>
        ///-------------------------------------------------------------------------------------------------

        public Rectangle? Segments { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the highlight. </summary>
        ///
        /// <value> The color of the highlight. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color HighlightColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the text highlight. </summary>
        ///
        /// <value> The color of the text highlight. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color TextHighlightColor { get; set; }

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
        /// <summary>   Gets or sets the tag. </summary>
        ///
        /// <value> The tag. </value>
        ///-------------------------------------------------------------------------------------------------

        public object? Tag { get; set; }

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
            TextHighlightColor = Color.White;
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
            if (IsMouseOver)// && IsTopMost)
            {
                // Mouse over, highlight
                bgColor = HighlightColor;
                txtColor = TextHighlightColor;
                

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
            _spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState);

            Color colorBG = bgColor;
            Color colorTx = txtColor;

            if (!Enabled)
            {
                colorBG = GreyScaleColor(colorBG);                
                colorTx = GreyScaleColor(colorTx);
            }

            // Draw BG

            // This is really messy, need to have a clean up, but it seems to work, for now :P
            if (Segments != null)
            {
                if (ButtonShadow != Vector2.Zero)
                {
                    DrawSegmentedBackground(BackgroundTexture, Segments, ButtonShadowColor, ButtonShadow.ToPoint());
                }

                DrawSegmentedBackground(BackgroundTexture, Segments, colorBG, Point.Zero);
            }
            else
            {
                if (ButtonShadow != Vector2.Zero)
                {
                    Rectangle shdowRect = Rectangle;
                    shdowRect.X += (int)ButtonShadow.X;
                    shdowRect.Y += (int)ButtonShadow.Y;

                    DrawBackgroundElement(BackgroundTexture, shdowRect, null, ButtonShadowColor);
                }

                DrawBackgroundElement(BackgroundTexture, Rectangle, null, colorBG);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                if (TextShadow != Vector2.Zero)
                {
                    _spriteBatch.DrawString(Font, Text, TextPosition + TextShadow, TextShadowColor);
                }

                _spriteBatch.DrawString(Font, Text, TextPosition, colorTx);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
