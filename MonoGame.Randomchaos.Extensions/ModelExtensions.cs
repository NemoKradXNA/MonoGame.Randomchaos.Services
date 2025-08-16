
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A model extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 16/08/2025. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class ModelExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Model extension method that creates bounding box. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/08/2025. </remarks>
        ///
        /// <param name="model">    The model to act on. </param>
        ///
        /// <returns>   The new bounding box. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BoundingBox CreateBoundingBox(this Model model)
        {
            var positions = new List<Vector3>();

            foreach (var mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    var vb = part.VertexBuffer;
                    int stride = vb.VertexDeclaration.VertexStride;

                    // Find the vertex element that contains position
                    var posElem = vb.VertexDeclaration.GetVertexElements()
                        .First(e => e.VertexElementUsage == VertexElementUsage.Position &&
                                    e.VertexElementFormat == VertexElementFormat.Vector3);

                    // Create an array for exactly NumVertices positions
                    var tempPositions = new Vector3[part.NumVertices];

                    // Read them directly from the vertex buffer
                    vb.GetData(
                        offsetInBytes: (part.VertexOffset * stride) + posElem.Offset,
                        data: tempPositions,
                        startIndex: 0,
                        elementCount: tempPositions.Length,
                        vertexStride: stride
                    );

                    positions.AddRange(tempPositions);
                }
            }

            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            foreach (var v in positions)
            {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }

            return new BoundingBox(min, max);
        }
    }
}
