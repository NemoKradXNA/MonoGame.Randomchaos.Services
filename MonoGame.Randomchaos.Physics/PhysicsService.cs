using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Physics
{
    public class PhysicsService : ServiceBase<PhysicsService>, IPhysicsService
    {
        protected Vector3 _earthGravity = new Vector3 (0, -9.81f, 0);

        public Vector3 EarthGravity { get { return _earthGravity; } }

        public bool IsPaused { get; set; }

        public List<IPhysicsObject> PhysicsObjects { get; protected set; } = new List<IPhysicsObject>();

        public PhysicsService(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsPaused)
            {
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
    }
}
