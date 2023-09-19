
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;
using MonoGame.Randomchaos.Primitives3D.Models;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A basic 3D ball. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Basic3DBall : BasicPhysicsObject
    {
        /// <summary>   The sphere. </summary>
        SphereBasicEfect sphere;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public Basic3DBall(Game game) : base(game)
        {
            Transform = new Transform();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            base.LoadContent();

            sphere = new SphereBasicEfect(Game);
            sphere.Transform.Parent = Transform;

            sphere.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Draw(GameTime gameTime)
        {
            sphere.Draw(gameTime);
        }
    }
}
