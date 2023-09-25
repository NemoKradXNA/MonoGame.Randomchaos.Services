using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Primitives3D.Models.Voxel
{
    public class VertexPoint
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;

        public VertexPoint(Vector3 position, Vector3 norm)
        {
            Position = position;
            Normal = norm;
        }
    }
}
