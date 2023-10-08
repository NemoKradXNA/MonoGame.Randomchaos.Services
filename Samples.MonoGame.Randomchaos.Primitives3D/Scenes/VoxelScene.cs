using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Primitives3D.Models.Voxel;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using System.Linq;

namespace Samples.MonoGame.Randomchaos.Primitives3D.Scenes
{
    public class VoxelScene : SceneFadeBase
    {
        protected ICameraService _camera { get { return Game.Services.GetService<ICameraService>(); } }

        /// <summary>   True to render wire frame. </summary>
        protected bool _renderWireFrame = false;
        /// <summary>   True to disable, false to enable the culling. </summary>
        protected bool _cullingOff = false;


        /// <summary>   The font. </summary>
        private SpriteFont _spriteFont;

        /// <summary>   The next scene. </summary>
        protected string NextScene;

        VoxelBasicEffect voxel;

        CubeBasicEffect voxelCursor;

        GeometryLines _geomLines;

        public VoxelScene(Game game, string name) : base(game, name) { }

        public override void Initialize()
        {
            _spriteFont = Game.Content.Load<SpriteFont>("Fonts/font");

            base.Initialize();

            Vector3 ld = new Vector3(1, -1, -1);

            voxel.SetDirectionalLight(ld, new Color(.125f, .125f, .125f).ToVector3());
        }

        public override void LoadScene()
        {
            _camera.Transform.Position = new Vector3(0, 0, 10);
            _camera.Transform.Rotation = Quaternion.Identity;

            voxelCursor = new CubeBasicEffect(Game)
            {
                Texture = Game.Content.Load<Texture2D>("Textures/Cursors/VoxelCursor1"),
            };

            voxelCursor.Visible = false;
            voxelCursor.Transform.Scale = Vector3.One * 1.01f;

            voxel = new VoxelBasicEffect(Game);
            voxel.Texture = Game.Content.Load<Texture2D>("Textures/Atlas/TileMap");
            voxel.AtlasDimensions = new Point(16,16);
            voxel.Transform.Position = new Vector3(0, 0, -10);

            Components.Add(voxel);

            Components.Add(voxelCursor);

            base.LoadScene();

            _geomLines = new GeometryLines(Game);
        }

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

                float minD = float.MaxValue;

                IHitInfo hitInfo = null;

                foreach (VoxelChunk chunk in voxel.VisibleChunks)
                {
                    BoundingBox box = TransformedBoundingBoxAA(chunk.BoundingBox, voxel.Transform);

                    IHitInfo hit;
                    float d = camera.RayPicking(msManager.ScreenPoint, box, out hit);

                    if (d < minD)
                    {
                        minD = d;
                        //liveChunk = chunk;
                        hitInfo = hit;
                        hitInfo.ContactObject = chunk;
                    }
                }

                if (hitInfo != null)
                {
                    voxelCursor.Transform.Position = Vector3.Transform(((VoxelChunk)hitInfo.ContactObject).Position - (voxel.VoxelCentre - new Vector3(.5f, .5f, .5f)), Matrix.CreateScale(voxel.Transform.Scale) * Matrix.CreateTranslation(voxel.Transform.Position));
                    voxelCursor.Visible = true;
                }
                else
                {
                    voxelCursor.Visible = false;
                }

                if (msManager.RightClicked && hitInfo != null)
                {
                    ((VoxelChunk)hitInfo.ContactObject).On = false;
                    voxel.ReBuild();
                }

                if (msManager.LeftClicked && hitInfo != null)
                {
                    Vector3 p = ((VoxelChunk)hitInfo.ContactObject).Position;

                    var t = ((VoxelChunk)hitInfo.ContactObject).Triangles.Where(s => s.ContansPoint(hitInfo.ContactPoint, voxel.Transform.World)).FirstOrDefault();
                    voxel.SetVoxelChunk(p + t.Normal, true, 1);
                    voxel.ReBuild();
                }
            }

            base.Update(gameTime);

            SetRasterizerState();
        }


        public BoundingBox TransformedBoundingBoxAA(BoundingBox box, ITransform transform)
        {
            Matrix AAWorld = Matrix.CreateScale(transform.Scale) * Matrix.CreateTranslation(transform.Position);

            Vector3 halfBlock = voxel.VoxelCentre- new Vector3(.5f, .5f, .5f);

            Vector3 min = Vector3.Transform(box.Min - halfBlock, AAWorld);
            Vector3 max = Vector3.Transform(box.Max - halfBlock, AAWorld);

            return new BoundingBox(min, max);
        }

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

            line = DrawString("Voxels", line);
            line = DrawString($"ESC - Return to Menu", line);
            line = DrawString($"F1 - Toggle Wire Frame [{_renderWireFrame}]", line);
            line = DrawString($"F2 - Toggle Cull Mode [{_cullingOff}]", line);
            line = DrawString($"LMB - Add selected chunk", line);
            line = DrawString($"RMB - Delete selected chunk", line);

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
