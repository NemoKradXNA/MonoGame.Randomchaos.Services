
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Randomchaos.PostProcessing.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A screen quad. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ScreenQuad
    {
        /// <summary>   The corners. </summary>
        private VertexPositionTexture[] corners = new VertexPositionTexture[]
        {
            new VertexPositionTexture(
                new Vector3(0,0,0),
                new Vector2(1,1)),
            new VertexPositionTexture(
                new Vector3(0,0,0),
                new Vector2(0,1)),
            new VertexPositionTexture(
                new Vector3(0,0,0),
                new Vector2(0,0)),
            new VertexPositionTexture(
                new Vector3(0,0,0),
                new Vector2(1,0))
        };
        /// <summary>   The VB. </summary>
        private VertexBuffer vb;
        /// <summary>   The ib. </summary>
        private short[] ib = new short[] { 0, 1, 2, 2, 3, 0 };

        /// <summary>   (Immutable) the game. </summary>
        private readonly Game Game;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public ScreenQuad(Game game)
        {
            Game = game;
            corners = new VertexPositionTexture[4];
            corners[0].Position = new Vector3(0, 0, 0);
            corners[0].TextureCoordinate = Vector2.Zero;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load content.
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Initialize()
        {   
            vb = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionTexture), corners.Length, BufferUsage.None);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/10/2023. </remarks>
        ///
        /// <param name="v1">   The first value. </param>
        /// <param name="v2">   The second value. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Draw(Vector2 v1, Vector2 v2)
        {
            corners[0].Position.X = v2.X; // 1
            corners[0].Position.Y = v1.Y; // -1

            corners[1].Position.X = v1.X; // -1
            corners[1].Position.Y = v1.Y; // -1

            corners[2].Position.X = v1.X; // -1
            corners[2].Position.Y = v2.Y; // 1

            corners[3].Position.X = v2.X; // 1
            corners[3].Position.Y = v2.Y; // 1

            vb.SetData(corners);
            Game.GraphicsDevice.SetVertexBuffer(vb);

            Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, corners, 0, 4, ib, 0, 2);
        }
    }
}
