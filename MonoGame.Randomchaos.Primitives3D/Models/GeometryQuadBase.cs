
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   This class holds all the data needed to render a Quad. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///
    /// <typeparam name="T">    . </typeparam>
    ///-------------------------------------------------------------------------------------------------

    public class GeometryQuadBase<T> : GeometryBase<T> where T : IVertexType
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   ctor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> . </param>
        ///-------------------------------------------------------------------------------------------------

        public GeometryQuadBase(Game game) : base(game) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   LoadContent method. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            Vertices = new List<Vector3>()
            {
                new Vector3(-.5f,.5f,0),
                new Vector3(.5f,.5f,0),
                new Vector3(.5f,-.5f,0),
                new Vector3(-.5f,-.5f,0)
            };

            Normals = new List<Vector3>()
            {
                Vector3.Backward,
                Vector3.Backward,
                Vector3.Backward,
                Vector3.Backward
            };

            Texcoords = new List<Vector2>()
            {
                new Vector2(0,0),
                new Vector2(1,0f),
                new Vector2(1f, 1f),
                new Vector2(0f, 1f)
            };

            if (Colors == null)
            {
                Colors = new List<Color>();

                for (int v = 0; v < Vertices.Count; v++)
                {
                    Colors.Add(Color.White);
                }
            }

            Indicies = new List<int>()
            {
                0, 1, 2, 2, 3, 0
            };

            base.LoadContent();
        }
    }
}
