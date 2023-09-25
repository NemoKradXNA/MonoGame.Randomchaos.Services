using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Primitives3D.Models.Voxel
{
    public class GeometryVoxelBase<T> : GeometryBase<T> where T : IVertexType
    {
        static Vector2 uvBase = new Vector2(0.0625f, 0.0625f);
        static Vector3 halfBlock = new Vector3(.5f, .5f, .5f);

        static Dictionary<CubeMapFace, List<Vector3>> ChunkFaceVertices = new Dictionary<CubeMapFace, List<Vector3>>()
        {
            { CubeMapFace.PositiveZ, new List<Vector3>(){ new Vector3(-.5f, .5f, .5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, -.5f, .5f),new Vector3(-.5f, -.5f, .5f) } },
            { CubeMapFace.NegativeZ, new List<Vector3>(){ new Vector3(-.5f, .5f, -.5f), new Vector3(.5f, .5f, -.5f), new Vector3(.5f, -.5f, -.5f), new Vector3(-.5f, -.5f, -.5f) } },
            { CubeMapFace.PositiveY, new List<Vector3>(){ new Vector3(-.5f, .5f, .5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, .5f, -.5f), new Vector3(-.5f, .5f, -.5f) } },
            { CubeMapFace.NegativeY, new List<Vector3>(){ new Vector3(-.5f, -.5f, .5f), new Vector3(.5f, -.5f, .5f), new Vector3(.5f, -.5f, -.5f), new Vector3(-.5f, -.5f, -.5f) } },
            { CubeMapFace.NegativeX, new List<Vector3>(){ new Vector3(-.5f, .5f, -.5f), new Vector3(-.5f, .5f, .5f), new Vector3(-.5f, -.5f, .5f), new Vector3(-.5f, -.5f, -.5f) } },
            { CubeMapFace.PositiveX, new List<Vector3>(){ new Vector3(.5f, .5f, -.5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, -.5f, .5f), new Vector3(.5f, -.5f, -.5f) } }
        };

        static Dictionary<CubeMapFace, List<Vector3>> ChunkFaceNormals = new Dictionary<CubeMapFace, List<Vector3>>()
        {
            { CubeMapFace.PositiveZ, new List<Vector3>(){ Vector3.Backward, Vector3.Backward, Vector3.Backward, Vector3.Backward } },
            { CubeMapFace.NegativeZ, new List<Vector3>(){ Vector3.Forward, Vector3.Forward, Vector3.Forward, Vector3.Forward } },
            { CubeMapFace.PositiveY, new List<Vector3>(){ Vector3.Up, Vector3.Up, Vector3.Up, Vector3.Up } },
            { CubeMapFace.NegativeY, new List<Vector3>(){ Vector3.Down,Vector3.Down,Vector3.Down,Vector3.Down } },
            { CubeMapFace.NegativeX, new List<Vector3>(){ Vector3.Left, Vector3.Left, Vector3.Left, Vector3.Left } },
            { CubeMapFace.PositiveX, new List<Vector3>(){ Vector3.Right, Vector3.Right, Vector3.Right, Vector3.Right } }
        };

        static Dictionary<CubeMapFace, List<Vector2>> ChunkFaceTexcoords = new Dictionary<CubeMapFace, List<Vector2>>()
        {
            { CubeMapFace.PositiveZ, new List<Vector2>(){  new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1) } },
            { CubeMapFace.NegativeZ, new List<Vector2>(){  new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1) } },
            { CubeMapFace.PositiveY, new List<Vector2>(){  new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1) } },
            { CubeMapFace.NegativeY, new List<Vector2>(){  new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1) } },
            { CubeMapFace.NegativeX, new List<Vector2>(){  new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1) } },
            { CubeMapFace.PositiveX, new List<Vector2>(){  new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1) } }
        };

        static Dictionary<CubeMapFace, List<Color>> ChunkFaceColors = new Dictionary<CubeMapFace, List<Color>>()
        {
            { CubeMapFace.PositiveZ, new List<Color>(){  Color.White, Color.White, Color.White, Color.White } },
            { CubeMapFace.NegativeZ, new List<Color>(){  Color.White, Color.White, Color.White, Color.White } },
            { CubeMapFace.PositiveY, new List<Color>(){  Color.White, Color.White, Color.White, Color.White } },
            { CubeMapFace.NegativeY, new List<Color>(){  Color.White, Color.White, Color.White, Color.White } },
            { CubeMapFace.NegativeX, new List<Color>(){  Color.White, Color.White, Color.White, Color.White } },
            { CubeMapFace.PositiveX, new List<Color>(){  Color.White, Color.White, Color.White, Color.White } }
        };

        static Dictionary<CubeMapFace, List<int>> ChunkFaceIndex = new Dictionary<CubeMapFace, List<int>>()
        {
            { CubeMapFace.PositiveZ, new List<int>() { 0, 1, 2, 2, 3, 0 } },
            { CubeMapFace.NegativeZ, new List<int>() { 0, 3, 2, 2, 1, 0 } },
            { CubeMapFace.PositiveY, new List<int>() { 0, 3, 2, 2, 1, 0 } },
            { CubeMapFace.NegativeY, new List<int>() { 0, 1, 2, 2, 3, 0 } },
            { CubeMapFace.NegativeX, new List<int>() { 0, 1, 2, 2, 3, 0 } },
            { CubeMapFace.PositiveX, new List<int>() { 0, 3, 2, 2, 1, 0 } }
        };

        public Point? AtlasDimensions { get; set; } = null;
        public Vector3 VoxelCentre { get { return new Vector3(_blocksWide, _blocksHigh, _blocksDeep) * .5f; } }

        protected int _blocksWide;
        protected int _blocksHigh;
        protected int _blocksDeep;

        public VoxelChunk[,,] map;

        protected int _startBlockType;

        public bool GenerateEmpty { get; set; }

        public Vector3 UvOffset { get; set; } = Vector3.One;

        public GeometryVoxelBase(Game game, int startBlockType = 1, int blocksWide = 10, int blocksHigh = 10, int blocksDeep = 10) : base(game)
        {
            _startBlockType = startBlockType;

            _blocksWide = blocksWide;
            _blocksHigh = blocksHigh;
            _blocksDeep = blocksDeep;
        }

        protected override void LoadContent()
        {
            Generate();

            Build();

            base.LoadContent();
        }


        protected virtual void Generate()
        {
            if (map == null)
            {
                map = new VoxelChunk[_blocksWide, _blocksHigh, _blocksDeep];
            }


            for (int x = 0; x < _blocksWide; x++)
            {
                for (int y = 0; y < _blocksHigh; y++)
                {
                    for (int z = 0; z < _blocksDeep; z++)
                    {
                        map[x, y, z] = new VoxelChunk() { On = !GenerateEmpty, Position = new Vector3(x, y, z), BlockType = _startBlockType, Shape = 0 };
                    }
                }
            }
        }

        public virtual void Build()
        {
            Vector2 fracUV = uvBase / _blocksWide;

            for (int x = 0; x < _blocksWide; x++)
            {
                for (int y = 0; y < _blocksHigh; y++)
                {
                    for (int z = 0; z < _blocksDeep; z++)
                    {
                        if (!map[x, y, z].On || ChunkSurounded(x, y, z))
                            continue;

                        DrawChunk(x, y, z, map[x, y, z]);
                    }
                }
            }
        }



        protected void DrawChunk(int x, int y, int z, VoxelChunk block)
        {
            Vector3 pos = new Vector3(x, y, z) - VoxelCentre;

            UvOffset = new Vector3(x, y, z);

            block.Triangles.Clear();

            // Bottom
            //if (RenderYFace)
            {
                if (IsChunkTransparent(x, y - 1, z))
                {
                    DrawFace(pos, CubeMapFace.NegativeY, block);
                }

                // Top
                if (IsChunkTransparent(x, y + 1, z))
                {
                    DrawFace(pos, CubeMapFace.PositiveY, block);
                }
            }

            // right
            if (IsChunkTransparent(x - 1, y, z))
            {
                //offset1 = Vector3.up;
                //offset2 = Vector3.back;
                ////DrawFace(start, offset1, offset2, block);
                DrawFace(pos, CubeMapFace.NegativeX, block);
            }

            // left
            if (IsChunkTransparent(x + 1, y, z))
            {
                DrawFace(pos, CubeMapFace.PositiveX, block);
            }

            // Front
            if (IsChunkTransparent(x, y, z - 1))
            {
                DrawFace(pos, CubeMapFace.NegativeZ, block);
            }

            // Back
            if (IsChunkTransparent(x, y, z + 1))
            {
                DrawFace(pos, CubeMapFace.PositiveZ, block);
            }


            //BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            //bc.center = pos;
            //bc.size = Vector3.one;

        }

        public void DrawFace(Vector3 pos, CubeMapFace facing, VoxelChunk block)
        {
            if (Vertices == null)
            {
                Vertices = new List<Vector3>();
                Normals = new List<Vector3>();
                Colors = new List<Color>();
                Texcoords = new List<Vector2>();
                Indicies = new List<int>();
            }

            //int vLen = quad.Length;
            int idx = Vertices.Count;
            Vector3 sOff = Vector3.Zero;

            //Vertices.AddRange(ChunkFaceVertices[facing]);

            for (int v = 0; v < 4; v++)
            {
                sOff = Vector3.Zero;

                Vector3 p = ChunkFaceVertices[facing][v] + pos + halfBlock + sOff;
                Vector3 n = ChunkFaceNormals[facing][v];

                Vertices.Add(p);
                Normals.Add(n);
                Colors.Add(ChunkFaceColors[facing][v]);
                if (AtlasDimensions == null)
                {
                    Texcoords.Add(ChunkFaceTexcoords[facing][v]);
                }
                else
                {
                    Texcoords.Add(GetTextCoords(block.BlockType, v));
                }
            }

            for (int i = 0; i < 6; i++)
            {
                Indicies.Add(ChunkFaceIndex[facing][i] + idx);
            }

            for (int t = 0; t < 6; t+=3)
            {
                Triangle triangle = new Triangle();

                int i1 = ChunkFaceIndex[facing][t + 0] + idx;
                int i2 = ChunkFaceIndex[facing][t + 1] + idx;
                int i3 = ChunkFaceIndex[facing][t + 2] + idx;

                triangle.Point1 = new VertexPoint(Vertices[i1], Normals[i1]);
                triangle.Point2 = new VertexPoint(Vertices[i2], Normals[i2]);
                triangle.Point3 = new VertexPoint(Vertices[i3], Normals[i3]);


                block.Triangles.Add(triangle);
            }
        }

        // new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1)
        static Vector2[] uvMap = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0.0625f, 0),
            new Vector2(0.0625f, 0.0625f),
            new Vector2(0, 0.0625f),
        };

        public Vector2 GetTextCoords(int BlockType, int vert)
        {
            int x = 0, y = 0;

            // 
            int blockType = BlockType - 1;

            x = (blockType % AtlasDimensions.Value.X);
            y = (blockType / AtlasDimensions.Value.Y);

            Vector2 offset = new Vector2(uvBase.X * x, uvBase.Y * y);

            Vector2 fv = uvMap[vert] / _blocksWide;
            Vector2 uv = new Vector2(uvMap[vert].X + offset.X, uvMap[vert].Y + offset.Y);

            return uv;
        }

        public VoxelChunk GetVoxelChunk(int x, int y, int z)
        {
            if (x >= 0 && x < _blocksWide &&
                y >= 0 && y < _blocksHigh &&
                z >= 0 && z < _blocksDeep)
            {
                return map[x, y, z];
            }
            else
            {
                return null;
            }
        }

        public bool IsChunkTransparent(int x, int y, int z)
        {
            //if (TransparentChunk)
            //    return true;

            if (x < 0 || y < 0 || z < 0 || x >= _blocksWide || y >= _blocksHigh || z >= _blocksDeep)
            {
                if (x < 0)// && neigbours.ContainsKey(left) && neigbours[left].Generated)
                          //if (neigbours[left].map[neigbours[left].width - 1, y, z].BlockType == 0 || (neigbours[left].map[neigbours[left].width - 1, y, z].BlockType != 0 && neigbours[left].map[neigbours[left].width - 1, y, z].IsSlope))
                    return true;

                if (x >= _blocksWide)// && neigbours.ContainsKey(right) && neigbours[right].Generated)
                                     //if (neigbours[right].map[0, y, z].BlockType == 0 || (neigbours[right].map[0, y, z].BlockType != 0 && neigbours[right].map[0, y, z].IsSlope))
                    return true;

                if (z < 0)// && neigbours.ContainsKey(bak) && neigbours[bak].Generated)
                          //if (neigbours[bak].map[x, y, neigbours[bak].width - 1].BlockType == 0 || (neigbours[bak].map[x, y, neigbours[bak].width - 1].BlockType != 0 && neigbours[bak].map[x, y, neigbours[bak].width - 1].IsSlope))
                    return true;

                if (z >= _blocksDeep)// && neigbours.ContainsKey(fwd) && neigbours[fwd].Generated)
                                     //if (neigbours[fwd].map[x, y, 0].BlockType == 0 || (neigbours[fwd].map[x, y, 0].BlockType != 0 && neigbours[fwd].map[x, y, 0].IsSlope))
                    return true;

                if (y < 0)// && neigbours.ContainsKey(down) && neigbours[down].Generated && neigbours[down].map[x, neigbours[down].width - 1, z].BlockType == 0)
                    return true;

                if (y >= _blocksHigh)// && neigbours.ContainsKey(up) && neigbours[up].Generated && neigbours[up].map[x, 0, z].BlockType == 0)
                    return true;

                if (y >= _blocksHigh)// && !neigbours.ContainsKey(up))
                    return true;

                return false;

            }

            if (map[x, y, z].On && map[x, y, z].Shape != 0)
            {
                return true;
            }

            return !map[x, y, z].On;
        }

        protected bool ChunkSurounded(int x, int y, int z)
        {
            bool retVal = false;

            if (x == 0 || y == 0 || z == 0 || x == _blocksWide - 1 || y == _blocksHigh - 1 || z == _blocksDeep - 1)
                return retVal;

            // Later check for transparency..
            if (map[x - 1, y, z].On &&
                map[x + 1, y, z].On &&
                map[x, y - 1, z].On &&
                map[x, y + 1, z].On &&
                map[x, y, z - 1].On &&
                map[x, y, z + 1].On &&

                map[x - 1, y - 1, z - 1].On &&
                map[x + 1, y - 1, z + 1].On &&
                map[x - 1, y - 1, z + 1].On &&
                map[x + 1, y - 1, z - 1].On &&

                map[x - 1, y + 1, z - 1].On &&
                map[x + 1, y + 1, z + 1].On &&
                map[x - 1, y + 1, z + 1].On &&
                map[x + 1, y + 1, z - 1].On
                )
            {
                retVal = true;
            }

            map[x, y, z].IsTransparent = retVal;

            return retVal;
        }
    }
}