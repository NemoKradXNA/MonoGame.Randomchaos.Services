using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Animation.Animation2D;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;

namespace Sample.MonoGme.Randomchaos.Animation2D.Models
{
    public class Sprite : DrawableGameComponent
    {
        protected SpriteBatch _spriteBatch;
        public Vector2 Position { get; set; }
        public Point Size { get; set; }

        public string CurrentAnimation { get { return _animator.CurrentAnimation; } }

        public Color Tint { get; set; }

        protected ISpriteAnimator _animator;

        public Sprite(Game game, SpriteAnimator animator, Point size) : base(game)
        {
            _animator = animator;
            Tint = Color.White;
            Size = size;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }


        public virtual void StartAnimation(string animation)
        {
            _animator.StartAnimation(animation);
        }

        public virtual void StopAnimation()
        {
            _animator.StopAnimation();
        }
                

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead);

            _spriteBatch.Draw(_animator.SpriteSheetTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), _animator.CurrentCellRect, Tint);
            
            _spriteBatch.End();
        }
    }
}
