
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MonoGame.Randomchaos.Animation.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation3D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A data Model for the randomchaos. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class RandomchaosModelData : IRandomchaosModelData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the vertices. </summary>
        ///
        /// <value> The vertices. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Vector3>> Vertices { get; set; } = new Dictionary<int, List<Vector3>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tex coordinates. </summary>
        ///
        /// <value> The tex coordinates. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Vector2>> TexCoords { get; set; } = new Dictionary<int, List<Vector2>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the normals. </summary>
        ///
        /// <value> The normals. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Vector3>> Normals { get; set; } = new Dictionary<int, List<Vector3>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tangents. </summary>
        ///
        /// <value> The tangents. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Vector3>> Tangents { get; set; } = new Dictionary<int, List<Vector3>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bi normals. </summary>
        ///
        /// <value> The bi normals. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Vector3>> BiNormals { get; set; } = new Dictionary<int, List<Vector3>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the zero-based index of the blend. </summary>
        ///
        /// <value> The blend index. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Byte4>> BlendIndex { get; set; } = new Dictionary<int, List<Byte4>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the blend weight. </summary>
        ///
        /// <value> The blend weight. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Vector4>> BlendWeight { get; set; } = new Dictionary<int, List<Vector4>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the colors. </summary>
        ///
        /// <value> The colors. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<Color>> Colors { get; set; } = new Dictionary<int, List<Color>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the indicies. </summary>
        ///
        /// <value> The indicies. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, List<int>> Indicies { get; set; } = new Dictionary<int, List<int>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transforms. </summary>
        ///
        /// <value> The transforms. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Matrix> Transforms { get; set; } = new List<Matrix>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bounding boxs. </summary>
        ///
        /// <value> The bounding boxs. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<BoundingBox> BoundingBoxs { get; set; } = new List<BoundingBox>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bounding spheres. </summary>
        ///
        /// <value> The bounding spheres. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<BoundingSphere> BoundingSpheres { get; set; } = new List<BoundingSphere>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the skinning. </summary>
        ///
        /// <value> Information describing the skinning. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISkinningData SkinningData { get; set; } = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the names. </summary>
        ///
        /// <value> The names. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> Names { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tag. </summary>
        ///
        /// <value> The tag. </value>
        ///-------------------------------------------------------------------------------------------------

        public object Tag { get; set; } = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the textures. </summary>
        ///
        /// <value> The textures. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<int, Dictionary<string,string>> Textures { get; set; }
    }
}
