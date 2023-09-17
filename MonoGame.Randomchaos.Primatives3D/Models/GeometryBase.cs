using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    public class GeometryBase<T> : DrawableGameComponent where T : IVertexType
    {
        protected ICameraService _camera { get { return Game.Services.GetService<ICameraService>(); } }

        /// <summary>
        /// All the position data four our vertices's
        /// </summary>
        public List<Vector3> Vertices { get; protected set; }

        /// <summary>
        /// All the normals for our vertices
        /// </summary>
        public List<Vector3> Normals { get; protected set; }

        /// <summary>
        /// All the texture coordinates for our vertices
        /// </summary>
        public List<Vector2> Texcoords { get; protected set; }

        /// <summary>
        /// All the tangents for our vertices
        /// </summary>
        public List<Vector3> Tangents { get; protected set; }

        /// <summary>
        /// All the colours for our vertices
        /// </summary>
        public List<Color> Colors { get; protected set; }

        /// <summary>
        /// The index or draw order used to draw our vertices
        /// </summary>
        public List<int> Indicies { get; protected set; }

        /// <summary>
        /// If true, normals will be calculated for us.
        /// </summary>
        public bool AutoCalculateNormals { get; set; } = false;

        /// <summary>
        /// If true, tangents will be calculated for us.
        /// </summary>
        public bool AutoCalculateTangents { get; set; } = false;

        /// <summary>
        /// The compiles shader to be used
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// Vertex array for the given IVertexType
        /// </summary>
        protected List<T> _vertexArray { get; set; }

        public GeometryBase(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            BuildData();
        }

        //public abstract void SetEffect(GameTime gameTime);
        public virtual void SetEffect(GameTime gameTime) { }

        public virtual void BuildData()
        {
            if (AutoCalculateNormals)
            {
                CalculateNormals();
            }

            if (AutoCalculateTangents)
            {
                CalculateTangents();
            }
        }

        /// <summary>
        /// Some very useful functions I found here, for calculating tangent and normals data from the vertex and texture coordinate lists.
        /// https://gamedev.stackexchange.com/questions/68612/how-to-compute-tangent-and-bitangent-vectors
        /// http://foundationsofgameenginedev.com/FGED2-sample.pdf 
        /// </summary>
        public virtual void CalculateTangents()
        {
            int triangleCount = Indicies.Count;
            int vertexCount = Vertices.Count;

            Vector3[] tan1 = new Vector3[vertexCount];

            for (int i = 0; i < triangleCount; i += 3)
            {
                // Get the index for this triangle.
                int i1 = Indicies[i + 0];
                int i2 = Indicies[i + 1];
                int i3 = Indicies[i + 2];

                // Get the positions of each vertex.
                Vector3 v1 = Vertices[i1];
                Vector3 v2 = Vertices[i2];
                Vector3 v3 = Vertices[i3];

                // Get the texture coordinates of each vertex.
                Vector2 w1 = Texcoords[i1];
                Vector2 w2 = Texcoords[i2];
                Vector2 w3 = Texcoords[i3];

                // Calculate the vertex directions
                Vector3 vd1 = v2 - v1;
                Vector3 vd2 = v3 - v1;

                // Calculate the texture coordinates directions.
                Vector2 td1 = w2 - w1;
                Vector2 td2 = w3 - w1;

                // Calculate final direction.
                Vector3 dir = ((vd1 * td2.Y) - (vd2 * td1.Y));

                dir.Normalize();

                // Store ready to be returned in vertex order.
                tan1[i1] += dir;
                tan1[i2] += dir;
                tan1[i3] += dir;
            }

            Tangents = new List<Vector3>();

            // Populate tangents in vertex order. 
            for (int v = 0; v < vertexCount; v++)
                Tangents.Add(tan1[v]);
        }

        public virtual void CalculateNormals()
        {
            Normals = new List<Vector3>();

            // clear out the normals
            foreach (Vector3 v in Vertices)
                Normals.Add(Vector3.Zero);

            // Calculate the new normals.
            foreach (Vector3 v in Vertices)
            {
                for (int i = 0; i < Indicies.Count; i += 3)
                {
                    int idxA = Indicies[i];
                    int idxB = Indicies[i + 1];
                    int idxC = Indicies[i + 2];

                    Vector3 A = Vertices[idxA];
                    Vector3 B = Vertices[idxB];
                    Vector3 C = Vertices[idxC];

                    Vector3 p = Vector3.Cross(C - A, B - A);
                    Normals[idxA] += p;
                    Normals[idxB] += p;
                    Normals[idxC] += p;
                }

            }

            // Normalize
            foreach (Vector3 v in Normals)
                v.Normalize();
        }

        
    }
}
