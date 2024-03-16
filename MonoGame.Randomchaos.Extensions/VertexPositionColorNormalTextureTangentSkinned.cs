
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A vertex position color normal texture tangent skinned. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public struct VertexPositionColorNormalTextureTangentSkinned : IVertexType
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
        /// <summary>   The bone indices. </summary>
        public Byte4 BoneIndices;
        /// <summary>   The bone weights. </summary>
        public Vector4 BoneWeights;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="position">     The position. </param>
        /// <param name="normal">       The normal. </param>
        /// <param name="tangent">      The tangent. </param>
        /// <param name="texcoord">     The texcoord. </param>
        /// <param name="color">        The color. </param>
        /// <param name="boneIndices">  The bone indices. </param>
        /// <param name="boneWeights">  The bone weights. </param>
        ///-------------------------------------------------------------------------------------------------

        public VertexPositionColorNormalTextureTangentSkinned(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 texcoord, Color color, Byte4 boneIndices, Vector4 boneWeights)
        {
            Position = position;
            Normal = normal;
            TexCoord = texcoord;
            Tangent = tangent;
            Color = color.ToVector4();
            BoneIndices = boneIndices;
            BoneWeights = boneWeights;
        }

        /// <summary>   The vertex elements. </summary>
        static public VertexElement[] VertexElements = new VertexElement[]
        {
                new VertexElement(0,VertexElementFormat.Vector3,VertexElementUsage.Position,0),
                new VertexElement(4*3,VertexElementFormat.Vector3,VertexElementUsage.Normal,0),
                new VertexElement(4*6,VertexElementFormat.Vector2 ,VertexElementUsage.TextureCoordinate,0),
                new VertexElement(4*8,VertexElementFormat.Vector3,VertexElementUsage.Tangent,0),
                new VertexElement(4*11,VertexElementFormat.Vector4,VertexElementUsage.Color,0),
                new VertexElement(4*15,VertexElementFormat.Byte4,VertexElementUsage.BlendIndices,0),
                new VertexElement(4*16,VertexElementFormat.Vector4,VertexElementUsage.BlendWeight,0)
        };

        /// <summary>   The size in bytes. </summary>
        public static int SizeInBytes = (3 + 3 + 2 + 3 + 4 + 4 + 4) * 4;

        #region IVertexType Members

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
                        new VertexElement(4 * 11, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
                        new VertexElement(4 * 15, VertexElementFormat.Byte4, VertexElementUsage.BlendIndices, 0),
                        new VertexElement(4 * 16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0)
                        );
            }
        }
        #endregion
    }
}
