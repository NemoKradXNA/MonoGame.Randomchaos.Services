
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A quaternion extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class QuaternionExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Quaternion extension method that quaternion to euler angle vector 3. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="rotation"> The rotation to act on. </param>
        ///
        /// <returns>   A Vector3. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector3 QuaternionToEulerAngleVector3(this Quaternion rotation)
        {
            Vector3 rotationaxes = new Vector3();
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            rotationaxes = new Vector3().AngleTo(forward);

            if (rotationaxes.X == MathHelper.PiOver2)
            {
                rotationaxes.Y = (float)Math.Atan2((double)up.X, (double)up.Z);
                rotationaxes.Z = 0;
            }
            else if (rotationaxes.X == -MathHelper.PiOver2)
            {
                rotationaxes.Y = (float)Math.Atan2((double)-up.X, (double)-up.Z);
                rotationaxes.Z = 0;
            }
            else
            {
                up = Vector3.Transform(up, Matrix.CreateRotationY(-rotationaxes.Y));
                up = Vector3.Transform(up, Matrix.CreateRotationX(-rotationaxes.X));

                rotationaxes.Z = (float)Math.Atan2((double)-up.X, (double)up.Y);
            }

            return rotationaxes;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Quaternion extension method that locks the rotation. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="rotation">     The rotation to act on. </param>
        /// <param name="lockedRots">   The locked rots. </param>
        ///-------------------------------------------------------------------------------------------------

        public static Quaternion LockRotation(this Quaternion rotation, Vector3 lockedRots)
        {
            lockedRots -= Vector3.One;

            Vector3 rots = rotation.QuaternionToEulerAngleVector3();

            rots *= lockedRots;

            rotation = Quaternion.CreateFromRotationMatrix(
                Matrix.CreateFromAxisAngle(Vector3.Left, rots.X) *
                Matrix.CreateFromAxisAngle(Vector3.Down, rots.Y) *
                Matrix.CreateFromAxisAngle(Vector3.Backward, rots.Z)
                );

            return rotation;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Quaternion extension method that from string. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/10/2023. </remarks>
        ///
        /// <param name="q">                The q to act on. </param>
        /// <param name="quaternionString"> The quaternion string. </param>
        ///-------------------------------------------------------------------------------------------------

        public static Quaternion FromString(this Quaternion quaternion, string quaternionString)
        {
            string[] xyxw = quaternionString.Split(",");

            quaternion = new Quaternion(float.Parse(xyxw[0]), float.Parse(xyxw[1]), float.Parse(xyxw[2]), float.Parse(xyxw[3]));

            return quaternion;
        }
    }
}
