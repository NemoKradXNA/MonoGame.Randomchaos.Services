
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A basic 2D ball. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Basic2DBall : BasicPhysicsObject
    {
        /// <summary>   The sprite batch. </summary>
        SpriteBatch _spriteBatch;
        /// <summary>   The texture. </summary>
        Texture2D _texture;
        /// <summary>   The font. </summary>
        SpriteFont _font;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color. </summary>
        ///
        /// <value> The color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Color { get; set; } = Color.White;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the size. </summary>
        ///
        /// <value> The size. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point Size { get; set; } = new Point(128, 128);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the origin. </summary>
        ///
        /// <value> The origin. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Vector2 origin { get { return new Vector2(_texture.Width, _texture.Height) * .5f; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public Basic2DBall(Game game) : base(game) 
        {
            Transform = new Transform();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _texture = Game.Content.Load<Texture2D>("Textures/Ball");
            _font = Game.Content.Load<SpriteFont>("Fonts/font");
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
            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend,samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(_texture, new Rectangle((int)Transform.Position.X, (int)Transform.Position.Y, Size.X, Size.Y), null, Color, Transform.Rotation.Z, origin, SpriteEffects.None, 1);

            _spriteBatch.DrawString(_font, $"Ball\nPosition: {Transform.Position}\nVelocity: {Velocity}", new Vector2(Transform.Position.X,Transform.Position.Y), Color.Black);

            _spriteBatch.End();
        }
    }
}
