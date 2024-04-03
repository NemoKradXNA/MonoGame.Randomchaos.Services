
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using System;

namespace MonoGame.Randomchaos.UI
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A slider. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UISlider : UIBase
    {
        /// <summary>   The image bar. </summary>
        protected UIImage imgBar;
        /// <summary>   The button button. </summary>
        protected UIButton btnButton;

        /// <summary>   The label label. </summary>
        protected UILabel lblLabel;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the thickness. </summary>
        ///
        /// <value> The thickness. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Thickness { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the font. </summary>
        ///
        /// <value> The font. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteFont Font
        {
            get { return lblLabel.Font; }
            set
            {
                lblLabel.Font = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the label. </summary>
        ///
        /// <value> The label. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Label { get { return lblLabel.Text; } set { lblLabel.Text = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the label tint. </summary>
        ///
        /// <value> The label tint. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color LabelTint { get { return lblLabel.Tint; } set { lblLabel.Tint = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bar texture. </summary>
        ///
        /// <value> The bar texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D BarTexture { get { return imgBar.Texture; } set { imgBar.Texture = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the slider texture. </summary>
        ///
        /// <value> The slider texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D SliderTexture
        {
            get { return btnButton.BackgroundTexture; }
            set
            {
                btnButton.BackgroundTexture = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/10/2023. </remarks>
        ///
        /// <param name="game">         The game. </param>
        /// <param name="position">     The position. </param>
        /// <param name="size">         The size. </param>
        /// <param name="thickness">    (Optional) The thickness of the bar. </param>
        /// <param name="buttonSize">   (Optional) Size of the button. </param>
        ///-------------------------------------------------------------------------------------------------

        public UISlider(Game game, Point position, Point size, int thickness = 4, Point? buttonSize = null) : base(game, position, size)
        {
            if (buttonSize == null)
            {
                buttonSize = new Point(32, 32);
            }

            Thickness = thickness;

            lblLabel = new UILabel(Game) { TextAlingment = TextAlingmentEnum.LeftMiddle };

            imgBar = new UIImage(Game, Position, new Point(Size.X, Thickness));

            btnButton = new UIButton(Game, Position, buttonSize.Value);

            btnButton.OnMouseDown += btnOnMouseDown;
        }

        /// <summary>   True to dragging. </summary>
        bool dragging;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Button mouse down. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sender">       The sender. </param>
        /// <param name="mouseState">   State of the mouse. </param>
        ///-------------------------------------------------------------------------------------------------

        private void btnOnMouseDown(IUIBase sender, IMouseStateManager mouseState)
        {
            dragging = true;
        }

        /// <summary>   The value. </summary>
        protected float _Value;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the value. </summary>
        ///
        /// <value> The value. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the slider. </summary>
        ///
        /// <value> The color of the slider. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color SliderColor { get { return btnButton.Tint; } set { btnButton.Tint = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the slider hover. </summary>
        ///
        /// <value> The color of the slider hover. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color SliderHoverColor { get { return btnButton.HighlightColor; } set { btnButton.HighlightColor = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {

            base.Initialize();
            lblLabel.Initialize();
            imgBar.Initialize();
            btnButton.Initialize();

            Point m = lblLabel.Font.MeasureString(lblLabel.Text).ToPoint();
            lblLabel.Position = new Point(Position.X, Position.Y);// + (m.Y / 1));

            imgBar.Size = new Point(Size.X - (lblLabel.Size.X + 16), Thickness);
            //imgBar.Position = new Point(lblLabel.Position.X + 16 + m.X / 2, lblLabel.Position.Y - 2);
            imgBar.Position = new Point(lblLabel.Position.X + 16 + m.X, lblLabel.Position.Y - Thickness/2);

            btnButton.Position = new Point(imgBar.Position.X, (imgBar.Position.Y + (imgBar.Size.Y / 2)) - (btnButton.Size.Y / 2));
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
            bool handled = inputManager.MouseManager.Handled;

            lblLabel.Enabled = imgBar.Enabled = btnButton.Enabled = Enabled;

            base.Update(gameTime);

            inputManager.MouseManager.Handled = handled;

            btnButton.Update(gameTime);

            lblLabel.Update(gameTime);
            imgBar.Update(gameTime);
           

            if (dragging)
            {
                float xDelta = -inputManager.MouseManager.PositionDelta.X;
                Value = Math.Max(0, Math.Min(1, invLerp(imgBar.Position.X, (imgBar.Position.X + imgBar.Size.X) - btnButton.Size.X, btnButton.Position.X + xDelta)));
            }

            // Lerp button position.
            int x = (int)MathHelper.Lerp(imgBar.Position.X, (imgBar.Position.X + imgBar.Size.X) - btnButton.Size.X, _Value);
            btnButton.Position = new Point(x, btnButton.Position.Y);

            if (!inputManager.MouseManager.LeftButtonDown || !btnButton.IsMouseOver)
                dragging = false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Lerps. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="s">    A float to process. </param>
        /// <param name="e">    A float to process. </param>
        /// <param name="v">    The value. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected float lerp(float s, float e, float v)
        {
            return s * (1 - v) + e * v;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Inverse linearly interpolate. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="s">    A float to process. </param>
        /// <param name="e">    A float to process. </param>
        /// <param name="v">    The value. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected float invLerp(float s, float e, float v)
        {
            return (v - s) / (e - s);
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

            lblLabel.Draw(gameTime);
            imgBar.Draw(gameTime);
            btnButton.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
