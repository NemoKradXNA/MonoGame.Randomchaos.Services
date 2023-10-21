using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.UI.Enums;
using System.Collections.Generic;

namespace SampleMonoGame.Randomchaos.Services.P2P.Scenes
{
    public class ClientStartScene : P2PBaseScene
    {
        protected UILabel lblLocalAddress;
        protected UILabel lblExternalIPv4;
        protected UIInputText txtExternalIPv4;
        protected UILabel lblPort;
        protected UIInputText txtPort;

        protected UILabel lblSessionName;
        protected UIInputText txtSessionName;

        protected UILabel lblSessionToken;
        protected UIInputText txtSessionToken;

        protected UILabel lblUdpPort;
        protected UIInputText txtUdpPort;

        protected UILabel lblYourName;
        protected UIInputText txtYourName;

        protected UIButton btnEnterLoby;
        /// <summary>   The button exit. </summary>
        protected UIButton btnBack;

        public ClientStartScene(Game game, string name) : base(game, name) { }

        public override void Initialize()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/font");
            buttonFont = Game.Content.Load<SpriteFont>("Fonts/ButtonFont");

            Vector2 c = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * .5f;
            Point btnSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 8);

            int menuTop = GraphicsDevice.Viewport.Height / 4;

            Point pos = new Point(0, menuTop) + (new Point((int)c.X, 0)) - new Point(btnSize.X / 2, btnSize.Y / 2);

            Texture2D txtBg = new Texture2D(GraphicsDevice, 512, 38);
            Texture2D txtBdr = new Texture2D(GraphicsDevice, 512, 38);

            txtBg.FillWithColor(new Color(.1f, .1f, .1f, .25f));
            txtBdr.FillWithBorder(Color.Transparent, Color.Black, new Rectangle(1, 1, 1, 1));

            lblLocalAddress = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Local IP: [{p2pService.LocalIPv4Address}] Machine Name: [{p2pService.MachineName}]",
                Tint = Color.Black,
                Size = new Point(btnSize.X, buttonFont.LineSpacing + 8),
            };

            pos += new Point(0, buttonFont.LineSpacing + 16);
            lblExternalIPv4 = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Server Address: ",
                Tint = Color.Black,
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                Size = new Point(128, buttonFont.LineSpacing + 8),
            };

            txtExternalIPv4 = new UIInputText(Game, pos + new Point(200, 0), txtBg, txtBdr)
            {
                Font = buttonFont,
                Size = new Point(512, buttonFont.LineSpacing + 8),
                Text = $"{p2pService.LocalIPv4Address}",
                TextAlingment = TextAlingmentEnum.Middle,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(1, 1),
                TextInputType = TextInputTypeEnum.Numeric,
                AllowedKeys = new List<Keys>() { Keys.OemPeriod, Keys.Decimal },
            };

            pos += new Point(0, buttonFont.LineSpacing + 16);
            lblPort = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Server Port: ",
                Tint = Color.Black,
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                Size = new Point(200, buttonFont.LineSpacing + 8),
            };
            txtPort = new UIInputText(Game, pos + new Point(200, 0), txtBg, txtBdr)
            {
                Font = buttonFont,
                Size = new Point(512, buttonFont.LineSpacing + 8),
                Text = "6060",
                TextAlingment = TextAlingmentEnum.Middle,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(1, 1),
                TextInputType = TextInputTypeEnum.Numeric,
            };

            pos += new Point(0, buttonFont.LineSpacing + 16);
            lblUdpPort = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Your Port: ",
                Tint = Color.Black,
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                Size = new Point(200, buttonFont.LineSpacing + 8),
            };
            txtUdpPort = new UIInputText(Game, pos + new Point(200, 0), txtBg, txtBdr)
            {
                Font = buttonFont,
                Size = new Point(512, buttonFont.LineSpacing + 8),
                Text = $"{p2pService.GetRandomPortNumber(6060)}",
                TextAlingment = TextAlingmentEnum.Middle,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(1, 1),
                TextInputType = TextInputTypeEnum.Numeric,
            };

            pos += new Point(0, buttonFont.LineSpacing + 16);
            lblYourName = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Your Name: ",
                Tint = Color.Black,
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                Size = new Point(200, buttonFont.LineSpacing + 8),
            };
            txtYourName = new UIInputText(Game, pos + new Point(200, 0), txtBg, txtBdr)
            {
                Font = buttonFont,
                Size = new Point(512, buttonFont.LineSpacing + 8),
                Text = $"Client {p2pService.LocalIPv4Address}:{txtUdpPort.Text}",
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(1, 1),
                TextInputType = TextInputTypeEnum.AlphaNumeric,
                TextPositionOffset = new Vector2(8, 0)
            };

            pos += new Point(0, buttonFont.LineSpacing + 16);
            lblSessionName = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Session Name: ",
                Tint = Color.Black,
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                Size = new Point(200, buttonFont.LineSpacing + 8),
            };
            txtSessionName = new UIInputText(Game, pos + new Point(200, 0), txtBg, txtBdr)
            {
                Font = buttonFont,
                Size = new Point(512, buttonFont.LineSpacing + 8),
                Text = "P2P Test",
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(1, 1),
                TextInputType = TextInputTypeEnum.AlphaNumeric,
                TextPositionOffset = new Vector2(8, 0)
            };

            pos += new Point(0, buttonFont.LineSpacing + 16);
            lblSessionToken = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"Session Token: ",
                Tint = Color.Black,
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                Size = new Point(200, buttonFont.LineSpacing + 8),
            };
            txtSessionToken = new UIInputText(Game, pos + new Point(200, 0), txtBg, txtBdr)
            {
                Font = buttonFont,
                Size = new Point(512, buttonFont.LineSpacing + 8),
                Text = "FB505159-C137",
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(1, 1),
                TextInputType = TextInputTypeEnum.AlphaNumeric,
                TextPositionOffset = new Vector2(8, 0)
            };

            pos += new Point(0, buttonFont.LineSpacing + 32);
            btnEnterLoby = CreateButton("Enter Lobby", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnBack = CreateButton("Back", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            Components.Add(lblLocalAddress);
            Components.Add(lblExternalIPv4);
            Components.Add(txtExternalIPv4);
            Components.Add(lblPort);
            Components.Add(txtPort);
            Components.Add(lblSessionName);
            Components.Add(txtSessionName);
            Components.Add(lblSessionToken);
            Components.Add(txtSessionToken);
            Components.Add(lblUdpPort);
            Components.Add(txtUdpPort);
            Components.Add(lblYourName);
            Components.Add(txtYourName);
            Components.Add(btnEnterLoby);
            Components.Add(btnBack);

            base.Initialize();
        }

        protected override void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (State == SceneStateEnum.Loaded)
            {
                if (sender == btnEnterLoby)
                {
                    int port = 6666;
                    int clientPort = 6011;

                    if (int.TryParse(txtPort.Text, out port))
                    {
                        if (int.TryParse(txtUdpPort.Text, out clientPort))
                        {
                            p2pService.ConnectClient(txtExternalIPv4.Text, port, p2pService.LocalIPv4Address, clientPort, txtSessionName.Text, txtSessionToken.Text);
                            p2pService.PlayerData.Name = txtYourName.Text;
                            sceneManager.LoadScene("lobyScene");
                        }
                        else
                        {
                            txtUdpPort.Tint = Color.Red;
                        }
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
