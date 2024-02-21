
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Animation.Interfaces;
using MonoGame.Randomchaos.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Randomchaos.Animation.Animation3D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A skinned model data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SkinnedModelData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the bones. </summary>
        ///
        /// <value> The bones. </value>
        ///-------------------------------------------------------------------------------------------------

        public ModelBoneCollection Bones { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the meshes. </summary>
        ///
        /// <value> The meshes. </value>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMeshCollection Meshes { get; }

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
        /// <param name="bones">    The bones. </param>
        /// <param name="meshes">   The meshes. </param>
        ///-------------------------------------------------------------------------------------------------

        public SkinnedModelData(List<ModelBone> bones, List<BaseModelMesh> meshes)
        {
            Bones = new ModelBoneCollection(bones);
            Meshes = new BaseModelMeshCollection(meshes);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="GraphicsDevice">   The graphics device. </param>
        /// <param name="meshData">         Information describing the mesh. </param>
        ///-------------------------------------------------------------------------------------------------

        public SkinnedModelData(GraphicsDevice GraphicsDevice, IRandomchaosModelData meshData)
        {
            List<ModelBone> bones = new List<ModelBone>();
            List<BaseModelMesh> meshes = new List<BaseModelMesh>();
            List<BaseModelMeshPart> parts = new List<BaseModelMeshPart>();

            Name = meshData.Name;

            foreach (int key in meshData.Vertices.Keys)
            {
                bones.Add(new ModelBone()
                {
                    Index = 0,
                    ModelTransform = Matrix.Identity,
                    Name = $"bone {key}",
                    Parent = new ModelBone(),
                    Transform = Matrix.Identity,
                });


                List<VertexPositionColorNormalTextureTangent> verts = new List<VertexPositionColorNormalTextureTangent>();
                List<VertexPositionColorNormalTextureTangentSkinned> skinnedVerts = new List<VertexPositionColorNormalTextureTangentSkinned>();
                for (int v = 0; v < meshData.Vertices[key].Count; v++)
                {
                    if (meshData.SkinningData == null)
                        verts.Add(new VertexPositionColorNormalTextureTangent(meshData.Vertices[key][v], meshData.Normals[key][v], meshData.Tangents[key][v], meshData.TexCoords[key][v], meshData.Colors[key][v]));
                    else
                        skinnedVerts.Add(new VertexPositionColorNormalTextureTangentSkinned(meshData.Vertices[key][v], meshData.Normals[key][v], meshData.Tangents[key][v], meshData.TexCoords[key][v], meshData.Colors[key][v], meshData.BlendIndex[key][v], meshData.BlendWeight[key][v]));
                }

                IndexBuffer indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, meshData.Indicies[key].Count, BufferUsage.WriteOnly);
                indexBuffer.SetData(meshData.Indicies[key].ToArray());

                VertexBuffer vertexBuffer = null;
                if (meshData.SkinningData == null)
                {
                    vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorNormalTextureTangent), verts.Count, BufferUsage.WriteOnly);
                    vertexBuffer.SetData(verts.ToArray());
                }
                else
                {
                    vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorNormalTextureTangentSkinned), skinnedVerts.Count, BufferUsage.WriteOnly);
                    vertexBuffer.SetData(skinnedVerts.ToArray());
                }

                parts.Add(new BaseModelMeshPart()
                {
                    IndexBuffer = indexBuffer,
                    NumVertices = verts.Count,
                    PrimitiveCount = meshData.Indicies[key].Count / 3,
                    StartIndex = 0,
                    VertexBuffer = vertexBuffer,
                    VertexOffset = 0,
                    Color = meshData.Colors[key][0],
                    Name = meshData.Names[key],
                    TextureAsset = meshData.Textures != null && meshData.Textures.Count > 0 ? meshData.Textures[0].FirstOrDefault().Value : null
                });


            }
            meshes.Add(new BaseModelMesh($"{meshData.Name}", parts));

            Bones = new ModelBoneCollection(bones);
            Meshes = new BaseModelMeshCollection(meshes);
        }
    }
}
