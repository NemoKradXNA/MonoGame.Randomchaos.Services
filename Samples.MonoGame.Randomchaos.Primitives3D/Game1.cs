using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;

namespace Samples.MonoGame.Randomchaos.Primitives3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        IInputStateService inputService;
        IKeyboardStateManager kbState;
        IMouseStateManager mState;

        ICameraService camera;

        TriangleBasicEffect triangle;
        QuadBasicEffect quad;
        CubeBasicEffect cube;
        SphereBasicEfect sphere;

        protected bool _renderWireFrame = false;
        protected bool _cullingOff = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            kbState = new KeyboardStateManager(this);
            mState = new MouseStateManager(this);
            inputService = new InputHandlerService(this, kbState, mState);

            camera = new CameraService(this, .1f, 20000);
            camera.Transform.Position = new Vector3(0, 0, 10);
            camera.ClearColor = Color.Black;

            triangle = new TriangleBasicEffect(this);
            triangle.Transform.Position = new Vector3(-1f, 0, 0);
            Components.Add(triangle);

            quad = new QuadBasicEffect(this);
            quad.Transform.Position = new Vector3(1f, 0, 0);
            Components.Add(quad);

            cube = new CubeBasicEffect(this);
            cube.Transform.Position = new Vector3(-1f, 1.5f, 0);
            Components.Add(cube);

            sphere = new SphereBasicEfect(this);
            sphere.Transform.Position = new Vector3(1f, 1.5f, 0);
            Components.Add(sphere);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
                camera.Transform.Rotate(Vector3.Right, -speedRot);

            if (kbState.KeyPress(Keys.F1))
            {
                _renderWireFrame = !_renderWireFrame;
                SetRasterizerState();
            }

            if (kbState.KeyPress(Keys.F2))
            {
                _cullingOff = !_cullingOff;
                SetRasterizerState();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            RasterizerState rasterizerState = GraphicsDevice.RasterizerState;
            BlendState blendState = GraphicsDevice.BlendState;
            DepthStencilState depthStencilState = GraphicsDevice.DepthStencilState;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            int line = 8;

            line = DrawString("Primitives 3D", line);
            line = DrawString($"F1 - Toggle Wire Frame [{_renderWireFrame}]", line);
            line = DrawString($"F2 - Toggle Cull Mode [{_cullingOff}]", line);

            _spriteBatch.End();

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = blendState;
            GraphicsDevice.DepthStencilState = depthStencilState;
        }

        private int DrawString(string text, int line)
        {
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) - new Vector2(1, -1), Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) + new Vector2(1, -1), Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line), Color.Gold);
            return line + _spriteFont.LineSpacing;
        }

        public void SetRasterizerState()
        {
            GraphicsDevice.RasterizerState = new RasterizerState() { FillMode = _renderWireFrame ? FillMode.WireFrame : FillMode.Solid, CullMode = _cullingOff ? CullMode.None : CullMode.CullCounterClockwiseFace };
        }
    }
}