using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Randomchaos.Physics.Interfaces
{
    public interface IPhysicsService
    {
        Vector3 EarthGravity { get; }
        List<IPhysicsObject> PhysicsObjects { get; }
        bool IsPaused { get; set; }

        void RegisterObject(IPhysicsObject physicsObject);
        void RemoveObject(IPhysicsObject physicsObject);
    }
}
