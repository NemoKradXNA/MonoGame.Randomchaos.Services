using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.UI.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.UI
{
    public class UIList : UIBase
    {
        protected UILabel lblTitle { get; set; }
        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
        public SpriteFont TitleFont { get; set; }
        public SpriteFont ListFont { get; set; }
        public Texture2D ListBackgroundTexture { get; set; }

        public List<IListItem> Items { get; set; }

        private Rectangle _scissorRectangle;
        protected Rectangle scissorRectangle
        {
            get
            {
                int titleHeight = TitleFont.LineSpacing;

                if (_scissorRectangle == Rectangle.Empty || (Rectangle.X != Position.X || Rectangle.Y != Position.Y || Rectangle.Width != Size.X || Rectangle.Height != Size.Y))
                    _scissorRectangle = new Rectangle(Rectangle.X + 1, Rectangle.Y + 1, Rectangle.Width - 2, Rectangle.Height - 2);

                return _scissorRectangle;
            }
        }

        public UIList(Game game, Point position, Point size) : base(game, position, size)
        {
            lblTitle = new UILabel(Game);
        }

        public override void Initialize()
        {
            base.Initialize();


            lblTitle.Font = TitleFont;
            lblTitle.Tint = Tint;
            lblTitle.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            lblTitle.Enabled = Enabled;
            lblTitle.Position = Position - new Point((int)(TitleFont.MeasureString(Title).X / -2) - 8, (int)(TitleFont.LineSpacing * .5f));
            lblTitle.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {

            // Draw LAbel
            lblTitle.Draw(gameTime);

            // Draw List BG
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            _spriteBatch.Draw(ListBackgroundTexture, Rectangle, Tint);
            _spriteBatch.End();

            // Render culled content.
            Rectangle orgRect = _spriteBatch.GraphicsDevice.ScissorRectangle;
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, new RasterizerState() { ScissorTestEnable = true, });
            _spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
            Vector2 rootPosition = new Vector2(Position.X + 12, scissorRectangle.Y + ListFont.LineSpacing * .5f);
            for (int e = Items.Count - 1; e >= 0; e--)
            {
                IListItem thisItem = Items[e];

                Color tint = thisItem.DisplayColor;

                if (!Enabled || !thisItem.Enabled)
                {
                    tint = GreyScaleColor(tint);
                }

                _spriteBatch.DrawString(ListFont, string.Format($"{thisItem.Format}", thisItem.DisplayText), rootPosition, tint);
                rootPosition.Y += ListFont.LineSpacing;
            }

            _spriteBatch.End();
        }
    }
}
