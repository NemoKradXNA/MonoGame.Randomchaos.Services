using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Physics.Basic
{
    public class ForceRegistry : IForceRegistry
    {
        protected Dictionary<IForceGenerator, List<IPhysicsObject>> registry { get; set; } = new Dictionary<IForceGenerator, List<IPhysicsObject>>();

        public void Add(IForceGenerator forceGenerator, IPhysicsObject physicsObject)
        {
            if (!registry.ContainsKey(forceGenerator))
            {
                registry.Add(forceGenerator, new List<IPhysicsObject>());
            }

            registry[forceGenerator].Add(physicsObject);
        }

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

        public void Clear()
        {
            registry.Clear();
        }

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
