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
    public class BasicBallistics3DScene : SceneFadeBase
    {
        /// <summary>   The sprite font. </summary>
        private SpriteFont _spriteFont;

        /// <summary>   The physics service. </summary>
        IPhysicsService PhysicsService { get { return Game.Services.GetService<IPhysicsService>(); } }

        /// <summary>   The ball. </summary>
        protected Basic3DBall ball;

        /// <summary>   True to step physics. </summary>
        bool StepPhysics = false;
        /// <summary>   The ts. </summary>
        TimeSpan ts = new TimeSpan(0, 0, 0, 1, 0);
        /// <summary>   The st. </summary>
        DateTime? st;
        /// <summary>   The step second. </summary>
        int stepSecond = 0;

        public BasicBallistics3DScene(Game game, string name) : base(game, name) { }

        public override void LoadScene()
        {
            camera.Transform.Position = new Vector3(0, 0, 10);
            camera.Transform.Rotation = Quaternion.Identity;

            _spriteFont = Game.Content.Load<SpriteFont>("Fonts/font");

            ball = new Basic3DBall(Game);
            ball.Transform.Position = new Vector3(0, 0, 0);
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
                if (kbManager.KeyPress(Microsoft.Xna.Framework.Input.Keys.Escape))
                    sceneManager.LoadScene("mainMenu");

                if (kbManager.KeyDown(Keys.F1))
                {
                    ball.Mass = 2;
                    ball.Damping = .99f;
                    ball.Velocity = new Vector3(0, 0, -35f);
                    ball.Acceleration = new Vector3(0, -1f, 0);
                }

                if (kbManager.KeyDown(Keys.F2))
                {
                    ball.Mass = 200;
                    ball.Damping = .99f;
                    ball.Velocity = new Vector3(0, 30f, -40f);
                    ball.Acceleration = new Vector3(0, -20f, 0);
                }

                if (kbManager.KeyDown(Keys.F3))
                {
                    ball.Mass = 1;
                    ball.Damping = .9f;
                    ball.Velocity = new Vector3(0, 0, -10f);
                    ball.Acceleration = new Vector3(0, .6f, 0);
                }

                if (kbManager.KeyDown(Keys.F4))
                {
                    ball.Mass = .1f;
                    ball.Damping = .99f;
                    ball.Velocity = new Vector3(0, 0, -100f);
                    ball.Acceleration = new Vector3(0, 0, 0);
                }

                if (kbManager.KeyDown(Keys.F12))
                {
                    ball.Mass = 1;
                    ball.Velocity = Vector3.Zero;
                    ball.Acceleration = Vector3.Zero;

                    ball.Transform.Position = new Vector3(0, 0, 0);
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

                // Camera controls..
                float speedTran = .1f;
                float speedRot = .01f;

                if (kbManager.KeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                    camera.Transform.Translate(Vector3.Forward * speedTran);
                if (kbManager.KeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                    camera.Transform.Translate(Vector3.Backward * speedTran);
                if (kbManager.KeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    camera.Transform.Translate(Vector3.Left * speedTran);
                if (kbManager.KeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    camera.Transform.Translate(Vector3.Right * speedTran);

                if (kbManager.KeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
                    camera.Transform.Rotate(Vector3.Up, speedRot);
                if (kbManager.KeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
                    camera.Transform.Rotate(Vector3.Up, -speedRot);
                if (kbManager.KeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0)
                    camera.Transform.Rotate(Vector3.Right, speedRot);
                if (kbManager.KeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < 0)
                    camera.Transform.Rotate(Vector3.Right, -speedRot);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            int line = 8;

            line = DrawString("Basic Ballistics 3D", line);
            line = DrawString("ESC - Return to menu", line);
            line = DrawString("F1 - Shoot Standard", line);
            line = DrawString("F1 - Shoot Standard", line);
            line = DrawString("F2 - Shoot Cannon", line);
            line = DrawString("F3 - Shoot Fireball", line);
            line = DrawString("F4 - Shoot Laser", line);
            line = DrawString("F12 - Reset", line);
            line = DrawString($"P - Physics Pause [{PhysicsService.IsPaused}]", line);
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
