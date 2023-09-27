using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Physics.Models.ForceGenerators;
using MonoGame.Randomchaos.Physics.Models.ForceGenerators.Springs;
using MonoGame.Randomchaos.Physics.Models.ForceGenerators.Springs.Buoyancy;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Interfaces.Physics;
using MonoGame.Randomchaos.Services.Scene.Models;
using Samples.MonoGame.Randomchaos.Physics.Models;

namespace Samples.MonoGame.Randomchaos.Physics.Scenes
{
    public class BasicForces3DScene : SceneFadeBase
    {
        /// <summary>   The sprite font. </summary>
        private SpriteFont _spriteFont;

        /// <summary>   The physics service. </summary>
        IPhysicsService PhysicsService { get { return Game.Services.GetService<IPhysicsService>(); } }

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

        public BasicForces3DScene(Game game, string name) : base(game, name) { }

        public override void LoadScene()
        {
            camera.Transform.Position = new Vector3(0, 0, 10);
            camera.Transform.Rotation = Quaternion.Identity;

            _spriteFont = Game.Content.Load<SpriteFont>("Fonts/font");

            cube = new Basic3DCube(Game);
            cube.Transform.Position = new Vector3(-10, 2, -20);
            Components.Add(cube);

            ball = new Basic3DBall(Game);
            ball.Transform.Position = new Vector3(-10, 0, -20);
            Components.Add(ball);

            cube2 = new Basic3DCube(Game);
            cube2.Transform.Position = new Vector3(-5, 2, -20);
            Components.Add(cube2);

            ball2 = new Basic3DBall(Game);
            ball2.Transform.Position = new Vector3(-5, 0, -20);
            Components.Add(ball2);

            cube3 = new Basic3DCube(Game);
            cube3.Transform.Position = new Vector3(0, 2, -20);
            Components.Add(cube3);

            ball3 = new Basic3DBall(Game);
            ball3.Transform.Position = new Vector3(0, 0, -20);
            Components.Add(ball3);

            cube4 = new Basic3DCube(Game);
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

            base.LoadScene();
        }

        public override void UnloadScene()
        {
            PhysicsService.RemoveObject(ball);
            PhysicsService.RemoveObject(ball2);
            PhysicsService.RemoveObject(ball3);
            PhysicsService.RemoveObject(cube4);

            PhysicsService.ClearForces();

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

                    ball2.Mass = 2;
                    ball2.Damping = .99f;
                    ball2.Velocity = new Vector3(0, 0, -35f);
                }

                //if (kbManager.KeyDown(Keys.F2))
                //{
                //    ball.Mass = 200;
                //    ball.Damping = .99f;
                //    ball.Velocity = new Vector3(0, 30f, -40f);
                //    //ball.Acceleration = new Vector3(0, -20f, 0);
                //}

                //if (kbManager.KeyDown(Keys.F3))
                //{
                //    ball.Mass = 1;
                //    ball.Damping = .9f;
                //    ball.Velocity = new Vector3(0, 0, -10f);
                //    //ball.Acceleration = new Vector3(0, .6f, 0);
                //}

                //if (kbManager.KeyDown(Keys.F4))
                //{
                //    ball.Mass = .1f;
                //    ball.Damping = .99f;
                //    ball.Velocity = new Vector3(0, 0, -100f);
                //    //ball.Acceleration = new Vector3(0, 0, 0);
                //}

                if (kbManager.KeyDown(Keys.F12))
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

            line = DrawString("Basic Forces 3D", line);
            line = DrawString("ESC - Return to menu", line);
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
