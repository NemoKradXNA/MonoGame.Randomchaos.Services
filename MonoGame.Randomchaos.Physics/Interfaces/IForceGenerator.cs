using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Physics.Interfaces
{
    public interface IForceGenerator
    {
        void UpdateForce(IPhysicsObject physicsObject, GameTime gameTime);
    }
}
