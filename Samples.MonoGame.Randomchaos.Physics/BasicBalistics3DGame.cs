using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Physics;
using MonoGame.Randomchaos.Physics.Interfaces;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using Samples.MonoGame.Randomchaos.Physics.Models;
using System;

namespace Samples.MonoGame.Randomchaos.Physics
{
    public class BasicBalistics3DGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        IPhysicsService PhysicsService;
        IInputStateService inputService;
        IKeyboardStateManager kbState;
        IMouseStateManager mState;

        ICameraService camera;

        protected Basic3DBall ball;

        bool StepPhysics = false;
        TimeSpan ts = new TimeSpan(0, 0, 0, 1, 0);
        DateTime? st;
        int stepSecond = 0;

        public BasicBalistics3DGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            camera = new CameraService(this, .1f, 20000);
            camera.Transform.Position = new Vector3(0, 0, 10);
            camera.ClearColor = Color.Black;

            PhysicsService = new PhysicsService(this);

            kbState = new KeyboardStateManager(this);
            mState = new MouseStateManager(this);
            inputService = new InputHandlerService(this, kbState, mState);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            

            ball = new Basic3DBall(this);
            ball.Transform.Position = new Vector3(0, 0, 0);
            Components.Add(ball);

            base.Initialize();           

            PhysicsService.RegisterObject(ball);
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
            if (kbState.KeyDown(Keys.F1))
            {
                ball.Mass = 2;
                ball.Damping = .99f;
                ball.Velocity = new Vector3(0, 0, -35f);
                ball.Acceleration = new Vector3(0, -1f, 0);
            }

            if (kbState.KeyDown(Keys.F2))
            {
                ball.Mass = 200;
                ball.Damping = .99f;
                ball.Velocity = new Vector3(0, 30f, -40f);
                ball.Acceleration = new Vector3(0, -20f, 0);
            }

            if (kbState.KeyDown(Keys.F3))
            {
                ball.Mass = 1;
                ball.Damping = .9f;
                ball.Velocity = new Vector3(0, 0, -10f);
                ball.Acceleration = new Vector3(0, .6f, 0);
            }

            if (kbState.KeyDown(Keys.F4))
            {
                ball.Mass = .1f;
                ball.Damping = .99f;
                ball.Velocity = new Vector3(0, 0, -100f);
                ball.Acceleration = new Vector3(0, 0, 0);
            }

            if (kbState.KeyDown(Keys.F12))
            {
                ball.Mass = 1;
                ball.Velocity = Vector3.Zero;
                ball.Acceleration = Vector3.Zero;

                ball.Transform.Position = new Vector3(0, 0, 0);
            }

            base.Update(gameTime);

            if (kbState.KeyPress(Keys.P))
            {
                PhysicsService.IsPaused = !PhysicsService.IsPaused;
                StepPhysics = false;
            }

            // Camera controls..
            float speedTran = .1f;
            float speedRot = .01f;

            if (kbState.KeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                camera.Transform.Translate(Vector3.Forward * speedTran);
            if (kbState.KeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                camera.Transform.Translate(Vector3.Backward * speedTran);
            if (kbState.KeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                camera.Transform.Translate(Vector3.Left * speedTran);
            if (kbState.KeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                camera.Transform.Translate(Vector3.Right * speedTran);

            if (kbState.KeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
                camera.Transform.Rotate(Vector3.Up, speedRot);
            if (kbState.KeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
                camera.Transform.Rotate(Vector3.Up, -speedRot);
            if (kbState.KeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0)
                camera.Transform.Rotate(Vector3.Right, speedRot);
            if (kbState.KeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < 0)
                camera.Transform.Rotate(Vector3.Right, -speedRot);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            int line = 8;

            line = DrawString("BasicBalistics", line);
            line = DrawString("F1 - Shoot Standard", line);
            line = DrawString("F2 - Shoot Cannon", line);
            line = DrawString("F3 - Shoot Fireball", line);
            line = DrawString("F4 - Shoot Laser", line);
            line = DrawString("F12 - Reset", line);
            line = DrawString($"P - Physics Pause [{PhysicsService.IsPaused}]", line);
            DrawString($"Ball Transform: \n    X: {ball.Transform.Position.X}\n    Y: {ball.Transform.Position.Y}\n    Z: {ball.Transform.Position.Z}\n    Velocity: {ball.Velocity}",line);

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