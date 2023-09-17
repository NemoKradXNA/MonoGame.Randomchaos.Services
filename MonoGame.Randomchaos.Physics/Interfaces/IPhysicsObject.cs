using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;

namespace MonoGame.Randomchaos.Physics.Interfaces
{
    public interface IPhysicsObject
    {
        ITransform Transform { get; set; }

        Vector3 Velocity { get; set; }
        Vector3 Acceleration { get; set; }
        Vector3 ForceAccumilated { get; set; }
        float Damping { get; set; }

        float Mass { get; set; }
        float InverseMass { get; }

        void Integrate(GameTime gameTime);
    }
}
