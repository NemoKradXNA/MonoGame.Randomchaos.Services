using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    /// <summary>
    /// This class holds all the data needed to render a Cube
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GeometryCubeBase<T> : GeometryBase<T> where T : IVertexType
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="game"></param>
        public GeometryCubeBase(Game game) : base(game) { }

        /// <summary>
        /// LoadContent method
        /// </summary>
        protected override void LoadContent()
        {
            Vertices = new List<Vector3>()
            {
                new Vector3(-.5f, .5f, .5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, -.5f, .5f),new Vector3(-.5f, -.5f, .5f),
                new Vector3(-.5f, .5f, -.5f), new Vector3(.5f, .5f, -.5f), new Vector3(.5f, -.5f, -.5f), new Vector3(-.5f, -.5f, -.5f),
                new Vector3(-.5f, .5f, .5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, .5f, -.5f),new Vector3(-.5f, .5f, -.5f),
                new Vector3(-.5f, -.5f, .5f),new Vector3(.5f, -.5f, .5f),new Vector3(.5f, -.5f, -.5f),new Vector3(-.5f, -.5f, -.5f),
                new Vector3(-.5f, .5f, -.5f),new Vector3(-.5f, .5f, .5f),new Vector3(-.5f, -.5f, .5f),new Vector3(-.5f, -.5f, -.5f),
                new Vector3(.5f, .5f, -.5f),new Vector3(.5f, .5f, .5f),new Vector3(.5f, -.5f, .5f),new Vector3(.5f, -.5f, -.5f)
            };

            Normals = new List<Vector3>()
            {
                Vector3.Backward,Vector3.Backward,Vector3.Backward,Vector3.Backward,
                Vector3.Forward,Vector3.Forward,Vector3.Forward,Vector3.Forward,
                Vector3.Up,Vector3.Up,Vector3.Up,Vector3.Up,
                Vector3.Down,Vector3.Down,Vector3.Down,Vector3.Down,
                Vector3.Left,Vector3.Left,Vector3.Left,Vector3.Left,
                Vector3.Right,Vector3.Right,Vector3.Right,Vector3.Right,
            };

            Texcoords = new List<Vector2>()
            {
                new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1),
                new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1),
                new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1),
                new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1),
                new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1),
                new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1),
            };

            if (Colors == null)
            {
                Colors = new List<Color>();
                for (int v = 0; v < Vertices.Count; v++)
                    Colors.Add(Color.White);
            }

            Indicies = new List<int>()
            {
                0, 1, 2, 2, 3, 0, // Front
                4, 7, 6, 6, 5, 4, // Back
                8, 11, 10, 10, 9, 8, // Top
                12, 13, 14, 14, 15, 12, // Bottom
                16, 17, 18, 18, 19, 16, // Left
                20, 23, 22, 22, 21, 20, // Right
            };

            base.LoadContent();
        }
    }
}
