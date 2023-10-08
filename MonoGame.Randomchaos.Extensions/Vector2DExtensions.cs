
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A vector 2D extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class Vector2DExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Vector2 extension method that turn to face. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. Code originally written by Shawn Hargreaves </remarks>
        ///
        /// <param name="position">     The position to act on. </param>
        /// <param name="faceThis">     The face this. </param>
        /// <param name="currentAngle"> The current angle. </param>
        /// <param name="turnSpeed">    The turn speed. </param>
        /// <param name="offset">       The offset. </param>
        ///
        /// <returns>   A float. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static float GetTurnToFaceAngle(this Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed, float offset)
        {
            // consider this diagram:
            //         C 
            //        /|
            //      /  |
            //    /    | y
            //  / o    |
            // S--------
            //     x
            // 
            // where S is the position of the spot light, C is the position of the cat,
            // and "o" is the angle that the spot light should be facing in order to 
            // point at the cat. we need to know what o is. using trig, we know that
            //      tan(theta)       = opposite / adjacent
            //      tan(o)           = y / x
            // if we take the arctan of both sides of this equation...
            //      arctan( tan(o) ) = arctan( y / x )
            //      o                = arctan( y / x )
            // so, we can use x and y to find o, our "desiredAngle."
            // x and y are just the differences in position between the two objects.
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            // we'll use the Atan2 function. Atan will calculates the arc tangent of 
            // y / x for us, and has the added benefit that it will use the signs of x
            // and y to determine what cartesian quadrant to put the result in.
            // http://msdn2.microsoft.com/en-us/library/system.math.atan2.aspx
            float desiredAngle = (float)Math.Atan2(y, x) + offset;

            // so now we know where we WANT to be facing, and where we ARE facing...
            // if we weren't constrained by turnSpeed, this would be easy: we'd just 
            // return desiredAngle.
            // instead, we have to calculate how much we WANT to turn, and then make
            // sure that's not more than turnSpeed.

            // first, figure out how much we want to turn, using WrapAngle to get our
            // result from -Pi to Pi ( -180 degrees to 180 degrees )
            float difference = (desiredAngle - currentAngle).WrapAngle();

            // clamp that between -turnSpeed and turnSpeed.
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            // so, the closest we can get to our target is currentAngle + difference.
            // return that, using WrapAngle again.
            return (currentAngle + difference).WrapAngle();
        }
    }
}
