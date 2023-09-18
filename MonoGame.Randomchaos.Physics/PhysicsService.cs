using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Basic;
using MonoGame.Randomchaos.Physics.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Physics
{
    public class PhysicsService : ServiceBase<PhysicsService>, IPhysicsService, IForceRegistry
    {
        protected readonly IForceRegistry _forceRegistry;

        protected Vector3 _earthGravity = new Vector3 (0, -9.81f, 0);

        public Vector3 EarthGravity { get { return _earthGravity; } }

        public bool IsPaused { get; set; }

        public List<IPhysicsObject> PhysicsObjects { get; protected set; } = new List<IPhysicsObject>();

        public PhysicsService(Game game, IForceRegistry registry = null) : base(game)
        {
            _forceRegistry = registry;

            if (_forceRegistry == null)
            {
                _forceRegistry = new ForceRegistry();
            }
        }

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

        public void RegisterObject(IPhysicsObject physicsObject)
        {
            if (!PhysicsObjects.Contains(physicsObject))
            {
                PhysicsObjects.Add(physicsObject);
            }
        }

        public void RemoveObject(IPhysicsObject physicsObject)
        {
            if (PhysicsObjects.Contains(physicsObject))
            {
                PhysicsObjects.Remove(physicsObject);
            }
        }

        public void Add(IForceGenerator forceGenerator, IPhysicsObject physicsObject)
        {
            _forceRegistry.Add(forceGenerator, physicsObject);
        }

        public void Remove(IForceGenerator forceGenerator, IPhysicsObject physicsObject)
        {
            _forceRegistry.Remove(forceGenerator, physicsObject);
        }

        public void Clear()
        {
            _forceRegistry.Clear();
        }

        public void UpdateForcees(GameTime gameTime)
        {
            _forceRegistry.UpdateForcees(gameTime);
        }
    }
}
