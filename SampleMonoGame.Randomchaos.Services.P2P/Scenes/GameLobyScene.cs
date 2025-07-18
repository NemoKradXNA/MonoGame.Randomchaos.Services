﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using Newtonsoft.Json;
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using MonoGame.Randomchaos.Services.P2P.Enums;
using MonoGame.Randomchaos.Services.P2P.Interfaces;
using MonoGame.Randomchaos.Services.P2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        protected bool StartGame = false;


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
                Text = $"{(p2pService.IsServer ? $"Listening on Port [{p2pService.ListeningPort}]" : $"Server: [{p2pService.ServerIPv4Address}:{p2pService.ListeningPort}]")} Local IP: [{p2pService.LocalIPv4Address}] Machine Name: [{p2pService.MachineName}]",
                Tint = Color.Black,
                Size = btnSize,
            };

            pos += new Point(0, btnSize.Y + 32);
            lblStatus = new UILabel(Game)
            {
                Font = buttonFont,
                Position = pos,
                Text = $"{(p2pService.IsServer ? $"Waiting for players to join..." : $"Waiting for server to start the game...")}",
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
                Size = new Point(512, buttonFont.LineSpacing + 8),
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
                Size = new Point(1024, 256),
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

        public override void UnloadScene()
        {
            p2pService.OnConnectionAttempt -= P2pService_OnConnectionAttempt;
            p2pService.OnNewClientAdded -= P2pService_OnNewClientAdded;
            p2pService.OnConnectionDropped -= P2pService_OnConnectionDropped;
            p2pService.OnUdpDataReceived -= P2pService_OnUdpDataReceived;
            p2pService.OnTcpDataReceived -= P2pService_OnTcpDataReceived;
            p2pService.OnLog -= P2pService_OnLog;

            txtBroadcast.OnUIInputComplete -= TxtBroadcast_OnUIInputComplete;

            foreach (var component in Components.Components)
            {
                if (component is UIInputText)
                {
                    ((UIInputText)component).OnUIInputComplete -= GameLobyScene_OnUIInputComplete;
                }
            }

            ClientIds.Clear();

            base.UnloadScene();
        }

        public override void LoadScene()
        {
            p2pService.OnConnectionAttempt += P2pService_OnConnectionAttempt;
            p2pService.OnNewClientAdded += P2pService_OnNewClientAdded;
            p2pService.OnConnectionDropped += P2pService_OnConnectionDropped;
            p2pService.OnUdpDataReceived += P2pService_OnUdpDataReceived;
            p2pService.OnTcpDataReceived += P2pService_OnTcpDataReceived;
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

        protected bool AuthenticateClient(IClientPacketData client)
        {
            if (!p2pService.IsServer)
            {
                return true; // Not up to clients to authenticate users.
            }

            bool retVal = false;

            if (client.PlayerGameData != null)
            {
                IPlayerData plrData = JsonConvert.DeserializeObject<PlayerData>(client.PlayerGameData.ToString());

                retVal = plrData != null && plrData.Session.Name == p2pService.Session.Name && plrData.Session.Token == p2pService.Session.Token;
            }
            return retVal;
        }

        private void P2pService_OnNewClientAdded(IClientPacketData client)
        {
            // Authorize the user here.
            bool IsAuthenticated = AuthenticateClient(client);

            IPlayerData pd = null;
            if (client.PlayerGameData != null)
            {
                pd = JsonConvert.DeserializeObject<PlayerData>(client.PlayerGameData.ToString());
            }

            if (!p2pService.IsServer)
            {
                if (client.Id == p2pService.ClientId)
                {
                    return;
                }
            }

            if (IsAuthenticated)
            {
                if (!ClientIds.ContainsKey(client.Id))
                {
                    var s = buttonFont.MeasureString(client.Id.ToString());
                    Point btnSize = new Point((int)s.X, buttonFont.LineSpacing + 8);

                    bool ready = pd.Properties.ContainsKey("Status") && (ThisGamesStateEnum)((long)pd.Properties["Status"]) == ThisGamesStateEnum.ClientReady;

                    List<UIBase> ctrls = new List<UIBase>()
                    {
                        new UILabel(Game)
                        {
                            Font = buttonFont,
                            //Text = $"{(client.Id != Guid.Empty ? client.Id : "Server")}",
                            Text = pd != null ? $"{pd.Name}{(ready ? " Ready" : "")}" : $"{(client.Id != Guid.Empty ? client.Id : "Server")}",
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

                    if (p2pService.IsServer)
                    {
                        ctrls.Add(new UIButton(Game, Point.Zero, new Point(32, buttonFont.LineSpacing + 8))
                        {
                            BackgroundTexture = txtBg,
                            Font = buttonFont,
                            Text = "X",
                            HighlightColor = Color.SkyBlue,
                            Segments = new Rectangle(8, 8, 8, 8),
                            TextColor = Color.DarkSlateGray,
                            Tag = client.Id,
                        });

                        ((UIButton)ctrls[2]).OnMouseClick += GameLobyScene_OnMouseClick;

                        ctrls[2].Initialize();
                        Components.Add(ctrls[2]);
                    }

                ((UIInputText)ctrls[1]).OnUIInputComplete += GameLobyScene_OnUIInputComplete;

                    ctrls[0].Initialize();
                    ctrls[1].Initialize();

                    Components.Add(ctrls[0]);
                    Components.Add(ctrls[1]);

                    ClientIds.Add(client.Id, ctrls);
                }
            }
            else
            {
                p2pService.BootClient(client.Id);
                AddToMessages($"Could not authenticate connection [{client.Id}] from [{client}], Session and/or Token where wrong.");
            }
        }

        private void P2pService_OnTcpDataReceived(ICommsPacket pkt)
        {
            
        }       

        
        private void P2pService_OnLog(LogLevelEnum lvl, string message, Exception ex = null, params object[] args)
        {
            AddToMessages($"[{DateTime.UtcNow: dd-MM-yyyy hh:mm:ss}] - {lvl}: {message}");
        }

        private void P2pService_OnUdpDataReceived(ICommsPacket pkt)
        {
            var pd = JsonConvert.DeserializeObject<PlayerData>(pkt.Data.ToString());

            var client = p2pService.GetClientById(pkt.Id);
            ThisGamesStateEnum status = ThisGamesStateEnum.None;

            if (pd.Properties.ContainsKey("Status"))
            {
                status = (ThisGamesStateEnum)((long)pd.Properties["Status"]);
            }

            bool ready = status == ThisGamesStateEnum.ClientReady;
            var lbl = ClientIds[pkt.Id].FirstOrDefault(f => f is UILabel);

            if (ready)
            {
                ((UILabel)lbl).Text += " Ready";
            }

            StartGame = status == ThisGamesStateEnum.InGame;
            if (StartGame && !p2pService.IsServer && State == SceneStateEnum.Loaded)
            {
                p2pService.PlayerData.RemoveProperty("Message");
                sceneManager.LoadScene("gameScene");
            }


            AddToMessages($"[{(pd.Name)}{(ready ? " Ready" : "")}] - {pd.Properties["Message"]}");
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
                IPlayerData data = null;

                if (client.PlayerGameData != null)
                {
                    data = JsonConvert.DeserializeObject<PlayerData>(client.PlayerGameData.ToString());
                }


                AddToMessages($"[{(data == null ? client.Id == Guid.Empty ? "Server" : client.Id : data.Name)}] - Disconnected...");
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
            
        }

        private void GameLobyScene_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (p2pService.IsServer)
            {
                UIButton btn = (UIButton)sender;
                Guid id = (Guid)btn.Tag;

                p2pService.BootClient(id);
            }
        }

        private void TxtBroadcast_OnUIInputComplete(IUIBase sender)
        {
            if (!string.IsNullOrEmpty(txtBroadcast.Text))
            {
                AddToMessages($"You -> All: {txtBroadcast.Text}");

                p2pService.PlayerData.SetProperty("Message", txtBroadcast.Text);
                p2pService.Broadcast(p2pService.PlayerData);
                txtBroadcast.Text = string.Empty;
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

                    var pkt = p2pService.GetClientById(id);
                    IPlayerData pd = null;

                    if (pkt.PlayerGameData != null)
                    {
                        pd = JsonConvert.DeserializeObject<PlayerData>(pkt.PlayerGameData.ToString());
                    }

                    AddToMessages($"You -> [{(pd != null ? pd.Name : id == Guid.Empty ? "Server" : id)}]: {msg}");

                    p2pService.PlayerData.SetProperty("Message", msg);
                    p2pService.SendDataTo(id, p2pService.PlayerData);
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
                else if (sender == btnStart)
                {
                    if (p2pService.IsServer)
                    {
                        btnStart.Enabled = false;

                        p2pService.AcceptingConnections = false;
                        sceneManager.LoadScene("gameScene");

                        p2pService.PlayerData.SetProperty("Status", ThisGamesStateEnum.InGame);
                        p2pService.PlayerData.SetProperty("Message",  "Game On!");

                        p2pService.Broadcast(p2pService.PlayerData);

                        StartGame = true;
                    }
                    else
                    {
                        btnStart.Enabled = !btnStart.Enabled;

                        
                        p2pService.PlayerData.SetProperty("Status", btnStart.Enabled ? ThisGamesStateEnum.ClientNotReady : ThisGamesStateEnum.ClientReady);
                        p2pService.PlayerData.SetProperty("Message", btnStart.Enabled ? "Not ready to play!" :  "Ready to play.");
                        
                        p2pService.Broadcast(p2pService.PlayerData);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!p2pService.IsServer)
            {
                lblLocalAddress.Text = $"{p2pService.PlayerData.Name} {(p2pService.IsServer ? $"Listening on Port[{p2pService.ListeningPort}]" : $"Server: [{p2pService.ServerIPv4Address}:{p2pService.ListeningPort}]")} Local IP: [{p2pService.LocalIPv4Address}] Machine Name: [{p2pService.MachineName}] Id [{p2pService.ClientId}]";
            }

            if (!p2pService.IsServer)
            {
                btnStart.Enabled = p2pService.PlayerCount > 0 &&
                    (!p2pService.PlayerData.Properties.ContainsKey("Status") || (ThisGamesStateEnum)p2pService.PlayerData.Properties["Status"] != ThisGamesStateEnum.ClientReady);

            }
            else
            {
                // Are all the clients ready?
                int playersNotReady = p2pService.GetClientsPlayerGameData<PlayerData>().Count(c => !c.Properties.ContainsKey("Status") || (ThisGamesStateEnum)(long)c.Properties["Status"] != ThisGamesStateEnum.ClientReady);

                btnStart.Enabled = p2pService.PlayerCount > 0 && playersNotReady == 0 && !StartGame;

            }

            if (btnStart.Enabled)
            {
                btnStart.Text = $"{(p2pService.IsServer ? "Start Game" : "I'm Ready")}";
            }
            else 
            {   
                btnStart.Text = $"{(p2pService.IsServer ? !StartGame ? "Waiting for players..." : "Starting Game..." : "I'm Ready")}";
            }

            lblStatus.Text = $"{(p2pService.IsServer ? $"Waiting for players to join [({p2pService.Session.Name}) ({p2pService.Session.Token})]..." : $"Waiting for server to start the game...")} [{p2pService.PlayerCount + 1}]";


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
                        pos.X = ctrl.Position.X + ctrl.Size.X + 8;
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

                    if (ctrl is UIButton)
                    {
                        ((UIButton)ctrl).OnMouseClick -= GameLobyScene_OnMouseClick;
                    }

                    Components.Remove(ctrl);
                }
            }
        }

    }
}
