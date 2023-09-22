
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A geometry plane base. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    ///-------------------------------------------------------------------------------------------------

    public class GeometryPlaneBase<T> : GeometryBase<T> where T : IVertexType
    {

        /// <summary>   Size of the square. </summary>
        protected int SquareSize = 10;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="game">         The game. </param>
        /// <param name="squareSize">   (Optional) Size of the square. </param>
        ///-------------------------------------------------------------------------------------------------

        public GeometryPlaneBase(Game game, int squareSize = 10) :
            base(game)
        {
            SquareSize = squareSize;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            int[] index = new int[(SquareSize - 1) * (SquareSize - 1) * 6];
            Vector2 uv = Vector2.Zero;
            Vector3 center = new Vector3(SquareSize - 1, 0, SquareSize - 1) * .5f;

            Vertices = new List<Vector3>();

            Normals = new List<Vector3>();

            Texcoords = new List<Vector2>();

            Colors = new List<Color>();

            for (int x = 0; x < SquareSize; x++)
            {
                for (int y = 0; y < SquareSize; y++)
                {
                    uv = new Vector2(x, y) / (float)(SquareSize - 1);
                    Vertices.Add(new Vector3(x, 0, y) - center);
                    Normals.Add(Vector3.Up);
                    Texcoords.Add(uv);
                    Colors.Add(Color.White);
                }
            }



            for (int x = 0; x < SquareSize - 1; x++)
            {
                for (int y = 0; y < SquareSize - 1; y++)
                {
                    index[(x + y * (SquareSize - 1)) * 6] = ((x + 1) + (y + 1) * SquareSize);
                    index[(x + y * (SquareSize - 1)) * 6 + 1] = ((x + 1) + y * SquareSize);
                    index[(x + y * (SquareSize - 1)) * 6 + 2] = (x + y * SquareSize);

                    index[(x + y * (SquareSize - 1)) * 6 + 3] = ((x + 1) + (y + 1) * SquareSize);
                    index[(x + y * (SquareSize - 1)) * 6 + 4] = (x + y * SquareSize);
                    index[(x + y * (SquareSize - 1)) * 6 + 5] = (x + (y + 1) * SquareSize);
                }
            }

            Indicies = new List<int>(index);

            base.LoadContent();
        }
    }
}
