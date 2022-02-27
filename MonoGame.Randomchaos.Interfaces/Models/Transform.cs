using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;

namespace MonoGame.Randomchaos.Models
{
    public class Transform : ITransform
    {
        public ITransform Parent { get; set; }
        #region Scale
        protected virtual Vector3 _Scale { get; set; }

        /// <summary>
        /// The scale of the transform (how big it is) relative to it's parent
        /// </summary>
        public virtual Vector3 Scale
        {
            get
            {
                if (Parent != null)
                    return _Scale * Parent.Scale;
                else
                    return _Scale;
            }
            set
            {
                if (Parent != null)
                {
                    _Scale = value / Parent.Scale;
                }
                else
                    _Scale = value;
            }
        }

        /// <summary>
        /// The scale of the transform in 2D
        /// </summary>
        public virtual Vector2 Scale2D
        {
            get
            {
                return new Vector2(Scale.X, Scale.Y);
            }
            set
            {
                Scale = new Vector3(value.X, value.Y, Scale.Z);
            }
        }

        /// <summary>
        /// The local scale of the transform. 
        /// </summary>
        public virtual Vector3 LocalScale
        {
            get
            {
                return _Scale;
            }
            set { _Scale = value; }
        }

        /// <summary>
        /// The local scale of the transform in 2D
        /// </summary>
        public virtual Vector2 LocalScale2D
        {
            get
            {
                return new Vector2(LocalScale.X, LocalScale.Y);
            }
            set
            {
                LocalScale = new Vector3(value.X, value.Y, LocalScale.Z);
            }
        }
        #endregion

        #region Position
        protected Vector3 _position;
        /// <summary>
        /// The position of the transform in world space (relative to it's parent)
        /// </summary>
        public virtual Vector3 Position
        {
            get
            {
                if (Parent != null)
                    return Vector3.Transform(_position, Parent.World);
                else
                    return _position;
            }
            set
            {
                if (Parent != null)
                    _position = Vector3.Transform(value, Matrix.Invert(Parent.World));
                else
                    _position = value;
            }
        }

        /// <summary>
        /// The position of the transform in 2D (relative to it's parent)
        /// </summary>
        public virtual Vector2 Position2D
        {
            get
            {
                return new Vector2(Position.X, Position.Y);
            }
            set
            {
                Position = new Vector3(value.X, value.Y, Position.Z);
            }
        }

        /// <summary>
        /// The position of the transform in relation to it's own world space
        /// </summary>
        public virtual Vector3 LocalPosition
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// The 2D position of the transform in relation to it's own world space
        /// </summary>
        public virtual Vector2 LocalPosition2D
        {
            get
            {
                return new Vector2(LocalPosition.X, LocalPosition.Y);
            }
            set
            {
                LocalPosition = new Vector3(value.X, value.Y, LocalPosition.Z);
            }
        }
        #endregion

        #region Rotation
        /// <summary>
        /// This is a reference to the rotation, it can't be a property, it has to be a field.
        /// </summary>
        public Quaternion RotationRef;

        /// <summary>
        /// The rotation of the transform in world space (relative to it's parent)
        /// </summary>

        public virtual Quaternion Rotation
        {
            get
            {
                if (Parent != null)
                {
                    return RotationRef / Quaternion.Inverse(Parent.Rotation);
                }
                else
                    return RotationRef;
            }
            set
            {
                if (Parent != null)
                {
                    RotationRef = value * Quaternion.Inverse(Parent.Rotation);
                }
                else
                    RotationRef = value;
            }
        }


        /// <summary>
        /// The rotation of the transform in local space (relative to it's parent)
        /// </summary>
        public virtual Quaternion LocalRotation
        {
            get
            {
                return RotationRef;
            }
            set
            {
                RotationRef = value;
            }
        }



        #endregion

        #region World

        public Vector3 Forward { get { return _world.Forward; } }
        protected Matrix _world { get; set; }

        /// <summary>
        /// World transform.
        /// </summary>
        public virtual Matrix World
        {
            get
            {
                if (Parent != null)
                    _world = LocalWorld * Parent.World;
                else
                    _world = LocalWorld;

                return _world;
            }
        }
        public virtual Matrix LocalWorld { get { return Matrix.CreateScale(LocalScale) * Matrix.CreateFromQuaternion(LocalRotation) * Matrix.CreateTranslation(LocalPosition); } }
        #endregion

        public Transform()
        {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;

            Parent = null;
        }

        public Transform(ITransform transform) : this()
        {
            Parent = transform;
        }


        public override string ToString()
        {
            return $"{{{Position} {Scale} {Rotation}}}";
        }

        public void Translate(Vector3 distance)
        {
            Position += Vector3.Transform(distance, Rotation);
        }

        public void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Rotation));
            Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * Rotation);
        }
    }
}
