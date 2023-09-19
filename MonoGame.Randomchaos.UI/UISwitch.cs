
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.Delegates;

namespace MonoGame.Randomchaos.UI
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A switch. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UISwitch : UIButton
    {
        /// <summary>   Event queue for all listeners interested in OnMouseClickOn events. </summary>
        public event UIMouseEvent OnMouseClickOn;
        /// <summary>   Event queue for all listeners interested in OnMouseClickOff events. </summary>
        public event UIMouseEvent OnMouseClickOff;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the border. </summary>
        ///
        /// <value> The color of the border. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color BorderColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the button. </summary>
        ///
        /// <value> The color of the button. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color ButtonColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the on. </summary>
        ///
        /// <value> The color of the on. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color OnColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the off. </summary>
        ///
        /// <value> The color of the off. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color OffColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the off text. </summary>
        ///
        /// <value> The off text. </value>
        ///-------------------------------------------------------------------------------------------------

        public string OffText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the on text. </summary>
        ///
        /// <value> The on text. </value>
        ///-------------------------------------------------------------------------------------------------

        public string OnText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is on. </summary>
        ///
        /// <value> True if this object is on, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsOn { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the switch border. </summary>
        ///
        /// <value> The switch border. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D SwitchBorder { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the switch on. </summary>
        ///
        /// <value> The switch on. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D SwitchOn { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the switch off. </summary>
        ///
        /// <value> The switch off. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D SwitchOff { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the text position. </summary>
        ///
        /// <value> The text position. </value>
        ///-------------------------------------------------------------------------------------------------

        protected override Vector2 TextPosition
        {
            get
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    Vector2 tp = Position.ToVector2();
                    Vector2 m = Font.MeasureString(Text) * .5f;

                    tp.Y += (Size.Y / 2) - m.Y;

                    if (IsOn)
                        tp.X += 8;
                    else
                        tp.X += Size.X - (m.X * 2) - 8;

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

        public UISwitch(Game game, Point position, Point size) : base(game, position, size)
        {
            ButtonColor = Color.White;
            OnColor = Color.Olive;
            OffColor = Color.Silver;
            
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
            base.Update(gameTime);


            if (IsMouseOver && IsTopMost)
            {
                // Mouse over, highlight
                bgColor = HighlightColor;
                txtColor = HighlightColor;

                if (inputManager.MouseManager.LeftClicked)
                {
                    IsOn = !IsOn;

                    if (IsOn && OnMouseClickOn != null)
                        OnMouseClickOn(this, inputManager.MouseManager);

                    if (!IsOn && OnMouseClickOff != null)
                        OnMouseClickOff(this, inputManager.MouseManager);
                }
            }
            else
            {
                bgColor = Tint;
                txtColor = TextColor;
            }
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


            if (IsOn)
            {
                Text = OnText;
                bgColor = OnColor;
            }
            else
            {
                Text = OffText;
                bgColor = OffColor;
            }

            _spriteBatch.Draw(BackgroundTexture, Rectangle, bgColor);

            if(SwitchBorder != null)
                _spriteBatch.Draw(SwitchBorder, Rectangle, BorderColor);
            else
                _spriteBatch.Draw(SwitchBorder, Rectangle, BorderColor);

            if (IsOn)
                _spriteBatch.Draw(SwitchOn, Rectangle, ButtonColor);
            else
                _spriteBatch.Draw(SwitchOff, Rectangle, ButtonColor);

            if (!string.IsNullOrEmpty(Text))
                _spriteBatch.DrawString(Font, Text, TextPosition, txtColor);
            _spriteBatch.End();
        }
    }
}
