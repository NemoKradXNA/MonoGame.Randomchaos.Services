using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Delegates;

namespace MonoGame.Randomchaos.UI
{
    public class UIButton : UIBase
    {
        public Texture2D BackgroundTexture { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Vector2 TextPositionOffset { get; set; }
        public Color TextColor { get; set; }
        public Color HighlightColor { get; set; }

        protected Color bgColor;
        protected Color txtColor;

        public event UIMouseEvent OnMouseOver;
        public event UIMouseEvent OnMouseClick;
        public event UIMouseEvent OnMouseDown;

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


        public UIButton(Game game, Point position, Point size) : base(game, position, size)
        {
            TextColor = Color.White;
            HighlightColor = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (IsMouseOver)
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
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            // Draw BG
            _spriteBatch.Draw(BackgroundTexture, Rectangle, bgColor);
            if (!string.IsNullOrEmpty(Text))
                _spriteBatch.DrawString(Font, Text, TextPosition, txtColor);
            _spriteBatch.End();
        }
    }
}
