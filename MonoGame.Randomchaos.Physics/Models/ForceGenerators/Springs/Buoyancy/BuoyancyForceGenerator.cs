
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces.Physics;

namespace MonoGame.Randomchaos.Physics.Models.ForceGenerators.Springs.Buoyancy
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A buoyancy force generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BuoyancyForceGenerator : IForceGenerator
    {
        /// <summary>   The maximum depth. </summary>
        protected float _maxDepth;
        /// <summary>   The volume. </summary>
        protected float _volume;
        /// <summary>   Height of the water. </summary>
        protected float _waterHeight;
        /// <summary>   The liquid density. </summary>
        protected float _liquidDensity;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="maxDepth">         The maximum depth. </param>
        /// <param name="volume">           The volume. </param>
        /// <param name="waterHeight">      Height of the water. </param>
        /// <param name="liquidDensity">    (Optional) The liquid density. </param>
        ///-------------------------------------------------------------------------------------------------

        public BuoyancyForceGenerator(float maxDepth, float volume, float waterHeight, float liquidDensity = 1000.0f)
        {
            _maxDepth = maxDepth;
            _volume = volume;
            _waterHeight = waterHeight;
            _liquidDensity = liquidDensity;
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
            // Calculate submersion depth.
            float depth = physicsObject.Transform.Position.Y;

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check if out of the water.
            if (depth >= _waterHeight + _maxDepth)
            {
                return;
            }

            Vector3 force = Vector3.Zero;

            // Are we at maximum depth?
            if (depth <= _waterHeight - _maxDepth)
            {
                force.Y = _liquidDensity * _volume * t;
                physicsObject.AddForce(force);
                return;
            }

            // If not, then we are part submerged.
            force.Y = _liquidDensity * _volume * (depth - _maxDepth - _waterHeight) / 2 * _maxDepth * t;

            physicsObject.AddForce(force);
        }
    }
}
