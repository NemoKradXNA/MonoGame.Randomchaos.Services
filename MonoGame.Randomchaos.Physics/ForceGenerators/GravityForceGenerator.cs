
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Interfaces;

namespace MonoGame.Randomchaos.Physics.ForceGenerators
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A gravity force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class GravityForceGenerator : IForceGenerator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the earth gravity. </summary>
        ///
        /// <value> The earth gravity. </value>
        ///-------------------------------------------------------------------------------------------------

        public static Vector3 EarthGravity { get { return Vector3.Up * 9.8f; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the gravity. </summary>
        ///
        /// <value> The gravity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Gravity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gravity">  (Optional) The gravity. </param>
        ///-------------------------------------------------------------------------------------------------

        public GravityForceGenerator(Vector3? gravity = null)
        {
            if (gravity != null)
            {
                Gravity = gravity.Value;
            }
            else
            {
                Gravity = EarthGravity;
            }
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
            if (physicsObject.HasFiniteMass)
            {
                physicsObject.AddForce(Gravity * physicsObject.Mass);
            }
        }
    }
}
