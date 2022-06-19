using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.UI
{
    public class UIInputText : UIBase
    {

        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Vector2 TextPositionOffset { get; set; }

        public Texture2D Background;

        public TextAlingmentEnum TextAlingment { get; set; }

        public Vector2 ShadowOffset { get; set; }
        public Color ShadowColor { get; set; }


        public bool CaptureUserInput = true;

        Texture2D cursor;

        public Vector2 Measure
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                    return Vector2.Zero;
                return Font.MeasureString(Text);
            }
        }

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

        protected Texture2D Border;

        private bool _cursorOn = true;

        public bool IsSelected;

        public override void Initialize()
        {
            base.Initialize();

            cursor = new Texture2D(Game.GraphicsDevice, 1, 1);
            cursor.SetData(new Color[] { Color.White });
        }

        public UIInputText(Game game, Point position, Texture2D background, Texture2D border, TextAlingmentEnum textAlingnment = TextAlingmentEnum.LeftMiddle) : base(game, position, new Point(border.Width, border.Height))
        {
            TextAlingment = textAlingnment;
            Background = background;
            Border = border;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsMouseOver && inputManager.MouseManager.LeftClicked)
            {
                IsSelected = true;
            }

            if (IsSelected)
            {
                if (CaptureUserInput)
                {
                    List<Keys> keysPressed = inputManager.KeyboardManager.KeysPressedThisFrame().ToList();

                    if (keysPressed != null && keysPressed.Count > 0)
                    {
                        bool ucase = inputManager.KeyboardManager.CapsLock;

                        foreach (Keys key in keysPressed)
                        {
                            if (key == Keys.LeftShift || key == Keys.RightShift)
                                ucase = true;

                            if (key == Keys.Back || key == Keys.Delete && Text.Length > 0)
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
                            }
                            else if ((int)key >= 65 && (int)key <= 90)
                            {
                                Text = Text + key;
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            Color tint = Tint;

            if (!Enabled)
            {
                tint = GreyScaleColor(Tint);
            }

            // Draw BG
            tint = Color.White;

            if (!Enabled)
            {
                tint = GreyScaleColor(Color.White);
            }

            _spriteBatch.Draw(Background, new Rectangle(Position.X, Position.Y, Size.X, Size.Y), tint);

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,null, new RasterizerState() { ScissorTestEnable = true, });
            _spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(Position.X,Position.Y,Background.Width,Background.Height);

            if (!string.IsNullOrEmpty(Text))
            {
                if (ShadowOffset != Vector2.Zero)
                    _spriteBatch.DrawString(Font, Text, TextPosition + ShadowOffset, ShadowColor);
                _spriteBatch.DrawString(Font, Text, TextPosition, tint);
            }

            if (IsSelected && _cursorOn)
            {
                int p = (int)Measure.X;
                _spriteBatch.Draw(cursor, new Rectangle((int)TextPosition.X + p + 2, Position.Y + 4, 2, (int)(Size.Y * .75f)), tint);
            }

            _cursorOn = !_cursorOn;

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
