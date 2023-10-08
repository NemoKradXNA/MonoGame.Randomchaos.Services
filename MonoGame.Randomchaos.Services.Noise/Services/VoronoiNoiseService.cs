using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System;

namespace MonoGame.Randomchaos.Services.Noise.Services
{
    public class VoronoiNoiseService : ServiceBase<VoronoiNoiseService>, INoiseService
    {
        public float Displacement { get; set; } = 1.0f;
        public float Frequency { get; set; } = 1.0f;

        protected int? _seed;
        public int Seed
        {
            get 
            {
                if (_seed == null)
                {
                    _seed = DateTime.Now.Millisecond;
                }

                return _seed.Value;
            }
            set { _seed = value; }
        }
        public bool Distance { get; set; } = false;
        public float Sqrt3 { get { return 1.7320508076f; } }

        public VoronoiNoiseService(Game game) : base(game) { }

        public float Noise(float x)
        {
            return Noise(x, 1, 1);
        }

        public float Noise(float x, float y)
        {
            return Noise(x, y, 1);
        }

        public float Noise(Vector2 coord)
        {
            return Noise(coord.X, coord.Y);
        }

        public float Noise(float x, float y, float z)
        {
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            var xi = (x > 0.0 ? (int)x : (int)x - 1);
            var iy = (y > 0.0 ? (int)y : (int)y - 1);
            var iz = (z > 0.0 ? (int)z : (int)z - 1);
            var md = 2147483647.0f;
            double xc = 0;
            double yc = 0;
            double zc = 0;
            for (var zcu = iz - 2; zcu <= iz + 2; zcu++)
            {
                for (var ycu = iy - 2; ycu <= iy + 2; ycu++)
                {
                    for (var xcu = xi - 2; xcu <= xi + 2; xcu++)
                    {
                        var xp = xcu + ValueNoise3D(xcu, ycu, zcu, Seed);
                        var yp = ycu + ValueNoise3D(xcu, ycu, zcu, Seed + 1);
                        var zp = zcu + ValueNoise3D(xcu, ycu, zcu, Seed + 2);
                        var xd = xp - x;
                        var yd = yp - y;
                        var zd = zp - z;
                        var d = xd * xd + yd * yd + zd * zd;
                        if (d < md)
                        {
                            md = d;
                            xc = xp;
                            yc = yp;
                            zc = zp;
                        }
                    }
                }
            }

            float v;
            
            if (Distance)
            {
                var xd = xc - x;
                var yd = yc - y;
                var zd = zc - z;
                v = (float) (Math.Sqrt(xd * xd + yd * yd + zd * zd)) * Sqrt3 - 1.0f;
            }
            else
            {
                v = 0.0f;
            }
            
            return v + (Displacement * ValueNoise3D((int)(Math.Floor(xc)), (int)(Math.Floor(yc)), (int)(Math.Floor(zc)), Seed)); 
        }

        public float Noise(Vector3 coord)
        {
            return Noise(coord.X, coord.Y, coord.Z);            
        }

        protected float ValueNoise3D(int x, int y, int z, int seed)
        {
            return 1.0f - (ValueNoise3DInt(x, y, z, seed) / 1073741824.0f);
        }

        protected long ValueNoise3DInt(int x, int y, int z, int seed)
        {
            long n = (x + 1971 * y + 221171 * z + 969 * seed) & 0x7fffffff;
            n = (n >> 13) ^ n;
            return (n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;
        }
    }
}
