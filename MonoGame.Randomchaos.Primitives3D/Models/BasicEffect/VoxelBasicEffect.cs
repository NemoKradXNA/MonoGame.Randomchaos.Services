using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Primitives3D.Models.Voxel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    public class VoxelBasicEffect : GeometryVoxelBase<VertexPositionColorNormalTexture>
    {
        public ITransform Transform { get; set; }

        protected Texture2D _texture;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the texture. </summary>
        ///
        /// <value> The texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D Texture
        {
            get
            {
                if (_texture == null)
                {
                    _texture = new Texture2D(Game.GraphicsDevice, 1, 1);
                    _texture.SetData(new Color[] { Color.White });
                }

                return _texture;
            }

            set
            {
                _texture = value;
            }
        }

        public List<VoxelChunk> VisibleChunks
        {
            get
            {
                // get all visible chunks
                List<VoxelChunk> boxs = map.Cast<VoxelChunk>().Where(w => w.On && !w.IsTransparent).ToList();
                return boxs;
            }
        }

       

        public VoxelBasicEffect(Game game, int startBlockType = 0, int blocksWide = 10, int blocksHigh = 10, int blocksDeep = 10)
            : base(game, startBlockType, blocksWide, blocksHigh, blocksDeep)
        {
            Transform = new Transform();
        }

        protected override void LoadContent()
        {
            Effect = new BasicEffect(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void BuildData()
        {
            base.BuildData();

            int vCount = Vertices.Count;
            _vertexArray = new List<VertexPositionColorNormalTexture>();

            for (int v = 0; v < vCount; v++)
                _vertexArray.Add(new VertexPositionColorNormalTexture(Vertices[v], Colors[v], Normals[v], Texcoords[v]));
        }

        public override void SetEffect(GameTime gameTime)
        {
            ((BasicEffect)Effect).World = Transform.World;
            ((BasicEffect)Effect).View = _camera.View;
            ((BasicEffect)Effect).Projection = _camera.Projection;
            ((BasicEffect)Effect).VertexColorEnabled = true;
            ((BasicEffect)Effect).TextureEnabled = true;
            ((BasicEffect)Effect).Texture = Texture;
        }

        public void SetDirectionalLight(Vector3 direction, Vector3? ambientColor = null, Vector3? diffuseColor = null, Vector3? specularColor = null, float specularPower = 0, int light = 0, bool enable = true, bool preferPerPixelLighting = true)
        {
            ((BasicEffect)Effect).LightingEnabled = true;

            ((BasicEffect)Effect).PreferPerPixelLighting = preferPerPixelLighting;

            if (ambientColor != null)
            {
                ((BasicEffect)Effect).AmbientLightColor = ambientColor.Value;
            }

            if (specularColor != null)
            {
                ((BasicEffect)Effect).SpecularColor = specularColor.Value;
                ((BasicEffect)Effect).SpecularPower = specularPower;
            }

            switch (light)
            {
                case 0:
                    if (diffuseColor != null)
                    {
                        ((BasicEffect)Effect).DirectionalLight0.DiffuseColor = diffuseColor.Value;
                    }
                    else
                    {
                        ((BasicEffect)Effect).DirectionalLight0.DiffuseColor = Vector3.One;
                    }

                    ((BasicEffect)Effect).DirectionalLight0.Direction = direction;
                    ((BasicEffect)Effect).DirectionalLight0.Enabled = enable;
                    break;
                case 1:
                    if (diffuseColor != null)
                    {
                        ((BasicEffect)Effect).DirectionalLight1.DiffuseColor = diffuseColor.Value;
                    }

                    ((BasicEffect)Effect).DirectionalLight1.Direction = direction;
                    ((BasicEffect)Effect).DirectionalLight1.Enabled = enable;
                    break;
                case 2:
                    if (diffuseColor != null)
                    {
                        ((BasicEffect)Effect).DirectionalLight2.DiffuseColor = diffuseColor.Value;
                    }

                    ((BasicEffect)Effect).DirectionalLight2.Direction = direction;
                    ((BasicEffect)Effect).DirectionalLight2.Enabled = enable;
                    break;
            }
        }

        bool rebuild = false;
        bool rebuilding = false;
        public void ReBuild()
        {
            rebuild = true;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible && Vertices.Count > 0)
            {
                SetEffect(gameTime);

                Effect.CurrentTechnique.Passes[0].Apply();

                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertexArray.ToArray(), 0, _vertexArray.Count, Indicies.ToArray(), 0, Indicies.Count / 3);
            }

            if (rebuild)
            {
                GeometryVoxelBase<VertexPositionColorNormalTexture> rebuildData = new GeometryVoxelBase<VertexPositionColorNormalTexture>(Game, _blocksWide, _blocksHigh, _blocksDeep);

                rebuildData.map = map;

                rebuildData.Build();

                Vertices = rebuildData.Vertices;
                Normals = rebuildData.Normals;
                Colors = rebuildData.Colors;
                Texcoords = rebuildData.Texcoords;
                Indicies = rebuildData.Indicies;

                int vCount = Vertices.Count;
                _vertexArray = new List<VertexPositionColorNormalTexture>();

                for (int v = 0; v < vCount; v++)
                    _vertexArray.Add(new VertexPositionColorNormalTexture(Vertices[v], Colors[v], Normals[v], Texcoords[v]));

                rebuildData = null;
                rebuild = false;
            }
        }

    }
}
