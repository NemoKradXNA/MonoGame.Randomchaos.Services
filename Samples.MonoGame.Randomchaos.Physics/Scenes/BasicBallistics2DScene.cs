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
    public class BasicBallistics2DScene : SceneFadeBase
    {
        /// <summary>   The sprite font. </summary>
        private SpriteFont _spriteFont;

        /// <summary>   The physics service. </summary>
        IPhysicsService PhysicsService { get { return Game.Services.GetService<IPhysicsService>(); } }

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

        public BasicBallistics2DScene(Game game, string name) : base(game, name) { }

        public override void LoadScene()
        {
            _spriteFont = Game.Content.Load<SpriteFont>("Fonts/font");

            ball = new Basic2DBall(Game);
            ball.Transform.Position = new Vector3(70, 540, 0);
            ball.Size = new Point(64, 64);

            Components.Add(ball);

            PhysicsService.RegisterObject(ball);

            base.LoadScene();
        }

        public override void UnloadScene()
        {
            PhysicsService.RemoveObject(ball);
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
                    ball.Mass = 2;
                    ball.Damping = .99f;
                    ball.Velocity = new Vector3(350f, 0, 0);
                    ball.Acceleration = new Vector3(0, 10f, 0);
                }

                if (kbManager.KeyDown(Keys.F2))
                {
                    ball.Mass = 200;
                    ball.Damping = .99f;
                    ball.Velocity = new Vector3(400f, -300f, 0);
                    ball.Acceleration = new Vector3(0, 200f, 0);
                }

                if (kbManager.KeyDown(Keys.F3))
                {
                    ball.Mass = 1;
                    ball.Damping = .9f;
                    ball.Velocity = new Vector3(100f, 0, 0);
                    ball.Acceleration = new Vector3(0, -60f, 0);
                }

                if (kbManager.KeyDown(Keys.F4))
                {
                    ball.Mass = .1f;
                    ball.Damping = .99f;
                    ball.Velocity = new Vector3(1000f, 0, 0);
                    ball.Acceleration = new Vector3(0, 0, 0);
                }

                if (kbManager.KeyDown(Keys.F12))
                {
                    ball.Mass = 1;
                    ball.Velocity = Vector3.Zero;
                    ball.Acceleration = Vector3.Zero;

                    ball.Transform.Position = new Vector3(70, 540, 0);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            int line = 8;

            line = DrawString("Basic Ballistics 2D", line);
            line = DrawString("ESC - Return to menu", line);
            line = DrawString("F1 - Shoot Standard", line);
            line = DrawString("F2 - Shoot Cannon", line);
            line = DrawString("F3 - Shoot Fireball", line);
            line = DrawString("F4 - Shoot Laser", line);
            line = DrawString("F12 - Reset", line);
            line = DrawString($"P - Physics Pause [{PhysicsService.IsPaused}]", line);
            line = DrawString($"S - Physics Step is on [{StepPhysics}] each second {stepSecond}s", line);
            DrawString($"Ball Transform: \n    X: {ball.Transform.Position.X}\n    Y: {ball.Transform.Position.Y}\n    Z: {ball.Transform.Position.Z}\n    Velocity: {ball.Velocity}", line);

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
