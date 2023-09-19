
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Basic;
using MonoGame.Randomchaos.Physics.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Physics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing physics information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PhysicsService : ServiceBase<PhysicsService>, IPhysicsService, IForceRegistry
    {
        /// <summary>   (Immutable) the force registry. </summary>
        protected readonly IForceRegistry _forceRegistry;

        /// <summary>   The earth gravity. </summary>
        protected Vector3 _earthGravity = new Vector3 (0, -9.81f, 0);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the earth gravity. </summary>
        ///
        /// <value> The earth gravity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 EarthGravity { get { return _earthGravity; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is paused. </summary>
        ///
        /// <value> True if this object is paused, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsPaused { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the physics objects. </summary>
        ///
        /// <value> The physics objects. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<IPhysicsObject> PhysicsObjects { get; protected set; } = new List<IPhysicsObject>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="registry"> (Optional) The registry. </param>
        ///-------------------------------------------------------------------------------------------------

        public PhysicsService(Game game, IForceRegistry registry = null) : base(game)
        {
            _forceRegistry = registry;

            if (_forceRegistry == null)
            {
                _forceRegistry = new ForceRegistry();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsPaused)
            {
                _forceRegistry.UpdateForcees(gameTime);

                // get all physics objects..
                foreach (IPhysicsObject obj in PhysicsObjects)
                {
                    obj.Integrate(gameTime);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Registers the object described by physicsObject. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RegisterObject(IPhysicsObject physicsObject)
        {
            if (!PhysicsObjects.Contains(physicsObject))
            {
                PhysicsObjects.Add(physicsObject);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the object described by physicsObject. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="physicsObject">    The physics object. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RemoveObject(IPhysicsObject physicsObject)
        {
            if (PhysicsObjects.Contains(physicsObject))
            {
                PhysicsObjects.Remove(physicsObject);
            }
        }

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
            _forceRegistry.Add(forceGenerator, physicsObject);
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
            _forceRegistry.Remove(forceGenerator, physicsObject);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Clears this object to its blank/initial state. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void Clear()
        {
            _forceRegistry.Clear();
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
            _forceRegistry.UpdateForcees(gameTime);
        }
    }
}
