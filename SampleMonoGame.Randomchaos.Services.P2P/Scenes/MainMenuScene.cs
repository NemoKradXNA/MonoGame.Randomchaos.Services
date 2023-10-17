
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;

namespace SampleMonoGame.Randomchaos.Services.P2P.Scenes
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A main menu scene. </summary>
    ///
    /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class MainMenuScene : P2PBaseScene
    {
        
        /// <summary>   True to exiting. </summary>
        private bool exiting;

        protected UILabel lblLocalAddress;
        /// <summary>   The button server. </summary>
        protected UIButton btnServer;
        /// <summary>   The button client. </summary>
        protected UIButton btnClient;
        /// <summary>   The button exit. </summary>
        protected UIButton btnExit;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public MainMenuScene(Game game, string name) : base(game, name) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");
            buttonFont = Game.Content.Load<SpriteFont>("Fonts/ButtonFont");

            Vector2 c = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * .5f;
            Point btnSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 8);

            int menuTop = GraphicsDevice.Viewport.Height / 4;
            
            Point pos = new Point(0, menuTop) + (new Point((int)c.X, 0)) - new Point(btnSize.X / 2, btnSize.Y / 2);

            lblLocalAddress = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Local IP: [{p2pService.LocalIPv4Address}] Machine Name: [{p2pService.MachineName}]",
                Tint = Color.Black,
                Size = btnSize,
            };

            pos += new Point(0, btnSize.Y + 32);
            btnServer = CreateButton("Host Server", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnClient = CreateButton("Join Server", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnExit = CreateButton("Exit Game", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            Components.Add(lblLocalAddress);
            Components.Add(btnServer);
            Components.Add(btnClient);
            Components.Add(btnExit);

            base.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Button mouse click. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
        ///
        /// <param name="sender">       The sender. </param>
        /// <param name="mouseState">   State of the mouse. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (sender == btnServer)
                {
                    p2pService.IsServer = true;
                    sceneManager.LoadScene("serverStartScene");
                }
                else if (sender == btnClient)
                {
                    p2pService.IsServer = true;
                    sceneManager.LoadScene("clientStartScene");
                }
                else if (sender == btnExit)
                {
                    exiting = true;
                    State = SceneStateEnum.Unloading;
                    UnloadScene();
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            if (State == SceneStateEnum.Unloaded && exiting)
                Game.Exit();
        }
    }
}
