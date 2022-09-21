using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.UI
{
    public class UISwitch : UIButton
    {
        public event UIMouseEvent OnMouseClickOn;
        public event UIMouseEvent OnMouseClickOff;

        public Color BorderColor { get; set; }
        public Color ButtonColor { get; set; }
        public Color OnColor { get; set; }
        public Color OffColor { get; set; }
        public string OffText { get; set; }
        public string OnText { get; set; }

        public bool IsOn { get; set; }

        public Texture2D SwitchBorder { get; set; }
        public Texture2D SwitchOn { get; set; }
        public Texture2D SwitchOff { get; set; }

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

        public UISwitch(Game game, Point position, Point size) : base(game, position, size)
        {
            ButtonColor = Color.White;
            OnColor = Color.Olive;
            OffColor = Color.Silver;
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (IsMouseOver && TopMostMouseOver == this)
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
