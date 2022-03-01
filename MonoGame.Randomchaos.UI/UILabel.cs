

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;

namespace MonoGame.Randomchaos.UI
{
    public class UILabel : UIBase
    {
        public SpriteFont Font { get; set; }
        public string Text { get; set; }

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
                Vector2 m = Font.MeasureString(Text) * .5f;

                tp.Y += (Size.Y / 2) - m.Y;
                tp.X += (Size.X / 2) - m.X;

                return tp;
            }
        }

        public UILabel(Game game) : base(game, Point.Zero, Point.Zero) { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            // Draw BG
            if (!string.IsNullOrEmpty(Text))
            {
                if (ShadowOffset != Vector2.Zero)
                    _spriteBatch.DrawString(Font, Text, TextPosition + ShadowOffset, ShadowColor);
                _spriteBatch.DrawString(Font, Text, TextPosition, Tint);
            }
            _spriteBatch.End();
        }
    }
}
