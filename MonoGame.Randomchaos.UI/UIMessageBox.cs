
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
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A message box. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UIMessageBox : UIBase
    {
        /// <summary>   The button positive. </summary>
        protected UIButton btnPositive;
        /// <summary>   The button negative. </summary>
        protected UIButton btnNegative;
        /// <summary>   The button retry. </summary>
        protected UIButton btnRetry;

        /// <summary>   Message describing the label. </summary>
        protected UILabel lblMessage;

        /// <summary>   The back splash. </summary>
        protected Texture2D backSplash;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the title background. </summary>
        ///
        /// <value> The title background. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D TitleBackground { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the background. </summary>
        ///
        /// <value> The background. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D Background { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the button background. </summary>
        ///
        /// <value> The button background. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D ButtonBackground { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the button font. </summary>
        ///
        /// <value> The button font. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteFont ButtonFont { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the title font. </summary>
        ///
        /// <value> The title font. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteFont TitleFont { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the title. </summary>
        ///
        /// <value> The title. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Title { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the title shadow. </summary>
        ///
        /// <value> The color of the title shadow. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color TitleShadowColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the message font. </summary>
        ///
        /// <value> The message font. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteFont MessageFont { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the message. </summary>
        ///
        /// <value> The message. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Message { get; set; }

        /// <summary>   The button px offset. </summary>
        private int btnPXOffset;
        /// <summary>   Height of the button. </summary>
        private int btnHeight;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the retry button is shown. </summary>
        ///
        /// <value> True if show retry button, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool ShowRetryBtn { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the positive button is shown. </summary>
        ///
        /// <value> True if show positive button, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool ShowPositiveButton { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the negative button is shown. </summary>
        ///
        /// <value> True if show negative button, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool ShowNegativeButton { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the positive text. </summary>
        ///
        /// <value> The positive text. </value>
        ///-------------------------------------------------------------------------------------------------

        public string PositiveText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the negative text. </summary>
        ///
        /// <value> The negative text. </value>
        ///-------------------------------------------------------------------------------------------------

        public string NegativeText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the retry text. </summary>
        ///
        /// <value> The retry text. </value>
        ///-------------------------------------------------------------------------------------------------

        public string RetryText { get; set; }

        /// <summary>   Event queue for all listeners interested in OnPositiveClicked events. </summary>
        public event UIMouseEvent OnPositiveClicked;
        /// <summary>   Event queue for all listeners interested in OnNegativeClicked events. </summary>
        public event UIMouseEvent OnNegativeClicked;
        /// <summary>   Event queue for all listeners interested in OnRetryClicked events. </summary>
        public event UIMouseEvent OnRetryClicked;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="position"> The position. </param>
        /// <param name="size">     The size. </param>
        ///-------------------------------------------------------------------------------------------------

        public UIMessageBox(Game game, Point position, Point size) : base(game, position, size)
        {
            TitleShadowColor = Color.Black;

            ShowRetryBtn = ShowPositiveButton = ShowNegativeButton = true;

            PositiveText = "Yes";
            NegativeText = "No";
            RetryText = "Retry";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Button mouse click. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sender">       The sender. </param>
        /// <param name="mouseState">   State of the mouse. </param>
        ///-------------------------------------------------------------------------------------------------

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

            base.Draw(gameTime);
        }
    }
}
