using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public abstract class ServiceBase<T> : GameComponent 
    {
        public ServiceBase(Game game) : base(game) 
        {
            game.Services.AddService(typeof(T), this);
            game.Components.Add(this);
        }
    }
}
