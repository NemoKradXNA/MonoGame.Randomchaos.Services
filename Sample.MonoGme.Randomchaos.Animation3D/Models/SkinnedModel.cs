using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Animation.Animation3D;
using MonoGame.Randomchaos.Animation.Interfaces;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Sample.MonoGme.Randomchaos.Animation3D.Models
{
    public class SkinnedModel : DrawableGameComponent
    {
        public ITransform Transform { get; set; } = MonoGame.Randomchaos.Models.Transform.Identity;

        ICameraService _camera { get { return (ICameraService)Game.Services.GetService<ICameraService>(); } }

        protected Texture2D texture;
        public string ModelAsset { get; set; }
        public ISkinnedMesh _modelData { get; set; }
        public SkinnedModelData Model { get; set; }

        /// <summary>
        /// Axis Aligned bounding spheres property
        /// </summary>
        public List<BoundingSphere> AABoundingSpheres { get; set; } = new List<BoundingSphere>();

        /// <summary>
        /// Axis Aligned bounding box's property
        /// </summary>
        public List<BoundingBox> AABoundingBoxs { get; set; } = new List<BoundingBox>();

        protected Matrix[] _transforms;
        protected Matrix _meshWorld;
        protected Matrix _meshWVP;

        public Dictionary<string, SkinnedEffect> MeshEffects { get; set; } = new Dictionary<string, SkinnedEffect>();

        public ISkinnedData SkinningData { get; set; }
        public IKeyframeAnimationPlayer AnimationPlayer { get; set; }

        public SkinnedModel(Game game, string modelToLoad) : base(game)
        {
            ModelAsset = modelToLoad;
        }

        protected override void LoadContent()
        {
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });

            if (ModelAsset != null)
            {
                _modelData = Game.Content.Load<ISkinnedMesh>(ModelAsset);

                if (_modelData != null)
                {
                    SkinningData = _modelData.SkinningData;

                    if (SkinningData != null)
                    {
                        if (AnimationPlayer == null)
                            AnimationPlayer = new KeyFrameAnimationPlayer(SkinningData);
                        else
                            AnimationPlayer.SkinningDataValue = SkinningData;
                    }

                    _transforms = new Matrix[_modelData.Transforms.Count];
                    _modelData.Transforms.ToArray().CopyTo(_transforms, 0);

                    Model = new SkinnedModelData(GraphicsDevice, _modelData);

                    AABoundingBoxs.AddRange(_modelData.BoundingBoxs);
                    AABoundingSpheres.AddRange(_modelData.BoundingSpheres);

                }
                else
                {
                    // BAD LOAD!!
                    throw new Exception($"BAD MESH LOAD: IVoidEngineModel not found for '{ModelAsset}'.");
                }
            }
        }

        public void StartAnimation(string name, float blendSpeed = .001f)
        {
            AnimationPlayer.StartClip(SkinningData.AnimationClips[name], blendSpeed);
        }

        public void StopAnimation()
        {
            AnimationPlayer.StopClip();
        }

        public override void Update(GameTime gameTime)
        {
            if (AnimationPlayer != null)
                AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Transform.World);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int m = 0; m < Model.Meshes.Count; m++)
            {
                if (Model.Meshes[m].ParentBone != null)
                    _meshWorld = _transforms[Model.Meshes[m].ParentBone.Index] * Transform.World;
                else
                    _meshWorld = _transforms[0] * Transform.World;

                for (int mp = 0; mp < Model.Meshes[m].MeshParts.Count; mp++)
                {
                    // See if we can get a named material
                    SkinnedEffect effect = MeshEffects[Model.Meshes[m].MeshParts[mp].Name];

                    //Vector3 meshColor = Model.Meshes[m].MeshParts[mp].Color.ToVector3();
                    //effect.DiffuseColor = meshColor;// effect.DiffuseColor + meshColor;
                    SetEffect(effect, _meshWorld, AnimationPlayer);

                    int pCnt = effect.CurrentTechnique.Passes.Count;

                    for (int p = 0; p < pCnt; p++)
                    {
                        effect.CurrentTechnique.Passes[p].Apply();

                        Game.GraphicsDevice.SetVertexBuffer(Model.Meshes[m].MeshParts[mp].VertexBuffer);
                        Game.GraphicsDevice.Indices = Model.Meshes[m].MeshParts[mp].IndexBuffer;
                        Game.GraphicsDevice.DrawIndexedPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, Model.Meshes[m].MeshParts[mp].VertexOffset,
                            0, Model.Meshes[m].MeshParts[mp].PrimitiveCount);
                    }
                }
            }
        }

        protected void SetEffect(SkinnedEffect effect, Matrix world, IKeyframeAnimationPlayer animationPlayer)
        {
            if (animationPlayer != null)
                effect.SetBoneTransforms(animationPlayer.GetSkinTransforms());

            //effect.World = world;
            effect.View = _camera.View;
            effect.Projection = _camera.Projection;
            effect.Texture = texture;
        }
    }
}
