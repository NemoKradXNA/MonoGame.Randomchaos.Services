

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.UI.BaseClasses
{
    public abstract class UIBase : DrawableGameComponent, IUIBase
    {
        protected static List<IUIBase> TopMostMouseOver = new List<IUIBase>();

        public bool IsMouseOver { get; set; }

        protected IAudioService audioManager { get { return Game.Services.GetService<IAudioService>(); } }
        protected IInputStateService inputManager { get { return Game.Services.GetService<IInputStateService>(); } }

        public Point Position { get; set; }
        public Point Size { get; set; }

        protected SpriteBatch _spriteBatch { get; set; }

        private Rectangle _rectangle;
        public Rectangle Rectangle
        {
            get
            {
                if ((_rectangle.X != Position.X || _rectangle.Y != Position.Y || _rectangle.Width != Size.X || _rectangle.Height != Size.Y))
                    _rectangle = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

                return _rectangle;
            }
        }
        public Color Tint { get; set; }

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

        protected void AddTopMost()
        {
            if (!TopMostMouseOver.Contains(this))
                TopMostMouseOver.Add(this);

            if (!Enabled || !Visible)
                TopMostMouseOver.Remove(this);
        }

        

        public UIBase(Game game, Point position, Point size) : base(game)
        {
            Position = position;
            Size = size;
            Tint = Color.White;
        }

        public override void Initialize()
        {
            base.Initialize();
            TopMostMouseOver.Clear();
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

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

        public virtual Color GreyScaleColor(Color color)
        {
            int c = ((color.R + color.G + color.B) / 6);
            return new Color(c, c, c, color.A);
        }
    }
}
