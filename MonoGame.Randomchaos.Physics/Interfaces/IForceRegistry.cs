using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Physics.Interfaces
{
    public interface IForceRegistry
    {
        void Add(IForceGenerator forceGenerator, IPhysicsObject physicsObject);
        void Remove(IForceGenerator forceGenerator, IPhysicsObject physicsObject);
        void Clear();
        void UpdateForcees(GameTime gameTime);
    }
}
