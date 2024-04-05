
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.UI.BaseClasses
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A base. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class UIBase : DrawableGameComponent, IUIBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite sort mode. </summary>
        ///
        /// <value> The sprite sort mode. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Immediate;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the blend. </summary>
        ///
        /// <value> The blend state. </value>
        ///-------------------------------------------------------------------------------------------------

        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the sampler. </summary>
        ///
        /// <value> The sampler state. </value>
        ///-------------------------------------------------------------------------------------------------

        public SamplerState SamplerState { get; set; } = SamplerState.PointClamp;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether we allow mouse pass through. </summary>
        ///
        /// <value> True if allow mouse p ass through, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool AllowMousePassThrough { get; set; } = false;

        /// <summary>   The top most mouse over. </summary>
        protected static List<IUIBase> TopMostMouseOver = new List<IUIBase>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is mouse over. </summary>
        ///
        /// <value> True if this object is mouse over, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsMouseOver { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for audio. </summary>
        ///
        /// <value> The audio manager. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IAudioService audioManager { get { return Game.Services.GetService<IAudioService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for input. </summary>
        ///
        /// <value> The input manager. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IInputStateService inputManager { get { return Game.Services.GetService<IInputStateService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the position. </summary>
        ///
        /// <value> The position. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point Position { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the size. </summary>
        ///
        /// <value> The size. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point Size { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite batch. </summary>
        ///
        /// <value> The sprite batch. </value>
        ///-------------------------------------------------------------------------------------------------

        protected SpriteBatch _spriteBatch { get; set; }

        /// <summary>   The rectangle. </summary>
        private Rectangle _rectangle;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the rectangle. </summary>
        ///
        /// <value> The rectangle. </value>
        ///-------------------------------------------------------------------------------------------------

        public Rectangle Rectangle
        {
            get
            {
                if ((_rectangle.X != Position.X || _rectangle.Y != Position.Y || _rectangle.Width != Size.X || _rectangle.Height != Size.Y))
                    _rectangle = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

                return _rectangle;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tint. </summary>
        ///
        /// <value> The tint. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Tint { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is top most. </summary>
        ///
        /// <value> True if this object is top most, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        protected bool IsTopMost
        {
            get
            {
                if (TopMostMouseOver.Contains(this))
                {
                    return TopMostMouseOver.IndexOf(this) == TopMostMouseOver.Count - 1;
                }

                return false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds top most. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected void AddTopMost()
        {
            if (!TopMostMouseOver.Contains(this))
                TopMostMouseOver.Add(this);

            if (!Enabled || !Visible)
                TopMostMouseOver.Remove(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="position"> The position. </param>
        /// <param name="size">     The size. </param>
        ///-------------------------------------------------------------------------------------------------

        public UIBase(Game game, Point position, Point size) : base(game)
        {
            Position = position;
            Size = size;
            Tint = Color.White;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            TopMostMouseOver.Clear();
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
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

            IsMouseOver = inputManager.MouseManager.PositionRect.Intersects(Rectangle);

            if (IsMouseOver)
            {
                inputManager.MouseManager.Handled = true && !AllowMousePassThrough;
                //AddTopMost();
            }
            else
            {
                //TopMostMouseOver.Remove(this);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Grey scale color. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="color">    The color. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual Color GreyScaleColor(Color color)
        {
            int c = ((color.R + color.G + color.B) / 6);
            return new Color(c, c, c, color.A);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 02/04/2024. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        protected virtual void DrawSegmentedBackground(Texture2D backgroundTexture, Rectangle? segments, Color colorBG, Point offset)
        {
            Rectangle seg = segments.Value;

            // Top Left
            float tlWidth = seg.Left;
            float tlHeight = seg.Top;

            Rectangle srect = new Rectangle(0, 0, seg.Left, seg.Top);
            Rectangle dest = new Rectangle(Rectangle.Left + offset.X, Rectangle.Top + offset.Y, (int)Math.Round(tlWidth), (int)Math.Round(tlHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);
            // Top
            float tWidth = Rectangle.Width - (tlWidth * 2);

            srect = new Rectangle(seg.Left, 0, backgroundTexture.Width - seg.Right, seg.Top);
            dest = new Rectangle(Rectangle.Left + (int)Math.Round(tlWidth) + offset.X, Rectangle.Top + offset.Y, (int)Math.Round(tWidth), (int)Math.Round(tlHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);
            // Top right
            float trWidth = seg.Left;
            float trHeight = seg.Top;

            srect = new Rectangle(backgroundTexture.Width - (seg.Right - seg.Left), 0, (seg.Right - seg.Left), seg.Top);
            dest = new Rectangle(Rectangle.Right - (int)Math.Round(trWidth) + offset.X, Rectangle.Top + offset.Y, (int)Math.Round(trWidth), (int)Math.Round(trHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);
            // Left
            float lHeight = Rectangle.Height - (tlHeight * 2);
            srect = new Rectangle(0, seg.Top, seg.Left, backgroundTexture.Height - seg.Bottom);
            dest = new Rectangle(Rectangle.Left + offset.X, Rectangle.Top + (int)Math.Round(tlHeight) + offset.Y, (int)Math.Round(tlWidth), (int)Math.Round(lHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);

            // Middle
            srect = new Rectangle(seg.Left, seg.Top, backgroundTexture.Width - seg.Right, backgroundTexture.Height - seg.Bottom);
            dest = new Rectangle(Rectangle.Left + (int)Math.Round(tlWidth) + offset.X, Rectangle.Top + (int)Math.Round(tlHeight) + offset.Y, (int)Math.Round(tWidth), (int)Math.Round(lHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);
            // Right
            float rHeight = Rectangle.Height - (trHeight * 2);

            srect = new Rectangle(backgroundTexture.Width - (seg.Right - seg.Left), seg.Top, (seg.Right - seg.Left), backgroundTexture.Height - seg.Bottom);
            dest = new Rectangle(Rectangle.Right - (int)Math.Round(trWidth) + offset.X, Rectangle.Top + (int)Math.Round(trHeight) + offset.Y, (int)Math.Round(trWidth), (int)Math.Round(rHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);

            // Bottom Left
            float blWidth = seg.Left;
            float blHeight = (seg.Bottom - seg.Top);

            srect = new Rectangle(0, backgroundTexture.Height - (seg.Bottom - seg.Top), seg.Left, (seg.Bottom - seg.Top));
            dest = new Rectangle(Rectangle.Left + offset.X, Rectangle.Bottom - (int)Math.Round(blHeight) + offset.Y, (int)Math.Round(blWidth), (int)Math.Round(blHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);
            // Bottom 
            float bWidth = Rectangle.Width - (trWidth * 2);

            srect = new Rectangle(seg.Left, backgroundTexture.Height - (seg.Bottom - seg.Top), backgroundTexture.Width - seg.Right, seg.Bottom - seg.Top);
            dest = new Rectangle(Rectangle.Left + (int)Math.Round(blWidth) + offset.X, Rectangle.Bottom - (int)Math.Round(blHeight) + offset.Y, (int)Math.Round(bWidth), (int)Math.Round(blHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);

            // Bottom Right
            float brWidth = (seg.Right - seg.Left);
            float brHeight = (seg.Bottom - seg.Top);

            srect = new Rectangle(backgroundTexture.Width - (seg.Right - seg.Left), backgroundTexture.Height - (seg.Bottom - seg.Top), seg.Right - seg.Left, (seg.Bottom - seg.Top));
            dest = new Rectangle(Rectangle.Right - (int)Math.Round(brWidth) + offset.X, Rectangle.Bottom - (int)Math.Round(brHeight) + offset.Y, (int)Math.Round(brWidth), (int)Math.Round(brHeight));

            DrawBackgroundElement(backgroundTexture, dest, srect, colorBG);
        }

        protected virtual void DrawBackgroundElement(Texture2D backgroundTexture, Rectangle dest, Rectangle? srect, Color colorBG)
        {
            if (srect != null)
            {
                _spriteBatch.Draw(backgroundTexture, dest, srect, colorBG);
            }
            else
            {
                _spriteBatch.Draw(backgroundTexture, dest, colorBG);
            }
        }
    }
}
