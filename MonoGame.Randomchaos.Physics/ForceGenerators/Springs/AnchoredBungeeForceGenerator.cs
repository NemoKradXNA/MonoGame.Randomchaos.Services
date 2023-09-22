
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Physics.Interfaces;

namespace MonoGame.Randomchaos.Physics.ForceGenerators.Springs
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An anchored bungee force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class AnchoredBungeeForceGenerator : IForceGenerator
    {
        /// <summary>   The anchor. </summary>
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

        public AnchoredBungeeForceGenerator(ITransform anchor, float springConstant, float restLength)
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

            // Check if bungee is compressed.
            float magnitude = force.Length();

            if (magnitude <= _restLength)
            {
                return;
            }

            // Calculate the magnitude (length) of the force.
            magnitude = _springConstant * (magnitude - _restLength);

            // Calculate the final force.
            force.Normalize();
            force *= -magnitude;

            physicsObject.AddForce(force);
        }
    }
}
