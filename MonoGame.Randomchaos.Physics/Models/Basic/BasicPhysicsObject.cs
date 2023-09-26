
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Physics;
using System;

namespace MonoGame.Randomchaos.Physics.Models.Basic
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A basic physics object. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class BasicPhysicsObject : DrawableGameComponent, IPhysicsObject
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        public ITransform Transform { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the velocity. </summary>
        ///
        /// <value> The velocity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Velocity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the acceleration. </summary>
        ///
        /// <value> The acceleration. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Acceleration { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the force accumilated. </summary>
        ///
        /// <value> The force accumilated. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 ForceAccumilated { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object has finite mass. </summary>
        ///
        /// <value> True if this object has finite mass, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool HasFiniteMass { get { return InverseMass >= 0f; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the damping. </summary>
        ///
        /// <value> The damping. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Damping { get; set; } = 1f;

        /// <summary>   The mass. </summary>
        protected float _mass = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the mass. </summary>
        ///
        /// <value> The mass. </value>
        ///-------------------------------------------------------------------------------------------------

        public float Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                _inverseMass = null; // forces it to recalculate when needed.
            }
        }

        /// <summary>   The inverse mass. </summary>
        protected float? _inverseMass;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the inverse mass. </summary>
        ///
        /// <value> The inverse mass. </value>
        ///-------------------------------------------------------------------------------------------------

        public float InverseMass
        {
            get
            {
                if (_inverseMass == null)
                {
                    _inverseMass = 1 / _mass;
                }

                return _inverseMass.Value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public BasicPhysicsObject(Game game) : base(game) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Integrates the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Integrate(GameTime gameTime)
        {
            // Time 
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (t > 0)
            {
                // Linear position
                Transform.Translate(Velocity * t);

                // Acceleration
                Vector3 ra = Acceleration + ForceAccumilated * InverseMass;

                // Velocity
                Velocity += ra * t;

                // Damping
                Velocity *= (float)Math.Pow(Damping, t);

                // Clear accumilated.
                ForceAccumilated = Vector3.Zero;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a force. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="force">    The force. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void AddForce(Vector3 force)
        {
            ForceAccumilated += force;
        }
    }
}
