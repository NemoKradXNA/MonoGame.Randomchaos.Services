using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using Samples.MonoGame.Randomchaos.Physics.Models;
using System;

namespace Samples.MonoGame.Randomchaos.Physics.Scenes
{
    public class BasicParticles2DScene : SceneFadeBase
    {
        /// <summary>   The sprite font. </summary>
        private SpriteFont _spriteFont;

        /// <summary>   The physics service. </summary>
        IPhysicsService PhysicsService { get { return Game.Services.GetService<IPhysicsService>(); } }

        /// <summary>   The smoke emitter. </summary>
        Basic2DParticleEmitter smokeEmitter;
        /// <summary>   The pop emitter. </summary>
        Basic2DParticleEmitter popEmitter;
        /// <summary>   The star emitter. </summary>
        Basic2DParticleEmitter starEmitter;

        /// <summary>   True to step physics. </summary>
        bool StepPhysics = false;
        /// <summary>   The ts. </summary>
        TimeSpan ts = new TimeSpan(0, 0, 0, 1, 0);
        /// <summary>   The st. </summary>
        DateTime? st;
        /// <summary>   The step second. </summary>
        int stepSecond = 0;

        public BasicParticles2DScene(Game game, string name) : base(game, name) { }

        public override void LoadScene()
        {
            _spriteFont = Game.Content.Load<SpriteFont>("Fonts/font");

            smokeEmitter = new Basic2DParticleEmitter(Game, 100, "Textures/smoke")
            {
                PhysicsService = PhysicsService,
                MaxAge = 1.75f,
                MinVelocity = new Vector3(-100, -50, 0),
                MaxVelocity = new Vector3(100, -400, 0),
                MinSize = 8,
                MaxSize = 64,
                MinAlpha = .25f,
                MaxAlpha = .75f,
                EndColor = new Color(.5f, .5f, .5f, .75f),
                StartColor = new Color(.25f, .25f, .25f, .75f),
                CycleParticles = true,
                FadeOut = true,
                BoundToEmiiter = false,
                MinRotation = -.01f,
                MaxRotation = .05f,
                Name = "Smoke Emitter"
            };

            smokeEmitter.Transform.Position = new Vector3(960, 540, 0);

            Components.Add(smokeEmitter);
            PhysicsService.RegisterObject(smokeEmitter);

            popEmitter = new Basic2DParticleEmitter(Game, 255, "Textures/star2")
            {
                PhysicsService = PhysicsService,
                MaxAge = 2,
                MinVelocity = new Vector3(-400, -400, 0),
                MaxVelocity = new Vector3(400, 400, 0),
                MinSize = 16,
                MaxSize = 32,
                MinAlpha = 1,
                MaxAlpha = 1,
                EndColor = Color.White,
                StartColor = Color.Red,
                CycleParticles = false,
                FadeOut = true,
                MinRotation = -.5f,
                MaxRotation = .5f,
                Name = "Pop Emitter",
                EmissionBatchSize = 255,
                BlendState = BlendState.Additive,
            };

            popEmitter.Transform.Position = new Vector3(960 / 2, 540, 0);

            Components.Add(popEmitter);
            PhysicsService.RegisterObject(popEmitter);

            starEmitter = new Basic2DParticleEmitter(Game, 60, "Textures/star1")
            {
                PhysicsService = PhysicsService,
                MaxAge = 1f,
                MinVelocity = new Vector3(-400, -50, 0),
                MaxVelocity = new Vector3(400, -900, 0),
                MinSize = 8,
                MaxSize = 64,
                MinAlpha = .25f,
                MaxAlpha = .75f,
                EndColor = new Color(1, .75f, 0, .75f),
                StartColor = new Color(1, .25f, 0, .75f),
                CycleParticles = true,
                FadeOut = true,
                BoundToEmiiter = false,
                Name = "Star Emitter",
                EmissionBatchSize = 1,
                //BlendState = BlendState.Additive,
            };

            starEmitter.Transform.Position = new Vector3(960 + 960 / 2, 540, 0);

            Components.Add(starEmitter);
            PhysicsService.RegisterObject(starEmitter);

            base.LoadScene();
        }

        public override void UnloadScene()
        {

            PhysicsService.RemoveObject(smokeEmitter);
            PhysicsService.RemoveObject(popEmitter);
            PhysicsService.RemoveObject(starEmitter);

            base.UnloadScene();
        }

        public override void Update(GameTime gameTime)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (kbManager.KeyPress(Keys.Escape))
                    sceneManager.LoadScene("mainMenu");

                if (kbManager.KeyDown(Keys.F1))
                {
                    popEmitter.Reset();
                }

                if (kbManager.KeyPress(Keys.P))
                {
                    PhysicsService.IsPaused = !PhysicsService.IsPaused;
                    StepPhysics = false;
                }

                if (kbManager.KeyPress(Keys.S))
                {
                    StepPhysics = !StepPhysics;
                    st = DateTime.UtcNow;
                    stepSecond = 0;
                }

                if (StepPhysics)
                {
                    PhysicsService.IsPaused = true;

                    TimeSpan? d = (DateTime.UtcNow - st);

                    if (d >= ts)
                    {
                        stepSecond++;
                        st = DateTime.UtcNow;
                        PhysicsService.IsPaused = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            base.Draw(gameTime);

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            int line = 8;

            line = DrawString("Basic Particles 2D", line);
            line = DrawString("ESC - Return to menu", line);
            line = DrawString("F1 - Rest Pop Emitter", line);
            line = DrawString($"P - Physics Pause [{PhysicsService.IsPaused}]", line);
            line = DrawString($"S - Physics Step is on [{StepPhysics}] each second {stepSecond}s", line);

            _spriteBatch.End();

            DrawFader(gameTime);
        }

        private int DrawString(string text, int line)
        {
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) - new Vector2(1, -1), Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line), Color.Gold);
            return line + _spriteFont.LineSpacing;
        }
    }
}
