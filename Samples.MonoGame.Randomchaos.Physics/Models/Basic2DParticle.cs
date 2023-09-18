using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    public class Basic2DParticle : BasicPhysicsObject
    {
        SpriteBatch _spriteBatch;
        Texture2D _texture;

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }


        public float Age { get; set; }

        public float RotationSpeed { get; set; }
        public float Rotation { get; set; }


        public Color Color { get; set; } = Color.White;
        public Point Size { get; set; } = new Point(128, 128);

        protected Vector2 origin { get { return new Vector2(_texture.Width, _texture.Height) * .5f; } }

        protected readonly string _texureAsset;

        public Basic2DParticle(Game game, string texureAsset = null) : base(game)
        {
            Transform = new Transform();
            _texureAsset = texureAsset;
        }


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

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                _spriteBatch.Draw(_texture, new Rectangle((int)Transform.Position.X, (int)Transform.Position.Y, Size.X, Size.Y), null, Color, Rotation, origin, SpriteEffects.None, 1);
            }
        }
    }
}
