
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Physics;
using System;

namespace MonoGame.Randomchaos.Physics.Models.ForceGenerators.Springs
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A fake implicit spring force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 27/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class FakeImplicitSpringForceGenerator : IForceGenerator
    {
        /// <summary>   The anchor. </summary>
        protected ITransform _anchor;
        /// <summary>   The spring constant. </summary>
        protected float _springConstant;
        /// <summary>   The dampling. </summary>
        protected float _dampling;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 27/09/2023. </remarks>
        ///
        /// <param name="anchor">           The anchor. </param>
        /// <param name="springConstant">   The spring constant. </param>
        /// <param name="dampling">         The dampling. </param>
        ///-------------------------------------------------------------------------------------------------

        public FakeImplicitSpringForceGenerator(ITransform anchor, float springConstant, float dampling)
        {
            _anchor = anchor;
            _springConstant = springConstant;
            _dampling = dampling;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the force. </summary>
        ///
        /// <remarks>   Charles Humphrey, 27/09/2023. </remarks>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        /// <param name="gameTime">         The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateForce(IPhysicsObject physicsObject, GameTime gameTime)
        {
            // Check have finite mass.
            if (physicsObject.HasFiniteMass)
            {
                // Calculate relative position.
                Vector3 position = physicsObject.Transform.Position;
                position -= _anchor.Position;

                // Calculate constants and check they are in bounds.
                float gamma = .5f + (float)Math.Sqrt(4 * _springConstant - _dampling * _dampling);

                if (gamma != 0 && gamma != float.NaN)
                {
                    Vector3 currentVelocity = physicsObject.Velocity;

                    Vector3 constant = position * (_dampling / (2f * gamma)) + currentVelocity * (1f / gamma);

                    float t = 1;// (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // Calculate the target position.
                    Vector3 target = position * (float)Math.Cos(gamma * t) + constant * (float)Math.Sin(gamma * t);

                    target *= (float)Math.Exp(-.5f * t * _dampling);

                    // Calculate the acceleration, ergo, the force
                    Vector3 accel = (target - position) * (1f / t * t) - currentVelocity * t;

                    physicsObject.AddForce(accel * physicsObject.Mass);
                }
            }
        }
    }
}
