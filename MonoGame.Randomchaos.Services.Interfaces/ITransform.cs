using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface ITransform
    {
        ITransform Parent { get; set; }
        Vector3 Position { get; set; }
        Vector3 Scale { get; set; }
        Quaternion Rotation { get; set; }

        Matrix World { get; }

        void Translate(Vector3 distance);
        void Rotate(Vector3 axis, float angle);
    }
}
