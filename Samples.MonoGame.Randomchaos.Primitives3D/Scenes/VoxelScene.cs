using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Primitives3D.Models.Voxel;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

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

        public VoxelScene(Game game, string name) : base(game, name) { }

        public override void Initialize()
        {
            _spriteFont = Game.Content.Load<SpriteFont>("Fonts/font");

            base.Initialize();

            Vector3 ld = new Vector3(1, -1, -1);

            voxel.SetDirectionalLight(ld);
        }

        public override void LoadScene()
        {
            _camera.Transform.Position = new Vector3(0, 0, 10);
            _camera.Transform.Rotation = Quaternion.Identity;

            voxel = new VoxelBasicEffect(Game);//,0,3,3,3);
            voxel.Texture = Game.Content.Load<Texture2D>("Textures/grass1");
            voxel.Transform.Position = new Vector3(0, 0, -10);

            Components.Add(voxel);

            base.LoadScene();
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

                selectedBox = null;
                float minD = float.MaxValue;
                VoxelChunk liveChunk = null;

                foreach (VoxelChunk chunk in voxel.VisibleChunks)
                {
                    BoundingBox box = TransformedBoundingBoxAA(chunk.BoundingBox, voxel.Transform);
                    float d = RayPicking(msManager.ScreenPoint, box);

                    if (d < minD)
                    {
                        minD = d;
                        liveChunk = chunk;
                    }
                }

                if (liveChunk != null)
                {
                    selectedBox = liveChunk.BoundingBox;
                }

                if (msManager.LeftClicked && liveChunk != null)
                {
                    liveChunk.On = false;
                    voxel.ReBuild();
                }
            }

            base.Update(gameTime);

            SetRasterizerState();
        }

        BasicEffect basicEffect;
        public virtual void DrawBoundsBoxs(List<BoundingBox> boxs)
        {
            VertexPositionColor[] points;
            short[] index;

            BuildBoxCorners(boxs, new List<Matrix>() { Matrix.Identity }, Color.White, out points, out index);

            if (basicEffect == null)
                basicEffect = new BasicEffect(GraphicsDevice);

            basicEffect.World = Matrix.CreateScale(voxel.Transform.Scale) *
                      Matrix.CreateTranslation(voxel.Transform.Position);
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.VertexColorEnabled = true;

            basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, points, 0, points.Length, index, 0, 12 * boxs.Count);
        }
        public void BuildBoxCorners(List<BoundingBox> boxs, List<Matrix> worlds, Color color, out VertexPositionColor[] points, out short[] index)
        {
            short[] BoxIndexMap = new short[] {
                0, 1, 0,
                2, 1, 3,
                2, 3, 4,
                5, 4, 6,
                5, 7, 6,
                7, 0, 4,
                1, 5, 2,
                6, 3, 7
                };

            points = new VertexPositionColor[boxs.Count * 8];
            short[] inds = new short[points.Length * 3];

            for (int b = 0; b < boxs.Count; b++)
            {
                Vector3[] thisCorners = boxs[b].GetCorners();

                points[(b * 8) + 0] = new VertexPositionColor(thisCorners[1], color);
                points[(b * 8) + 1] = new VertexPositionColor(thisCorners[0], color);
                points[(b * 8) + 2] = new VertexPositionColor(thisCorners[2], color);
                points[(b * 8) + 3] = new VertexPositionColor(thisCorners[3], color);
                points[(b * 8) + 4] = new VertexPositionColor(thisCorners[5], color);
                points[(b * 8) + 5] = new VertexPositionColor(thisCorners[4], color);
                points[(b * 8) + 6] = new VertexPositionColor(thisCorners[6], color);
                points[(b * 8) + 7] = new VertexPositionColor(thisCorners[7], color);

                for (int i = 0; i < 24; i++)
                {
                    inds[(b * 24) + i] = (short)(BoxIndexMap[i] + (b * 8));
                }
            }

            index = inds;
        }

        public BoundingBox TransformedBoundingBoxAA(BoundingBox box, ITransform transform)
        {
            Matrix AAWorld = Matrix.CreateScale(transform.Scale) * Matrix.CreateTranslation(transform.Position);

            Vector3 halfBlock = voxel.VoxelCentre- new Vector3(.5f, .5f, .5f);

            Vector3 min = Vector3.Transform(box.Min - halfBlock, AAWorld);
            Vector3 max = Vector3.Transform(box.Max - halfBlock, AAWorld);

            return new BoundingBox(min, max);
        }

        public Ray BuildRay(Point screenPixel)
        {
            Viewport vp = new Viewport(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            vp.MinDepth = _camera.Viewport.MinDepth;
            vp.MaxDepth = _camera.Viewport.MaxDepth;

            Vector3 nearSource = vp.Unproject(new Vector3(screenPixel.X, screenPixel.Y, camera.Viewport.MinDepth), camera.Projection, camera.View, Matrix.Identity);
            Vector3 farSource = vp.Unproject(new Vector3(screenPixel.X, screenPixel.Y, camera.Viewport.MaxDepth), camera.Projection, camera.View, Matrix.Identity);
            Vector3 direction = farSource - nearSource;

            direction.Normalize();

            return new Ray(nearSource, direction);
        }

        public float RayPicking(Point screenPixel, BoundingBox volume)
        {
            float? retVal = float.MaxValue;

            BuildRay(screenPixel).Intersects(ref volume, out retVal);

            if (retVal != null)
                return retVal.Value;
            else
                return float.MaxValue;
        }

        BoundingBox? selectedBox = null;

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_camera.ClearColor);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            if (selectedBox != null)
            {
                Vector3 halfBlock = new Vector3(.5f, .5f, .5f);

                List<BoundingBox> boxs = new List<BoundingBox>()
                {
                    selectedBox.Value
                };

                boxs = boxs.Select(s => new BoundingBox()
                {
                    Max = s.Max - voxel.VoxelCentre + halfBlock,
                    Min = s.Min - voxel.VoxelCentre + halfBlock,
                }).ToList();

                DrawBoundsBoxs(boxs);
            }

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
            line = DrawString($"Left Mouse Button - Delete selected chunk", line);

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
