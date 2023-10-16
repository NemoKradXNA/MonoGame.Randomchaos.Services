using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.UI.Enums;

namespace SampleMonoGame.Randomchaos.Services.P2P.Scenes
{
    public class ServerStartScene : P2PBaseScene
    {
        protected UILabel lblLocalAddress;
        protected UILabel lblPort;
        /// <summary>   The button server. </summary>
        protected UIInputText txtPort;
        /// <summary>   The button client. </summary>
        protected UIButton btnEnterLoby;
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

        public ServerStartScene(Game game, string name) : base(game, name) { }

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

            Texture2D txtBg = new Texture2D(GraphicsDevice,  512, 38);
            Texture2D txtBdr = new Texture2D(GraphicsDevice, 512, 38);

            txtBg.FillWithColor(new Color(.1f,.1f,.1f,.25f));
            txtBdr.FillWithBorder(Color.Transparent, Color.Black, new Rectangle(1, 1, 1, 1));

            lblLocalAddress = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Local IP: [{p2pService.LocalIPv4Address}] Machine Name: [{p2pService.MachineName}]",
                Tint = Color.Black,
                Size = btnSize,
            };

            pos += new Point(0, btnSize.Y + 32);
            lblPort = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Port: ",
                Tint = Color.Black,
                Size = new Point(128, buttonFont.LineSpacing + 8),
            };
            txtPort = new UIInputText(Game, pos + new Point(128,0), txtBg, txtBdr)
            {
                Font = buttonFont,
                Size = new Point(512, buttonFont.LineSpacing+8),
                Text = "6060",
                TextAlingment = TextAlingmentEnum.Middle,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(1,1),                
            };

            pos += new Point(0, btnSize.Y + 32);
            btnEnterLoby = CreateButton("Enter Lobby", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnBack = CreateButton("Back", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            Components.Add(lblLocalAddress);
            Components.Add(lblPort);
            Components.Add(txtPort);
            Components.Add(btnEnterLoby);
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
                if (sender == btnEnterLoby)
                {
                    int port = 6666;

                    if (int.TryParse(txtPort.Text, out port))
                    {
                        p2pService.StartServer(port);
                        sceneManager.LoadScene("lobyScene");
                    }
                    else
                    {
                        txtPort.Tint = Color.Red;
                    }
                }
                else if (sender == btnBack)
                {
                    sceneManager.LoadScene("mainMenu");
                }
            }
        }

        
    }
}
