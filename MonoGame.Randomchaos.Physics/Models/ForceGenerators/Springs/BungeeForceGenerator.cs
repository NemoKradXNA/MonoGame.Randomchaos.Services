
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces.Physics;

namespace MonoGame.Randomchaos.Physics.Models.ForceGenerators.Springs
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A bungee force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BungeeForceGenerator : IForceGenerator
    {
        /// <summary>   The other. </summary>
        protected IPhysicsObject _other;


        /// <summary>   The spring constant. </summary>
        protected float _springConstant;
        /// <summary>   Length of the REST. </summary>
        protected float _restLength;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="other">            The other. </param>
        /// <param name="springConstant">   The spring constant. </param>
        /// <param name="restLength">       Length of the REST. </param>
        ///-------------------------------------------------------------------------------------------------

        public BungeeForceGenerator(IPhysicsObject other, float springConstant, float restLength)
        {
            _other = other;
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
            Vector3 force = physicsObject.Transform.Position - _other.Transform.Position;

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
