using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Physics.Basic;
using MonoGame.Randomchaos.Primitives3D.Models;

namespace Samples.MonoGame.Randomchaos.Physics.Models
{
    public class Basic3DCube : BasicPhysicsObject
    {
        protected CubeBasicEffect cube;

        public Basic3DCube(Game game) : base(game)
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

            cube = new CubeBasicEffect(Game);
            cube.Transform.Parent = Transform;

            cube.Texture = new Texture2D(Game.GraphicsDevice, 1, 1);
            cube.Texture.SetData(new Color[] { new Color(.8f, .8f, .8f) });
            
            cube.Initialize();

            cube.SetDirectionalLight(Vector3.Forward + Vector3.Down + Vector3.Right);
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
            cube.Draw(gameTime);
        }
    }
}
