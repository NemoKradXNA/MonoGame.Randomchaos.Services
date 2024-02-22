
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Randomchaos.Mesh
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   This is the analogue to ModelMeshPart. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BaseModelMeshPart
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color. </summary>
        ///
        /// <value> The color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Color { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the effect. </summary>
        ///
        /// <value> The effect. </value>
        ///-------------------------------------------------------------------------------------------------

        public Effect Effect { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the buffer for index data. </summary>
        ///
        /// <value> A buffer for index data. </value>
        ///-------------------------------------------------------------------------------------------------

        public IndexBuffer IndexBuffer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the number of vertices. </summary>
        ///
        /// <value> The total number of vertices. </value>
        ///-------------------------------------------------------------------------------------------------

        public int NumVertices { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the number of primitives. </summary>
        ///
        /// <value> The number of primitives. </value>
        ///-------------------------------------------------------------------------------------------------

        public int PrimitiveCount { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the zero-based index of the start. </summary>
        ///
        /// <value> The start index. </value>
        ///-------------------------------------------------------------------------------------------------

        public int StartIndex { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tag. </summary>
        ///
        /// <value> The tag. </value>
        ///-------------------------------------------------------------------------------------------------

        public object Tag { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the buffer for vertex data. </summary>
        ///
        /// <value> A buffer for vertex data. </value>
        ///-------------------------------------------------------------------------------------------------

        public VertexBuffer VertexBuffer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the vertex offset. </summary>
        ///
        /// <value> The vertex offset. </value>
        ///-------------------------------------------------------------------------------------------------

        public int VertexOffset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the texture. </summary>
        ///
        /// <value> The texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public string TextureAsset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMeshPart() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="part"> The part. </param>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMeshPart(ModelMeshPart part)
        {
            Effect = part.Effect;
            IndexBuffer = part.IndexBuffer;
            NumVertices = part.NumVertices;
            PrimitiveCount = part.PrimitiveCount;
            StartIndex = part.StartIndex;
            Tag = part.Tag;
            VertexBuffer = part.VertexBuffer;
            VertexOffset = part.VertexOffset;
        }
    }
}
