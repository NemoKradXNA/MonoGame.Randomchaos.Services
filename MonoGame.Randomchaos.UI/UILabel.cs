

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;

namespace MonoGame.Randomchaos.UI
{
    public class UILabel : UIBase
    {
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Vector2 TextPositionOffset { get; set; }

        public Texture2D Background;

        public TextAlingmentEnum TextAlingment { get; set; }

        public Vector2 ShadowOffset { get; set; }
        public Color ShadowColor { get; set; }

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

        public UILabel(Game game) : base(game, Point.Zero, Point.Zero) { TextAlingment = TextAlingmentEnum.Middle; }

        public override void Update(GameTime gameTime)
        {
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
        }
    }
}
