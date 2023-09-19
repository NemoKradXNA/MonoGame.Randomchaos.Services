
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Scene.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A scene base. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class SceneBase : DrawableGameComponent, IScene
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for scene. </summary>
        ///
        /// <value> The scene manager. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISceneService sceneManager { get { return Game.Services.GetService<ISceneService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the coroutine service. </summary>
        ///
        /// <value> The coroutine service. </value>
        ///-------------------------------------------------------------------------------------------------

        protected ICoroutineService coroutineService { get { return Game.Services.GetService<ICoroutineService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the camera. </summary>
        ///
        /// <value> The camera. </value>
        ///-------------------------------------------------------------------------------------------------

        protected ICameraService camera { get { return Game.Services.GetService<ICameraService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for kB. </summary>
        ///
        /// <value> The kB manager. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IKeyboardStateManager kbManager { get { return Game.Services.GetService<IInputStateService>().KeyboardManager; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for milliseconds. </summary>
        ///
        /// <value> The milliseconds manager. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IMouseStateManager msManager { get { return Game.Services.GetService<IInputStateService>().MouseManager; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for audio. </summary>
        ///
        /// <value> The audio manager. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IAudioService audioManager { get { return Game.Services.GetService<IAudioService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets options for controlling the operation. </summary>
        ///
        /// <value> The parameters. </value>
        ///-------------------------------------------------------------------------------------------------

        protected object[] Parameters { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the last scene. </summary>
        ///
        /// <value> The last scene. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual IScene LastScene { get; set; }

        /// <summary>   The state. </summary>
        SceneStateEnum _state;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        public virtual SceneStateEnum State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (_state == SceneStateEnum.Unloaded)
                {
                    foreach (IGameComponent compontent in Components)
                    {
                        Game.Components.Remove(compontent);
                    }

                    Components.Clear();
                    Game.Components.Remove(this);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the audio music asset. </summary>
        ///
        /// <value> The audio music asset. </value>
        ///-------------------------------------------------------------------------------------------------

        public string AudioMusicAsset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the components. </summary>
        ///
        /// <value> The components. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<IGameComponent> Components { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public SceneBase(Game game, string name) : base(game)
        {
            Name = name;
            Components = new List<IGameComponent>();

            game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">             The game. </param>
        /// <param name="name">             The name. </param>
        /// <param name="audioMusicAsset">  The audio music asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public SceneBase(Game game, string name, string audioMusicAsset) : this(game,name)
        {
            AudioMusicAsset = audioMusicAsset;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// OVerride this so when the window size changes you can set positions of elements that rely on
        /// screen space coordinates (UI)
        /// </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="sender">   . </param>
        /// <param name="e">        . </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();

            foreach (IGameComponent component in Components)
                component.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Unload content. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void UnloadContent()
        {
            base.UnloadContent();            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draws the given game time. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void LoadScene()
        {
            // Load our things up!
            Game.Components.Add(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads a scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="parameters">   A variable-length parameters list containing parameters. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void LoadScene(params object[] parameters)
        {
            Parameters = parameters;

            LoadScene();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Unload scene. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void UnloadScene()
        {
            // Unload our shit!
            UnloadContent();

        }
    }
}
