using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Scene.Models
{
    public class SceneComponentColection : ISceneComponentColection
    {
        public List<Type> UIComponentTypes { get; set; } = new List<Type>();
        public List<IGameComponent> Components { get; set; } = new List<IGameComponent>();
        public List<IGameComponent> UIComponents { get; set; } = new List<IGameComponent>();
        public List<IGameComponent> SceneComponents { get; set; } = new List<IGameComponent>();
        public void Add(IGameComponent component)
        {
            Components.Add(component);

            if (UIComponentTypes != null && UIComponentTypes.Count > 0)
            {
                UIComponents.Add(component);
            }
            else
            {
                SceneComponents.Add(component);
            }
        }

        public void Clear()
        {
            UIComponents.Clear();
            SceneComponents.Clear();
            Components.Clear();
        }
    }
}
