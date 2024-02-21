
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MonoGame.Randomchaos.Animation.Animation3D;
using MonoGame.Randomchaos.Animation.Interfaces;
using MonoGame.Randomchaos.ContentPipelineExtensions.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Processors
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A skinned mesh processor. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [ContentProcessor(DisplayName = "MonoGame.Randomchaos - Skinned Mesh Processor")]
    public class SkinnedMeshProcessor : ContentProcessor<NodeContent, IRandomchaosModelData>
    {
        #region Data
        /// <summary>   The vertices. </summary>
        Dictionary<int, List<Vector3>> vertices = new Dictionary<int, List<Vector3>>();
        /// <summary>   The tex coordinates. </summary>
        Dictionary<int, List<Vector2>> texCoords = new Dictionary<int, List<Vector2>>();
        /// <summary>   The normals. </summary>
        Dictionary<int, List<Vector3>> normals = new Dictionary<int, List<Vector3>>();
        /// <summary>   The tangents. </summary>
        Dictionary<int, List<Vector3>> tangents = new Dictionary<int, List<Vector3>>();
        /// <summary>   The bi normals. </summary>
        Dictionary<int, List<Vector3>> biNormals = new Dictionary<int, List<Vector3>>();
        /// <summary>   Zero-based index of the blend. </summary>
        Dictionary<int, List<Byte4>> blendIndex = new Dictionary<int, List<Byte4>>();
        /// <summary>   The blend weight. </summary>
        Dictionary<int, List<Vector4>> blendWeight = new Dictionary<int, List<Vector4>>();
        /// <summary>   The colors. </summary>
        Dictionary<int, List<Color>> colors = new Dictionary<int, List<Color>>();
        /// <summary>   The names. </summary>
        List<string> names = new List<string>();
        /// <summary>   The effects. </summary>
        Dictionary<int, Dictionary<string, string>> textures = new Dictionary<int, Dictionary<string, string>>();
        /// <summary>   The indicies. </summary>
        Dictionary<int, List<int>> indicies = new Dictionary<int, List<int>>();
        /// <summary>   The transforms. </summary>
        List<Matrix> transforms = new List<Matrix>();

        /// <summary>   The boxs. </summary>
        List<BoundingBox> boxs = new List<BoundingBox>();
        /// <summary>   The spheres. </summary>
        List<BoundingSphere> spheres = new List<BoundingSphere>();

        /// <summary>   The bind pose. </summary>
        List<Matrix> bindPose = new List<Matrix>();
        /// <summary>   The inverse bind pose. </summary>
        List<Matrix> inverseBindPose = new List<Matrix>();
        /// <summary>   The skeleton hierarchy. </summary>
        List<int> skeletonHierarchy = new List<int>();
        /// <summary>   The animation clips. </summary>
        Dictionary<string, IKeyframeAnimationClip> animationClips;
        #endregion

        #region Properties for MGCB

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the scale. </summary>
        ///
        /// <value> The scale. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(1)]
        public virtual float Scale { get { return _modelProcessor.Scale; } set { _modelProcessor.Scale = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation z coordinate. </summary>
        ///
        /// <value> The rotation z coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual float RotationZ { get { return _modelProcessor.RotationZ; } set { _modelProcessor.RotationZ = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation y coordinate. </summary>
        ///
        /// <value> The rotation y coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual float RotationY { get { return _modelProcessor.RotationY; } set { _modelProcessor.RotationY = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the rotation x coordinate. </summary>
        ///
        /// <value> The rotation x coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual float RotationX { get { return _modelProcessor.RotationX; } set { _modelProcessor.RotationX = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether the resize textures to power of two.
        /// </summary>
        ///
        /// <value> True if resize textures to power of two, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool ResizeTexturesToPowerOfTwo { get { return _modelProcessor.ResizeTexturesToPowerOfTwo; } set { _modelProcessor.ResizeTexturesToPowerOfTwo = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the premultiply vertex colors. </summary>
        ///
        /// <value> True if premultiply vertex colors, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(true)]
        public virtual bool PremultiplyVertexColors { get { return _modelProcessor.PremultiplyVertexColors; } set { _modelProcessor.PremultiplyVertexColors = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the premultiply texture alpha. </summary>
        ///
        /// <value> True if premultiply texture alpha, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(true)]
        public virtual bool PremultiplyTextureAlpha { get { return _modelProcessor.PremultiplyTextureAlpha; } set { _modelProcessor.PremultiplyTextureAlpha = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the swap winding order. </summary>
        ///
        /// <value> True if swap winding order, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool SwapWindingOrder { get { return _modelProcessor.SwapWindingOrder; } set { _modelProcessor.SwapWindingOrder = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Force all the materials to use our skinned model effect. </summary>
        ///
        /// <value> The default effect. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(MaterialProcessorDefaultEffect.SkinnedEffect)]
        public virtual MaterialProcessorDefaultEffect DefaultEffect
        {
            get
            {
                if (SkinnedMesh)
                    _modelProcessor.DefaultEffect = MaterialProcessorDefaultEffect.SkinnedEffect;
                else
                    _modelProcessor.DefaultEffect = MaterialProcessorDefaultEffect.BasicEffect;



                return _modelProcessor.DefaultEffect;
            }
            set
            {
                if (SkinnedMesh)
                    _modelProcessor.DefaultEffect = MaterialProcessorDefaultEffect.SkinnedEffect;
                else
                    _modelProcessor.DefaultEffect = MaterialProcessorDefaultEffect.BasicEffect;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the color key is enabled. </summary>
        ///
        /// <value> True if color key enabled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(true)]
        public virtual bool ColorKeyEnabled { get { return _modelProcessor.ColorKeyEnabled; } set { _modelProcessor.ColorKeyEnabled = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the color key. </summary>
        ///
        /// <value> The color of the color key. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual Color ColorKeyColor { get { return _modelProcessor.ColorKeyColor; } set { _modelProcessor.ColorKeyColor = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the generate mipmaps. </summary>
        ///
        /// <value> True if generate mipmaps, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(true)]
        public virtual bool GenerateMipmaps { get { return _modelProcessor.GenerateMipmaps; } set { _modelProcessor.GenerateMipmaps = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the texture format. </summary>
        ///
        /// <value> The texture format. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(typeof(TextureProcessorOutputFormat), "Compressed")]
        public virtual TextureProcessorOutputFormat TextureFormat { get { return _modelProcessor.TextureFormat; } set { _modelProcessor.TextureFormat = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Enable Logging. </summary>
        ///
        /// <value> True if enable logging, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(false)]
        [Description("Set to true if you want logging on"), TypeConverter(typeof(BooleanConverter))]
        [Category("Void Engine")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("Enable Logging")]
        public bool EnableLogging { get; set; } = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Skinned Mesh. </summary>
        ///
        /// <value> True if skinned mesh, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(false)]
        [Description("Set to true if Mesh is skinned"), TypeConverter(typeof(BooleanConverter))]
        [Category("Void Engine")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("Skinned Mesh")]
        public bool SkinnedMesh { get; set; } = true;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Skinned Mesh. </summary>
        ///
        /// <value> The name of the animation clip. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue("Animation")]
        [Description("If a skinned mesh, this will be the name of the animation clip"), TypeConverter(typeof(BooleanConverter))]
        [Category("Void Engine")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("Animation Clip Name")]
        public string AnimationClipName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the generate tangent frames. </summary>
        ///
        /// <value> True if generate tangent frames, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        [DefaultValue(true)]
        public bool GenerateTangentFrames
        {
            get
            {
                return _modelProcessor.GenerateTangentFrames;
            }
            set
            {
                _modelProcessor.GenerateTangentFrames = value;
            }
        }
        #endregion

        /// <summary>   The mp. </summary>
        protected ModelProcessor _modelProcessor;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public SkinnedMeshProcessor() : base()
        {
            _modelProcessor = new ModelProcessor();
            // Always want the tangents frames.
            GenerateTangentFrames = true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <exception cref="InvalidContentException">
        ///     Thrown when an Invalid Content error condition occurs.
        /// </exception>
        ///
        /// <param name="input">    The input. </param>
        /// <param name="context">  The context. </param>
        ///
        /// <returns>   An IRandomchaosModelData. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override IRandomchaosModelData Process(NodeContent input, ContentProcessorContext context)
        {
            string modelName = input.Identity.SourceFilename.Substring(input.Identity.SourceFilename.LastIndexOf("\\") + 1);

            if (EnableLogging)
            {
                Logger.LogName = string.Format("{0}.log", modelName.Substring(modelName.LastIndexOf("/") + 1));
                Logger.WriteToLog(string.Format("Process started for {0}", modelName));
            }

            ModelContent baseModel = _modelProcessor.Process(input, context);

            foreach (ModelBoneContent bone in baseModel.Bones)
            {
                transforms.Add(bone.Transform);
            }

            if (EnableLogging)
                Logger.WriteToLog(string.Format("Running GenerateData for {0}", modelName));

            GenerateData(input);

            try
            {
                // look for "DiffuseColor"
                foreach (ModelMeshContent mesh in baseModel.Meshes)
                {
                    int idx = baseModel.Meshes.IndexOf(mesh);
                    Color col = Color.White;
                    TextureReferenceDictionary trd;
                    names.Add(mesh.Name);

                    
                    foreach (ModelMeshPartContent part in mesh.MeshParts)
                    {
                        trd = part.Material.Textures;
                        foreach (var t in trd)
                        {
                            if (!textures.ContainsKey(idx))
                            {
                                textures.Add(idx, new Dictionary<string, string>());
                            }

                            if (!textures[idx].ContainsKey(t.Key))
                            {
                                textures[idx].Add(t.Key, t.Value.Filename);
                            }
                            else
                            {
                                textures[idx][t.Key] = t.Value.Filename;
                            }
                        }
                        col = new Color((Vector3)part.Material.OpaqueData["DiffuseColor"]);                        

                        if (EnableLogging)
                            Logger.WriteToLog($"{mesh.Name} DiffuseColor[{idx}] = {col}");
                    }

                    int c = colors[idx].Count;

                    for (int i = 0; i < c; i++)
                    {
                        colors[idx][i] = col;
                    }
                }
            }
            catch (Exception ex)
            {
                if (EnableLogging)
                    Logger.WriteToLog($"Error {ex.GetType().Name} - {ex.Message}");
            }

            if (SkinnedMesh)
            {
                if (EnableLogging)
                    Logger.WriteToLog("Get Animation Data");

                MeshUtilis.ValidateMesh(input, context, null);

                // Find the skeleton.
                BoneContent skeleton = MeshHelper.FindSkeleton(input);

                if (skeleton == null)
                    throw new InvalidContentException("Input skeleton not found.");

                // We don't want to have to worry about different parts of the model being
                // in different local coordinate systems, so let's just bake everything.
                MeshUtilis.FlattenTransforms(input, skeleton);

                // Read the bind pose and skeleton hierarchy data.
                IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

                if (EnableLogging)
                    Logger.WriteToLog($"Found {bones.Count} bones..");

                if (bones.Count > SkinnedEffect.MaxBones)
                {
                    throw new InvalidContentException(string.Format(
                        "Skeleton has {0} bones, but the maximum supported is {1}.",
                        bones.Count, SkinnedEffect.MaxBones));
                }

                foreach (BoneContent bone in bones)
                {
                    bindPose.Add(bone.Transform);
                    inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                    skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
                }

                if (EnableLogging)
                    Logger.WriteToLog($"Found {skeleton.Animations.Count} animation(s)..");

                // Convert animation data to our runtime format.
                animationClips = MeshUtilis.ProcessAnimations(skeleton.Animations, bones, AnimationClipName);

                if (EnableLogging)
                {
                    foreach (string key in animationClips.Keys)
                        Logger.WriteToLog($"Animation {key} : {animationClips[key].Duration}");
                }

            }

            IRandomchaosModelData ModelData = new RandomchaosModelData()
            {
                Vertices = vertices,
                Indicies = indicies,
                TexCoords = texCoords,
                Tangents = tangents,
                BiNormals = biNormals,
                Normals = normals,
                Colors = colors,
                BoundingBoxs = boxs,
                BoundingSpheres = spheres,
                BlendIndex = blendIndex,
                BlendWeight = blendWeight,
                Transforms = transforms,
                Names = names,       
                Textures = textures,
            };

            if (SkinnedMesh)
            {
                if (EnableLogging)
                    Logger.WriteToLog("About to store SkinningData...");

                ISkinningData skinningData = new SkinningData(animationClips, bindPose, inverseBindPose, skeletonHierarchy);
                
                if (EnableLogging)
                    Logger.WriteToLog($"Storing SkinningData: Clips: {animationClips.Keys.Count} , Poses: {inverseBindPose.Count}, Skel: {skeletonHierarchy.Count}");

                ModelData.SkinningData = skinningData;
            }
            else
                ModelData.SkinningData = null;

            ModelData.Name = modelName.Substring(modelName.LastIndexOf("/") + 1);

            if (EnableLogging)
                Logger.WriteToLog(string.Format("Process completed for {0}", modelName));

            return ModelData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Generates a data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="node"> The node. </param>
        ///-------------------------------------------------------------------------------------------------

        private void GenerateData(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                MeshHelper.OptimizeForCache(mesh);

                // Look up the absolute transform of the mesh.
                Matrix absoluteTransform = mesh.AbsoluteTransform;


                int i = 0;
                // Loop over all the pieces of geometry in the mesh.
                foreach (GeometryContent geometry in mesh.Geometry)
                {
                    Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                    Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                    #region Logging
                    if (EnableLogging)
                    {
                        Logger.WriteToLog($"{geometry.Name} : Indicies = {geometry.Indices.Count}");
                        Logger.WriteToLog($"{geometry.Name} : Verts = {geometry.Vertices.VertexCount}");
                        Logger.WriteToLog($"{geometry.Name} : Channels = {geometry.Vertices.Channels.Count}");
                        Logger.WriteToLog($"{geometry.Name} : Positions = {geometry.Vertices.Positions.Count}");

                        int cnt = 0;

                        foreach (VertexChannel c in geometry.Vertices.Channels)
                        {
                            Logger.WriteToLog(string.Format("{0} : Channel[{1}] = {2}", mesh.Name, cnt++, c.Name));
                        }
                    }

                    #endregion
                    // Loop over all the indices in this piece of geometry.
                    // Every group of three indices represents one triangle.
                    List<Vector3> thisVerts = new List<Vector3>();
                    List<int> ind = new List<int>();
                    List<Vector2> tex = new List<Vector2>();
                    List<Vector3> norm = new List<Vector3>();
                    List<Vector3> tang = new List<Vector3>();
                    List<Vector3> biN = new List<Vector3>();
                    List<Byte4> blIn = new List<Byte4>();
                    List<Vector4> blWe = new List<Vector4>();
                    List<Color> cols = new List<Color>();


                    List<Texture2D> txtrs = new List<Texture2D>();

                    Vector2 tmpTex = Vector2.Zero;
                    Vector3 tmpNorm = Vector3.Zero;
                    Vector3 vertex = Vector3.Zero;
                    Vector3 tangent = Vector3.Zero;
                    Vector3 biNormal = Vector3.Zero;
                    Byte4 bI = new Byte4();
                    Vector4 bw = Vector4.Zero;
                    Color color = Color.White;

                    if (EnableLogging)
                        Logger.WriteToLog($"geometry.Indices = {geometry.Indices.Count}");

                    i = 0;
                    foreach (int index in geometry.Indices)
                        ind.Add(index);

                    for (int v = 0; v < geometry.Vertices.Positions.Count; v++)
                    {
                        // Look up the position of this vertex.
                        vertex = Vector3.Transform(geometry.Vertices.Positions[v], absoluteTransform);

                        if (geometry.Vertices.Channels.Contains("TextureCoordinate0"))
                            tmpTex = (Vector2)geometry.Vertices.Channels["TextureCoordinate0"][v];

                        if (geometry.Vertices.Channels.Contains("Normal0"))
                            tmpNorm = Vector3.Transform((Vector3)geometry.Vertices.Channels["Normal0"][v], absoluteTransform);

                        if (geometry.Vertices.Channels.Contains("Tangent0"))
                            tangent = Vector3.Transform((Vector3)geometry.Vertices.Channels["Tangent0"][v], absoluteTransform);

                        if (geometry.Vertices.Channels.Contains("BlendIndices0"))
                            bI = (Byte4)geometry.Vertices.Channels["BlendIndices0"][v];

                        if (geometry.Vertices.Channels.Contains("BlendWeight0"))
                            bw = (Vector4)geometry.Vertices.Channels["BlendWeight0"][v];

                        if (geometry.Vertices.Channels.Contains("Binormal0"))
                            biNormal = Vector3.Transform((Vector3)geometry.Vertices.Channels["Binormal0"][v], absoluteTransform);

                        if (geometry.Vertices.Channels.Contains("Color0"))
                            color = (Color)geometry.Vertices.Channels["Color0"][v];

                        // Store this data.
                        min = Vector3.Min(min, vertex);
                        max = Vector3.Max(max, vertex);

                        norm.Add(tmpNorm);
                        tex.Add(tmpTex);
                        thisVerts.Add(vertex);
                        tang.Add(tangent);
                        biN.Add(biNormal);
                        blWe.Add(bw);
                        blIn.Add(bI);
                        cols.Add(color);


                    }

                    BoundingBox box = new BoundingBox(min, max);
                    boxs.Add(box);
                    spheres.Add(BoundingSphere.CreateFromBoundingBox(box));

                    texCoords.Add(texCoords.Count, tex);
                    normals.Add(normals.Count, norm);
                    indicies.Add(indicies.Count, ind);
                    vertices.Add(vertices.Count, thisVerts);
                    tangents.Add(tangents.Count, tang);
                    biNormals.Add(biNormals.Count, biN);
                    colors.Add(colors.Count, cols);

                    blendWeight.Add(blendWeight.Count, blWe);
                    blendIndex.Add(blendIndex.Count, blIn);
                }
            }

            // Recursively scan over the children of this node.
            foreach (NodeContent child in node.Children)
            {
                GenerateData(child);
            }
        }
    }
}