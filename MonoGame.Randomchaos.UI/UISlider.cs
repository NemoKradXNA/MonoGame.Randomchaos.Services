using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.UI.BaseClasses;
using System;

namespace MonoGame.Randomchaos.UI
{
    public class UISlider : UIBase
    {
        UIImage imgBar;
        UIButton btnButton;

        UILabel lblLabel;

        public SpriteFont Font
        {
            get { return lblLabel.Font; }
            set
            {
                lblLabel.Font = value;
            }
        }

        public string Label { get { return lblLabel.Text; } set { lblLabel.Text = value; } }

        public Texture2D BarTexture { get { return imgBar.Texture; } set { imgBar.Texture = value; } }


        public Texture2D SliderTexture
        {
            get { return btnButton.BackgroundTexture; }
            set
            {
                btnButton.BackgroundTexture = value;
            }
        }

        public UISlider(Game game, Point position, Point size) : base(game, position, size)
        {
            lblLabel = new UILabel(Game);

            imgBar = new UIImage(Game, Position, new Point(Size.X, 4));

            btnButton = new UIButton(Game, Position, new Point(32, 32));

            btnButton.OnMouseDown += btnOnMouseDown;
        }

        bool dragging;
        private void btnOnMouseDown(IUIBase sender, IMouseStateManager mouseState)
        {
            dragging = true;
        }

        protected float _Value;
        public float Value
        {
            get { return _Value; }
            set
            {
                _Value = value;

                // Lerp button position.
                int x = (int)MathHelper.Lerp(imgBar.Position.X, (imgBar.Position.X + imgBar.Size.X) - btnButton.Size.X, _Value);

                btnButton.Position = new Point(x, btnButton.Position.Y);
            }
        }

        public Color SliderColor { get { return btnButton.Tint; } set { btnButton.Tint = value; } }
        public Color SliderHoverColor { get { return btnButton.HighlightColor; } set { btnButton.HighlightColor = value; } }

        public override void Initialize()
        {

            base.Initialize();
            lblLabel.Initialize();
            imgBar.Initialize();
            btnButton.Initialize();

            Point m = lblLabel.Font.MeasureString(lblLabel.Text).ToPoint();
            lblLabel.Position = new Point(Position.X, Position.Y + (m.Y / 2));

            imgBar.Size = new Point(Size.X - (lblLabel.Size.X + 16), 4);
            imgBar.Position = new Point(lblLabel.Position.X + 16 + m.X / 2, lblLabel.Position.Y - 2);

            btnButton.Position = new Point(imgBar.Position.X, imgBar.Position.Y - btnButton.Size.Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            lblLabel.Update(gameTime);
            imgBar.Update(gameTime);
            btnButton.Update(gameTime);

            if (dragging)
            {
                float xDelta = -inputManager.MouseManager.PositionDelta.X;
                Value = Math.Max(0, Math.Min(1, invLerp(imgBar.Position.X, (imgBar.Position.X + imgBar.Size.X) - btnButton.Size.X, btnButton.Position.X + xDelta)));
            }

            if (!inputManager.MouseManager.LeftButtonDown || !btnButton.IsMouseOver)
                dragging = false;
        }

        protected float lerp(float s, float e, float v)
        {
            return s * (1 - v) + e * v;
        }

        protected float invLerp(float s, float e, float v)
        {
            return (v - s) / (e - s);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            lblLabel.Draw(gameTime);
            imgBar.Draw(gameTime);
            btnButton.Draw(gameTime);
        }
    }
}
