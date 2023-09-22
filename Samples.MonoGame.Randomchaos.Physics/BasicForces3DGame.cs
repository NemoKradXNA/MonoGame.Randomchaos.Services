

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Physics;
using MonoGame.Randomchaos.Physics.ForceGenerators;
using MonoGame.Randomchaos.Physics.ForceGenerators.Springs;
using MonoGame.Randomchaos.Physics.ForceGenerators.Springs.Buoyancy;
using MonoGame.Randomchaos.Physics.Interfaces;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using Samples.MonoGame.Randomchaos.Physics.Models;

namespace Samples.MonoGame.Randomchaos.Physics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A basic forces 3D game. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BasicForces3DGame : Game
    {
        /// <summary>   The graphics. </summary>
        private GraphicsDeviceManager _graphics;
        /// <summary>   The sprite batch. </summary>
        private SpriteBatch _spriteBatch;
        /// <summary>   The sprite font. </summary>
        private SpriteFont _spriteFont;

        /// <summary>   The physics service. </summary>
        IPhysicsService PhysicsService;
        /// <summary>   The input service. </summary>
        IInputStateService inputService;
        /// <summary>   State of the kB. </summary>
        IKeyboardStateManager kbState;
        /// <summary>   The state. </summary>
        IMouseStateManager mState;

        /// <summary>   The camera. </summary>
        ICameraService camera;

        /// <summary>   The ball. </summary>
        protected Basic3DBall ball;
        /// <summary>   The cube. </summary>
        protected Basic3DCube cube;

        /// <summary>   The second ball. </summary>
        protected Basic3DBall ball2;
        /// <summary>   The second cube. </summary>
        protected Basic3DCube cube2;

        /// <summary>   The third ball. </summary>
        protected Basic3DBall ball3;
        /// <summary>   The third cube. </summary>
        protected Basic3DCube cube3;

        /// <summary>   The fourth cube. </summary>
        protected Basic3DCube cube4;

        /// <summary>   The gravity. </summary>
        IForceGenerator gravity;
        /// <summary>   The spring. </summary>
        IForceGenerator spring;
        /// <summary>   The bungee. </summary>
        IForceGenerator bungee;
        /// <summary>   The buoyancy. </summary>
        IForceGenerator buoyancy;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public BasicForces3DGame()
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            cube = new Basic3DCube(this);
            cube.Transform.Position = new Vector3(-10, 2, -20);
            Components.Add(cube);

            ball = new Basic3DBall(this);
            ball.Transform.Position = new Vector3(-10, 0, -20);
            Components.Add(ball);

            cube2 = new Basic3DCube(this);
            cube2.Transform.Position = new Vector3(-5, 2, -20);
            Components.Add(cube2);

            ball2 = new Basic3DBall(this);
            ball2.Transform.Position = new Vector3(-5, 0, -20);
            Components.Add(ball2);

            cube3 = new Basic3DCube(this);
            cube3.Transform.Position = new Vector3(0, 2, -20);
            Components.Add(cube3);

            ball3 = new Basic3DBall(this);
            ball3.Transform.Position = new Vector3(0, 0, -20);
            Components.Add(ball3);

            cube4 = new Basic3DCube(this);
            cube4.Transform.Position = new Vector3(5, 2, -20);
            Components.Add(cube4);

            base.Initialize();           

            PhysicsService.RegisterObject(ball);
            PhysicsService.RegisterObject(ball2);
            PhysicsService.RegisterObject(ball3);
            PhysicsService.RegisterObject(cube4);

            gravity = new GravityForceGenerator();
            PhysicsService.AddForce(gravity, ball);
            PhysicsService.AddForce(gravity, ball2);
            PhysicsService.AddForce(gravity, ball3);
            PhysicsService.AddForce(gravity, cube4);

            spring = new SpringForceGenerator(cube2, 1f, 2f);
            PhysicsService.AddForce(spring, ball2);

            bungee = new BungeeForceGenerator(cube3, 1f, .2f);
            PhysicsService.AddForce(bungee, ball3);

            buoyancy = new BuoyancyForceGenerator(-1, MathHelper.PiOver2, 0);
            PhysicsService.AddForce(buoyancy, cube4);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteFont = Content.Load<SpriteFont>("Fonts/font");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

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

                ball2.Mass = 2;
                ball2.Damping = .99f;
                ball2.Velocity = new Vector3(0, 0, -35f);
            }

            //if (kbState.KeyDown(Keys.F2))
            //{
            //    ball.Mass = 200;
            //    ball.Damping = .99f;
            //    ball.Velocity = new Vector3(0, 30f, -40f);
            //    //ball.Acceleration = new Vector3(0, -20f, 0);
            //}

            //if (kbState.KeyDown(Keys.F3))
            //{
            //    ball.Mass = 1;
            //    ball.Damping = .9f;
            //    ball.Velocity = new Vector3(0, 0, -10f);
            //    //ball.Acceleration = new Vector3(0, .6f, 0);
            //}

            //if (kbState.KeyDown(Keys.F4))
            //{
            //    ball.Mass = .1f;
            //    ball.Damping = .99f;
            //    ball.Velocity = new Vector3(0, 0, -100f);
            //    //ball.Acceleration = new Vector3(0, 0, 0);
            //}

            if (kbState.KeyDown(Keys.F12))
            {
                ball.Mass = 1;
                ball.Velocity = Vector3.Zero;
                ball.Transform.Position = new Vector3(-10, 0, -20);

                ball2.Velocity = Vector3.Zero;
                ball2.Transform.Position = new Vector3(-5, 0, -20);

                ball3.Velocity = Vector3.Zero;
                ball3.Transform.Position = new Vector3(0, 0, -20);

                cube4.Velocity = Vector3.Zero;
                cube4.Transform.Position = new Vector3(5, 2, -20);
            }

            base.Update(gameTime);

            if (kbState.KeyPress(Keys.P))
            {
                PhysicsService.IsPaused = !PhysicsService.IsPaused;
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            int line = 8;

            line = DrawString("Basic Force Generators", line);
            line = DrawString("F12 - Reset", line);
            line = DrawString($"P - Physics Pause [{PhysicsService.IsPaused}]", line);
            DrawString($"Ball Transform: \n    X: {ball.Transform.Position.X}\n    Y: {ball.Transform.Position.Y}\n    Z: {ball.Transform.Position.Z}\n    Velocity: {ball.Velocity}",line);

            _spriteBatch.End();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draw string. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="text"> The text. </param>
        /// <param name="line"> The line. </param>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        private int DrawString(string text,  int line)
        {
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) - new Vector2(1,-1), Color.Black);
            //_spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) + new Vector2(1, -1), Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line), Color.Gold);
            return line + _spriteFont.LineSpacing;
        }
    }
}