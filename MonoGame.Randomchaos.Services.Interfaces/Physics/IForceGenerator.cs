
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Services.Interfaces.Physics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IForceGenerator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the force. </summary>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        /// <param name="gameTime">         The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void UpdateForce(IPhysicsObject physicsObject, GameTime gameTime);
    }
}
