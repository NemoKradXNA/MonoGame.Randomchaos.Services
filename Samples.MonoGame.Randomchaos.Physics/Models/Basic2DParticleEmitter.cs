
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;
using MonoGame.Randomchaos.Physics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A basic 2D particle emitter. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Basic2DParticleEmitter : BasicPhysicsObject
    {
        /// <summary>   The sprite batch. </summary>
        SpriteBatch _spriteBatch;
        /// <summary>   The font. </summary>
        SpriteFont _font;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the blend. </summary>
        ///
        /// <value> The blend state. </value>
        ///-------------------------------------------------------------------------------------------------

        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

        /// <summary>   The particles. </summary>
        public List<Basic2DParticle> particles = new List<Basic2DParticle>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the maximum age. </summary>
        ///
        /// <value> The maximum age. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MaxAge { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the maximum velocity. </summary>
        ///
        /// <value> The maximum velocity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 MaxVelocity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the minimum velocity. </summary>
        ///
        /// <value> The minimum velocity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 MinVelocity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the minimum size. </summary>
        ///
        /// <value> The minimum size of the. </value>
        ///-------------------------------------------------------------------------------------------------

        public int MinSize { get; set; } = 4;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the maximum size. </summary>
        ///
        /// <value> The maximum size of the. </value>
        ///-------------------------------------------------------------------------------------------------

        public int MaxSize { get; set; } = 4;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the minimum alpha. </summary>
        ///
        /// <value> The minimum alpha. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MinAlpha { get; set; } = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the maximum alpha. </summary>
        ///
        /// <value> The maximum alpha. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MaxAlpha { get; set; } = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the minimum rotation. </summary>
        ///
        /// <value> The minimum rotation. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MinRotation { get; set; } = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the maximum rotation. </summary>
        ///
        /// <value> The maximum rotation. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MaxRotation { get; set; } = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the start. </summary>
        ///
        /// <value> The color of the start. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color StartColor { get; set; } = Color.White;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the end. </summary>
        ///
        /// <value> The color of the end. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color EndColor { get; set; } = Color.White;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the physics service. </summary>
        ///
        /// <value> The physics service. </value>
        ///-------------------------------------------------------------------------------------------------

        public IPhysicsService PhysicsService { get; set; }

        /// <summary>   (Immutable) the total particles. </summary>
        protected readonly int _totalParticles;
        /// <summary>   (Immutable) the texture asset. </summary>
        protected readonly string _textureAsset;

        /// <summary>   The random. </summary>
        protected Random rnd;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; } = "Particle Emitter";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the fade out. </summary>
        ///
        /// <value> True if fade out, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool FadeOut { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the bound to emiiter. </summary>
        ///
        /// <value> True if bound to emiiter, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool BoundToEmiiter { get; set; } = true;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the cycle particles. </summary>
        ///
        /// <value> True if cycle particles, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool CycleParticles { get; set; } = true;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the size of the emission batch. </summary>
        ///
        /// <value> The size of the emission batch. </value>
        ///-------------------------------------------------------------------------------------------------

        public int EmissionBatchSize { get; set; } = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">             The game. </param>
        /// <param name="totalParticals">   The total particals. </param>
        /// <param name="textureAsset">     (Optional) The texture asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public Basic2DParticleEmitter(Game game, int totalParticals, string textureAsset = null) : base(game)
        {
            Transform = new Transform();
            _totalParticles = totalParticals;

            rnd = new Random(DateTime.UtcNow.Millisecond);
            _textureAsset = textureAsset;
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
            _font = Game.Content.Load<SpriteFont>("Fonts/font");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds particle. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void AddParticle()
        {

            float x = (MathHelper.Lerp(MinVelocity.X, MaxVelocity.X, (float)rnd.NextDouble()));
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Resets this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void Reset()
        {
            foreach (var p in particles)
            {
                PhysicsService.RemoveObject(p);
            }

            particles.Clear();
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

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
