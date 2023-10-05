
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A geometry cylinder base. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    ///-------------------------------------------------------------------------------------------------

    public class GeometryCylinderBase<T> : GeometryBase<T> where T : IVertexType
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public GeometryCylinderBase(Game game) : base(game) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void LoadContent()
        {
            Vertices = new List<Vector3>()
            {
                new Vector3(-0.4755286f, -1f, -0.1545086f), new Vector3(-0.4045088f, -1f, -0.2938928f), new Vector3(-0.2938928f, -1f, -0.4045087f), new Vector3(-0.1545086f, -1f, -0.4755285f), new Vector3(0f, -1f, -0.5000002f), new Vector3(0.1545086f, -1f, -0.4755285f), new Vector3(0.2938927f, -1f, -0.4045087f), new Vector3(0.4045086f, -1f, -0.2938927f), new Vector3(0.4755284f, -1f, -0.1545085f), new Vector3(0.5000001f, -1f, 0f), new Vector3(0.4755284f, -1f, 0.1545085f), new Vector3(0.4045086f, -1f, 0.2938927f), new Vector3(0.2938927f, -1f, 0.4045086f), new Vector3(0.1545085f, -1f, 0.4755283f), new Vector3(1.490116E-08f, -1f, 0.5000001f), new Vector3(-0.1545085f, -1f, 0.4755283f), new Vector3(-0.2938926f, -1f, 0.4045085f), new Vector3(-0.4045085f, -1f, 0.2938927f), new Vector3(-0.4755283f, -1f, 0.1545085f), new Vector3(-0.5f, -1f, 0f), new Vector3(-0.4755286f, 1f, -0.1545086f), new Vector3(-0.4045088f, 1f, -0.2938928f), new Vector3(-0.2938928f, 1f, -0.4045087f), new Vector3(-0.1545086f, 1f, -0.4755285f), new Vector3(0f, 1f, -0.5000002f), new Vector3(0.1545086f, 1f, -0.4755285f), new Vector3(0.2938927f, 1f, -0.4045087f), new Vector3(0.4045086f, 1f, -0.2938927f), new Vector3(0.4755284f, 1f, -0.1545085f), new Vector3(0.5000001f, 1f, 0f), new Vector3(0.4755284f, 1f, 0.1545085f), new Vector3(0.4045086f, 1f, 0.2938927f), new Vector3(0.2938927f, 1f, 0.4045086f), new Vector3(0.1545085f, 1f, 0.4755283f), new Vector3(1.490116E-08f, 1f, 0.5000001f), new Vector3(-0.1545085f, 1f, 0.4755283f), new Vector3(-0.2938926f, 1f, 0.4045085f), new Vector3(-0.4045085f, 1f, 0.2938927f), new Vector3(-0.4755283f, 1f, 0.1545085f), new Vector3(-0.5f, 1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0.5000001f, -1f, 0f), new Vector3(0.5000001f, 1f, 0f), new Vector3(-0.5f, -1f, 0f), new Vector3(-0.4755286f, 1f, -0.1545086f), new Vector3(-0.4755286f, -1f, -0.1545086f), new Vector3(-0.5f, 1f, 0f), new Vector3(-0.4045088f, -1f, -0.2938928f), new Vector3(-0.4755286f, -1f, -0.1545086f), new Vector3(-0.2938928f, -1f, -0.4045087f), new Vector3(-0.1545086f, -1f, -0.4755285f), new Vector3(0f, -1f, -0.5000002f), new Vector3(0.1545086f, -1f, -0.4755285f), new Vector3(0.2938927f, -1f, -0.4045087f), new Vector3(0.4045086f, -1f, -0.2938927f), new Vector3(0.4755284f, -1f, -0.1545085f), new Vector3(0.5000001f, -1f, 0f), new Vector3(0.4755284f, -1f, 0.1545085f), new Vector3(0.4045086f, -1f, 0.2938927f), new Vector3(0.2938927f, -1f, 0.4045086f), new Vector3(0.1545085f, -1f, 0.4755283f), new Vector3(1.490116E-08f, -1f, 0.5000001f), new Vector3(-0.1545085f, -1f, 0.4755283f), new Vector3(-0.2938926f, -1f, 0.4045085f), new Vector3(-0.4045085f, -1f, 0.2938927f), new Vector3(-0.4755283f, -1f, 0.1545085f), new Vector3(-0.5f, -1f, 0f), new Vector3(-0.4755286f, 1f, -0.1545086f), new Vector3(-0.4045088f, 1f, -0.2938928f), new Vector3(-0.2938928f, 1f, -0.4045087f), new Vector3(-0.1545086f, 1f, -0.4755285f), new Vector3(0f, 1f, -0.5000002f), new Vector3(0.1545086f, 1f, -0.4755285f), new Vector3(0.2938927f, 1f, -0.4045087f), new Vector3(0.4045086f, 1f, -0.2938927f), new Vector3(0.4755284f, 1f, -0.1545085f), new Vector3(0.5000001f, 1f, 0f), new Vector3(0.4755284f, 1f, 0.1545085f), new Vector3(0.4045086f, 1f, 0.2938927f), new Vector3(0.2938927f, 1f, 0.4045086f), new Vector3(0.1545085f, 1f, 0.4755283f), new Vector3(1.490116E-08f, 1f, 0.5000001f), new Vector3(-0.1545085f, 1f, 0.4755283f), new Vector3(-0.2938926f, 1f, 0.4045085f), new Vector3(-0.4045085f, 1f, 0.2938927f), new Vector3(-0.4755283f, 1f, 0.1545085f), new Vector3(-0.5f, 1f, 0f)
            };

            Normals = new List<Vector3>()
            {
                new Vector3(-0.9334423f, 0f, -0.3587276f), new Vector3(-0.776903f, 0f, -0.6296204f), new Vector3(-0.5443152f, 0f, -0.8388808f), new Vector3(-0.258446f, 0f, -0.9660257f), new Vector3(0.05272146f, 0f, -0.9986092f), new Vector3(0.3587284f, 0f, -0.933442f), new Vector3(0.6296204f, 0f, -0.7769028f), new Vector3(0.8388808f, 0f, -0.5443152f), new Vector3(0.9660257f, 0f, -0.2584461f), new Vector3(0.9986093f, 0f, 0.05272135f), new Vector3(0.933442f, 0f, 0.3587283f), new Vector3(0.7769029f, 0f, 0.6296204f), new Vector3(0.5443153f, 0f, 0.8388808f), new Vector3(0.2584461f, 0f, 0.9660256f), new Vector3(-0.05272149f, 0f, 0.9986092f), new Vector3(-0.3587283f, 0f, 0.933442f), new Vector3(-0.6296203f, 0f, 0.776903f), new Vector3(-0.8388807f, 0f, 0.5443153f), new Vector3(-0.9660256f, 0f, 0.2584462f), new Vector3(-0.9986093f, 0f, -0.05272005f), new Vector3(-0.9660261f, 0f, -0.2584449f), new Vector3(-0.8388807f, 0f, -0.5443153f), new Vector3(-0.6296201f, 0f, -0.7769031f), new Vector3(-0.3587281f, 0f, -0.9334421f), new Vector3(-0.05272127f, 0f, -0.9986093f), new Vector3(0.2584463f, 0f, -0.9660256f), new Vector3(0.5443155f, 0f, -0.8388806f), new Vector3(0.7769032f, 0f, -0.6296201f), new Vector3(0.9334421f, 0f, -0.3587282f), new Vector3(0.9986093f, 0f, -0.05272135f), new Vector3(0.9660256f, 0f, 0.2584462f), new Vector3(0.8388806f, 0f, 0.5443155f), new Vector3(0.6296202f, 0f, 0.7769031f), new Vector3(0.3587282f, 0f, 0.9334421f), new Vector3(0.05272129f, 0f, 0.9986093f), new Vector3(-0.2584463f, 0f, 0.9660256f), new Vector3(-0.5443153f, 0f, 0.8388807f), new Vector3(-0.776903f, 0f, 0.6296203f), new Vector3(-0.9334421f, 0f, 0.3587282f), new Vector3(-0.9986092f, 0f, 0.05272201f), new Vector3(0f, -1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0.9986093f, 0f, 0.05272135f), new Vector3(0.9986093f, 0f, -0.05272135f), new Vector3(-0.9986093f, 0f, -0.05272005f), new Vector3(-0.9660261f, 0f, -0.2584449f), new Vector3(-0.9334423f, 0f, -0.3587276f), new Vector3(-0.9986092f, 0f, 0.05272201f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, -1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 0f)
            };

            if (UVMap != null)
            {
                Texcoords = UVMap;
            }
            else
            {
                Texcoords = new List<Vector2>()
                {
                    new Vector2(0.1005999f, 0.0004537381f), new Vector2(0.2005054f, 0.0004537381f), new Vector2(0.300411f, 0.0004537381f), new Vector2(0.4003165f, 0.0004537381f), new Vector2(0.500222f, 0.0004537381f), new Vector2(0.6001273f, 0.0004537381f), new Vector2(0.7000327f, 0.0004537381f), new Vector2(0.799938f, 0.0004537381f), new Vector2(0.8998432f, 0.0004537381f), new Vector2(0.999749f, 0.0004537381f), new Vector2(0.1005993f, 0.0004537381f), new Vector2(0.200505f, 0.0004537381f), new Vector2(0.3004106f, 0.0004537381f), new Vector2(0.400316f, 0.0004537381f), new Vector2(0.5002215f, 0.0004537381f), new Vector2(0.6001268f, 0.0004537381f), new Vector2(0.7000322f, 0.0004537381f), new Vector2(0.7999375f, 0.0004537381f), new Vector2(0.8998428f, 0.0004537381f), new Vector2(0.9997481f, 0.0004537381f), new Vector2(0.1005996f, 0.9994308f), new Vector2(0.2005053f, 0.9994308f), new Vector2(0.300411f, 0.9994308f), new Vector2(0.4003165f, 0.9994308f), new Vector2(0.500222f, 0.9994308f), new Vector2(0.6001273f, 0.9994308f), new Vector2(0.7000328f, 0.9994308f), new Vector2(0.7999381f, 0.9994308f), new Vector2(0.8998435f, 0.9994308f), new Vector2(0.9997491f, 0.9994308f), new Vector2(0.1005996f, 0.9994308f), new Vector2(0.2005051f, 0.9994308f), new Vector2(0.3004106f, 0.9994308f), new Vector2(0.400316f, 0.9994308f), new Vector2(0.5002215f, 0.9994308f), new Vector2(0.6001268f, 0.9994308f), new Vector2(0.7000321f, 0.9994308f), new Vector2(0.7999371f, 0.9994308f), new Vector2(0.8998424f, 0.9994308f), new Vector2(0.9997478f, 0.9994308f), new Vector2(0.5002239f, 0.4999443f), new Vector2(0.5002245f, 0.4999442f), new Vector2(0.0006936856f, 0.0004537381f), new Vector2(0.0006940446f, 0.9994308f), new Vector2(0.00069427f, 0.0004537381f), new Vector2(0.1005996f, 0.9994308f), new Vector2(0.1005999f, 0.0004537381f), new Vector2(0.0006939089f, 0.9994308f), new Vector2(0.0988569f, 0.7915539f), new Vector2(0.02838877f, 0.6532523f), new Vector2(0.2086136f, 0.9013107f), new Vector2(0.3469154f, 0.971779f), new Vector2(0.500224f, 0.9960608f), new Vector2(0.6535327f, 0.971779f), new Vector2(0.7918344f, 0.9013106f), new Vector2(0.901591f, 0.7915536f), new Vector2(0.9720588f, 0.6532521f), new Vector2(0.9963405f, 0.4999437f), new Vector2(0.9720586f, 0.3466355f), new Vector2(0.9015904f, 0.2083341f), new Vector2(0.7918337f, 0.09857755f), new Vector2(0.653532f, 0.02810944f), new Vector2(0.5002234f, 0.003827875f), new Vector2(0.346915f, 0.02810972f), new Vector2(0.2086136f, 0.09857795f), new Vector2(0.09885708f, 0.2083347f), new Vector2(0.02838913f, 0.3466361f), new Vector2(0.004107505f, 0.4999442f), new Vector2(0.9720596f, 0.6532523f), new Vector2(0.9015915f, 0.7915539f), new Vector2(0.7918347f, 0.9013107f), new Vector2(0.653533f, 0.971779f), new Vector2(0.5002243f, 0.9960608f), new Vector2(0.3469157f, 0.971779f), new Vector2(0.2086142f, 0.9013106f), new Vector2(0.0988576f, 0.7915537f), new Vector2(0.0283896f, 0.6532522f), new Vector2(0.004108027f, 0.4999439f), new Vector2(0.02838971f, 0.3466356f), new Vector2(0.09885782f, 0.2083341f), new Vector2(0.2086144f, 0.09857755f), new Vector2(0.3469159f, 0.02810938f), new Vector2(0.5002242f, 0.003827723f), new Vector2(0.6535327f, 0.02810941f), new Vector2(0.7918344f, 0.09857755f), new Vector2(0.901591f, 0.2083343f), new Vector2(0.9720591f, 0.3466357f), new Vector2(0.9963408f, 0.4999439f)
                };
            }

            Colors = new List<Color>();
            for (int v = 0; v < Vertices.Count; v++)
            {
                Colors.Add(Color.White);
            }

            Indicies = new List<int>()
            {
                68, 41, 87, 87, 41, 86, 86, 41, 85, 85, 41, 84, 84, 41, 83, 83, 41, 82, 82, 41, 81, 81, 41, 80, 80, 41, 79, 79, 41, 78, 78, 41, 77, 77, 41, 76, 76, 41, 75, 75, 41, 74, 74, 41, 73, 73, 41, 72, 72, 41, 71, 71, 41, 70, 70, 41, 69, 69, 41, 68, 67, 40, 49, 66, 40, 67, 65, 40, 66, 64, 40, 65, 63, 40, 64, 62, 40, 63, 61, 40, 62, 60, 40, 61, 59, 40, 60, 58, 40, 59, 57, 40, 58, 56, 40, 57, 55, 40, 56, 54, 40, 55, 53, 40, 54, 52, 40, 53, 51, 40, 52, 50, 40, 51, 48, 40, 50, 49, 40, 48, 45, 47, 44, 46, 45, 44, 19, 39, 18, 39, 38, 18, 18, 38, 17, 38, 37, 17, 17, 37, 16, 37, 36, 16, 16, 36, 15, 36, 35, 15, 15, 35, 14, 35, 34, 14, 14, 34, 13, 34, 33, 13, 13, 33, 12, 33, 32, 12, 12, 32, 11, 32, 31, 11, 11, 31, 10, 31, 30, 10, 30, 43, 42, 10, 30, 42, 9, 29, 8, 29, 28, 8, 8, 28, 7, 28, 27, 7, 7, 27, 6, 27, 26, 6, 6, 26, 5, 26, 25, 5, 5, 25, 4, 25, 24, 4, 4, 24, 3, 24, 23, 3, 3, 23, 2, 23, 22, 2, 2, 22, 1, 22, 21, 1, 21, 20, 0, 1, 21, 0
            };


            base.LoadContent();
        }
    }
}
