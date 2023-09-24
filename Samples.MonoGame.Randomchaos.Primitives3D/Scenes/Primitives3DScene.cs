using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Services.Camera;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;

namespace Samples.MonoGame.Randomchaos.Primitives3D.Scenes
{
    public class Primitives3DScene : SceneFadeBase
    {

        /// <summary>   The camera. </summary>
        protected ICameraService _camera { get { return Game.Services.GetService<ICameraService>(); } }

        /// <summary>   The triangle. </summary>
        TriangleBasicEffect triangle;
        /// <summary>   The quad. </summary>
        QuadBasicEffect quad;
        /// <summary>   The cube. </summary>
        CubeBasicEffect cube;
        /// <summary>   The sphere. </summary>
        SphereBasicEfect sphere;
        /// <summary>   The capsule. </summary>
        CapsuleBasicEffect capsule;
        /// <summary>   The cylinder. </summary>
        CylinderBasicEffect cylinder;
        /// <summary>   The plane. </summary>
        PlaneBasicEffect plane;

        /// <summary>   True to render wire frame. </summary>
        protected bool _renderWireFrame = false;
        /// <summary>   True to disable, false to enable the culling. </summary>
        protected bool _cullingOff = false;


        /// <summary>   The font. </summary>
        private SpriteFont _spriteFont;

        /// <summary>   The next scene. </summary>
        protected string NextScene;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public Primitives3DScene(Game game, string name) : base(game, name) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            _spriteFont = Game.Content.Load<SpriteFont>("Fonts/font");

            base.Initialize();

            Vector3 ld = new Vector3(1, -1, -1);

            sphere.SetDirectionalLight(ld);
            cube.SetDirectionalLight(ld);
            triangle.SetDirectionalLight(ld);
            quad.SetDirectionalLight(ld);
            capsule.SetDirectionalLight(ld);
            cylinder.SetDirectionalLight(ld);
            plane.SetDirectionalLight(ld);
        }

        public override void LoadScene()
        {
            _camera.Transform.Position = new Vector3(0, 0, 10);
            _camera.Transform.Rotation = Quaternion.Identity;

            triangle = new TriangleBasicEffect(Game);
            triangle.Transform.Position = new Vector3(-1f, 0, 0);
            Components.Add(triangle);

            quad = new QuadBasicEffect(Game);
            quad.Transform.Position = new Vector3(1f, 0, 0);
            Components.Add(quad);

            cube = new CubeBasicEffect(Game);
            cube.Transform.Position = new Vector3(3, 1, 3);
            Components.Add(cube);

            sphere = new SphereBasicEfect(Game);
            sphere.Transform.Position = new Vector3(3, 1f, -3);
            Components.Add(sphere);

            capsule = new CapsuleBasicEffect(Game);
            capsule.Transform.Position = new Vector3(-3, 1, 3);
            Components.Add(capsule);

            cylinder = new CylinderBasicEffect(Game);
            cylinder.Transform.Position = new Vector3(-3, 1, -3);
            Components.Add(cylinder);

            plane = new PlaneBasicEffect(Game);
            plane.Transform.Position = new Vector3(0, -1, -.5f);
            Components.Add(plane);
            base.LoadScene();
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (kbManager.KeyPress(Microsoft.Xna.Framework.Input.Keys.Escape))
                    sceneManager.LoadScene("mainMenu");

                // Camera controls..
                float speedTran = .1f;
                float speedRot = .01f;

                if (kbManager.KeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                    _camera.Transform.Translate(Vector3.Forward * speedTran);
                if (kbManager.KeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                    _camera.Transform.Translate(Vector3.Backward * speedTran);
                if (kbManager.KeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    _camera.Transform.Translate(Vector3.Left * speedTran);
                if (kbManager.KeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    _camera.Transform.Translate(Vector3.Right * speedTran);

                if (kbManager.KeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
                    _camera.Transform.Rotate(Vector3.Up, speedRot);
                if (kbManager.KeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
                    _camera.Transform.Rotate(Vector3.Up, -speedRot);
                if (kbManager.KeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0)
                    _camera.Transform.Rotate(Vector3.Right, speedRot);
                if (kbManager.KeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < 0)
                    _camera.Transform.Rotate(Vector3.Right, -speedRot);

                if (kbManager.KeyPress(Keys.F1))
                {
                    _renderWireFrame = !_renderWireFrame;
                }

                if (kbManager.KeyPress(Keys.F2))
                {
                    _cullingOff = !_cullingOff;
                }
            }

            base.Update(gameTime);

            SetRasterizerState();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_camera.ClearColor);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);

            RasterizerState rasterizerState = GraphicsDevice.RasterizerState;
            BlendState blendState = GraphicsDevice.BlendState;
            DepthStencilState depthStencilState = GraphicsDevice.DepthStencilState;
            
            
            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            int line = 8;

            line = DrawString("Primitives 3D", line);
            line = DrawString($"ESC - Return to Menu", line);
            line = DrawString($"F1 - Toggle Wire Frame [{_renderWireFrame}]", line);
            line = DrawString($"F2 - Toggle Cull Mode [{_cullingOff}]", line);

            _spriteBatch.End();

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = blendState;
            GraphicsDevice.DepthStencilState = depthStencilState;

            DrawFader(gameTime);
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

        private int DrawString(string text, int line)
        {
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) - new Vector2(1, -1), Color.Black);
            //_spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line) + new Vector2(1, -1), Color.Black);
            _spriteBatch.DrawString(_spriteFont, text, new Vector2(8, line), Color.Gold);
            return line + _spriteFont.LineSpacing;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets rasterizer state. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void SetRasterizerState()
        {
            GraphicsDevice.RasterizerState = new RasterizerState() { FillMode = _renderWireFrame ? FillMode.WireFrame : FillMode.Solid, CullMode = _cullingOff ? CullMode.None : CullMode.CullCounterClockwiseFace };
        }
    }
}

