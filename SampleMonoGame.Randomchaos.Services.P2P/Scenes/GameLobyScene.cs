using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.UI.Enums;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Services;

namespace SampleMonoGame.Randomchaos.Services.P2P.Scenes
{
    public class GameLobyScene : P2PBaseScene
    {
        protected UILabel lblLocalAddress;
        protected UILabel lblStatus;
        /// <summary>   The button client. </summary>
        protected UIButton btnStart;
        /// <summary>   The button exit. </summary>
        protected UIButton btnBack;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 16/10/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public GameLobyScene(Game game, string name) : base(game, name) { }

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

            int menuTop = 32;

            Point pos = new Point(0, menuTop) + (new Point((int)c.X, 0)) - new Point(btnSize.X / 2, btnSize.Y / 2);

            Texture2D txtBg = new Texture2D(GraphicsDevice, 512, 38);
            Texture2D txtBdr = new Texture2D(GraphicsDevice, 512, 38);

            txtBg.FillWithColor(new Color(.1f, .1f, .1f, .25f));
            txtBdr.FillWithBorder(Color.Transparent, Color.Black, new Rectangle(1, 1, 1, 1));

            lblLocalAddress = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"{(p2pService.IsServer ? $"Listening on Port [{p2pService.ListeningPort}]" : $"Server: [{p2pService.ServerIPv4Address}]" )} Local IP: [{p2pService.LocalIPv4Address}] Machine Name: [{p2pService.MachineName}]",
                Tint = Color.Black,
                Size = btnSize,
            };

            pos += new Point(0, btnSize.Y + 32);
            lblStatus = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"{(p2pService.IsServer ? $"Waitign for players to join..." : $"Waitign for server to start the game...")}",
                Tint = Color.Black,
                Size = btnSize,
            };

            btnSize = new Point(256, buttonFont.LineSpacing + 12);

            pos = new Point(8, btnSize.Y + 32);
            btnStart = CreateButton($"{(p2pService.IsServer ? "Start Game" : "I'm Ready")}", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnBack = CreateButton("Back", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            Components.Add(lblLocalAddress);
            Components.Add(lblStatus);
            Components.Add(btnStart);
            Components.Add(btnBack);

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
                if (sender == btnBack)
                {
                    if (p2pService.IsServer)
                    {
                        p2pService.StopServer();
                        sceneManager.LoadScene("serverStartScene");
                    }
                    else
                    {
                        p2pService.Disconnect();
                        sceneManager.LoadScene("clientStartScene");
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            btnStart.Enabled = p2pService.PlayerCount > 0;
            if (btnStart.Enabled)
            {
                btnStart.Text = $"{(p2pService.IsServer ? "Start Game" : "I'm Ready")}";
            }
            else 
            {
                btnStart.Text = $"{(p2pService.IsServer ? "Waiting for players..." : "I'm Ready")}";
            }

            lblStatus.Text = $"{(p2pService.IsServer ? $"Waitign for players to join..." : $"Waitign for server to start the game...")} [{p2pService.PlayerCount + 1}]";
        }
    }
}
