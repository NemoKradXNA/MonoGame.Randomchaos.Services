using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Interfaces;

namespace MonoGame.Randomchaos.Physics.ForceGenerators
{
    public class DragForceGenerator : IForceGenerator
    {
        /// <summary>
        /// Holds the velocity drag coefficient.
        /// </summary>
        public float VelocityDragCoefficeint { get; set; }

        /// <summary>
        /// Holds the velocity squared drag coefficient.
        /// </summary>
        public float VelocitySqrDragCoefficient { get; set; }

        public DragForceGenerator(float velocityDragCoefficeint, float velocitySqrDragCoefficient)
        {
            VelocityDragCoefficeint = velocityDragCoefficeint;
            VelocitySqrDragCoefficient = velocitySqrDragCoefficient;
        }

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
