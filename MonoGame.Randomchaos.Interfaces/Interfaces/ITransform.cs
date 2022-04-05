using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Interfaces
{
    public interface ITransform
    {
        ITransform Parent { get; set; }
        Vector3 Position { get; set; }
        Vector2 Position2D { get; set; }
        Vector3 LocalPosition { get; set; }
        Vector2 LocalPosition2D { get; set; }

        Vector3 Scale { get; set; }
        Vector2 Scale2D { get; set; }
        Vector3 LocalScale { get; set; }
        Vector2 LocalScale2D { get; set; }
        Quaternion Rotation { get; set; }
        Quaternion LocalRotation { get; set; }

        Matrix World { get; }

        Vector3 Forward { get; }

        void Translate(Vector3 distance);
        void Rotate(Vector3 axis, float angle);
        void LocalRotate(Vector3 axis, float angle);
    }
}