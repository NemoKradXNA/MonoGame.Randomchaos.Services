using HardwareInstancedParticles.Models.VertexType;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HardwareInstancedParticles.Models
{
    public class InstancedParticleEmitter : DrawableGameComponent
    {
        protected ICameraService Camera { get { return Game.Services.GetService<ICameraService>(); } }

        public ITransform Transform { get; set; }
        public float ViewDistance { get; set; }

        protected Effect Effect;
        public List<ITransform> Particles = new List<ITransform>();
        public Dictionary<ITransform, VertexPositionColorNormalTextureTangent[]> vertexArray = new Dictionary<ITransform, VertexPositionColorNormalTextureTangent[]>();
        public Dictionary<ITransform, Texture2D> ParticleTextures = new Dictionary<ITransform, Texture2D>();
        public Dictionary<ITransform, bool> Active = new Dictionary<ITransform, bool>();

        int[] index = new int[] { 0, 1, 2, 2, 3, 0, };

        private VertexDeclaration instanceVertexDeclaration;
        private DynamicVertexBuffer instanceBuffer;
        private DynamicIndexBuffer indexBuffer;
        private DynamicVertexBuffer geometryBuffer;
        //private IndexBuffer indexBuffer;
        private VertexBufferBinding[] bindings;

        public struct instanceStruct
        {
            public Matrix world { get; set; }
            public Color color { get; set; }

            public void SetWorld(Matrix mat)
            {
                world = mat;
            }
        }

        public List<instanceStruct> instances = new List<instanceStruct>();

        public InstancedParticleEmitter(Game game) : base(game) { Transform = new Transform(); ViewDistance = -1; }

        protected void InitializeInstanceVertexBuffer()
        {
            if (Particles.Count == 0)
                return;

            VertexElement[] _instanceStreamElements = new VertexElement[5];

            // Position
            _instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 1);
            _instanceStreamElements[1] = new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector4, VertexElementUsage.Position, 2);
            _instanceStreamElements[2] = new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector4, VertexElementUsage.Position, 3);
            _instanceStreamElements[3] = new VertexElement(sizeof(float) * 12, VertexElementFormat.Vector4, VertexElementUsage.Position, 4);
            _instanceStreamElements[4] = new VertexElement(sizeof(float) * 16, VertexElementFormat.Color, VertexElementUsage.Color, 1);

            instanceVertexDeclaration = new VertexDeclaration(_instanceStreamElements);

            instanceBuffer = new DynamicVertexBuffer(Game.GraphicsDevice, instanceVertexDeclaration, Particles.Count, BufferUsage.WriteOnly);
            instanceBuffer.SetData(instances.ToArray());


            bindings = new VertexBufferBinding[2];
            bindings[0] = new VertexBufferBinding(geometryBuffer);
            bindings[1] = new VertexBufferBinding(instanceBuffer, 0, 1);
        }

        public override void Initialize()
        {
            InitializeInstanceVertexBuffer();
            Effect = Game.Content.Load<Effect>("Shaders/Billboard");

            if (ViewDistance < 0)
                ViewDistance = Camera.FarClipPlane;

            base.Initialize();
        }

        public ITransform AddParticle(Vector3 position, Vector3 scale, Quaternion rot, Texture2D texture, Color color, bool active = true, bool bound = true)
        {


            ITransform transform = new Transform() { Position = position, Rotation = rot, Scale = scale };

            if (bound)
                transform.Parent = Transform;

            Particles.Add(transform);
            VertexPositionColorNormalTextureTangent[] vb = new VertexPositionColorNormalTextureTangent[]{
                new VertexPositionColorNormalTextureTangent(Vector3.Zero, Vector3.Forward, Vector3.Zero, new Vector2(1,1), color),
                new VertexPositionColorNormalTextureTangent(Vector3.Zero, Vector3.Forward, Vector3.Zero, new Vector2(0, 1),color),
                new VertexPositionColorNormalTextureTangent(Vector3.Zero, Vector3.Forward, Vector3.Zero, new Vector2(0,0), color),
                new VertexPositionColorNormalTextureTangent(Vector3.Zero, Vector3.Forward, Vector3.Zero, new Vector2(1, 0),color)
            };

            if (geometryBuffer == null)
            {
                geometryBuffer = new DynamicVertexBuffer(Game.GraphicsDevice, VertexPositionColorNormalTextureTangent.VertDec,
                                                  4, BufferUsage.WriteOnly);
                geometryBuffer.SetData(vb);

                //indexBuffer = new IndexBuffer(Game.GraphicsDevice, typeof(int), index.Length, BufferUsage.WriteOnly);
                //indexBuffer.SetData(index);

                indexBuffer = new DynamicIndexBuffer(Game.GraphicsDevice, IndexElementSize.ThirtyTwoBits, index.Length, BufferUsage.WriteOnly);
                indexBuffer.SetData(index);
            }

            instances.Add(new instanceStruct() { world = transform.World, color = color });
            
            
            vertexArray.Add(transform, vb);

            if (texture == null)
                texture = Game.Content.Load<Texture2D>("Textures/Particles/smoke0");

            ParticleTextures.Add(transform, texture);
            Active.Add(transform, active);

            return transform;
        }

        public bool ParticlesUpdated = false;

        public override void Update(GameTime gameTime)
        {
            //int instanceDataSize = SizeOfMatrix * Particles.Count;
            //if ((instanceBuffer == null) || (instanceBuffer.su < instanceDataSize))
            //{
            //    if (vb2 != null)
            //        vb2.Dispose();

            //    vb2 = new DynamicVertexBuffer(Game.GraphicsDevice, instanceDataSize, BufferUsage.WriteOnly);
            //}
            //vb2.SetData(worlds.ToArray(), 0, worlds.ToArray().Length, SetDataOptions.Discard);

            base.Update(gameTime);
        }

        public void SetParticle(int p, ITransform transform)
        {
            instances[p] = new instanceStruct() { world = transform.World, color = instances[p].color };

            ParticlesUpdated = true;
        }

        public int ParticleType = 0;
        public bool Wire = false;

        public override void Draw(GameTime gameTime)
        {
            if (Particles.Count == 0)
                return;

            int pCnt = Effect.CurrentTechnique.Passes.Count;

            //Game.GraphicsDevice.BlendState = BlendState.Additive;
            //Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Game.GraphicsDevice.Indices = indexBuffer;
            Game.GraphicsDevice.SetVertexBuffers(bindings);

            //if (ParticlesUpdated)
            {
                instanceBuffer.SetData(instances.ToArray());
            }

            if (ParticleType == 0)
            {
                Game.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None , FillMode = Wire ? FillMode.WireFrame : FillMode.Solid };
            }

            for (int p = 0; p < pCnt; p++)
            {
                //foreach (ITransform transform in Particles)
                {
                    //Effect.Parameters["world"].SetValue(transform.World);
                    //Effect.Parameters["wvp"].SetValue(transform.World * Camera.View * Camera.Projection);
                    Effect.Parameters["vp"].SetValue(Camera.View * Camera.Projection);
                    Effect.Parameters["EyePosition"].SetValue(Camera.Transform.Position);
                    Effect.Parameters["viewDistance"].SetValue(ViewDistance);
                    if (Effect.Parameters["textureMat"] != null)
                        Effect.Parameters["textureMat"].SetValue(ParticleTextures[ParticleTextures.Keys.First()]);

                    Effect.Parameters["_StaticCylinderSpherical"].SetValue(ParticleType);
                    Effect.CurrentTechnique.Passes[p].Apply();

                    //Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertexArray[Particles.First()], 0, 4, index, 0, 2);
                    Game.GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, Particles.Count);
                }
            }
        }
    }
}
