using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Physics;
using MonoGame.Randomchaos.Physics.Interfaces;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using Samples.MonoGame.Randomchaos.Physics.Models;
using System;

namespace Samples.MonoGame.Randomchaos.Physics
{
    public class BasicParticles2DGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        IPhysicsService PhysicsService;
        IInputStateService inputService;
        IKeyboardStateManager kbState;
        IMouseStateManager mState;

        bool StepPhysics = false;
        TimeSpan ts = new TimeSpan(0, 0, 0, 1, 0);
        DateTime? st;
        int stepSecond = 0;

        Basic2DParticleEmitter smokeEmitter;
        Basic2DParticleEmitter popEmitter;
        Basic2DParticleEmitter starEmitter;

        public BasicParticles2DGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            PhysicsService = new PhysicsService(this);

            kbState = new KeyboardStateManager(this);
            mState = new MouseStateManager(this);
            inputService = new InputHandlerService(this, kbState, mState);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            smokeEmitter = new Basic2DParticleEmitter(this, 100, "Textures/smoke")
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

            popEmitter = new Basic2DParticleEmitter(this, 255, "Textures/star2")
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

            popEmitter.Transform.Position = new Vector3(960/2, 540, 0);

            Components.Add(popEmitter);
            PhysicsService.RegisterObject(popEmitter);

            starEmitter = new Basic2DParticleEmitter(this, 60, "Textures/star1")
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteFont = Content.Load<SpriteFont>("Fonts/font");
        }

        protected override void Update(GameTime gameTime)
        {
            inputService.PreUpdate(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);

            if (kbState.KeyDown(Keys.F1))
            {
                popEmitter.Reset();
            }

            if (kbState.KeyPress(Keys.P))
            {
                PhysicsService.IsPaused = !PhysicsService.IsPaused;
                StepPhysics = false;
            }

            if (kbState.KeyPress(Keys.S))
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointWrap);

            int line = 8;

            line = DrawString("BasicBalistics", line);
            line = DrawString("F1 - Rest Pop Emitter", line);
            line = DrawString($"P - Physics Pause [{PhysicsService.IsPaused}]", line);
            line = DrawString($"S - Physics Step is on [{StepPhysics}] each second {stepSecond}s", line);

            _spriteBatch.End();
        }

        private int DrawString(string text,  int line)
        {
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) - new Vector2(1,-1), Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) + new Vector2(1, -1), Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line), Color.Gold);
            return line + _spriteFont.LineSpacing;
        }
    }
}