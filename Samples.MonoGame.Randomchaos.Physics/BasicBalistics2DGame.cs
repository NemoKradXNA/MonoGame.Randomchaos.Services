
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
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A basic balistics 2D game. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BasicBalistics2DGame : Game
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

        /// <summary>   The ball. </summary>
        protected Basic2DBall ball;

        /// <summary>   True to step physics. </summary>
        bool StepPhysics = false;
        /// <summary>   The ts. </summary>
        TimeSpan ts = new TimeSpan(0, 0, 0, 1, 0);
        /// <summary>   The st. </summary>
        DateTime? st;
        /// <summary>   The step second. </summary>
        int stepSecond = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public BasicBalistics2DGame()
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            

            ball = new Basic2DBall(this);
            ball.Transform.Position = new Vector3(70, 540, 0);
            ball.Size = new Point(64, 64);

            Components.Add(ball);

            PhysicsService.RegisterObject(ball);

            base.Initialize();
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
                ball.Velocity = new Vector3(350f, 0, 0);
                ball.Acceleration = new Vector3(0, 10f, 0);
            }

            if (kbState.KeyDown(Keys.F2))
            {
                ball.Mass = 200;
                ball.Damping = .99f;
                ball.Velocity = new Vector3(400f, -300f, 0);
                ball.Acceleration = new Vector3(0, 200f, 0);
            }

            if (kbState.KeyDown(Keys.F3))
            {
                ball.Mass = 1;
                ball.Damping = .9f;
                ball.Velocity = new Vector3(100f, 0, 0);
                ball.Acceleration = new Vector3(0, -60f, 0);
            }

            if (kbState.KeyDown(Keys.F4))
            {
                ball.Mass = .1f;
                ball.Damping = .99f;
                ball.Velocity = new Vector3(1000f, 0, 0);
                ball.Acceleration = new Vector3(0, 0, 0);
            }

            if (kbState.KeyDown(Keys.F12))
            {
                ball.Mass = 1;
                ball.Velocity = Vector3.Zero;
                ball.Acceleration = Vector3.Zero;

                ball.Transform.Position = new Vector3(70, 540, 0);
            }

            base.Update(gameTime);

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

            line = DrawString("BasicBalistics", line);
            line = DrawString("F1 - Shoot Standard", line);
            line = DrawString("F2 - Shoot Cannon", line);
            line = DrawString("F3 - Shoot Fireball", line);
            line = DrawString("F4 - Shoot Laser", line);
            line = DrawString("F12 - Reset", line);
            line = DrawString($"P - Physics Pause [{PhysicsService.IsPaused}]", line);
            line = DrawString($"S - Physics Step is on [{StepPhysics}] each second {stepSecond}s", line);
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