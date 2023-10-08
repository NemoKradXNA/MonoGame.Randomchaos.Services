
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Interfaces.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A geometry lines. </summary>
    ///
    /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class GeometryLines
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the camera. </summary>
        ///
        /// <value> The camera. </value>
        ///-------------------------------------------------------------------------------------------------

        protected ICameraService camera { get { return _game.Services.GetService<ICameraService>(); } }

        /// <summary>   The basic effect. </summary>
        private BasicEffect basicEffect;
        /// <summary>   (Immutable) the game. </summary>
        private readonly Game _game;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public GeometryLines(Game game)
        {
            _game = game;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draw bounds boxs. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="boxs">         The boxs. </param>
        /// <param name="transform">    The transform. </param>
        /// <param name="color">        (Optional) The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void DrawBoundsBoxs(List<BoundingBox> boxs, ITransform transform, Color? color = null)
        {
            if (color == null)
            {
                color = Color.White;
            }

            VertexPositionColor[] points;
            short[] index;

            BuildBoxCorners(boxs, color.Value, out points, out index);

            DrawPoints(points, index, boxs.Count * 12, transform);
        }

        public void DrawBoundsSpheres(List<BoundingSphere> spheres, ITransform transform)
        {
            VertexPositionColor[] points;
            short[] index;


            int segments = 64;

            BuildSphere(spheres, segments, Color.Red, out points, out index, true);
            DrawPoints(points, index, segments * 3, transform);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draw points. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="points">       [out] The points. </param>
        /// <param name="index">        [out] Zero-based index of the. </param>
        /// <param name="primatives">   The primatives. </param>
        /// <param name="transform">    The transform. </param>
        ///-------------------------------------------------------------------------------------------------

        public void DrawPoints(VertexPositionColor[] points, short[] index, int primatives, ITransform transform)
        {
            if (basicEffect == null)
                basicEffect = new BasicEffect(_game.GraphicsDevice);

            basicEffect.World = Matrix.CreateScale(transform.Scale) *
                      Matrix.CreateTranslation(transform.Position);
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.VertexColorEnabled = true;

            basicEffect.CurrentTechnique.Passes[0].Apply();
            _game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, points, 0, points.Length, index, 0, primatives);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draw triangles. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="triangles">    The triangles. </param>
        /// <param name="transform">    The transform. </param>
        /// <param name="color">        (Optional) The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public void DrawTriangles(List<Triangle> triangles, ITransform transform, Color? color = null)
        {
            if (color == null)
            {
                color = Color.GreenYellow;
            }

            VertexPositionColor[] points = points = new VertexPositionColor[triangles.Count * 3];
            short[] index = new short[triangles.Count * 6];
            short[] idx = new short[] { 0, 1, 1, 2, 2, 0 };

            for (int t = 0; t < triangles.Count; t++)
            {
                Triangle triangle = triangles[t];

                points[(t * 3) + 0] = new VertexPositionColor(triangle.Point1.Position, color.Value);
                points[(t * 3) + 1] = new VertexPositionColor(triangle.Point2.Position, color.Value);
                points[(t * 3) + 2] = new VertexPositionColor(triangle.Point3.Position, color.Value);

                for (int i = 0; i < 6; i++)
                {
                    index[(t * 6) + i] = (short)(idx[i] + (t * 3));
                }
            }

            DrawPoints(points, index, triangles.Count * 3, transform);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds box corners. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="boxs">     The boxs. </param>
        /// <param name="color">    The color. </param>
        /// <param name="points">   [out] The points. </param>
        /// <param name="index">    [out] Zero-based index of the. </param>
        ///-------------------------------------------------------------------------------------------------

        public void BuildBoxCorners(List<BoundingBox> boxs, Color color, out VertexPositionColor[] points, out short[] index)
        {
            short[] BoxIndexMap = new short[] {
                0, 1, 0,
                2, 1, 3,
                2, 3, 4,
                5, 4, 6,
                5, 7, 6,
                7, 0, 4,
                1, 5, 2,
                6, 3, 7
                };

            points = new VertexPositionColor[boxs.Count * 8];
            short[] inds = new short[points.Length * 3];

            for (int b = 0; b < boxs.Count; b++)
            {
                Vector3[] thisCorners = boxs[b].GetCorners();

                points[(b * 8) + 0] = new VertexPositionColor(thisCorners[1], color);
                points[(b * 8) + 1] = new VertexPositionColor(thisCorners[0], color);
                points[(b * 8) + 2] = new VertexPositionColor(thisCorners[2], color);
                points[(b * 8) + 3] = new VertexPositionColor(thisCorners[3], color);
                points[(b * 8) + 4] = new VertexPositionColor(thisCorners[5], color);
                points[(b * 8) + 5] = new VertexPositionColor(thisCorners[4], color);
                points[(b * 8) + 6] = new VertexPositionColor(thisCorners[6], color);
                points[(b * 8) + 7] = new VertexPositionColor(thisCorners[7], color);

                for (int i = 0; i < 24; i++)
                {
                    inds[(b * 24) + i] = (short)(BoxIndexMap[i] + (b * 8));
                }
            }

            index = inds;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds a sphere. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="spheres">      The spheres. </param>
        /// <param name="segements">    The segements. </param>
        /// <param name="color">        The color. </param>
        /// <param name="points">       [out] The points. </param>
        /// <param name="index">        [out] Zero-based index of the. </param>
        /// <param name="axisColored">  True if axis colored. </param>
        ///-------------------------------------------------------------------------------------------------

        public void BuildSphere(List<BoundingSphere> spheres, int segements, Color color, out VertexPositionColor[] points, out short[] index, bool axisColored)
        {
            List<VertexPositionColor> pointsList = new List<VertexPositionColor>();

            List<short> inds = new List<short>();
            double step = MathHelper.TwoPi / segements;

            Vector3 p = Vector3.Zero;

            float r = MathHelper.ToRadians(90);

            foreach (BoundingSphere sphere in spheres)
            {
                float radius = sphere.Radius;
                Matrix rotMat = Matrix.Identity;

                for (int axis = 0; axis < 3; axis++)
                {
                    switch (axis)
                    {
                        case 0:
                            if (axisColored)
                                color = Color.Red;

                            rotMat = Matrix.CreateFromYawPitchRoll(r, 0, 0);
                            break;
                        case 1:
                            if (axisColored)
                                color = Color.Yellow;
                            rotMat = Matrix.CreateFromYawPitchRoll(0, r, 0);
                            break;
                        case 2:
                            if (axisColored)
                                color = Color.Blue;
                            rotMat = Matrix.CreateFromYawPitchRoll(0, 0, 1);
                            break;
                    }

                    for (double angle = 0; angle < MathHelper.TwoPi; angle += step)
                    {
                        float x = radius * (float)Math.Cos(angle);
                        float y = radius * (float)Math.Sin(angle);

                        p = Vector3.Transform(new Vector3(x, y, 0), rotMat);
                        pointsList.Add(new VertexPositionColor(p, color));
                        inds.Add((short)inds.Count);

                        x = radius * (float)Math.Cos(angle + step);
                        y = radius * (float)Math.Sin(angle + step);

                        p = Vector3.Transform(new Vector3(x, y, 0), rotMat);
                        pointsList.Add(new VertexPositionColor(p, color));
                        inds.Add((short)inds.Count);
                    }
                }
            }

            points = pointsList.ToArray();
            index = inds.ToArray();
        }
    }
}
