using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces.Interfaces;

namespace MonoGame.Randomchaos.Interfaces.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A triangle. </summary>
    ///
    /// <remarks>   Charles Humphrey, 25/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Triangle : ITriangle
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the plane. </summary>
        ///
        /// <value> The plane. </value>
        ///-------------------------------------------------------------------------------------------------

        public Plane Plane
        {
            get
            {
                return new Plane(Point1.Position, Point2.Position, Point3.Position);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            return $"({Point1.Position},{Point2.Position},{Point3.Position})";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="transform">    The transform. </param>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ToString(Matrix transform)
        {
            return $"{Point1.ToString(transform)},{Point2.ToString(transform)},{Point3.ToString(transform)}";
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Contans point. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="point">        The point. </param>
        /// <param name="transform">    (Optional) The transform. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool ContansPoint(Vector3 point, Matrix? transform = null)
        {
            if (transform == null)
            {
                transform = Matrix.Identity;
            }

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

            if (Vector3.Dot(v, w) < 0.0f)
            {
                return false;
            }

            return true;
        }
    }
}
