
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

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
        /// <summary>   Gets or sets the post processing. </summary>
        ///
        /// <value> The post processing. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IPostProcessingComponent postProcess { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the post process user interface. </summary>
        ///
        /// <value> The post process user interface. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IPostProcessingComponent postProcessUI { get; set; }


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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the clear. </summary>
        ///
        /// <value> The color of the clear. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color ClearColor { get; set; } = Color.CornflowerBlue;

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
                    foreach (IGameComponent compontent in Components.Components)
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

        /// <summary>   The music volume. </summary>
        float _MusicVolume = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the music volume. </summary>
        ///
        /// <value> The music volume. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MusicVolume
        {
            get { return _MusicVolume; }
            set
            {
                _MusicVolume = Math.Min(1, Math.Max(0, value));

                if (audioManager != null)
                {
                    audioManager.MusicVolume = _MusicVolume;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the music maximum volume. </summary>
        ///
        /// <value> The music maximum volume. </value>
        ///-------------------------------------------------------------------------------------------------

        /// <summary>   The music maximum volume. </summary>
        protected float _MusicMaxVolume = 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the music maximum volume. </summary>
        ///
        /// <value> The music maximum volume. </value>
        ///-------------------------------------------------------------------------------------------------

        public float MusicMaxVolume
        {
            get { return _MusicMaxVolume; }
            set
            {
                _MusicMaxVolume = Math.Min(1, Math.Max(0, value));
            }
        }


        /// <summary>   The components. </summary>
        protected List<IGameComponent> _Components;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the components. </summary>
        ///
        /// <value> The components. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISceneComponentColection Components { get; set; } = new SceneComponentColection();


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a list of types of the UI components. This is used to distinguish between UI and
        /// regular scene elements. UI elements will be rendered after scene objects and have their own
        /// render loop.
        /// </summary>
        ///
        /// <value> A list of types of the components. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Type> UIComponentTypes { get { return Components.UIComponentTypes; } set { Components.UIComponentTypes = value; } }

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

        public SceneBase(Game game, string name, string audioMusicAsset) : this(game, name)
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

            foreach (IGameComponent component in Components.Components)
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

            if (postProcess != null)
            {
                postProcess.Update(gameTime);
            }

            base.Update(gameTime);

            try
            {
                foreach (IGameComponent component in Components.Components)
                {
                    if (component is IUpdateable && ((IUpdateable)component).Enabled)
                        ((IUpdateable)component).Update(gameTime);
                }
            }
            catch (Exception ex)
            {
                // need to log this. This could be due to runtime changes in the list.
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

            // Main scene
            if (postProcess != null)
            {
                postProcess.StartPostProcess(gameTime);
                GraphicsDevice.Clear(camera != null ? camera.ClearColor : ClearColor);
            }

            try
            {
                foreach (IGameComponent component in Components.SceneComponents)
                {
                    if (component is IDrawable && ((IDrawable)component).Visible)
                        ((IDrawable)component).Draw(gameTime);

                }
            }
            catch (Exception ex)
            {
                // need to log this. This could be due to runtime changes in the list.
            }

            if (postProcess != null)
            {
                postProcess.EndPostProcess(gameTime);
            }

            // UI/Overlay
            if (postProcessUI != null)
            {
                postProcessUI.StartPostProcess(gameTime);
            }

            try{
            foreach (IGameComponent component in Components.UIComponents)
            {
                    if (component is IDrawable && ((IDrawable)component).Visible)
                        ((IDrawable)component).Draw(gameTime);
                
            }
            }
            catch (Exception ex)
            {
                // need to log this. This could be due to runtime changes in the list.
            }

            if (postProcessUI != null)
            {
                postProcessUI.EndPostProcess(gameTime);
            }
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
