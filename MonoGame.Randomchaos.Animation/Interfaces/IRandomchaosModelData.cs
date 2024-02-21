
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for randomchaos model. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IRandomchaosModelData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the vertices. </summary>
        ///
        /// <value> The vertices. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Vector3>> Vertices { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tex coordinates. </summary>
        ///
        /// <value> The tex coordinates. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Vector2>> TexCoords { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the normals. </summary>
        ///
        /// <value> The normals. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Vector3>> Normals { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tangents. </summary>
        ///
        /// <value> The tangents. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Vector3>> Tangents { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bi normals. </summary>
        ///
        /// <value> The bi normals. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Vector3>> BiNormals { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the zero-based index of the blend. </summary>
        ///
        /// <value> The blend index. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Byte4>> BlendIndex { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the blend weight. </summary>
        ///
        /// <value> The blend weight. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Vector4>> BlendWeight { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the colors. </summary>
        ///
        /// <value> The colors. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<Color>> Colors { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the indicies. </summary>
        ///
        /// <value> The indicies. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, List<int>> Indicies { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transforms. </summary>
        ///
        /// <value> The transforms. </value>
        ///-------------------------------------------------------------------------------------------------

        List<Matrix> Transforms { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bounding boxs. </summary>
        ///
        /// <value> The bounding boxs. </value>
        ///-------------------------------------------------------------------------------------------------

        List<BoundingBox> BoundingBoxs { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bounding spheres. </summary>
        ///
        /// <value> The bounding spheres. </value>
        ///-------------------------------------------------------------------------------------------------

        List<BoundingSphere> BoundingSpheres { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the skinning. </summary>
        ///
        /// <value> Information describing the skinning. </value>
        ///-------------------------------------------------------------------------------------------------

        ISkinningData SkinningData { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the names. </summary>
        ///
        /// <value> The names. </value>
        ///-------------------------------------------------------------------------------------------------

        List<string> Names { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the textures. </summary>
        ///
        /// <value> The textures. </value>
        ///-------------------------------------------------------------------------------------------------

        Dictionary<int, Dictionary<string, string>> Textures { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tag. </summary>
        ///
        /// <value> The tag. </value>
        ///-------------------------------------------------------------------------------------------------

        object Tag { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        string Name { get; set; }
    }
}
