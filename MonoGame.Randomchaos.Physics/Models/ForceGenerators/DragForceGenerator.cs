
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces.Physics;

namespace MonoGame.Randomchaos.Physics.Models.ForceGenerators
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A drag force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class DragForceGenerator : IForceGenerator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Holds the velocity drag coefficient. </summary>
        ///
        /// <value> The velocity drag coefficeint. </value>
        ///-------------------------------------------------------------------------------------------------

        public float VelocityDragCoefficeint { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Holds the velocity squared drag coefficient. </summary>
        ///
        /// <value> The velocity sqr drag coefficient. </value>
        ///-------------------------------------------------------------------------------------------------

        public float VelocitySqrDragCoefficient { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="velocityDragCoefficeint">      Holds the velocity drag coefficient. </param>
        /// <param name="velocitySqrDragCoefficient">   Holds the velocity squared drag coefficient. </param>
        ///-------------------------------------------------------------------------------------------------

        public DragForceGenerator(float velocityDragCoefficeint, float velocitySqrDragCoefficient)
        {
            VelocityDragCoefficeint = velocityDragCoefficeint;
            VelocitySqrDragCoefficient = velocitySqrDragCoefficient;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the force. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        /// <param name="gameTime">         The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateForce(IPhysicsObject physicsObject, GameTime gameTime)
        {
            Vector3 force = physicsObject.Velocity;

            // total drag coefficient.
            float dragCoeff = force.Length();
            dragCoeff = VelocityDragCoefficeint * dragCoeff * VelocitySqrDragCoefficient * dragCoeff * dragCoeff;

            // final force
            force.Normalize();
            force *= -dragCoeff;

            physicsObject.AddForce(force);
        }
    }
}
