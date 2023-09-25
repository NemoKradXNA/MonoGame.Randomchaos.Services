
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models.Voxel
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A voxel chunk. </summary>
    ///
    /// <remarks>   Charles Humphrey, 24/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class VoxelChunk
    {
        /// <summary>   The size of. </summary>
        public static int SizeOf = 164;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the on. </summary>
        ///
        /// <value> True if on, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool On { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is transparent. </summary>
        ///
        /// <value> True if this object is transparent, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsTransparent { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the triangles. </summary>
        ///
        /// <value> The triangles. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Triangle> Triangles { get; set; } = new List<Triangle>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the shape. </summary>
        ///
        /// <value> The shape. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Shape { get; set; }
        /// <summary>   The position. </summary>
        public Vector3 Position;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the type of the block. </summary>
        ///
        /// <value> The type of the block. </value>
        ///-------------------------------------------------------------------------------------------------

        public int BlockType { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the x coordinate. </summary>
        ///
        /// <value> The x coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public int x { get { return (int)Position.X; } set { Position = new Vector3(value, Position.Y, Position.Z); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the y coordinate. </summary>
        ///
        /// <value> The y coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public int y { get { return (int)Position.Y; } set { Position = new Vector3(Position.X, value, Position.Z); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the z coordinate. </summary>
        ///
        /// <value> The z coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public int z { get { return (int)Position.Z; } set { Position = new Vector3(Position.X, Position.Y, value); } }

        private BoundingBox? _boundingBox = null;

        public BoundingBox BoundingBox
        {
            get
            {
                if (_boundingBox == null)
                {
                    Vector3 halfChunk = Vector3.One * .5f;
                    _boundingBox = new BoundingBox(Position + halfChunk, Position - halfChunk);
                }

                return _boundingBox.Value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 24/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public VoxelChunk()
        {
            On = false;
            Shape = 0;
            Position = Vector3.Zero;
            BlockType = 0;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 24/09/2023. </remarks>
        ///
        /// <returns>   A VoxelChunk. </returns>
        ///-------------------------------------------------------------------------------------------------

        public VoxelChunk Copy()
        {
            return new VoxelChunk() { On = this.On, BlockType = this.BlockType, Position = this.Position, Shape = this.Shape };
        }
    }
}
