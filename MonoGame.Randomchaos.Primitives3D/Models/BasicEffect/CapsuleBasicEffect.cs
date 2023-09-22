
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A capsule basic effect. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CapsuleBasicEffect : GeometryCapsuleBase<VertexPositionColorNormalTexture>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        public ITransform Transform { get; set; }

        /// <summary>   The texture. </summary>
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public CapsuleBasicEffect(Game game) : base(game) { Transform = new Transform(); }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   LoadContent method. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            Effect = new BasicEffect(Game.GraphicsDevice);

            base.LoadContent();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void BuildData()
        {
            base.BuildData();

            int vCount = Vertices.Count;
            _vertexArray = new List<VertexPositionColorNormalTexture>();

            for (int v = 0; v < vCount; v++)
                _vertexArray.Add(new VertexPositionColorNormalTexture(Vertices[v], Colors[v], Normals[v], Texcoords[v]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets an effect. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void SetEffect(GameTime gameTime)
        {
            ((BasicEffect)Effect).World = Transform.World;
            ((BasicEffect)Effect).View = _camera.View;
            ((BasicEffect)Effect).Projection = _camera.Projection;
            ((BasicEffect)Effect).VertexColorEnabled = true;
            ((BasicEffect)Effect).TextureEnabled = true;
            ((BasicEffect)Effect).Texture = Texture;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets directional light. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="direction">                The direction. </param>
        /// <param name="ambientColor">             (Optional) The ambient color. </param>
        /// <param name="diffuseColor">             (Optional) The diffuse color. </param>
        /// <param name="specularColor">            (Optional) The specular color. </param>
        /// <param name="specularPower">            (Optional) The specular power. </param>
        /// <param name="light">                    (Optional) The light. </param>
        /// <param name="enable">                   (Optional) True to enable, false to disable. </param>
        /// <param name="preferPerPixelLighting">   (Optional) True to prefer per pixel lighting. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

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
