using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Services.Scene.Models
{
    public abstract class SceneBase : DrawableGameComponent, IScene
    {
        public ISceneService sceneManager { get { return Game.Services.GetService<ISceneService>(); } }
        protected ICoroutineService coroutineService { get { return Game.Services.GetService<ICoroutineService>(); } }
        protected ICameraService camera { get { return Game.Services.GetService<ICameraService>(); } }
        protected IKeyboardStateManager kbManager { get { return Game.Services.GetService<IInputStateService>().KeyboardManager; } }
        protected IMouseStateManager msManager { get { return Game.Services.GetService<IInputStateService>().MouseManager; } }

        protected IAudioService audioManager { get { return Game.Services.GetService<IAudioService>(); } }

        protected object[] Parameters { get; set; }

        public string Name { get; set; }
        public IScene LastScene { get; set; }

        SceneStateEnum _state;
        public SceneStateEnum State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (_state == SceneStateEnum.Unloaded)
                {
                    Components.Clear();
                    Game.Components.Remove(this);
                }
            }
        }

        public List<IGameComponent> Components { get; set; }

        public SceneBase(Game game, string name) : base(game)
        {
            Name = name;
            Components = new List<IGameComponent>();

            game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        /// <summary>
        /// OVerride this so when the window size changes you can set positions of elements that rely on screen space coordinates (UI)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (IGameComponent component in Components)
                component.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (State == SceneStateEnum.Unloaded)
                return;

            base.Update(gameTime);

            foreach (IGameComponent component in Components)
            {
                if (component is IUpdateable && ((IUpdateable)component).Enabled)
                    ((IUpdateable)component).Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (State == SceneStateEnum.Unloaded)
                return;

            foreach (IGameComponent component in Components)
            {
                if (component is IDrawable && ((IDrawable)component).Visible)
                    ((IDrawable)component).Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public virtual void LoadScene()
        {
            // Load our things up!
            Game.Components.Add(this);
        }

        public virtual void LoadScene(params object[] parameters)
        {
            Parameters = parameters;

            LoadScene();
        }

        public virtual void UnloadScene()
        {
            // Unload our shit!
            UnloadContent();

        }
    }
}
