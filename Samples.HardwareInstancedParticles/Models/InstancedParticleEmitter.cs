
using HardwareInstancedParticles.Models.VertexType;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HardwareInstancedParticles.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An instanced particle emitter. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class InstancedParticleEmitter : DrawableGameComponent
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the camera. </summary>
        ///
        /// <value> The camera. </value>
        ///-------------------------------------------------------------------------------------------------

        protected ICameraService Camera { get { return Game.Services.GetService<ICameraService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        public ITransform Transform { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the view distance. </summary>
        ///
        /// <value> The view distance. </value>
        ///-------------------------------------------------------------------------------------------------

        public float ViewDistance { get; set; }

        /// <summary>   The effect. </summary>
        protected Effect Effect;
        /// <summary>   The particles. </summary>
        public List<ITransform> Particles = new List<ITransform>();
        /// <summary>   Array of vertices. </summary>
        public Dictionary<ITransform, VertexPositionColorNormalTextureTangent[]> vertexArray = new Dictionary<ITransform, VertexPositionColorNormalTextureTangent[]>();
        /// <summary>   The particle textures. </summary>
        public Dictionary<ITransform, Texture2D> ParticleTextures = new Dictionary<ITransform, Texture2D>();
        /// <summary>   The active. </summary>
        public Dictionary<ITransform, bool> Active = new Dictionary<ITransform, bool>();

        /// <summary>   Zero-based index of the. </summary>
        int[] index = new int[] { 0, 1, 2, 2, 3, 0, };

        /// <summary>   The instance vertex declaration. </summary>
        private VertexDeclaration instanceVertexDeclaration;
        /// <summary>   Buffer for instance data. </summary>
        private DynamicVertexBuffer instanceBuffer;
        /// <summary>   Buffer for index data. </summary>
        private DynamicIndexBuffer indexBuffer;
        /// <summary>   Buffer for geometry data. </summary>
        private DynamicVertexBuffer geometryBuffer;
        //private IndexBuffer indexBuffer;
        /// <summary>   The bindings. </summary>
        private VertexBufferBinding[] bindings;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   An instance structure. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public struct instanceStruct
        {
            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets or sets the world. </summary>
            ///
            /// <value> The world. </value>
            ///-------------------------------------------------------------------------------------------------

            public Matrix world { get; set; }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets or sets the color. </summary>
            ///
            /// <value> The color. </value>
            ///-------------------------------------------------------------------------------------------------

            public Color color { get; set; }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Sets a world. </summary>
            ///
            /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
            ///
            /// <param name="mat">  The matrix. </param>
            ///-------------------------------------------------------------------------------------------------

            public void SetWorld(Matrix mat)
            {
                world = mat;
            }
        }

        /// <summary>   The instances. </summary>
        public List<instanceStruct> instances = new List<instanceStruct>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public InstancedParticleEmitter(Game game) : base(game) { Transform = new Transform(); ViewDistance = -1; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the instance vertex buffer. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            InitializeInstanceVertexBuffer();
            Effect = Game.Content.Load<Effect>("Shaders/Billboard");

            if (ViewDistance < 0)
                ViewDistance = Camera.FarClipPlane;

            base.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a particle. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="position"> The position. </param>
        /// <param name="scale">    The scale. </param>
        /// <param name="rot">      The rot. </param>
        /// <param name="texture">  The texture. </param>
        /// <param name="color">    The color. </param>
        /// <param name="active">   (Optional) True to active. </param>
        /// <param name="bound">    (Optional) True to bound. </param>
        ///
        /// <returns>   An ITransform. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        /// <summary>   True if particles updated. </summary>
        public bool ParticlesUpdated = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets a particle. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="p">            An int to process. </param>
        /// <param name="transform">    The transform. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SetParticle(int p, ITransform transform)
        {
            instances[p] = new instanceStruct() { world = transform.World, color = instances[p].color };

            ParticlesUpdated = true;
        }

        /// <summary>   Type of the particle. </summary>
        public int ParticleType = 0;
        /// <summary>   True to wire. </summary>
        public bool Wire = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

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
