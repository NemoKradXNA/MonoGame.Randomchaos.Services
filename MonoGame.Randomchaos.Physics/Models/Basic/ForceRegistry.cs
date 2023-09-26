
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces.Physics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Physics.Models.Basic
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A force registry. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ForceRegistry : IForceRegistry
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the registry. </summary>
        ///
        /// <value> The registry. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Dictionary<IForceGenerator, List<IPhysicsObject>> registry { get; set; } = new Dictionary<IForceGenerator, List<IPhysicsObject>>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a force to 'physicsObject'. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddForce(IForceGenerator forceGenerator, IPhysicsObject physicsObject)
        {
            if (!registry.ContainsKey(forceGenerator))
            {
                registry.Add(forceGenerator, new List<IPhysicsObject>());
            }

            registry[forceGenerator].Add(physicsObject);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the forces. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RemoveForce(IForceGenerator forceGenerator, IPhysicsObject physicsObject)
        {
            if (registry.ContainsKey(forceGenerator))
            {
                registry[forceGenerator].Remove(physicsObject);

                if (registry[forceGenerator].Count == 0)
                {
                    registry.Remove(forceGenerator);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Clears the forces. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void ClearForces()
        {
            registry.Clear();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the forcees described by gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateForcees(GameTime gameTime)
        {
            foreach (IForceGenerator forceGenerator in registry.Keys)
            {
                foreach (IPhysicsObject physicsObject in registry[forceGenerator])
                {
                    forceGenerator.UpdateForce(physicsObject, gameTime);
                }
            }
        }
    }
}
