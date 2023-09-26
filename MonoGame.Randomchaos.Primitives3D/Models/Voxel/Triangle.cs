
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Primitives3D.Models.Voxel
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A triangle. </summary>
    ///
    /// <remarks>   Charles Humphrey, 25/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Triangle
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the point 1. </summary>
        ///
        /// <value> The point 1. </value>
        ///-------------------------------------------------------------------------------------------------

        public VertexPoint Point1 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the point 2. </summary>
        ///
        /// <value> The point 2. </value>
        ///-------------------------------------------------------------------------------------------------

        public VertexPoint Point2 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the point 3. </summary>
        ///
        /// <value> The point 3. </value>
        ///-------------------------------------------------------------------------------------------------

        public VertexPoint Point3 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the center. </summary>
        ///
        /// <value> The center. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Center
        {
            get
            {
                return (Point1.Position + Point2.Position + Point3.Position) / 3;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the normal. </summary>
        ///
        /// <value> The normal. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Normal
        {
            get
            {
                return (Point1.Normal + Point2.Normal + Point3.Normal) / 3;
            }
        }

        public Plane Plane
        {
            get
            {
                return new Plane(Point1.Position, Point2.Position, Point3.Position);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 25/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Triangle() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 25/09/2023. </remarks>
        ///
        /// <param name="point1">   The first point. </param>
        /// <param name="point2">   The second point. </param>
        /// <param name="point3">   The third point. </param>
        ///-------------------------------------------------------------------------------------------------

        public Triangle(VertexPoint point1, VertexPoint point2, VertexPoint point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
        }

        public bool ContansPoint(Vector3 point, Matrix? transform = null)
        {
            if (transform == null)
            {
                transform = Matrix.Identity;
            }

            //Vector3 a = Vector3.Transform(Point1.Position, transform.Value);
            //Vector3 b = Vector3.Transform(Point2.Position, transform.Value);
            //Vector3 c = Vector3.Transform(Point3.Position, transform.Value);
            //Vector3 p = point;

            //// Compute vectors        
            //Vector3 v0 = c - a;
            //Vector3 v1 = b - a;
            //Vector3 v2 = p - a;

            //// Compute dot products
            //float dot00 = Vector3.Dot(v0, v0);
            //float dot01 = Vector3.Dot(v0, v1);
            //float dot02 = Vector3.Dot(v0, v2);
            //float dot11 = Vector3.Dot(v1, v1);
            //float dot12 = Vector3.Dot(v1, v2);

            //// Compute barycentric coordinates
            //float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            //float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            //float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            //// Check if point is in triangle
            //bool inTriangle = (u >= 0) && (v >= 0) && (u + v < 1);
            //return inTriangle;

            Vector3 a = Vector3.Transform(Point1.Position, transform.Value);
            Vector3 b = Vector3.Transform(Point2.Position, transform.Value);
            Vector3 c = Vector3.Transform(Point3.Position, transform.Value);

            a -= point;
            b -= point;
            c -= point;

            Vector3 u = Vector3.Cross(b, c);
            Vector3 v = Vector3.Cross(c, a);
            Vector3 w = Vector3.Cross(a, b);


            if (Vector3.Dot(u, v) < 0.0f)
            {
                return false;
            }

            if (Vector3.Dot(u, w) < 0.0f)
            {
                return false;
            }

            return true;
        }

    }
}
