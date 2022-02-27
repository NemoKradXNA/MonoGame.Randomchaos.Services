using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IAccelerometerHandler : IInputStateManager
    {
        Vector3 AccelerometerState { get; set; }
        DateTimeOffset TimeStamp { get; set; }
    }
}
