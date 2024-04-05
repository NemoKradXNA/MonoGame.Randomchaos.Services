
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.UI
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   List of user interfaces. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UIList : UIBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the label title. </summary>
        ///
        /// <value> The label title. </value>
        ///-------------------------------------------------------------------------------------------------

        protected UILabel lblTitle { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the title. </summary>
        ///
        /// <value> The title. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the title font. </summary>
        ///
        /// <value> The title font. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteFont TitleFont { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the list font. </summary>
        ///
        /// <value> The list font. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteFont ListFont { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the list background texture. </summary>
        ///
        /// <value> The list background texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D ListBackgroundTexture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the items. </summary>
        ///
        /// <value> The items. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<IListItem> Items { get; set; }

        /// <summary>   The scissor rectangle. </summary>
        private Rectangle _scissorRectangle;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scissor rectangle. </summary>
        ///
        /// <value> The scissor rectangle. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Rectangle scissorRectangle
        {
            get
            {
                int titleHeight = TitleFont.LineSpacing;

                if (_scissorRectangle == Rectangle.Empty || (ListRectangle.X != Position.X || ListRectangle.Y != Position.Y || ListRectangle.Width != Size.X || ListRectangle.Height != Size.Y))
                    _scissorRectangle = new Rectangle(ListRectangle.X + 1, ListRectangle.Y + 1, ListRectangle.Width - 2, ListRectangle.Height - 2);

                return _scissorRectangle;
            }
        }

        protected Rectangle ListRectangle
        {
            get
            {
                Rectangle lstRect = Rectangle;
                lstRect.Y += TitleFont.LineSpacing;

                return lstRect;
            }
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

        public UIList(Game game, Point position, Point size) : base(game, position, size)
        {
            lblTitle = new UILabel(Game);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();

            lblTitle.TextAlingment = TextAlingmentEnum.MiddleTop;
            lblTitle.Font = TitleFont;
            lblTitle.Tint = Tint;
            lblTitle.Initialize();
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
            lblTitle.Enabled = Enabled;
            lblTitle.Position = Position + new Point(Size.X / 2, 0);
            lblTitle.Update(gameTime);
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

            // Draw LAbel
            lblTitle.Draw(gameTime);

            // Draw List BG
            _spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState);
            DrawBackgroundElement(ListBackgroundTexture, ListRectangle, null, Tint);
            _spriteBatch.End();

            // Render culled content.
            Rectangle orgRect = _spriteBatch.GraphicsDevice.ScissorRectangle;
            _spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState.DepthRead, new RasterizerState() { ScissorTestEnable = true, });
            _spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
            Vector2 rootPosition = new Vector2(Position.X + 12, scissorRectangle.Y + 4);

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

            base.Draw(gameTime);
        }
    }
}
