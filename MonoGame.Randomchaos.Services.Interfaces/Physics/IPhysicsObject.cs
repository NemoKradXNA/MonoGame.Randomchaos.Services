﻿using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;

namespace MonoGame.Randomchaos.Services.Interfaces.Physics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for physics object. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IPhysicsObject
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        ITransform Transform { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the velocity. </summary>
        ///
        /// <value> The velocity. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 Velocity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the acceleration. </summary>
        ///
        /// <value> The acceleration. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 Acceleration { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the force accumilated. </summary>
        ///
        /// <value> The force accumilated. </value>
        ///-------------------------------------------------------------------------------------------------

        Vector3 ForceAccumilated { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the damping. </summary>
        ///
        /// <value> The damping. </value>
        ///-------------------------------------------------------------------------------------------------

        float Damping { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the mass. </summary>
        ///
        /// <value> The mass. </value>
        ///-------------------------------------------------------------------------------------------------

        float Mass { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the inverse mass. </summary>
        ///
        /// <value> The inverse mass. </value>
        ///-------------------------------------------------------------------------------------------------

        float InverseMass { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object has finite mass. </summary>
        ///
        /// <value> True if this object has finite mass, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool HasFiniteMass { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Integrates the given game time. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void Integrate(GameTime gameTime);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a force. </summary>
        ///
        /// <param name="force">    The force. </param>
        ///-------------------------------------------------------------------------------------------------

        void AddForce(Vector3 force);
    }
}
