using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Physics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Randomchaos.Physics.Basic
{
    public abstract class BasicPhysicsObject : DrawableGameComponent, IPhysicsObject
    {
        public ITransform Transform { get; set; }

        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 ForceAccumilated { get; set; }

        public float Damping { get; set; } = 1f;

        protected float _mass = 1;
        public float Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                _inverseMass = null; // forces it to recalculate when needed.
            }
        }

        protected float? _inverseMass;
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

        public BasicPhysicsObject(Game game) : base(game) { }

        public void Integrate(GameTime gameTime) 
        {
            // Time 
            float t = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (t > 0)
            {
                // Linear position
                Transform.Translate(Velocity * t);

                // Acceleration
                Vector3 ra = Acceleration + (ForceAccumilated * InverseMass);

                // Velocity
                Velocity += ra * t;

                // Damping
                Velocity *= (float)Math.Pow(Damping, t);
            }
        }
    }
}
