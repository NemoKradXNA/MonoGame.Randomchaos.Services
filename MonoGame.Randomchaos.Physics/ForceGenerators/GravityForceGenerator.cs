using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Physics.Interfaces;

namespace MonoGame.Randomchaos.Physics.ForceGenerators
{
    public class GravityForceGenerator : IForceGenerator
    {
        public static Vector3 EarthGravity { get { return Vector3.Up * 9.8f; } }

        public Vector3 Gravity { get; set; }

        public GravityForceGenerator(Vector3? gravity = null)
        {
            if (gravity != null)
            {
                Gravity = gravity.Value;
            }
            else
            {
                Gravity = EarthGravity;
            }
        }

        public void UpdateForce(IPhysicsObject physicsObject, GameTime gameTime)
        {
            if (physicsObject.HasFiniteMass)
            {
                physicsObject.AddForce(Gravity * physicsObject.Mass);
            }
        }
    }
}
