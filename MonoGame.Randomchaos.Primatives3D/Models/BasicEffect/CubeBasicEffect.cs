using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    /// <summary>
    /// Renders a Cube using the build in BasicEffect
    /// </summary>
    public class CubeBasicEffect : GeometryCubeBase<VertexPositionColorTexture>
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

        public CubeBasicEffect(Game game) : base(game) { Transform = new Transform(); }

        protected override void LoadContent()
        {
            Effect = new BasicEffect(Game.GraphicsDevice);
            Colors = new List<Color>()
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow,
                Color.Red, Color.Blue, Color.Green, Color.Yellow
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
