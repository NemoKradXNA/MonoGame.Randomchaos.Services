
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   This class holds all the data needed to render a Triangle. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///
    /// <typeparam name="T">    . </typeparam>
    ///-------------------------------------------------------------------------------------------------

    public abstract class GeometryTriangleBase<T> : GeometryBase<T> where T : IVertexType
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   ctor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> . </param>
        ///-------------------------------------------------------------------------------------------------

        public GeometryTriangleBase(Game game) : base(game) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   LoadContent method. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            // Our three points of the triangle.
            Vertices = new List<Vector3>()
            {
                new Vector3(0,.5f,0),
                new Vector3(.5f,-.5f,0),
                new Vector3(-.5f,-.5f,0)
            };

            // In MonoGamw a negative Z points out of the scene and a positive Z into the scene.
            // our camera will be behind the geometry, so we want the normals point in negative Z.
            // For more complex geometry, we will calculate these values.
            Normals = new List<Vector3>()
            {
                Vector3.Backward,
                Vector3.Backward,
                Vector3.Backward
            };

            // The texture coordinates of the triangle points.
            Texcoords = new List<Vector2>()
            {
                new Vector2(.5f,0f),
                new Vector2(1f,1f),
                new Vector2(0f, 1f)
            };

            // The colors for each triangle point.
            if (Colors == null)
            {
                Colors = new List<Color>();

                for (int v = 0; v < Vertices.Count; v++)
                {
                    Colors.Add(Color.White);
                }
            }

            // The draw order of the vertices.
            Indicies = new List<int>()
            {
                0, 1, 2
            };

            base.LoadContent();
        }
    }
}
