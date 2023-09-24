
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Delegates;
using System;
using System.ComponentModel.Design;
using System.Threading;

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

            // This is really messy, need to have a clean up, but it seems to work, for now :P
            if (Segments != null)
            {

                // texture is 64x64, we are rendering it 32x16, the segments are 8,8,8,8
                // ratio = (32,16) / (64,64) = (.5,.25)
                // TL src = 0,0,8,8 dest = 0,0, 8 * ration.X, 8 * ratio.Y = 4,2
                // *---------------------*
                // |                     |
                // |                     |
                // |                     |
                // |                     |
                // |                     |
                // *---------------------*
                Rectangle seg = Segments.Value;
                Vector2 ratio = new Vector2(Rectangle.Width,Rectangle.Height) / new Vector2(BackgroundTexture.Width,BackgroundTexture.Height);

                // Top Left
                float tlWidth = seg.Left * ratio.X;
                float tlHeight = seg.Top * ratio.Y;

                Rectangle srect = new Rectangle(0, 0, seg.Left, seg.Top);
                Rectangle dest = new Rectangle(Rectangle.Left, Rectangle.Top, (int)Math.Round(tlWidth), (int)Math.Round(tlHeight));

                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);
                // Top
                float tWidth = Rectangle.Width - (tlWidth * 2);

                srect = new Rectangle(seg.Left, 0, BackgroundTexture.Width - seg.Right, seg.Top);
                dest = new Rectangle(Rectangle.Left + (int)Math.Round(tlWidth), Rectangle.Top, (int)Math.Round(tWidth), (int)Math.Round(tlHeight));

                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);
                // Top right
                float trWidth = seg.Left * ratio.X;
                float trHeight = seg.Top * ratio.Y;

                srect = new Rectangle(BackgroundTexture.Width - (seg.Right - seg.Left), 0, (seg.Right - seg.Left), seg.Top);
                dest = new Rectangle(Rectangle.Right - (int)Math.Round(trWidth), Rectangle.Top, (int)Math.Round(trWidth), (int)Math.Round(trHeight));

                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);
                // Left
                float lHeight = Rectangle.Height - (tlHeight * 2);
                srect = new Rectangle(0, seg.Top, seg.Left, BackgroundTexture.Height - seg.Bottom);
                dest = new Rectangle(Rectangle.Left, Rectangle.Top + (int)Math.Round(tlHeight), (int)Math.Round(tlWidth), (int)Math.Round(lHeight));

                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);

                // Middle
                srect = new Rectangle(seg.Left,seg.Top, BackgroundTexture.Width-seg.Right,BackgroundTexture.Height - seg.Bottom);
                dest = new Rectangle(Rectangle.Left + (int)Math.Round(tlWidth), Rectangle.Top + (int)Math.Round(tlHeight), (int)Math.Round(tWidth), (int)Math.Round(lHeight));
                
                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);
                // Right
                float rHeight = Rectangle.Height - (trHeight * 2);

                srect = new Rectangle(BackgroundTexture.Width - (seg.Right - seg.Left), seg.Top, (seg.Right - seg.Left), BackgroundTexture.Height - seg.Bottom );
                dest = new Rectangle(Rectangle.Right - (int)Math.Round(trWidth), Rectangle.Top + (int)Math.Round(trHeight), (int)Math.Round(trWidth), (int)Math.Round(rHeight));

                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);

                // Bottom Left
                float blWidth = seg.Left * ratio.X;
                float blHeight = (seg.Bottom - seg.Top) * ratio.Y;

                srect = new Rectangle(0, BackgroundTexture.Height - (seg.Bottom - seg.Top), seg.Left, (seg.Bottom - seg.Top));
                dest = new Rectangle(Rectangle.Left, Rectangle.Bottom - (int)Math.Round(blHeight), (int)Math.Round(blWidth), (int)Math.Round(blHeight));

                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);
                // Bottom 
                float bWidth = Rectangle.Width - (trWidth * 2);

                srect = new Rectangle(seg.Left, BackgroundTexture.Height - (seg.Bottom-seg.Top), BackgroundTexture.Width - seg.Right, seg.Bottom - seg.Top);
                dest = new Rectangle(Rectangle.Left + (int)Math.Round(blWidth), Rectangle.Bottom - (int)Math.Round(blHeight), (int)Math.Round(bWidth), (int)Math.Round(blHeight));

                _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);

                // Bottom Right
                float brWidth = (seg.Right - seg.Left) * ratio.X;
                float brHeight = (seg.Bottom - seg.Top) * ratio.Y;

                srect = new Rectangle(BackgroundTexture.Width - (seg.Right - seg.Left), BackgroundTexture.Height - (seg.Bottom - seg.Top), seg.Right - seg.Left, (seg.Bottom - seg.Top));
                dest = new Rectangle(Rectangle.Right - (int)Math.Round(brWidth), Rectangle.Bottom - (int)Math.Round(brHeight), (int)Math.Round(brWidth), (int)Math.Round(brHeight));

               _spriteBatch.Draw(BackgroundTexture, dest, srect, colorBG);
            }
            else
            {
                _spriteBatch.Draw(BackgroundTexture, Rectangle, colorBG);
            }

            if (!string.IsNullOrEmpty(Text))
                _spriteBatch.DrawString(Font, Text, TextPosition, colorTx);
            _spriteBatch.End();
        }
    }
}
