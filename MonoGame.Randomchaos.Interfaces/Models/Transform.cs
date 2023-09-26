
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces;

namespace MonoGame.Randomchaos.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Form for viewing the transaction. </summary>
    ///
    /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Transform : ITransform
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the parent. </summary>
        ///
        /// <value> The parent. </value>
        ///-------------------------------------------------------------------------------------------------

        public ITransform Parent { get; set; }
        #region Scale

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the scale. </summary>
        ///
        /// <value> The scale. </value>
        ///-------------------------------------------------------------------------------------------------

        protected virtual Vector3 _Scale { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The scale of the transform (how big it is) relative to it's parent. </summary>
        ///
        /// <value> The scale. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The scale of the transform in 2D. </summary>
        ///
        /// <value> The scale 2D. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The local scale of the transform. </summary>
        ///
        /// <value> The local scale. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual Vector3 LocalScale
        {
            get
            {
                return _Scale;
            }
            set { _Scale = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The local scale of the transform in 2D. </summary>
        ///
        /// <value> The local scale 2D. </value>
        ///-------------------------------------------------------------------------------------------------

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
        /// <summary>   The position. </summary>
        protected Vector3 _position;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The position of the transform in world space (relative to it's parent) </summary>
        ///
        /// <value> The position. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The position of the transform in 2D (relative to it's parent) </summary>
        ///
        /// <value> The position 2D. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The position of the transform in relation to it's own world space. </summary>
        ///
        /// <value> The local position. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The 2D position of the transform in relation to it's own world space. </summary>
        ///
        /// <value> The local position 2D. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// This is a reference to the rotation, it can't be a property, it has to be a field.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------

        public Quaternion RotationRef;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The rotation of the transform in world space (relative to it's parent) </summary>
        ///
        /// <value> The rotation. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The rotation of the transform in local space (relative to it's parent) </summary>
        ///
        /// <value> The local rotation. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the forward. </summary>
        ///
        /// <value> The forward. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Forward { get { return _world.Forward; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the world. </summary>
        ///
        /// <value> The world. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Matrix _world { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   World transform. </summary>
        ///
        /// <value> The world. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the local world. </summary>
        ///
        /// <value> The local world. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual Matrix LocalWorld { get { return Matrix.CreateScale(LocalScale) * Matrix.CreateFromQuaternion(LocalRotation) * Matrix.CreateTranslation(LocalPosition); } }
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Transform()
        {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;

            Parent = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="transform">    The transform. </param>
        ///-------------------------------------------------------------------------------------------------

        public Transform(ITransform transform) : this()
        {
            Parent = transform;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            return $"{{{Position} {Scale} {Rotation}}}";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Translates the given distance. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="distance"> The distance. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Translate(Vector3 distance)
        {
            Position += Vector3.Transform(distance, Rotation);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Rotates. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="axis">     The axis. </param>
        /// <param name="angle">    The angle. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Rotation));
            Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * Rotation);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Local rotate. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="axis">     The axis. </param>
        /// <param name="angle">    The angle. </param>
        ///-------------------------------------------------------------------------------------------------

        public void LocalRotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(LocalRotation));
            LocalRotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * LocalRotation);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Transform point from local space to world space. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="point">    The point. </param>
        ///
        /// <returns>   A Vector3. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 TransformPoint(Vector3 point)
        {
            return Vector3.Transform(point, World);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Inverse transform point, moves position from world to local space. </summary>
        ///
        /// <remarks>   Charles Humphrey, 26/09/2023. </remarks>
        ///
        /// <param name="point">    The point. </param>
        ///
        /// <returns>   A Vector3. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 InverseTransformPoint(Vector3 point)
        {
            return Vector3.Transform(point, Matrix.Invert(World));
        }
    }
}
