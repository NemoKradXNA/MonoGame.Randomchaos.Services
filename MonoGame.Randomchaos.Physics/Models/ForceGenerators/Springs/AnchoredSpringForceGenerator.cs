
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Physics;
using System;

namespace MonoGame.Randomchaos.Physics.Models.ForceGenerators.Springs
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An anchored spring force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class AnchoredSpringForceGenerator : IForceGenerator
    {
        /// <summary>   The other. </summary>
        protected ITransform _anchor;
        /// <summary>   The spring constant. </summary>
        protected float _springConstant;
        /// <summary>   Length of the REST. </summary>
        protected float _restLength;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="anchor">           The other. </param>
        /// <param name="springConstant">   The spring constant. </param>
        /// <param name="restLength">       Length of the REST. </param>
        ///-------------------------------------------------------------------------------------------------

        public AnchoredSpringForceGenerator(ITransform anchor, float springConstant, float restLength)
        {
            _anchor = anchor;
            _springConstant = springConstant;
            _restLength = restLength;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the force. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        /// <param name="gameTime">         The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateForce(IPhysicsObject physicsObject, GameTime gameTime)
        {
            // Get the direction of the spring.
            Vector3 force = physicsObject.Transform.Position - _anchor.Position;

            // Calculate the magnitude (length) of the force.
            float magnitude = force.Length();
            magnitude = Math.Abs(magnitude - _restLength);
            magnitude *= _springConstant;

            // Calculate the final force.
            force.Normalize();
            force *= -magnitude;

            physicsObject.AddForce(force);
        }
    }
}
