
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Physics.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for force registry. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IForceRegistry
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds forceGenerator. </summary>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        void Add(IForceGenerator forceGenerator, IPhysicsObject physicsObject);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes this object. </summary>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        void Remove(IForceGenerator forceGenerator, IPhysicsObject physicsObject);
        /// <summary>   Clears this object to its blank/initial state. </summary>
        void Clear();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the forcees described by gameTime. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void UpdateForcees(GameTime gameTime);
    }
}
