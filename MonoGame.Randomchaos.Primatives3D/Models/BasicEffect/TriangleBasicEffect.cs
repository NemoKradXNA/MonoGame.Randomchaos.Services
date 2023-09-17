using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    /// <summary>
    ///  Renders a Triangle using the build in BasicEffect
    /// </summary>
    public class TriangleBasicEffect : GeometryTriangleBase<VertexPositionColorTexture>
    {
        public ITransform Transform { get; set; }

        protected Texture2D _texture;
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

        public TriangleBasicEffect(Game game) : base(game) { Transform = new Transform(); }

        protected override void LoadContent()
        {
            Effect = new BasicEffect(Game.GraphicsDevice);

            Colors = new List<Color>()
            {
                Color.Red,
                Color.Blue,
                Color.Green
            };
            base.LoadContent();
        }

        public override void BuildData()
        {
            base.BuildData();

            int vCount = Vertices.Count;
            _vertexArray = new List<VertexPositionColorTexture>();

            for (int v = 0; v < vCount; v++)
                _vertexArray.Add(new VertexPositionColorTexture(Vertices[v], Colors[v], Texcoords[v]));
        }

        public override void SetEffect(GameTime gameTime)
        {
            base.SetEffect(gameTime);

            ((BasicEffect)Effect).World = Transform.World;
            ((BasicEffect)Effect).View = _camera.View;
            ((BasicEffect)Effect).Projection = _camera.Projection;
            ((BasicEffect)Effect).VertexColorEnabled = true;
            ((BasicEffect)Effect).TextureEnabled = true;
            ((BasicEffect)Effect).Texture = Texture;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                SetEffect(gameTime);

                Effect.CurrentTechnique.Passes[0].Apply();

                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertexArray.ToArray(), 0, _vertexArray.Count, Indicies.ToArray(), 0, Indicies.Count / 3);
            }
        }
    }
}
