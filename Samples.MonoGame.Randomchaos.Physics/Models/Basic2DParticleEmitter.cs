using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;
using MonoGame.Randomchaos.Physics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    public class Basic2DParticleEmitter : BasicPhysicsObject
    {
        SpriteBatch _spriteBatch;
        SpriteFont _font;

        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

        public List<Basic2DParticle> particles = new List<Basic2DParticle>();

        public float MaxAge { get; set; }
        public Vector3 MaxVelocity { get; set; }
        public Vector3 MinVelocity { get; set; }

        public int MinSize { get; set; } = 4;
        public int MaxSize { get; set; } = 4;

        public float MinAlpha { get; set; } = 1;
        public float MaxAlpha { get; set; } = 1;

        public float MinRotation { get; set; } = 0;
        public float MaxRotation { get; set; } = 0;

        public Color StartColor { get; set; } = Color.White;
        public Color EndColor { get; set; } = Color.White;

        public IPhysicsService PhysicsService { get; set; }

        protected readonly int _totalParticles;
        protected readonly string _textureAsset;

        protected Random rnd;

        public string Name { get; set; } = "Particle Emitter";

        public bool FadeOut { get; set; }
        public bool BoundToEmiiter { get; set; } = true;

        public bool CycleParticles { get; set; } = true;

        public int EmissionBatchSize { get; set; } = 1;

        public Basic2DParticleEmitter(Game game, int totalParticals, string textureAsset = null) : base(game)
        {
            Transform = new Transform();
            _totalParticles = totalParticals;

            rnd = new Random(DateTime.UtcNow.Millisecond);
            _textureAsset = textureAsset;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _font = Game.Content.Load<SpriteFont>("Fonts/font");
        }

        public void AddParticle()
        {

            float x = MathHelper.Lerp(MinVelocity.X, MaxVelocity.X, (float) rnd.NextDouble());
            float y = MathHelper.Lerp(MinVelocity.Y, MaxVelocity.Y, (float)rnd.NextDouble());
            
            Vector3 v = new Vector3(x,y,0);

            int s = rnd.Next(MinSize, MaxSize);

            float a = MathHelper.Lerp(MinAlpha,MaxAlpha, (float)rnd.NextDouble());
            float r = MathHelper.Lerp(MinRotation, MaxRotation, (float)rnd.NextDouble());


            var p = new Basic2DParticle(Game, _textureAsset)
            {
                Age = 0,
                Color = StartColor* a,
                Damping = .1f,
                Size = new Point(s, s),
                SpriteBatch = _spriteBatch,
                Velocity = v,
                Acceleration = new Vector3(0, 10f, 0),
                RotationSpeed = r,
            };


            p.Initialize();

            if (BoundToEmiiter)
            {
                p.Transform.Parent = Transform;
            }
            else
            {
                p.Transform.Position = Transform.Position;
            }

            

            particles.Add(p);
            PhysicsService.RegisterObject(p);
        }


        public void Reset()
        {
            foreach (var p in particles)
            {
                PhysicsService.RemoveObject(p);
            }

            particles.Clear();
        }

        public override void Update(GameTime gameTime)
        {

            // Get next particle.
            if (particles.Count < _totalParticles)
            {
                for (int e = 0; e < EmissionBatchSize && (particles.Count < EmissionBatchSize || EmissionBatchSize == 1); e++)
                {
                    AddParticle();
                }
            }

            base.Update(gameTime);

            List<Basic2DParticle> deadParticles = new List<Basic2DParticle>();

            foreach (Basic2DParticle particle in particles)
            {
                particle.Update(gameTime);

                if (particle.Age > MaxAge)
                {
                    if (CycleParticles)
                    {
                        deadParticles.Add(particle);
                    }
                    else
                    {
                        particle.Visible = false;
                    }

                    PhysicsService.RemoveObject(particle);
                }
                else
                {
                    float a = particle.Age / MaxAge;

                    particle.Color = Color.Lerp(StartColor, EndColor, a);

                    if (FadeOut)
                    {                        
                        particle.Color *= 1f-a;
                    }
                }
            }

            particles = particles.Where(w => !deadParticles.Contains(w)).ToList();
        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState, SamplerState.LinearWrap);
            
            foreach (Basic2DParticle particle in particles)
            {
                particle.Draw(gameTime);
            }

            _spriteBatch.End();

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointWrap);

            float l = _font.MeasureString($"{Name} [{particles.Count}]").X;
            Vector2 p = new Vector2(Transform.Position.X, Transform.Position.Y) + new Vector2(-l/2, _font.LineSpacing);
            _spriteBatch.DrawString(_font, $"{Name} [{particles.Count}/{_totalParticles}]", p, Color.Black);
            

            _spriteBatch.End();
        }

    }
}
