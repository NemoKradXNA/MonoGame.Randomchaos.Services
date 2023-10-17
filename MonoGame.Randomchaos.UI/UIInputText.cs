
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Delegates;
using MonoGame.Randomchaos.UI.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.UI
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An input text. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UIInputText : UIBase
    {
        public event UIInputCompleteEvent OnUIInputComplete;

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

        public string Text { get; set; } = string.Empty;

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


        /// <summary>   True to capture user input. </summary>
        public bool CaptureUserInput = true;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cursor check. </summary>
        ///
        /// <value> The cursor check. </value>
        ///-------------------------------------------------------------------------------------------------

        public float CursorCheck { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cursor timing. </summary>
        ///
        /// <value> The cursor timing. </value>
        ///-------------------------------------------------------------------------------------------------

        public float CursorTiming { get; set; }

        /// <summary>   The cursor. </summary>
        Texture2D cursor;

        public Color TextColor { get; set; } = Color.Black;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tag. </summary>
        ///
        /// <value> The tag. </value>
        ///-------------------------------------------------------------------------------------------------

        public object? Tag { get; set; }

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

        /// <summary>   The border. </summary>
        protected Texture2D Border;

        /// <summary>   True to enable, false to disable the cursor. </summary>
        private bool _cursorOn = true;

        /// <summary>   True if is selected, false if not. </summary>
        public bool IsSelected;

        public TextInputTypeEnum TextInputType { get; set; } = TextInputTypeEnum.AlphaNumeric;

        public List<Keys> AllowedKeys { get; set; } = null;

        // else if ((int)key >= 48 && (int)key <= 90)
        protected int _minKeyValue
        {
            get
            {
                if (TextInputType == TextInputTypeEnum.AlphaNumeric || TextInputType == TextInputTypeEnum.Numeric)
                {
                    return 48;
                }
                
                return 65;
            }
        }
        protected int _maxKeyValue
        {
            get
            {
                if (TextInputType == TextInputTypeEnum.AlphaNumeric || TextInputType == TextInputTypeEnum.Alpha)
                {
                    return 90;
                }
                return 57;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();

            cursor = new Texture2D(Game.GraphicsDevice, 1, 1);
            cursor.SetData(new Color[] { Color.White });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">             The game. </param>
        /// <param name="position">         The position. </param>
        /// <param name="background">       The background. </param>
        /// <param name="border">           The border. </param>
        /// <param name="textAlingnment">   (Optional) The text alingnment. </param>
        ///-------------------------------------------------------------------------------------------------

        public UIInputText(Game game, Point position, Texture2D background, Texture2D border, TextAlingmentEnum textAlingnment = TextAlingmentEnum.LeftMiddle) : base(game, position, new Point(border.Width, border.Height))
        {
            TextAlingment = textAlingnment;
            Background = background;
            Border = border;

            CursorCheck = .5f;
            CursorTiming = .125f;
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
            if (IsTopMost)
            {
                if (IsMouseOver && inputManager.MouseManager.LeftClicked)
                {
                    IsSelected = true;
                }

                if (!IsMouseOver && inputManager.MouseManager.LeftClicked)
                {
                    IsSelected = false;
                }
            }

            if (IsSelected)
            {
                if (CaptureUserInput)
                {
                    List<Keys> allKeys = inputManager.KeyboardManager.KeysPressed().ToList();
                    List<Keys> keysPressed = inputManager.KeyboardManager.KeysPressedThisFrame().ToList();

                    if (keysPressed != null && keysPressed.Count > 0)
                    {
                        bool ucase = inputManager.KeyboardManager.CapsLock;

                        ucase = !ucase && (allKeys.Contains(Keys.LeftShift) || allKeys.Contains(Keys.RightShift));

                        foreach (Keys key in keysPressed)
                        {
                            if ((key == Keys.Back || key == Keys.Delete) && Text.Length > 0)
                            {
                                Text = Text.Substring(0, Text.Length - 1);
                            }
                            else if (key == Keys.Space)
                            {
                                Text += " ";
                            }
                            else if (key == Keys.Enter)
                            {
                                IsSelected = false;
                                if (OnUIInputComplete != null)
                                {
                                    OnUIInputComplete(this);
                                }
                            }
                            else if (TextInputType == TextInputTypeEnum.AlphaNumeric)
                            {
                                string keyValue = inputManager.KeyboardManager.KeysToString(key);

                                if (!string.IsNullOrEmpty(keyValue))
                                {
                                    if (!ucase)
                                        keyValue = keyValue.ToLower();

                                    Text = Text + keyValue;
                                }
                            }
                            else if (((int)key >= _minKeyValue && (int)key <= _maxKeyValue) || (AllowedKeys != null && AllowedKeys.Contains(key)))
                            {
                                string keyValue = $"{key}";

                                if (AllowedKeys != null && AllowedKeys.Contains(key))
                                {
                                    keyValue = inputManager.KeyboardManager.KeysToString(key);
                                }

                                // Remove numeric prefix
                                if (TextInputType != TextInputTypeEnum.Alpha && keyValue.Length > 1)
                                {
                                    keyValue = keyValue.Replace("D", "");
                                }

                                if (!ucase)
                                    keyValue = keyValue.ToLower();

                                Text = Text + keyValue;
                            }
                        }
                    }
                }
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

            Color tint = Tint;

            if (!Enabled)
            {
                tint = GreyScaleColor(Tint);
            }


            _spriteBatch.Draw(Background, new Rectangle(Position.X, Position.Y, Size.X, Size.Y), tint);

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,null, new RasterizerState() { ScissorTestEnable = true, });
            _spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(Position.X,Position.Y,Background.Width,Background.Height);

            if (!string.IsNullOrEmpty(Text))
            {
                if (ShadowOffset != Vector2.Zero)
                    _spriteBatch.DrawString(Font, Text, TextPosition + ShadowOffset, ShadowColor);
                _spriteBatch.DrawString(Font, Text, TextPosition, TextColor);
            }

            if (IsSelected && _cursorOn)
            {
                int p = (int)Measure.X;
                _spriteBatch.Draw(cursor, new Rectangle((int)TextPosition.X + p + 2, Position.Y + 4, 2, (int)(Size.Y * .75f)), tint);
            }


            _cursorOn = ((gameTime.TotalGameTime.TotalSeconds) % CursorCheck) > CursorTiming;

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            // Draw border
            if (!Enabled)
            {
                tint = GreyScaleColor(Color.White);
            }

            _spriteBatch.Draw(Border, new Rectangle(Position.X, Position.Y, Size.X, Size.Y), tint);

            _spriteBatch.End();
        }
    }
}
