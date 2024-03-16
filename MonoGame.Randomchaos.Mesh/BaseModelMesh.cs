
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Mesh
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Analogue for ModelMesh. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BaseModelMesh
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bounding sphere. </summary>
        ///
        /// <value> The bounding sphere. </value>
        ///-------------------------------------------------------------------------------------------------

        public BoundingSphere BoundingSphere { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the mesh parts. </summary>
        ///
        /// <value> The mesh parts. </value>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMeshPartCollection MeshParts { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the parent bone. </summary>
        ///
        /// <value> The parent bone. </value>
        ///-------------------------------------------------------------------------------------------------

        public ModelBone ParentBone { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tag. </summary>
        ///
        /// <value> The tag. </value>
        ///-------------------------------------------------------------------------------------------------

        public object Tag { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="parts">        The parts. </param>
        /// <param name="parentBone">   (Optional) The parent bone. </param>
        /// <param name="tag">          (Optional) The tag. </param>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMesh(string name, List<BaseModelMeshPart> parts, ModelBone parentBone = null, object tag = null)
        {
            Name = name;
            ParentBone = parentBone;
            Tag = tag;
            MeshParts = new BaseModelMeshPartCollection(parts);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="modelMesh">    The model mesh. </param>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMesh(ModelMesh modelMesh)
        {
            List<BaseModelMeshPart> lst = new List<BaseModelMeshPart>();

            foreach (ModelMeshPart part in modelMesh.MeshParts)
            {
                lst.Add(new BaseModelMeshPart(part));
            }
            MeshParts = new BaseModelMeshPartCollection(lst);

            ParentBone = modelMesh.ParentBone;
        }
    }
}
