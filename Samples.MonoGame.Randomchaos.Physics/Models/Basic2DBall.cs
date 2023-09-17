using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    public class Basic2DBall : BasicPhysicsObject
    {
        SpriteBatch _spriteBatch;
        Texture2D _texture;
        SpriteFont _font;

        public Color Color { get; set; } = Color.White;
        public Point Size { get; set; } = new Point(128, 128);

        protected Vector2 origin { get { return new Vector2(_texture.Width, _texture.Height) * .5f; } }

        public Basic2DBall(Game game) : base(game) 
        {
            Transform = new Transform();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _texture = Game.Content.Load<Texture2D>("Textures/Ball");
            _font = Game.Content.Load<SpriteFont>("Fonts/font");
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend,samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(_texture, new Rectangle((int)Transform.Position.X, (int)Transform.Position.Y, Size.X, Size.Y), null, Color, Transform.Rotation.Z, origin, SpriteEffects.None, 1);

            _spriteBatch.DrawString(_font, $"Ball\nPosition: {Transform.Position}\nVelocity: {Velocity}", new Vector2(Transform.Position.X,Transform.Position.Y), Color.Black);

            _spriteBatch.End();
        }
    }
}
