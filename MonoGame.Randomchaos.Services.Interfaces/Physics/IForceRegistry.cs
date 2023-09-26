using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Services.Interfaces.Physics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for force registry. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IForceRegistry
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a force to 'physicsObject'. </summary>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        void AddForce(IForceGenerator forceGenerator, IPhysicsObject physicsObject);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the forces. </summary>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        void RemoveForce(IForceGenerator forceGenerator, IPhysicsObject physicsObject);

        /// <summary>   Clears the forces. </summary>
        void ClearForces();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the forcees described by gameTime. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void UpdateForcees(GameTime gameTime);
    }
}
