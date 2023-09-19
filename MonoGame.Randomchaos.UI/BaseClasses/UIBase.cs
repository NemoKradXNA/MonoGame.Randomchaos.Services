


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
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
                AddTopMost();
            }
            else
            {
                TopMostMouseOver.Remove(this);
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
    }
}
