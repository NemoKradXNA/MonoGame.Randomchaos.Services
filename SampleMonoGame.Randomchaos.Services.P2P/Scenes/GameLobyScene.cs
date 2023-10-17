using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

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

        protected UILabel lblBroadcast;
        protected UIInputText txtBroadcast;

        protected Dictionary<Guid, List<UIBase>> ClientIds = new Dictionary<Guid, List<UIBase>>();


        List<string> MessageFeed = new List<string>()
        {
            "Incoming messages are shown here..."
        };

        UILabel lblIncommingMessages;

        Texture2D txtBg;
        Texture2D txtBdr;

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

            txtBg = new Texture2D(GraphicsDevice, 512, 38);
            txtBdr = new Texture2D(GraphicsDevice, 512, 38);

            txtBg.FillWithColor(new Color(.1f, .1f, .1f, .25f));
            txtBdr.FillWithBorder(Color.Transparent, Color.Black, new Rectangle(1, 1, 1, 1));

            lblLocalAddress = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"{(p2pService.IsServer ? $"Listening on Port [{p2pService.ListeningPort}]" : $"Server: [{p2pService.ServerIPv4Address}]")} Local IP: [{p2pService.LocalIPv4Address}] Machine Name: [{p2pService.MachineName}]",
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

            lblBroadcast = new UILabel(Game)
            {
                Font = buttonFont,
                Position = new Point(32, GraphicsDevice.Viewport.Height - 348),
                Text = $"Broadcast to all:",
                Tint = Color.Black,
                Size = new Point(256, buttonFont.LineSpacing + 8),
            };

            txtBroadcast = new UIInputText(Game, new Point(lblBroadcast.Position.X + lblBroadcast.Size.X + 32, lblBroadcast.Position.Y), txtBg, txtBdr)
            {
                Size = new Point(btnSize.X, buttonFont.LineSpacing + 8),
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                TextPositionOffset = new Vector2(8, 0),
                TextColor = Color.White,
                Font = buttonFont,
            };

            txtBroadcast.OnUIInputComplete += TxtBroadcast_OnUIInputComplete;

            btnSize = new Point(256, buttonFont.LineSpacing + 12);

            lblIncommingMessages = new UILabel(Game)
            {
                Font = font,
                Position = new Point(32, GraphicsDevice.Viewport.Height - 300),
                Text = $"",
                Tint = Color.Black,
                Background = txtBg,
                TextAlingment = TextAlingmentEnum.LeftTop,
                TextPositionOffset = new Vector2(8,8),
                Size = new Point(GraphicsDevice.Viewport.Width - 64, 256),
            };


            pos = new Point(8, btnSize.Y + 32);
            btnStart = CreateButton($"{(p2pService.IsServer ? "Start Game" : "I'm Ready")}", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            pos += new Point(0, btnSize.Y + 32);
            btnBack = CreateButton("Back", Game.Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);

            Components.Add(lblLocalAddress);
            Components.Add(lblBroadcast);
            Components.Add(txtBroadcast);
            Components.Add(lblStatus);
            Components.Add(btnStart);
            Components.Add(btnBack);
            Components.Add(lblIncommingMessages);

            base.Initialize();

        }

        public override void LoadScene()
        {
            p2pService.OnConnectionAttempt += P2pService_OnConnectionAttempt;
            p2pService.OnConnectionDropped += P2pService_OnConnectionDropped;
            p2pService.OnUdpDataReceived += P2pService_OnUdpDataReceived;
            p2pService.OnLog += P2pService_OnLog;

            if (txtBroadcast != null)
            {
                txtBroadcast.OnUIInputComplete += TxtBroadcast_OnUIInputComplete;
            }

            MessageFeed = new List<string>()
            {
                "Incoming messages are shown here..."
            };

            base.LoadScene();
        }

        public override void UnloadScene()
        {
            p2pService.OnConnectionAttempt -= P2pService_OnConnectionAttempt;
            p2pService.OnConnectionDropped -= P2pService_OnConnectionDropped;
            p2pService.OnUdpDataReceived -= P2pService_OnUdpDataReceived;
            p2pService.OnLog -= P2pService_OnLog;

            txtBroadcast.OnUIInputComplete -= TxtBroadcast_OnUIInputComplete;

            foreach (var component in Components.Components)
            {
                if (component is UIInputText)
                {
                    ((UIInputText)component).OnUIInputComplete -= GameLobyScene_OnUIInputComplete;
                }
            }

            base.UnloadScene();
        }

        private void TxtBroadcast_OnUIInputComplete(IUIBase sender)
        {
            if (!string.IsNullOrEmpty(txtBroadcast.Text))
            {
                AddToMessages($"You -> All: {txtBroadcast.Text}");
                p2pService.Broadcast(txtBroadcast.Text);
                txtBroadcast.Text = string.Empty;
            }
        }

        private void P2pService_OnLog(Enums.LogLevelEnum lvl, string message, Exception ex = null, params object[] args)
        {
            AddToMessages($"[{DateTime.UtcNow: dd-MM-yyyy hh:mm:ss}] - {lvl}: {message}");
        }

        private void P2pService_OnUdpDataReceived(IClientPacketData client, object? data)
        {
            AddToMessages($"[{client.Id}] - {data}");
        }

        private void AddToMessages(string msg)
        {
            MessageFeed.Add(msg);

            if (MessageFeed.Count > 13)
            {
                MessageFeed.RemoveRange(0, MessageFeed.Count - 13);
            }
        }

        private void P2pService_OnConnectionDropped(IClientPacketData client)
        {
            if (ClientIds.ContainsKey(client.Id))
            {
                AddToMessages($"[{client.Id}] - Disconnected...");
                // disable and hide their controls.
                foreach (UIBase ctrl in ClientIds[client.Id])
                {
                    ctrl.Enabled = false;
                    ctrl.Visible = false;
                }
            }
        }

        private void P2pService_OnConnectionAttempt(IClientPacketData client)
        {
            // Authorize the user here?
            if (!ClientIds.ContainsKey(client.Id))
            {
                var s = buttonFont.MeasureString(client.Id.ToString());
                Point btnSize = new Point((int)s.X, buttonFont.LineSpacing + 8);

                List<UIBase> ctrls = new List<UIBase>()
                {
                    new UILabel(Game)
                    {
                        Font = buttonFont,
                        Text = $"{client.Id}",
                        Tint = Color.Black,
                        Size = btnSize,
                    },
                    new UIInputText(Game, Point.Zero,txtBg,txtBdr)
                    {
                        Size = new Point(btnSize.X, buttonFont.LineSpacing+8),
                        TextAlingment = TextAlingmentEnum.LeftMiddle,
                        TextPositionOffset = new Vector2(8,0),
                        TextColor = Color.White,
                        Font = buttonFont,
                        Tag = client.Id,
                    }
                };

                ((UIInputText)ctrls[1]).OnUIInputComplete += GameLobyScene_OnUIInputComplete;

                ctrls[0].Initialize();
                ctrls[1].Initialize();

                Components.Add(ctrls[0]);
                Components.Add(ctrls[1]);

                ClientIds.Add(client.Id,ctrls);
            }
        }

        private void GameLobyScene_OnUIInputComplete(IUIBase sender)
        {
            UIInputText txtBox = (UIInputText)sender;

            if (txtBox != null)
            {
                // Send message to this client..
                string msg = txtBox.Text;
                if (!string.IsNullOrEmpty(msg))
                {
                    Guid id = (Guid)txtBox.Tag;
                    AddToMessages($"You -> [{id}]: {msg}");
                    p2pService.SendDataTo(id, msg);
                    txtBox.Text = string.Empty;
                }
            }
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


            lblIncommingMessages.Text = "";
            foreach (string msg in MessageFeed)
            {
                lblIncommingMessages.Text += $"{msg}\n";
            }

            // set positions of client controls on screen.
            int topP = 256;
            List<Guid> delete = new List<Guid>();

            foreach (Guid id in ClientIds.Keys)
            {
                bool visible = true;
                Point pos = new Point(64 , topP);
                foreach (UIBase ctrl in ClientIds[id])
                {
                    visible = ctrl.Visible;
                    if (visible)
                    {
                        ctrl.Position = pos;
                        pos.X += ctrl.Position.X + ctrl.Size.X;
                        //pos.X += 256;
                    }
                    else
                        break;
                }

                if (visible)
                {
                    topP += 48;
                }
                else
                {
                    delete.Add(id);
                }
            }

            foreach (Guid id in delete)
            {
                foreach (UIBase ctrl in ClientIds[id])
                {
                    if (ctrl is UIInputText)
                    {
                        ((UIInputText)ctrl).OnUIInputComplete -= GameLobyScene_OnUIInputComplete;
                    }
                    Components.Remove(ctrl);
                }
            }
        }
    }
}
