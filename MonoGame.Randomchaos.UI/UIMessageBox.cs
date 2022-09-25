using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Delegates;
using MonoGame.Randomchaos.UI.Enums;
using System;


namespace MonoGame.Randomchaos.UI
{
    public class UIMessageBox : UIBase
    {
        protected UIButton btnPositive;
        protected UIButton btnNegative;
        protected UIButton btnRetry;

        protected UILabel lblMessage;

        protected Texture2D backSplash;

        public Texture2D TitleBackground { get; set; }
        public Texture2D Background { get; set; }
        public Texture2D ButtonBackground { get; set; }
        public SpriteFont ButtonFont { get; set; }
        public SpriteFont TitleFont { get; set; }
        public string Title { get; set; }
        public Color TitleShadowColor { get; set; }

        public SpriteFont MessageFont { get; set; }
        public string Message { get; set; }

        private int btnPXOffset;
        private int btnHeight;

        public bool ShowRetryBtn { get; set; }
        public bool ShowPositiveButton { get; set; }
        public bool ShowNegativeButton { get; set; }

        public string PositiveText { get; set; }
        public string NegativeText { get; set; }
        public string RetryText { get; set; }

        public event UIMouseEvent OnPositiveClicked;
        public event UIMouseEvent OnNegativeClicked;
        public event UIMouseEvent OnRetryClicked;


        public UIMessageBox(Game game, Point position, Point size) : base(game, position, size)
        {
            TitleShadowColor = Color.Black;

            ShowRetryBtn = ShowPositiveButton = ShowNegativeButton = true;

            PositiveText = "Yes";
            NegativeText = "No";
            RetryText = "Retry";
        }

        public override void Initialize()
        {
            backSplash = new Texture2D(Game.GraphicsDevice, 1, 1);
            backSplash.SetData(new Color[] { new Color(1, 1, 1, .75f) });

            btnPXOffset = (Size.X / 4) - 8;
            btnHeight = Size.X / 8;

            Point btnPos = Position + new Point(8, btnPXOffset);
            Point btnSize = new Point(btnPXOffset, btnHeight);

            btnPositive = new UIButton(Game, btnPos, btnSize);
            btnPositive.BackgroundTexture = ButtonBackground;
            btnPositive.Font = ButtonFont;
            btnPositive.Text = PositiveText;
            btnPositive.TextColor = Color.Silver;
            btnPositive.OnMouseClick += button_OnMouseClick;

            btnPos = Position + new Point(Size.X - (btnSize.X + 16), Size.Y - (btnHeight + 16));

            btnNegative = new UIButton(Game, btnPos, btnSize);
            btnNegative.BackgroundTexture = ButtonBackground;
            btnNegative.Font = ButtonFont;
            btnNegative.Text = NegativeText;
            btnNegative.TextColor = Color.Silver;
            btnNegative.OnMouseClick += button_OnMouseClick;

            btnPos = Position + new Point((Size.X / 2) - (btnSize.X / 2), Size.Y - (btnHeight + 16));

            btnRetry = new UIButton(Game, btnPos, btnSize);
            btnRetry.BackgroundTexture = ButtonBackground;
            btnRetry.Font = ButtonFont;
            btnRetry.Text = RetryText;
            btnRetry.TextColor = Color.Silver;
            btnRetry.OnMouseClick += button_OnMouseClick;


            btnPositive.Initialize();
            btnNegative.Initialize();
            btnRetry.Initialize();

            lblMessage = new UILabel(Game);
            lblMessage.Font = MessageFont;
            lblMessage.Text = Message;
            lblMessage.Initialize();

            base.Initialize();
        }

        private void button_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            audioManager.PlaySFX("Audio/SFX/Personal", .125f);

            if (sender == btnPositive && OnPositiveClicked != null)
            {
                OnPositiveClicked(sender, mouseState);
            }

            if (sender == btnNegative && OnNegativeClicked != null)
            {
                OnNegativeClicked(sender, mouseState);
            }

            if (sender == btnRetry && OnRetryClicked != null)
            {
                OnRetryClicked(sender, mouseState);
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AddTopMost();

            btnPXOffset = (Size.X / 4) - 8;
            btnHeight = Size.Y / 8;

            Point btnPos = Position + new Point(16, Size.Y - (btnHeight + 16));
            Point btnSize = new Point(btnPXOffset, btnHeight);

            btnPositive.Visible = btnPositive.Enabled = ShowPositiveButton;

            btnPositive.Position = btnPos;
            btnPositive.Size = btnSize;

            if (btnPositive.Enabled)
                btnPositive.Update(gameTime);

            btnPos = Position + new Point(Size.X - (btnNegative.Size.X + 16), Size.Y - (btnHeight + 16));

            btnNegative.Visible = btnNegative.Enabled = ShowNegativeButton;

            btnNegative.Position = btnPos;
            btnNegative.Size = btnSize;

            if (btnNegative.Enabled)
                btnNegative.Update(gameTime);

            btnPos = Position + new Point((Size.X / 2) - (btnNegative.Size.X / 2), Size.Y - (btnHeight + 16));

            btnRetry.Visible = btnRetry.Enabled = ShowRetryBtn;

            btnRetry.Position = btnPos;
            btnRetry.Size = btnSize;

            if (btnRetry.Enabled)
                btnRetry.Update(gameTime);

            lblMessage.Text = Message;
            lblMessage.TextAlingment = TextAlingmentEnum.LeftTop;
            lblMessage.Position = Position + new Point(16, TitleBackground.Height + 16);

            Vector2 m = lblMessage.Font.MeasureString(lblMessage.Text);

            if (m.X > Size.X - 32)
            {
                string[] letters = lblMessage.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string final = string.Empty;

                foreach (string letter in letters)
                {
                    m = lblMessage.Font.MeasureString($"{final} {letter}");

                    if (m.X < Size.X - 32)
                    {
                        if (string.IsNullOrEmpty(final))
                            final = letter;
                        else
                            final = $"{final} {letter}";
                    }
                    else
                        final = $"{final}\n{letter}";
                }
                lblMessage.Text = Message = final;
            }
            lblMessage.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            _spriteBatch.Draw(backSplash, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Black);

            Color tint = Tint;

            if (!Enabled)
            {
                tint = GreyScaleColor(Tint);
            }

            // Draw BG
            if (Background != null)
                _spriteBatch.Draw(Background, Rectangle, tint);

            if (TitleBackground != null)
            {
                _spriteBatch.Draw(TitleBackground, new Rectangle(Position.X, Position.Y, TitleBackground.Width, TitleBackground.Height), tint);
                if (TitleFont != null)
                {
                    Vector2 p = Position.ToVector2();
                    Vector2 t = TitleFont.MeasureString(Title);

                    p += new Vector2(16, (TitleBackground.Height / 2) - t.Y / 2);

                    _spriteBatch.DrawString(TitleFont, Title, p + new Vector2(2, 2), TitleShadowColor);
                    _spriteBatch.DrawString(TitleFont, Title, p, Tint);
                }
            }

            _spriteBatch.End();

            lblMessage.Draw(gameTime);

            if (btnPositive.Visible)
                btnPositive.Draw(gameTime);
            if (btnNegative.Visible)
                btnNegative.Draw(gameTime);
            if (btnRetry.Visible)
                btnRetry.Draw(gameTime);
        }
    }
}
