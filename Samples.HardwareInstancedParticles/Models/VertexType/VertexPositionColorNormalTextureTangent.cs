


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardwareInstancedParticles.Models.VertexType
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A vertex position color normal texture tangent. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public struct VertexPositionColorNormalTextureTangent : IVertexType
    {
        /// <summary>   The position. </summary>
        public Vector3 Position;
        /// <summary>   The normal. </summary>
        public Vector3 Normal;
        /// <summary>   The tex coordinate. </summary>
        public Vector2 TexCoord;
        /// <summary>   The tangent. </summary>
        public Vector3 Tangent;
        /// <summary>   The color. </summary>
        public Vector4 Color;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="position"> The position. </param>
        /// <param name="normal">   The normal. </param>
        /// <param name="tangent">  The tangent. </param>
        /// <param name="texcoord"> The texcoord. </param>
        /// <param name="color">    The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public VertexPositionColorNormalTextureTangent(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 texcoord, Color color)
        {
            Position = position;
            Normal = normal;
            TexCoord = texcoord;
            Tangent = tangent;
            Color = color.ToVector4();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the size in bytes. </summary>
        ///
        /// <value> The size in bytes. </value>
        ///-------------------------------------------------------------------------------------------------

        public int SizeInBytes { get { return sizeof(float) * 15; } }

        #region IVertexType Members

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the vertical decrement. </summary>
        ///
        /// <value> The vertical decrement. </value>
        ///-------------------------------------------------------------------------------------------------

        public static VertexDeclaration VertDec
        {
            get
            {
                return new VertexDeclaration
                        (
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(4 * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                        new VertexElement(4 * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                        new VertexElement(4 * 8, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
                        new VertexElement(4 * 11, VertexElementFormat.Vector4, VertexElementUsage.Color, 0)
                        );
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the vertex declaration. </summary>
        ///
        /// <value> The vertex declaration. </value>
        ///-------------------------------------------------------------------------------------------------

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return new VertexDeclaration
                        (
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(4 * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                        new VertexElement(4 * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                        new VertexElement(4 * 8, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
                        new VertexElement(4 * 11, VertexElementFormat.Vector4, VertexElementUsage.Color, 0)
                        );
            }
        }
        #endregion
    }
}
