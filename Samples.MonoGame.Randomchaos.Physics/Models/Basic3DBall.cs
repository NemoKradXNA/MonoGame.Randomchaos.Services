using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;
using MonoGame.Randomchaos.Primitives3D.Models;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    public class Basic3DBall : BasicPhysicsObject
    {
        SphereBasicEfect sphere;

        public Basic3DBall(Game game) : base(game)
        {
            Transform = new Transform();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            sphere = new SphereBasicEfect(Game);
            sphere.Transform.Parent = Transform;

            sphere.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            sphere.Draw(gameTime);
        }
    }
}
