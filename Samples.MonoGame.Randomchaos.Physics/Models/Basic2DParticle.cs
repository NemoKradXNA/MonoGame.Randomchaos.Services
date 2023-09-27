
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Models.Basic;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A basic 2D particle. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Basic2DParticle : BasicPhysicsObject
    {
        /// <summary>   The sprite batch. </summary>
        SpriteBatch _spriteBatch;
        /// <summary>   The texture. </summary>
        Texture2D _texture;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite batch. </summary>
        ///
        /// <value> The sprite batch. </value>
        ///-------------------------------------------------------------------------------------------------

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the age. </summary>
        ///
        /// <value> The age. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Age { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation speed. </summary>
        ///
        /// <value> The rotation speed. </value>
        ///-------------------------------------------------------------------------------------------------

        public float RotationSpeed { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation. </summary>
        ///
        /// <value> The rotation. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Rotation { get; set; }

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

        /// <summary>   (Immutable) the texure asset. </summary>
        protected readonly string _texureAsset;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">         The game. </param>
        /// <param name="texureAsset">  (Optional) The texure asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public Basic2DParticle(Game game, string texureAsset = null) : base(game)
        {
            Transform = new Transform();
            _texureAsset = texureAsset;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            base.LoadContent();

            if (_texureAsset == null)
            {
                Color[] c = new Color[] { Color };

                _texture = new Texture2D(Game.GraphicsDevice, 1, 1);

                _texture.SetData(c);
            }
            else
            {
                _texture = Game.Content.Load<Texture2D>(_texureAsset);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Integrates the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Integrate(GameTime gameTime)
        {
            base.Integrate(gameTime);

            Age += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rotation += RotationSpeed;

            if (Rotation > MathHelper.Pi)
            {
                Rotation = 0;
            }
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
            if (Visible)
            {
                _spriteBatch.Draw(_texture, new Rectangle((int)Transform.Position.X, (int)Transform.Position.Y, Size.X, Size.Y), null, Color, Rotation, origin, SpriteEffects.None, 1);
            }
        }
    }
}
