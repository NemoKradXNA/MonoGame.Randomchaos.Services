
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Physics.Basic
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
        /// <summary>   Adds forceGenerator. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Add(IForceGenerator forceGenerator, IPhysicsObject physicsObject)
        {
            if (!registry.ContainsKey(forceGenerator))
            {
                registry.Add(forceGenerator, new List<IPhysicsObject>());
            }

            registry[forceGenerator].Add(physicsObject);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="forceGenerator">   The force generator. </param>
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Remove(IForceGenerator forceGenerator, IPhysicsObject physicsObject)
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
        /// <summary>   Clears this object to its blank/initial state. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void Clear()
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
