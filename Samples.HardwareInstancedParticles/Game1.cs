using HardwareInstancedParticles.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Coroutine;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HardwareInstancedParticles
{
    public class Game1 : Game
    {
        ICoroutineService coroutineService { get { return Services.GetService<ICoroutineService>(); } }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        IInputStateService inputService;
        IKeyboardStateManager kbState;
        IMouseStateManager mState;

        ICameraService camera;

        InstancedParticleEmitter emitter;

        Dictionary<ITransform,float> particleVelocities;

        List<ITransform> particleStartTransform;
        List<ITransform> particleEndTransform;

        bool play;
        float playSpeed = 0;
        const float speed = .025f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            new CoroutineService(this);

            IsMouseVisible = true;
            Window.AllowUserResizing = true;


            kbState = new KeyboardStateManager(this);
            mState = new MouseStateManager(this);
            inputService = new InputHandlerService(this, kbState, mState);


            camera = new CameraService(this, .1f, 20000);
            camera.Transform.Position = new Vector3(0, 1, 0);
            camera.ClearColor = Color.Black;

            emitter = new InstancedParticleEmitter(this);
            emitter.Transform.Position = new Vector3(0, 1, -5);
            emitter.Transform.Scale = Vector3.One * .01f;
            Components.Add(emitter);
        }

        protected override void Initialize()
        {

            // Create particle starting positions.
            Texture2D whitepixel = new Texture2D(GraphicsDevice, 1, 1);
            whitepixel.SetData(new Color[] { Color.White });

            //Texture2D logo = Content.Load<Texture2D>("Textures/mglogo1");
            //Texture2D logo = Content.Load<Texture2D>("Textures/waves");
            Texture2D logo = Content.Load<Texture2D>("Textures/chicken");

            Color[] logoPixels = new Color[logo.Width * logo.Height];
            logo.GetData(logoPixels);

            Vector3 center = new Vector3(logo.Width, logo.Height, 0) * .5f;

            float z = 0;

            particleVelocities = new Dictionary<ITransform, float>();
            particleEndTransform = new List<ITransform>();
            particleStartTransform = new List<ITransform>();

            float minSpeed = 1;
            float maxSpeed = 10;

            Random rnd = new Random(1971);

            int boxSize = 100;

            for (int x = 0; x < logo.Width; x++)
            {
                for (int y = 0; y < logo.Height; y++)
                {
                    Color c = logoPixels[x + y * logo.Width];

                    emitter.AddParticle(new Vector3(x, logo.Height-y, z) - center, Vector3.One, Quaternion.Identity, whitepixel, c);

                    ITransform thisParticle = emitter.Particles[emitter.Particles.Count - 1];

                    particleVelocities.Add(thisParticle, MathHelper.Lerp(minSpeed,maxSpeed, (float)rnd.NextDouble()));

                    particleEndTransform.Add(new Transform()
                    {
                        LocalPosition = thisParticle.LocalPosition,
                        LocalRotation = thisParticle.LocalRotation,
                        LocalScale = thisParticle.LocalScale,
                        Parent = emitter.Transform
                    });

                    float vx = MathHelper.Lerp(-boxSize, boxSize, (float)rnd.NextDouble());
                    float vy = MathHelper.Lerp(-boxSize, boxSize, (float)rnd.NextDouble());
                    float vz = MathHelper.Lerp(-boxSize, boxSize, (float)rnd.NextDouble());

                    Vector3 p = emitter.Transform.Position + new Vector3(vx, vy, vz);

                    float pi = (float)Math.PI;

                    vx = MathHelper.Lerp(-1, 1, (float)rnd.NextDouble());
                    vy = MathHelper.Lerp(-1, 1, (float)rnd.NextDouble());
                    vz = MathHelper.Lerp(-1, 1, (float)rnd.NextDouble());

                    Vector3 a = new Vector3(vx, vy, vz);

                    Vector3 d = (p * boxSize) - emitter.Transform.Position;// * MathHelper.PiOver2;
                    d.Normalize();

                    Quaternion r = Quaternion.Identity;

                    ITransform transform = new Transform()
                    {
                        Position = p,
                        LocalRotation = r,
                        Scale = thisParticle.LocalScale,
                        Parent = emitter.Transform
                    };

                    transform.LocalRotate(d, -MathHelper.PiOver2);

                    particleStartTransform.Add(transform);

                    emitter.SetParticle(emitter.Particles.Count - 1, transform);

                }
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Fonts/font");

            // TODO: use this.Content to load your game content here
            //t = new Thread(new ThreadStart(Worker));
            //t.Start();

            Task.Run(async () =>
            {
                await Worker();
            });
        }

        protected override void Update(GameTime gameTime)
        {
            inputService.PreUpdate(gameTime);
            base.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbState.KeyDown(Keys.Escape))
                Exit();

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
                camera.Transform    .Rotate(Vector3.Right, -speedRot);

            if (kbState.KeyPress(Keys.F3))
                emitter.Wire = !emitter.Wire;

            if (kbState.KeyPress(Keys.F1))
            {
                play = !play;
                playSpeed = 0;
            }

            if (kbState.KeyPress(Keys.Space))
            {
                play = !play;

                playSpeed = !play ? speed : -speed;
            }

            if (kbState.KeyDown(Keys.N))
                emitter.Transform.Rotate(Vector3.Up, -speedRot);
            if (kbState.KeyDown(Keys.M))
                emitter.Transform.Rotate(Vector3.Up, speedRot);
            if (kbState.KeyDown(Keys.G))
                emitter.Transform.Rotate(Vector3.Right, -speedRot);
            if (kbState.KeyDown(Keys.B))
                emitter.Transform.Rotate(Vector3.Right, speedRot);
        }

        Thread t;
        bool runThread;

        protected async Task Worker()
        {
            float pos = 0;
            runThread = true;
            while (runThread)
            {
                int idx = 0;

                foreach (ITransform particle in particleVelocities.Keys)
                {
                    //float s = particleVelocities[particle] * playSpeed;
                    //particle.Position = new Vector3(particle.Position.X, particle.Position.Y, MathHelper.Min(emitter.Transform.Position.Z, MathHelper.Max(emitter.Transform.Position.Z - 10, particle.Position.Z - s)));

                    float i = MathF.Min(1, MathF.Max(0, Math.Abs(pos)));

                    particle.Position = Vector3.Lerp(particleStartTransform[idx].Position, particleEndTransform[idx].Position, i);
                    particle.Rotation = Quaternion.Lerp(particleStartTransform[idx].Rotation, particleEndTransform[idx].Rotation, i);

                    emitter.SetParticle(idx, particle);
                    idx++;
                }

                if (playSpeed != 0)
                {
                    pos += playSpeed;

                    if (playSpeed > 0 && pos > 0)
                        pos = 0;

                    if (playSpeed < 0 && pos < -1)
                        pos = -1;

                }
                await Task.Delay(1);
            }
        }        

        protected override void OnExiting(object sender, EventArgs args)
        {
            runThread = play = false;
            emitter.Visible = false;
            base.OnExiting(sender, args);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here           

            base.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, $"Particles Count: {emitter.Particles.Count, 0:0,000,000}", new Vector2(8, 8), Color.Gold);
            _spriteBatch.DrawString(_font, $"Play Speed: {playSpeed}", new Vector2(8, 8 + _font.LineSpacing), Color.Gold);

            _spriteBatch.DrawString(_font, $"Space = Play/Rewind", new Vector2(8, 8 + _font.LineSpacing * 3), Color.Gold);
            _spriteBatch.DrawString(_font, $"F2 = Stop", new Vector2(8, 8 + _font.LineSpacing * 4), Color.Gold);
            _spriteBatch.DrawString(_font, $"F3 = Toggle Wire Frame", new Vector2(8, 8 + _font.LineSpacing * 5), Color.Gold);

            _spriteBatch.DrawString(_font, $"WASD = Translate Camera", new Vector2(8, 8 + _font.LineSpacing * 7), Color.Gold);
            _spriteBatch.DrawString(_font, $"Arrow Keys = Rotate Camera", new Vector2(8, 8 + _font.LineSpacing * 8), Color.Gold);
            _spriteBatch.End();
        }

        protected override void EndDraw()
        {
            base.EndDraw();

            coroutineService.UpdateEndFrame(null);
        }
    }
}
