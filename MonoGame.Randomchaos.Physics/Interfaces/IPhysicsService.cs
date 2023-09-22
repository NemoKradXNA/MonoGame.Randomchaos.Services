
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Physics.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for physics service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IPhysicsService : IForceRegistry
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the earth gravity. </summary>
        ///
        /// <value> The earth gravity. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 EarthGravity { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the physics objects. </summary>
        ///
        /// <value> The physics objects. </value>
        ///-------------------------------------------------------------------------------------------------

        List<IPhysicsObject> PhysicsObjects { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is paused. </summary>
        ///
        /// <value> True if this object is paused, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool IsPaused { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Registers the object described by physicsObject. </summary>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        void RegisterObject(IPhysicsObject physicsObject);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the object described by physicsObject. </summary>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        void RemoveObject(IPhysicsObject physicsObject);
    }
}
